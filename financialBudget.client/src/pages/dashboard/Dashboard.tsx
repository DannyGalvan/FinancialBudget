import { useEffect, useState } from "react";
import { Link } from "react-router";
import { Icon } from "../../components/icons/Icon";
import { LoadingComponent } from "../../components/spinner/LoadingComponent";
import { BudgetChart } from "../../components/charts/BudgetChart";
import { getDashboardStats, getPendingSolicitudesStats, getTransactionHistory } from "../../services/budgetService";
import type { DashboardStats, PendingSolicitudesStats, TransactionHistory } from "../../types/Budget";

// Función para convertir fecha de DD/MM/YYYY a Date
const parseDate = (dateStr: string): Date | null => {
  if (!dateStr) return null;
  
  // Si ya es formato ISO (YYYY-MM-DD), usar directamente
  if (dateStr.includes('-') && dateStr.indexOf('-') === 4) {
    return new Date(dateStr);
  }
  
  // Si es formato DD/MM/YYYY, convertir
  if (dateStr.includes('/')) {
    const [day, month, year] = dateStr.split('/');
    return new Date(`${year}-${month}-${day}`);
  }
  
  return null;
};

export function Dashboard() {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [pendingStats, setPendingStats] = useState<PendingSolicitudesStats[]>([]);
  const [transactions, setTransactions] = useState<TransactionHistory[]>([]);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      
      console.log('🔄 Cargando datos del dashboard...');
      
      // Cargar todas las estadísticas en paralelo
      const [statsResponse, pendingResponse, transactionsResponse] = await Promise.all([
        getDashboardStats(),
        getPendingSolicitudesStats(),
        getTransactionHistory(10)
      ]);

      console.log('📊 Respuesta de getDashboardStats:', statsResponse);

      if (statsResponse.success && statsResponse.data) {
        console.log('✅ Stats recibidas:', statsResponse.data);
        setStats(statsResponse.data);
      } else {
        console.error('❌ Error obteniendo stats:', statsResponse.message);
      }

      if (pendingResponse.success && pendingResponse.data) {
        console.log('✅ Pending stats recibidas:', pendingResponse.data);
        setPendingStats(pendingResponse.data);
      }

      if (transactionsResponse.success && transactionsResponse.data) {
        console.log('✅ Transactions recibidas:', transactionsResponse.data);
        setTransactions(transactionsResponse.data);
      }
    } catch (error) {
      console.error("❌ Error al cargar datos del dashboard:", error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <LoadingComponent />;
  }

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-100 to-gray-50 p-6">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-4xl font-semibold text-neutral-700 mb-2">
          Resumen de Fondos
        </h1>
        <p className="text-gray-400 text-base">
          Obtener un resumen de las solicitudes pendientes mas recientes
        </p>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Fondos Section - Left Column (2/3 width) */}
        <div className="lg:col-span-2 space-y-6">
          {/* Fondos Card */}
          <div className="bg-white rounded-3xl shadow-lg p-6">
            <div className="flex justify-between items-start mb-6">
              <h2 className="text-xl font-semibold text-neutral-700">Fondos</h2>
              <div className="flex flex-col items-end">
                <div className="flex items-center gap-4 mb-2">
                  <div className="text-right">
                    <p className="text-xs text-gray-400">Usado</p>
                    <p className="text-lg font-bold text-blue-600">
                      Q{stats?.usedBudget.toLocaleString() || '0'}
                    </p>
                    <p className="text-xs text-gray-500">
                      {stats?.usagePercentage.toFixed(1) || '0'}% usado
                    </p>
                  </div>
                  <div className="text-right">
                    <p className="text-xs text-gray-400">Límite Total</p>
                    <p className="text-lg font-bold text-neutral-700">
                      Q{stats?.totalBudget.toLocaleString() || '0'}
                    </p>
                    <p className="text-xs text-gray-500">
                      Q{stats?.availableBudget.toLocaleString() || '0'} disponible
                    </p>
                  </div>
                </div>
                <div className="w-80 bg-gray-200 rounded-full h-3 relative">
                  <div 
                    className="bg-gradient-to-r from-blue-600 to-blue-400 h-3 rounded-full transition-all duration-500 flex items-center justify-end pr-2" 
                    style={{ width: `${Math.min(stats?.usagePercentage || 0, 100)}%` }}
                  >
                    {(stats?.usagePercentage || 0) > 10 && (
                      <span className="text-[10px] font-bold text-white">
                        {stats?.usagePercentage.toFixed(1)}%
                      </span>
                    )}
                  </div>
                  {(stats?.usagePercentage || 0) <= 10 && (stats?.usagePercentage || 0) > 0 && (
                    <span className="absolute left-2 top-0.5 text-[10px] font-bold text-gray-600">
                      {stats?.usagePercentage.toFixed(1)}%
                    </span>
                  )}
                </div>
              </div>
            </div>
            
            {/* Chart */}
            <div className="h-64 bg-white rounded-2xl flex items-center justify-center border border-gray-100">
              {stats ? (
                <BudgetChart 
                  lastMonthUsage={stats.lastMonthUsage}
                  currentMonthUsage={stats.currentMonthUsage}
                  totalBudget={stats.totalBudget}
                />
              ) : (
                <div className="text-center">
                  <Icon name="bi bi-bar-chart-line" size={48} color="#197BBD" />
                  <p className="text-gray-400 mt-4">Cargando gráfico...</p>
                </div>
              )}
            </div>
            
            <div className="flex justify-center gap-6 mt-4">
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-blue-500"></div>
                <span className="text-xs text-gray-600">Este Mes</span>
              </div>
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-purple-300"></div>
                <span className="text-xs text-gray-600">Mes Pasado</span>
              </div>
            </div>
          </div>

          {/* Historial De Transacciones Recientes */}
          <div className="bg-white rounded-3xl shadow-lg p-6">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-xl font-semibold text-neutral-700">
                Historial De Transacciones Recientes
              </h2>
              <Link to="/solicitudes/historial" className="cursor-pointer hover:scale-110 transition-transform">
                <Icon name="bi bi-arrow-right" size={20} color="#404040" />
              </Link>
            </div>
            
            {/* Table Headers */}
            <div className="grid grid-cols-5 gap-4 text-sm text-gray-400 mb-4 pb-2">
              <div>Solicitud</div>
              <div>Area</div>
              <div>Date</div>
              <div>Estado</div>
              <div className="text-right">Monto</div>
            </div>
            
            {/* Table Rows */}
            <div className="space-y-4">
              {transactions.length === 0 ? (
                <div className="text-center py-8">
                  <Icon name="bi bi-inbox" size={32} color="text-gray-300" />
                  <p className="text-gray-400 mt-2 text-sm">No hay transacciones recientes</p>
                </div>
              ) : (
                transactions.map((transaction, index) => {
                  const isApproved = transaction.statusName === 'Aprobada';
                  const isRejected = transaction.statusName === 'Rechazada';
                  
                  // Determinar icono según el área (OriginId)
                  // 1 = Mantenimiento, 2 = Eventos
                  const iconName = transaction.originId === 1 
                    ? 'bi bi-wrench' 
                    : 'bi bi-calendar-event';
                  const iconBgColor = transaction.originId === 1 
                    ? 'bg-green-50' 
                    : 'bg-blue-100';
                  const iconColor = transaction.originId === 1 
                    ? '#2BC255' 
                    : '#70A6E8';
                  
                  return (
                    <div 
                      key={transaction.id} 
                      className={`grid grid-cols-5 gap-4 items-center py-3 ${
                        index < transactions.length - 1 ? 'border-b border-gray-100' : ''
                      }`}
                    >
                      <div className="flex items-center gap-3">
                        <div className={`w-8 h-8 rounded-lg ${iconBgColor} flex items-center justify-center`}>
                          <Icon 
                            name={iconName} 
                            size={16} 
                            color={iconColor}
                          />
                        </div>
                        <span className="text-sm text-neutral-700 font-medium">
                          {transaction.name || 'Sin nombre'}
                        </span>
                      </div>
                      <span className="text-sm text-gray-500">{transaction.originName || 'N/A'}</span>
                      <span className="text-sm text-gray-500">
                        {transaction.requestDate 
                          ? (() => {
                              const date = parseDate(transaction.requestDate);
                              return date ? date.toLocaleDateString('es-ES', { 
                                day: 'numeric', 
                                month: 'short', 
                                year: 'numeric' 
                              }) : 'N/A';
                            })()
                          : 'N/A'}
                      </span>
                      <div>
                        <span className={`inline-flex items-center px-3 py-1 rounded-lg text-xs font-semibold ${
                          isApproved 
                            ? 'bg-green-500 text-white' 
                            : isRejected
                            ? 'bg-red-500 text-white'
                            : 'bg-gray-400 text-white'
                        }`}>
                          {transaction.statusName || 'N/A'}
                        </span>
                      </div>
                      <span className="text-sm font-bold text-right text-neutral-700">
                        Q{transaction.requestAmount?.toLocaleString() || '0.00'}
                      </span>
                    </div>
                  );
                })
              )}
            </div>
          </div>
        </div>

        {/* Right Column - Usado y Solicitudes Pendientes */}
        <div className="space-y-6">
          {/* Usado Section */}
          <div>
            <div className="flex items-center gap-2 mb-4">
              <h2 className="text-xl font-semibold text-neutral-700">Usado</h2>
              <Icon name="bi bi-plus-circle" size={20} color="#197BBD" />
            </div>
            
            <div className="grid grid-cols-1 gap-4">
              {pendingStats.length === 0 ? (
                <div className="bg-white rounded-3xl shadow-md p-5 text-center">
                  <p className="text-sm text-gray-400">No hay datos disponibles</p>
                </div>
              ) : (
                pendingStats.map((stat) => (
                  <div key={stat.originId} className="bg-white rounded-3xl shadow-md p-5">
                    <p className="text-2xl font-semibold text-gray-600 mb-1">
                      Q {stat.totalUsed.toLocaleString()}
                    </p>
                    <p className="text-sm text-gray-400 mb-2">
                      {new Date().toLocaleDateString('es-ES')}
                    </p>
                    <p className="text-lg text-neutral-700">{stat.originName}</p>
                  </div>
                ))
              )}
            </div>
          </div>

          {/* Solicitudes Pendientes */}
          <div>
            <h2 className="text-xl font-semibold text-neutral-700 mb-4">
              Solicitudes Pendientes
            </h2>
            
            <div className="space-y-4">
              {pendingStats.length === 0 ? (
                <div className="text-center py-8">
                  <Icon name="bi bi-inbox" size={32} color="text-gray-300" />
                  <p className="text-gray-400 mt-2 text-sm">No hay solicitudes pendientes</p>
                </div>
              ) : (
                pendingStats.map((stat) => {
                  const totalPending = pendingStats.reduce((sum, s) => sum + s.totalPending, 0);
                  const percentage = totalPending > 0 ? (stat.totalPending / totalPending) * 100 : 0;
                  const iconName = stat.originId === 1 ? "bi bi-wrench" : "bi bi-calendar-event";
                  const iconColor = stat.originId === 1 ? "#2BC255" : "#70A6E8";
                  const bgColor = stat.originId === 1 ? "bg-green-50" : "bg-blue-50";
                  const barColor = stat.originId === 1 
                    ? "bg-gradient-to-r from-green-600 to-green-400" 
                    : "bg-blue-400";

                  return (
                    <div key={stat.originId} className="flex items-center gap-4">
                      <div className={`w-11 h-11 ${bgColor} rounded-sm shadow-md flex items-center justify-center`}>
                        <Icon name={iconName} size={20} color={iconColor} />
                      </div>
                      <div className="flex-1">
                        <p className="text-sm text-gray-400 mb-1">{stat.originName}</p>
                        <div className="w-full bg-gray-200 rounded-full h-2.5">
                          <div 
                            className={`${barColor} h-2.5 rounded-full`} 
                            style={{ width: `${percentage}%` }}
                          ></div>
                        </div>
                      </div>
                      <span className="text-2xl font-semibold text-gray-600">
                        {stat.totalPending}
                      </span>
                    </div>
                  );
                })
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
