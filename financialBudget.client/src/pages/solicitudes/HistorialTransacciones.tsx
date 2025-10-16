import { Card, CardBody } from "@heroui/card";
import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter } from "@heroui/modal";
import { Button } from "@heroui/button";
import { useEffect, useState } from "react";
import { Icon } from "../../components/icons/Icon";
import { getTransactionHistory } from "../../services/budgetService";
import type { TransactionHistory } from "../../types/Budget";
import { LoadingComponent } from "../../components/spinner/LoadingComponent";
import { getSolicitudById } from "../../services/solicitudService";
import type { Solicitud } from "../../types/Solicitud";

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

export function Component() {
  const [transactions, setTransactions] = useState<TransactionHistory[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState<Solicitud | null>(null);

  useEffect(() => {
    loadTransactions();
  }, []);

  const loadTransactions = async () => {
    try {
      setLoading(true);
      const response = await getTransactionHistory(1000); // Cargar todas
      
      if (response.success && response.data) {
        setTransactions(response.data);
      }
    } catch (error) {
      console.error("Error loading transactions:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDetail = async (transactionId: number) => {
    try {
      setLoading(true);
      const response = await getSolicitudById(transactionId);
      
      if (response.success && response.data) {
        setSelectedTransaction(response.data);
        setIsModalOpen(true);
      }
    } catch (error) {
      console.error("Error loading transaction detail:", error);
    } finally {
      setLoading(false);
    }
  };

  if (loading && transactions.length === 0) {
    return <LoadingComponent />;
  }

  return (
    <div className="p-6">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-800">Historial de Transacciones</h1>
        <p className="text-gray-500 mt-2">Todas las solicitudes aprobadas y rechazadas</p>
      </div>

      <Card className="shadow-lg">
        <CardBody className="p-6">
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-xl font-semibold text-neutral-700">
              Total: {transactions.length} transacciones
            </h2>
            <Button 
              color="primary" 
              variant="shadow" 
              size="md"
              onPress={loadTransactions}
            >
              <Icon name="bi bi-arrow-clockwise" size={16} />
              Actualizar
            </Button>
          </div>

          {transactions.length === 0 ? (
            <div className="text-center py-12">
              <Icon name="bi bi-inbox" size={48} color="text-gray-400" />
              <p className="text-gray-500 mt-4">No hay transacciones disponibles</p>
            </div>
          ) : (
            <>
              {/* Table Header */}
              <div className="grid grid-cols-6 gap-4 pb-4 border-b border-gray-200 text-sm text-gray-400 font-semibold">
                <div>Solicitud</div>
                <div>Area</div>
                <div>Fecha</div>
                <div>Prioridad</div>
                <div>Estado</div>
                <div className="text-right">Monto</div>
              </div>

              {/* Table Rows */}
              <div className="space-y-2 mt-4">
                {transactions.map((transaction) => {
                  const isApproved = transaction.statusName === 'Aprobada';
                  const isRejected = transaction.statusName === 'Rechazada';
                  
                  // Determinar icono según el área
                  const iconName = transaction.originId === 1 
                    ? 'bi bi-wrench' 
                    : 'bi bi-calendar-event';
                  const iconBgColor = transaction.originId === 1 
                    ? 'bg-green-100' 
                    : 'bg-blue-100';
                  const iconColor = transaction.originId === 1 
                    ? '#2BC255' 
                    : '#70A6E8';
                  
                  return (
                    <div 
                      key={transaction.id}
                      className="grid grid-cols-6 gap-4 items-center py-4 border-b border-gray-100 hover:bg-gray-50 transition-colors cursor-pointer rounded-lg px-2"
                      onClick={() => handleOpenDetail(transaction.id)}
                    >
                      <div className="flex items-center gap-3">
                        <div className={`w-10 h-10 rounded-lg ${iconBgColor} flex items-center justify-center`}>
                          <Icon 
                            name={iconName} 
                            size={18} 
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
                      <span className="text-sm text-gray-600 font-medium">
                        {transaction.priorityName || 'N/A'}
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
                })}
              </div>
            </>
          )}
        </CardBody>
      </Card>

      {/* Modal de Detalle */}
      <Modal 
        isOpen={isModalOpen} 
        onClose={() => setIsModalOpen(false)}
        size="2xl"
        scrollBehavior="inside"
      >
        <ModalContent>
          {(onClose) => (
            <>
              <ModalHeader className="flex flex-col gap-1 border-b">
                <div className="flex items-center gap-3">
                  <div className={`w-12 h-12 rounded-lg flex items-center justify-center ${
                    selectedTransaction?.requestStatus?.name === 'Aprobada' 
                      ? 'bg-green-100' 
                      : 'bg-red-100'
                  }`}>
                    <Icon 
                      name={
                        selectedTransaction?.requestStatus?.name === 'Aprobada' 
                          ? 'bi bi-check-circle' 
                          : 'bi bi-x-circle'
                      } 
                      size={24} 
                      color={
                        selectedTransaction?.requestStatus?.name === 'Aprobada' 
                          ? 'text-green-600' 
                          : 'text-red-600'
                      }
                    />
                  </div>
                  <div>
                    <h3 className="text-xl font-bold text-gray-800">
                      Detalle de Transacción
                    </h3>
                    <p className="text-sm text-gray-500">
                      #{selectedTransaction?.requestNumber || selectedTransaction?.id}
                    </p>
                  </div>
                </div>
              </ModalHeader>
              <ModalBody className="py-6">
                {selectedTransaction && (
                  <div className="space-y-6">
                    {/* Información Principal */}
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Descripción</label>
                        <p className="text-gray-800 mt-1">{selectedTransaction.name || 'N/A'}</p>
                      </div>
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Estado</label>
                        <p className={`mt-1 font-bold ${
                          selectedTransaction.requestStatus?.name === 'Aprobada' 
                            ? 'text-green-600' 
                            : 'text-red-600'
                        }`}>
                          {selectedTransaction.requestStatus?.name || 'N/A'}
                        </p>
                      </div>
                    </div>

                    {/* Razón Original */}
                    <div>
                      <label className="text-sm font-semibold text-gray-600">Razón de la Solicitud</label>
                      <p className="text-gray-800 mt-1">{selectedTransaction.reason || 'N/A'}</p>
                    </div>

                    {/* Comentarios (Razón de Rechazo o Aprobación) */}
                    {selectedTransaction.comments && (
                      <div className={`p-4 rounded-lg ${
                        selectedTransaction.requestStatus?.name === 'Aprobada' 
                          ? 'bg-green-50 border-l-4 border-green-500' 
                          : 'bg-red-50 border-l-4 border-red-500'
                      }`}>
                        <label className={`text-sm font-semibold ${
                          selectedTransaction.requestStatus?.name === 'Aprobada' 
                            ? 'text-green-700' 
                            : 'text-red-700'
                        }`}>
                          {selectedTransaction.requestStatus?.name === 'Aprobada' 
                            ? 'Comentarios de Aprobación' 
                            : 'Razón del Rechazo'}
                        </label>
                        <p className={`mt-2 ${
                          selectedTransaction.requestStatus?.name === 'Aprobada' 
                            ? 'text-green-800' 
                            : 'text-red-800'
                        }`}>
                          {selectedTransaction.comments}
                        </p>
                      </div>
                    )}

                    {/* Detalles Financieros */}
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Monto</label>
                        <p className="text-2xl font-bold text-blue-600 mt-1">
                          Q{selectedTransaction.requestAmount?.toLocaleString() || '0.00'}
                        </p>
                      </div>
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Fecha de Solicitud</label>
                        <p className="text-gray-800 mt-1">
                          {selectedTransaction.requestDate 
                            ? (() => {
                                const date = parseDate(selectedTransaction.requestDate);
                                return date ? date.toLocaleDateString('es-ES', {
                                  year: 'numeric',
                                  month: 'long',
                                  day: 'numeric'
                                }) : 'N/A';
                              })()
                            : 'N/A'}
                        </p>
                      </div>
                    </div>

                    {/* Información Adicional */}
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Origen</label>
                        <p className="text-gray-800 mt-1">{selectedTransaction.origin?.name || 'N/A'}</p>
                      </div>
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Prioridad</label>
                        <p className="text-gray-800 mt-1">{selectedTransaction.priority?.name || 'N/A'}</p>
                      </div>
                    </div>

                    {/* Email */}
                    {selectedTransaction.email && (
                      <div>
                        <label className="text-sm font-semibold text-gray-600">Email de Contacto</label>
                        <p className="text-gray-800 mt-1">{selectedTransaction.email}</p>
                      </div>
                    )}
                  </div>
                )}
              </ModalBody>
              <ModalFooter className="border-t">
                <Button color="default" variant="shadow" onPress={onClose}>
                  Cerrar
                </Button>
              </ModalFooter>
            </>
          )}
        </ModalContent>
      </Modal>
    </div>
  );
}

Component.displayName = "HistorialTransacciones";
