import { useEffect, useState } from "react";
import { Card, CardBody } from "@heroui/card";
import { Button } from "@heroui/button";
import { Icon } from "../../components/icons/Icon";
import { LoadingComponent } from "../../components/spinner/LoadingComponent";
import { getDashboardStats, getPendingSolicitudesStats, getTransactionHistory } from "../../services/budgetService";
import type { DashboardStats, PendingSolicitudesStats, TransactionHistory } from "../../types/Budget";

// Función para convertir fecha de DD/MM/YYYY a Date
const parseDate = (dateStr: string): Date | null => {
  if (!dateStr) return null;
  
  if (dateStr.includes('-') && dateStr.indexOf('-') === 4) {
    return new Date(dateStr);
  }
  
  if (dateStr.includes('/')) {
    const [day, month, year] = dateStr.split('/');
    return new Date(`${year}-${month}-${day}`);
  }
  
  return null;
};

interface MonthlyData {
  month: string;
  mantenimiento: number;
  eventos: number;
  total: number;
}

export function Component() {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [pendingStats, setPendingStats] = useState<PendingSolicitudesStats[]>([]);
  const [transactions, setTransactions] = useState<TransactionHistory[]>([]);
  const [monthlyData, setMonthlyData] = useState<MonthlyData[]>([]);

  useEffect(() => {
    loadReportData();
  }, []);

  const loadReportData = async () => {
    try {
      setLoading(true);
      
      console.log('🔄 CARGANDO DATOS DEL REPORTE...');
      
      const [statsResponse, pendingResponse, transactionsResponse] = await Promise.all([
        getDashboardStats(),
        getPendingSolicitudesStats(),
        getTransactionHistory(1000) // Todas las transacciones
      ]);

      console.log('📦 RESPUESTAS RECIBIDAS:', {
        stats: statsResponse.success,
        pending: pendingResponse.success,
        transactions: transactionsResponse.success
      });

      if (statsResponse.success && statsResponse.data) {
        console.log('✅ Stats cargadas:', statsResponse.data);
        setStats(statsResponse.data);
      }

      if (pendingResponse.success && pendingResponse.data) {
        console.log('✅ Pending stats cargadas:', pendingResponse.data);
        setPendingStats(pendingResponse.data);
      }

      if (transactionsResponse.success && transactionsResponse.data) {
        console.log('✅ Transacciones cargadas:', transactionsResponse.data.length, 'registros');
        console.log('📋 Primeras 3 transacciones:', transactionsResponse.data.slice(0, 3));
        setTransactions(transactionsResponse.data);
        processMonthlyData(transactionsResponse.data);
      } else {
        console.log('❌ No se cargaron transacciones:', transactionsResponse);
      }

    } catch (error) {
      console.error("❌ ERROR loading report data:", error);
    } finally {
      setLoading(false);
    }
  };

  const processMonthlyData = (transactions: TransactionHistory[]) => {
    console.log('📅 PROCESANDO DATOS MENSUALES...');
    console.log('📅 Total transacciones recibidas:', transactions.length);
    
    const monthMap = new Map<string, { mantenimiento: number; eventos: number }>();
    
    const aprobadas = transactions.filter(t => {
      const isAprobada = t.statusName?.toLowerCase().includes('aprobada') || 
                         t.statusName?.toLowerCase().includes('aprobado') ||
                         t.statusName?.toLowerCase() === 'aprobada';
      return isAprobada;
    });

    console.log('📅 Transacciones aprobadas encontradas:', aprobadas.length);
    
    if (aprobadas.length > 0) {
      console.log('📅 Muestra de aprobadas:', aprobadas.slice(0, 3).map(t => ({
        nombre: t.name,
        fecha: t.requestDate,
        monto: t.requestAmount,
        origen: t.originName,
        originId: t.originId
      })));
    }
    
    aprobadas.forEach(transaction => {
      console.log('📅 Procesando transacción:', {
        nombre: transaction.name,
        fecha: transaction.requestDate,
        monto: transaction.requestAmount,
        originId: transaction.originId
      });
      
      if (transaction.requestDate) {
        const date = parseDate(transaction.requestDate);
        console.log('📅 Fecha parseada:', date);
        
        if (date) {
          const monthKey = date.toLocaleDateString('es-ES', { month: 'long', year: 'numeric' });
          console.log('📅 Clave del mes:', monthKey);
          
          if (!monthMap.has(monthKey)) {
            monthMap.set(monthKey, { mantenimiento: 0, eventos: 0 });
          }
          
          const monthData = monthMap.get(monthKey)!;
          const amount = transaction.requestAmount || 0;
          
          if (transaction.originId === 1) {
            monthData.mantenimiento += amount;
            console.log(`📅 Agregado a Mantenimiento: Q${amount}`);
          } else if (transaction.originId === 2) {
            monthData.eventos += amount;
            console.log(`📅 Agregado a Eventos: Q${amount}`);
          } else {
            console.log(`⚠️ OriginId desconocido: ${transaction.originId}`);
          }
        } else {
          console.log('❌ No se pudo parsear la fecha:', transaction.requestDate);
        }
      } else {
        console.log('❌ Transacción sin fecha:', transaction.name);
      }
    });

    console.log('📅 Mapa mensual completo:', Array.from(monthMap.entries()));

    const monthlyArray: MonthlyData[] = Array.from(monthMap.entries()).map(([month, data]) => ({
      month,
      mantenimiento: data.mantenimiento,
      eventos: data.eventos,
      total: data.mantenimiento + data.eventos
    }));

    // Ordenar por fecha (más reciente primero)
    monthlyArray.sort((a, b) => b.month.localeCompare(a.month));
    
    console.log('📅 REPORTE - Datos mensuales procesados (antes de slice):', monthlyArray);
    console.log('📅 REPORTE - Datos mensuales finales (después de slice):', monthlyArray.slice(0, 12));
    
    setMonthlyData(monthlyArray.slice(0, 12)); // Últimos 12 meses
  };

  const exportToCSV = () => {
    const headers = ['Mes', 'Mantenimiento', 'Eventos', 'Total'];
    const rows = monthlyData.map(m => [
      m.month,
      `Q${m.mantenimiento.toFixed(2)}`,
      `Q${m.eventos.toFixed(2)}`,
      `Q${m.total.toFixed(2)}`
    ]);

    const csvContent = [
      headers.join(','),
      ...rows.map(row => row.join(','))
    ].join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = `reporte_financiero_${new Date().toISOString().split('T')[0]}.csv`;
    link.click();
  };

  const printReport = () => {
    window.print();
  };

  if (loading) {
    return <LoadingComponent />;
  }

  console.log('📊 REPORTE - Total transacciones:', transactions.length);
  console.log('📊 REPORTE - Muestra de estados:', transactions.slice(0, 5).map(t => ({
    id: t.id,
    nombre: t.name,
    estado: t.statusName,
    monto: t.requestAmount,
    origen: t.originName
  })));

  const aprobadasCount = transactions.filter(t => {
    const isAprobada = t.statusName?.toLowerCase().includes('aprobada') || 
                       t.statusName?.toLowerCase().includes('aprobado') ||
                       t.statusName?.toLowerCase() === 'aprobada';
    return isAprobada;
  }).length;

  const rechazadasCount = transactions.filter(t => {
    const isRechazada = t.statusName?.toLowerCase().includes('rechazada') || 
                        t.statusName?.toLowerCase().includes('rechazado') ||
                        t.statusName?.toLowerCase() === 'rechazada';
    return isRechazada;
  }).length;

  const totalSolicitudes = transactions.length;

  console.log('📊 REPORTE - Aprobadas:', aprobadasCount, 'Rechazadas:', rechazadasCount, 'Total:', totalSolicitudes);

  const mantenimientoTotal = transactions
    .filter(t => {
      const isAprobada = t.statusName?.toLowerCase().includes('aprobada') || 
                         t.statusName?.toLowerCase().includes('aprobado');
      const isMantenimiento = t.originId === 1;
      return isMantenimiento && isAprobada;
    })
    .reduce((sum, t) => sum + (t.requestAmount || 0), 0);

  const eventosTotal = transactions
    .filter(t => {
      const isAprobada = t.statusName?.toLowerCase().includes('aprobada') || 
                         t.statusName?.toLowerCase().includes('aprobado');
      const isEventos = t.originId === 2;
      return isEventos && isAprobada;
    })
    .reduce((sum, t) => sum + (t.requestAmount || 0), 0);

  console.log('📊 REPORTE - Mantenimiento Total:', mantenimientoTotal, 'Eventos Total:', eventosTotal);

  return (
    <div className="p-6 space-y-6 print:p-4">
      {/* Header */}
      <div className="flex items-center justify-between print:mb-6">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">📊 Reporte Financiero</h1>
          <p className="text-gray-500 mt-1">Análisis completo del presupuesto y gastos</p>
          <p className="text-sm text-gray-400 mt-1">
            Generado el {new Date().toLocaleDateString('es-ES', { 
              day: 'numeric', 
              month: 'long', 
              year: 'numeric',
              hour: '2-digit',
              minute: '2-digit'
            })}
          </p>
        </div>
        <div className="flex gap-2 print:hidden">
          <Button 
            color="primary" 
            variant="flat"
            onPress={exportToCSV}
          >
            <Icon name="bi bi-download" size={16} />
            Exportar CSV
          </Button>
          <Button 
            color="primary" 
            variant="shadow"
            onPress={printReport}
          >
            <Icon name="bi bi-printer" size={16} />
            Imprimir
          </Button>
        </div>
      </div>

      {/* Resumen General */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card className="border-l-4 border-blue-500">
          <CardBody className="p-4">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-gray-500">Presupuesto Total</p>
                <p className="text-2xl font-bold text-blue-600">
                  Q{stats?.totalBudget.toLocaleString() || '0'}
                </p>
              </div>
              <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                <Icon name="bi bi-wallet2" size={24} color="text-blue-600" />
              </div>
            </div>
          </CardBody>
        </Card>

        <Card className="border-l-4 border-red-500">
          <CardBody className="p-4">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-gray-500">Total Usado</p>
                <p className="text-2xl font-bold text-red-600">
                  Q{stats?.usedBudget.toLocaleString() || '0'}
                </p>
                <p className="text-xs text-gray-400">
                  {stats?.usagePercentage.toFixed(1)}% del total
                </p>
              </div>
              <div className="w-12 h-12 bg-red-100 rounded-lg flex items-center justify-center">
                <Icon name="bi bi-graph-down-arrow" size={24} color="text-red-600" />
              </div>
            </div>
          </CardBody>
        </Card>

        <Card className="border-l-4 border-green-500">
          <CardBody className="p-4">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-gray-500">Disponible</p>
                <p className="text-2xl font-bold text-green-600">
                  Q{stats?.availableBudget.toLocaleString() || '0'}
                </p>
                <p className="text-xs text-gray-400">
                  {((stats?.availableBudget || 0) / (stats?.totalBudget || 1) * 100).toFixed(1)}% restante
                </p>
              </div>
              <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center">
                <Icon name="bi bi-cash-stack" size={24} color="text-green-600" />
              </div>
            </div>
          </CardBody>
        </Card>

        <Card className="border-l-4 border-purple-500">
          <CardBody className="p-4">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-gray-500">Total Solicitudes</p>
                <p className="text-2xl font-bold text-purple-600">
                  {totalSolicitudes}
                </p>
                <p className="text-xs text-green-600">{aprobadasCount} aprobadas</p>
                <p className="text-xs text-red-600">{rechazadasCount} rechazadas</p>
              </div>
              <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center">
                <Icon name="bi bi-file-earmark-text" size={24} color="text-purple-600" />
              </div>
            </div>
          </CardBody>
        </Card>
      </div>

      {/* Gastos por Área */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Card>
          <CardBody className="p-6">
            <h2 className="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2">
              <Icon name="bi bi-pie-chart-fill" size={20} color="text-blue-600" />
              Gastos por Área
            </h2>
            
            {mantenimientoTotal === 0 && eventosTotal === 0 ? (
              <div className="text-center py-8">
                <Icon name="bi bi-inbox" size={48} color="text-gray-400" />
                <p className="text-gray-500 mt-2">No hay gastos aprobados aún</p>
              </div>
            ) : (
              <div className="space-y-4">
                {/* Mantenimiento */}
                <div>
                  <div className="flex items-center justify-between mb-2">
                    <div className="flex items-center gap-2">
                      <div className="w-3 h-3 bg-green-500 rounded"></div>
                      <span className="text-sm font-semibold text-gray-700">Mantenimiento</span>
                    </div>
                    <span className="text-sm font-bold text-green-600">
                      Q{mantenimientoTotal.toLocaleString()}
                    </span>
                  </div>
                  <div className="w-full bg-gray-200 rounded-full h-3">
                    <div 
                      className="bg-gradient-to-r from-green-600 to-green-400 h-3 rounded-full transition-all duration-500"
                      style={{ width: `${stats?.usedBudget ? (mantenimientoTotal / stats.usedBudget) * 100 : 0}%` }}
                    ></div>
                  </div>
                  <p className="text-xs text-gray-500 mt-1">
                    {stats?.usedBudget ? ((mantenimientoTotal / stats.usedBudget) * 100).toFixed(1) : '0.0'}% del gasto total
                  </p>
                </div>

                {/* Eventos */}
                <div>
                  <div className="flex items-center justify-between mb-2">
                    <div className="flex items-center gap-2">
                      <div className="w-3 h-3 bg-blue-500 rounded"></div>
                      <span className="text-sm font-semibold text-gray-700">Eventos</span>
                    </div>
                    <span className="text-sm font-bold text-blue-600">
                      Q{eventosTotal.toLocaleString()}
                    </span>
                  </div>
                  <div className="w-full bg-gray-200 rounded-full h-3">
                    <div 
                      className="bg-gradient-to-r from-blue-600 to-blue-400 h-3 rounded-full transition-all duration-500"
                      style={{ width: `${stats?.usedBudget ? (eventosTotal / stats.usedBudget) * 100 : 0}%` }}
                    ></div>
                  </div>
                  <p className="text-xs text-gray-500 mt-1">
                    {stats?.usedBudget ? ((eventosTotal / stats.usedBudget) * 100).toFixed(1) : '0.0'}% del gasto total
                  </p>
                </div>

                {/* Resumen */}
                <div className="pt-4 border-t border-gray-200">
                  <div className="flex justify-between items-center">
                    <span className="font-semibold text-gray-700">Total Gastado</span>
                    <span className="text-lg font-bold text-gray-800">
                      Q{(mantenimientoTotal + eventosTotal).toLocaleString()}
                    </span>
                  </div>
                </div>
              </div>
            )}
          </CardBody>
        </Card>

        {/* Solicitudes Pendientes por Área */}
        <Card>
          <CardBody className="p-6">
            <h2 className="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2">
              <Icon name="bi bi-clock-history" size={20} color="text-orange-600" />
              Solicitudes Pendientes
            </h2>
            
            <div className="space-y-6">
              {pendingStats.map((area) => (
                <div key={area.originId} className="space-y-2">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-3">
                      <div className={`w-10 h-10 rounded-lg flex items-center justify-center ${
                        area.originId === 1 ? 'bg-green-100' : 'bg-blue-100'
                      }`}>
                        <Icon 
                          name={area.originId === 1 ? 'bi bi-wrench' : 'bi bi-calendar-event'}
                          size={20}
                          color={area.originId === 1 ? 'text-green-600' : 'text-blue-600'}
                        />
                      </div>
                      <div>
                        <p className="font-semibold text-gray-800">{area.originName}</p>
                        <p className="text-sm text-gray-500">{area.totalPending} solicitudes</p>
                      </div>
                    </div>
                    <div className="text-right">
                      <p className="text-lg font-bold text-orange-600">
                        Q{area.totalUsed.toLocaleString()}
                      </p>
                      <p className="text-xs text-gray-500">Monto pendiente</p>
                    </div>
                  </div>
                  <div className="w-full bg-gray-200 rounded-full h-2">
                    <div 
                      className={`h-2 rounded-full ${
                        area.originId === 1 
                          ? 'bg-gradient-to-r from-green-500 to-green-300' 
                          : 'bg-gradient-to-r from-blue-500 to-blue-300'
                      }`}
                      style={{ width: `${Math.min((area.totalUsed / (stats?.availableBudget || 1)) * 100, 100)}%` }}
                    ></div>
                  </div>
                </div>
              ))}

              {pendingStats.length === 0 && (
                <div className="text-center py-8">
                  <Icon name="bi bi-check-circle" size={48} color="text-green-400" />
                  <p className="text-gray-500 mt-2">No hay solicitudes pendientes</p>
                </div>
              )}
            </div>
          </CardBody>
        </Card>
      </div>

      {/* Tabla de Gastos Mensuales */}
      <Card>
        <CardBody className="p-6">
          <h2 className="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2">
            <Icon name="bi bi-calendar3" size={20} color="text-indigo-600" />
            Gastos Mensuales Aprobados
          </h2>

          {(() => {
            console.log('📊 RENDERIZANDO TABLA - monthlyData.length:', monthlyData.length);
            console.log('📊 RENDERIZANDO TABLA - monthlyData:', monthlyData);
            return null;
          })()}

          {monthlyData.length > 0 ? (
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b-2 border-gray-200">
                    <th className="text-left py-3 px-4 text-sm font-semibold text-gray-700">Mes</th>
                    <th className="text-right py-3 px-4 text-sm font-semibold text-gray-700">Mantenimiento</th>
                    <th className="text-right py-3 px-4 text-sm font-semibold text-gray-700">Eventos</th>
                    <th className="text-right py-3 px-4 text-sm font-semibold text-gray-700">Total</th>
                    <th className="text-right py-3 px-4 text-sm font-semibold text-gray-700">% del Presupuesto</th>
                  </tr>
                </thead>
                <tbody>
                  {monthlyData.map((month, index) => (
                    <tr 
                      key={month.month} 
                      className={`border-b border-gray-100 hover:bg-gray-50 ${
                        index % 2 === 0 ? 'bg-white' : 'bg-gray-50'
                      }`}
                    >
                      <td className="py-3 px-4 text-sm font-medium text-gray-800 capitalize">
                        {month.month}
                      </td>
                      <td className="py-3 px-4 text-sm text-right text-green-600 font-semibold">
                        Q{month.mantenimiento.toLocaleString()}
                      </td>
                      <td className="py-3 px-4 text-sm text-right text-blue-600 font-semibold">
                        Q{month.eventos.toLocaleString()}
                      </td>
                      <td className="py-3 px-4 text-sm text-right text-gray-800 font-bold">
                        Q{month.total.toLocaleString()}
                      </td>
                      <td className="py-3 px-4 text-sm text-right text-gray-600">
                        {((month.total / (stats?.totalBudget || 1)) * 100).toFixed(2)}%
                      </td>
                    </tr>
                  ))}
                </tbody>
                <tfoot>
                  <tr className="border-t-2 border-gray-300 bg-gray-100">
                    <td className="py-3 px-4 text-sm font-bold text-gray-800">TOTAL</td>
                    <td className="py-3 px-4 text-sm text-right font-bold text-green-700">
                      Q{monthlyData.reduce((sum, m) => sum + m.mantenimiento, 0).toLocaleString()}
                    </td>
                    <td className="py-3 px-4 text-sm text-right font-bold text-blue-700">
                      Q{monthlyData.reduce((sum, m) => sum + m.eventos, 0).toLocaleString()}
                    </td>
                    <td className="py-3 px-4 text-sm text-right font-bold text-gray-900">
                      Q{monthlyData.reduce((sum, m) => sum + m.total, 0).toLocaleString()}
                    </td>
                    <td className="py-3 px-4 text-sm text-right font-bold text-gray-900">
                      {((monthlyData.reduce((sum, m) => sum + m.total, 0) / (stats?.totalBudget || 1)) * 100).toFixed(2)}%
                    </td>
                  </tr>
                </tfoot>
              </table>
            </div>
          ) : (
            <div className="text-center py-8">
              <Icon name="bi bi-inbox" size={48} color="text-gray-400" />
              <p className="text-gray-500 mt-2">No hay datos mensuales disponibles</p>
            </div>
          )}
        </CardBody>
      </Card>

      {/* Top 10 Gastos Más Altos */}
      <Card>
        <CardBody className="p-6">
          <h2 className="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2">
            <Icon name="bi bi-trophy" size={20} color="text-yellow-600" />
            Top 10 - Gastos Más Altos (Aprobados)
          </h2>

          <div className="space-y-3">
            {(() => {
              const aprobadas = transactions.filter(t => {
                const isAprobada = t.statusName?.toLowerCase().includes('aprobada') || 
                                   t.statusName?.toLowerCase().includes('aprobado') ||
                                   t.statusName?.toLowerCase() === 'aprobada';
                return isAprobada;
              });

              if (aprobadas.length === 0) {
                return (
                  <div className="text-center py-8">
                    <Icon name="bi bi-inbox" size={48} color="text-gray-400" />
                    <p className="text-gray-500 mt-2">No hay solicitudes aprobadas aún</p>
                  </div>
                );
              }

              return aprobadas
                .sort((a, b) => (b.requestAmount || 0) - (a.requestAmount || 0))
                .slice(0, 10)
                .map((transaction, index) => (
                  <div 
                    key={transaction.id}
                    className="flex items-center gap-4 p-3 border border-gray-200 rounded-lg hover:bg-gray-50"
                  >
                    <div className={`w-8 h-8 rounded-full flex items-center justify-center font-bold text-white ${
                      index === 0 ? 'bg-yellow-500' :
                      index === 1 ? 'bg-gray-400' :
                      index === 2 ? 'bg-orange-600' :
                      'bg-gray-300 text-gray-700'
                    }`}>
                      {index + 1}
                    </div>
                    <div className={`w-8 h-8 rounded-lg flex items-center justify-center ${
                      transaction.originId === 1 ? 'bg-green-100' : 'bg-blue-100'
                    }`}>
                      <Icon 
                        name={transaction.originId === 1 ? 'bi bi-wrench' : 'bi bi-calendar-event'}
                        size={16}
                        color={transaction.originId === 1 ? 'text-green-600' : 'text-blue-600'}
                      />
                    </div>
                    <div className="flex-1">
                      <p className="font-semibold text-gray-800">{transaction.name || 'Sin nombre'}</p>
                      <p className="text-xs text-gray-500">{transaction.originName} - {transaction.priorityName}</p>
                    </div>
                    <div className="text-right">
                      <p className="text-lg font-bold text-blue-600">
                        Q{transaction.requestAmount?.toLocaleString()}
                      </p>
                      <p className="text-xs text-gray-500">
                        {transaction.requestDate ? parseDate(transaction.requestDate)?.toLocaleDateString('es-ES', {
                          day: 'numeric',
                          month: 'short'
                        }) : 'N/A'}
                      </p>
                    </div>
                  </div>
                ));
            })()}
          </div>
        </CardBody>
      </Card>
    </div>
  );
}
