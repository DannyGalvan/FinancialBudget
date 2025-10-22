import { Button } from "@heroui/button";
import { Card, CardBody } from "@heroui/card";
import { Textarea } from "@heroui/input";
import {
  Modal,
  ModalBody,
  ModalContent,
  ModalFooter,
  ModalHeader,
} from "@heroui/modal";
import { useCallback, useEffect, useState } from "react";
import { Icon } from "../../components/icons/Icon";
import { Response } from "../../components/messages/Response";
import { LoadingComponent } from "../../components/spinner/LoadingComponent";
import { useResponse } from "../../hooks/useResponse";
import {
  aprobarSolicitud,
  getSolicitudesFiltered,
  rechazarSolicitud,
} from "../../services/solicitudService";
import type { Solicitud } from "../../types/Solicitud";

const getEstadoColor = (estado: string) => {
  switch (estado) {
    case "Completada":
      return "text-green-600";
    case "Rechazada":
      return "text-red-600";
    case "Pendiente":
      return "text-yellow-600";
    case "Nueva":
      return "text-blue-600";
    default:
      return "text-gray-600";
  }
};

// Función para convertir fecha de DD/MM/YYYY a Date
const parseDate = (dateStr: string): Date | null => {
  if (!dateStr) return null;

  // Si ya es formato ISO (YYYY-MM-DD), usar directamente
  if (dateStr.includes("-") && dateStr.indexOf("-") === 4) {
    return new Date(dateStr);
  }

  // Si es formato DD/MM/YYYY, convertir
  if (dateStr.includes("/")) {
    const [day, month, year] = dateStr.split("/");
    return new Date(`${year}-${month}-${day}`);
  }

  return null;
};

const formatDate = (dateStr: string): string => {
  const date = parseDate(dateStr);
  return date ? date.toLocaleDateString("es-ES") : "N/A";
};

export function Component() {
  const [solicitudes, setSolicitudes] = useState<Solicitud[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedSolicitud, setSelectedSolicitud] = useState<Solicitud | null>(
    null,
  );
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isRejectModalOpen, setIsRejectModalOpen] = useState(false);
  const [razonRechazo, setRazonRechazo] = useState("");
  const { success, apiMessage, handleApiResponse } = useResponse<Solicitud[]>();

  const loadSolicitudes = useCallback(async () => {
    try {
      setLoading(true);

      // Filtrar por: OriginId = 2 (Eventos) Y RequestStatusId = 1 (Pendiente)
      const response = await getSolicitudesFiltered(
        {
          tipo: "2", // 2 = Eventos
          estado: "1", // 1 = Pendiente
        },
        1,
        100,
      );

      handleApiResponse(response);

      if (response.success && response.data) {
        setSolicitudes(response.data);
      }
    } catch (error) {
      console.error("Error loading solicitudes:", error);
    } finally {
      setLoading(false);
    }
  }, [handleApiResponse]);

  useEffect(() => {
    loadSolicitudes();
  }, []);

  const handleRechazar = async () => {
    if (!selectedSolicitud || !razonRechazo.trim()) return;

    try {
      setLoading(true);

      const response = await rechazarSolicitud(
        selectedSolicitud.id,
        razonRechazo,
      );

      // Normalize response for the generic handler — cast to any to satisfy ApiResponse<Solicitud[]>
      handleApiResponse(response as any);

      if (response.success) {
        setIsRejectModalOpen(false);
        setRazonRechazo("");
        setSelectedSolicitud(null);
        await loadSolicitudes();
      }
    } catch (error) {
      console.error("Error al rechazar solicitud:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleAprobar = async () => {
    if (!selectedSolicitud) return;

    try {
      setLoading(true);

      const response = await aprobarSolicitud(selectedSolicitud.id);
      handleApiResponse(response as any);

      if (response.success) {
        setIsModalOpen(false);
        setSelectedSolicitud(null);
        await loadSolicitudes();
      }
    } catch (error) {
      console.error("Error al aprobar solicitud:", error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <LoadingComponent />;
  }

  return (
    <div className="bg-gradient-to-b from-gray-100 to-gray-50 p-6">
      <div className="max-w-7xl mx-auto">
        {/* Response Message */}
        {success !== null && (
          <div className="mb-4">
            <Response message={apiMessage} type={success} />
          </div>
        )}

        {/* Header */}
        <div className="mb-8">
          <p className="text-gray-400 text-sm mb-2">Eventos Comunitarios</p>
          <h1 className="text-4xl font-semibold text-gray-700">
            Gestion de Eventos Comunitarios
          </h1>
        </div>

        {/* Main Card */}
        <Card className="shadow-xl">
          <CardBody className="p-8">
            <div className="mb-6 flex justify-between items-center">
              <h2 className="text-xl font-semibold text-gray-700">
                Solicitudes de Eventos
              </h2>
              <Button
                color="primary"
                size="md"
                variant="shadow"
                onPress={loadSolicitudes}
              >
                <Icon name="bi bi-arrow-clockwise" size={16} />
                Actualizar
              </Button>
            </div>

            {solicitudes.length === 0 ? (
              <div className="text-center py-12">
                <Icon color="text-gray-400" name="bi bi-inbox" size={48} />
                <p className="text-gray-500 mt-4">
                  No hay solicitudes de eventos disponibles
                </p>
              </div>
            ) : (
              <>
                {/* Table Header */}
                <div className="grid grid-cols-7 gap-4 pb-4 border-b border-gray-200 text-sm text-gray-400">
                  <div>Descripción</div>
                  <div>Razón</div>
                  <div>Numero</div>
                  <div>Fecha</div>
                  <div>Prioridad</div>
                  <div>Monto</div>
                  <div className="text-right">Estado</div>
                </div>

                {/* Table Rows */}
                <div className="space-y-4 mt-4">
                  {solicitudes.map((solicitud) => (
                    <div
                      key={solicitud.id}
                      className="grid grid-cols-7 gap-4 items-center py-4 border-b border-gray-100 hover:bg-gray-50 transition-colors cursor-pointer"
                      data-testid={`solicitud-row-${solicitud.id}`}
                      onClick={() => {
                        setSelectedSolicitud(solicitud);
                        setIsModalOpen(true);
                      }}
                    >
                      <div className="flex items-center gap-3">
                        <div className="w-15 h-15 bg-gray-100  flex items-center justify-center">
                          <Icon
                            color="text-gray-400"
                            name="bi bi-calendar-event"
                            size={18}
                          />
                        </div>
                        <span className="text-sm text-gray-700">
                          {solicitud.name || "Sin descripción"}
                        </span>
                      </div>
                      <div className="text-sm text-gray-600">
                        {solicitud.reason || "N/A"}
                      </div>
                      <div className="text-sm text-gray-400">
                        {solicitud.requestNumber || solicitud.id}
                      </div>
                      <div className="text-sm text-gray-400">
                        {formatDate(solicitud.requestDate || "")}
                      </div>
                      <div className="text-sm text-gray-600 font-medium">
                        {solicitud.priority?.name || "N/A"}
                      </div>
                      <div className="text-sm font-semibold text-gray-700">
                        Q{solicitud.requestAmount?.toLocaleString() || "0.00"}
                      </div>
                      <div
                        className={`text-sm font-bold text-right ${getEstadoColor(solicitud.requestStatus?.name || "Pendiente")}`}
                      >
                        {solicitud.requestStatus?.name || "Pendiente"}
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}
          </CardBody>
        </Card>
      </div>

      {/* Modal de Detalle */}
      <Modal
        isOpen={isModalOpen}
        scrollBehavior="inside"
        size="2xl"
        onClose={() => setIsModalOpen(false)}
      >
        <ModalContent>
          {(onClose) => (
            <>
              <ModalHeader className="flex flex-col gap-1 border-b">
                <div className="flex items-center gap-3">
                  <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center">
                    <Icon
                      color="text-purple-600"
                      name="bi bi-calendar-event"
                      size={24}
                    />
                  </div>
                  <div>
                    <h3 className="text-xl font-bold text-gray-800">
                      Detalle de Solicitud de Evento
                    </h3>
                    <p className="text-sm text-gray-500">
                      #
                      {selectedSolicitud?.requestNumber ||
                        selectedSolicitud?.id}
                    </p>
                  </div>
                </div>
              </ModalHeader>
              <ModalBody className="py-6">
                {selectedSolicitud ? (
                  <div className="space-y-6">
                    {/* Información Principal */}
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label className="text-sm font-semibold text-gray-600">
                          Descripción del Evento
                        </label>
                        <p className="text-gray-800 mt-1">
                          {selectedSolicitud.name || "N/A"}
                        </p>
                      </div>
                      <div>
                        <label className="text-sm font-semibold text-gray-600">
                          Estado
                        </label>
                        <p
                          className={`mt-1 font-bold ${getEstadoColor(selectedSolicitud.requestStatus?.name || "Pendiente")}`}
                        >
                          {selectedSolicitud.requestStatus?.name || "Pendiente"}
                        </p>
                      </div>
                    </div>

                    {/* Razón */}
                    <div>
                      <label className="text-sm font-semibold text-gray-600">
                        Razón / Motivo
                      </label>
                      <p className="text-gray-800 mt-1">
                        {selectedSolicitud.reason || "N/A"}
                      </p>
                    </div>

                    {/* Detalles Financieros */}
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label className="text-sm font-semibold text-gray-600">
                          Monto Solicitado
                        </label>
                        <p className="text-2xl font-bold text-purple-600 mt-1">
                          Q
                          {selectedSolicitud.requestAmount?.toLocaleString() ||
                            "0.00"}
                        </p>
                      </div>
                      <div>
                        <label className="text-sm font-semibold text-gray-600">
                          Fecha de Solicitud
                        </label>
                        <p className="text-gray-800 mt-1">
                          {selectedSolicitud.requestDate
                            ? (() => {
                                const date = parseDate(
                                  selectedSolicitud.requestDate,
                                );
                                return date
                                  ? date.toLocaleDateString("es-ES", {
                                      year: "numeric",
                                      month: "long",
                                      day: "numeric",
                                    })
                                  : "N/A";
                              })()
                            : "N/A"}
                        </p>
                      </div>
                    </div>

                    {/* Información Adicional */}
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label className="text-sm font-semibold text-gray-600">
                          Origen
                        </label>
                        <p className="text-gray-800 mt-1">
                          {selectedSolicitud.origin?.name || "N/A"}
                        </p>
                      </div>
                      <div>
                        <label className="text-sm font-semibold text-gray-600">
                          Prioridad
                        </label>
                        <p className="text-gray-800 mt-1">
                          {selectedSolicitud.priority?.name || "N/A"}
                        </p>
                      </div>
                    </div>
                  </div>
                ) : null}
              </ModalBody>
              <ModalFooter className="border-t">
                <Button color="default" variant="light" onPress={onClose}>
                  Cerrar
                </Button>
                {/* Solo mostrar botones de Aprobar/Rechazar si está Pendiente */}
                {selectedSolicitud?.requestStatus?.name === "Pendiente" && (
                  <>
                    <Button
                      color="danger"
                      startContent={<Icon name="bi bi-x-circle" size={18} />}
                      variant="flat"
                      onPress={() => {
                        setIsModalOpen(false);
                        setIsRejectModalOpen(true);
                      }}
                    >
                      Rechazar
                    </Button>
                    <Button
                      color="success"
                      startContent={
                        <Icon name="bi bi-check-circle" size={18} />
                      }
                      variant="shadow"
                      onPress={handleAprobar}
                    >
                      Aprobar
                    </Button>
                  </>
                )}
                {/* Mostrar mensaje si ya está aprobada o rechazada */}
                {selectedSolicitud?.requestStatus?.name === "Aprobada" && (
                  <div className="flex items-center gap-2 text-green-600">
                    <Icon name="bi bi-check-circle-fill" size={18} />
                    <span className="text-sm font-medium">
                      Esta solicitud ya fue aprobada
                    </span>
                  </div>
                )}
                {selectedSolicitud?.requestStatus?.name === "Rechazada" && (
                  <div className="flex items-center gap-2 text-red-600">
                    <Icon name="bi bi-x-circle-fill" size={18} />
                    <span className="text-sm font-medium">
                      Esta solicitud ya fue rechazada
                    </span>
                  </div>
                )}
              </ModalFooter>
            </>
          )}
        </ModalContent>
      </Modal>

      {/* Modal de Rechazo */}
      <Modal
        isOpen={isRejectModalOpen}
        size="lg"
        onClose={() => {
          setIsRejectModalOpen(false);
          setRazonRechazo("");
        }}
      >
        <ModalContent>
          {(onClose) => (
            <>
              <ModalHeader className="flex flex-col gap-1 border-b">
                <div className="flex items-center gap-3">
                  <div className="w-12 h-12 bg-red-100 rounded-lg flex items-center justify-center">
                    <Icon
                      color="text-red-600"
                      name="bi bi-x-circle"
                      size={24}
                    />
                  </div>
                  <div>
                    <h3 className="text-xl font-bold text-gray-800">
                      Rechazar Solicitud de Evento
                    </h3>
                    <p className="text-sm text-gray-500">
                      #
                      {selectedSolicitud?.requestNumber ||
                        selectedSolicitud?.id}
                    </p>
                  </div>
                </div>
              </ModalHeader>
              <ModalBody className="py-6">
                <div className="space-y-4">
                  <div>
                    <p className="text-sm text-gray-600 mb-4">
                      Se enviará un correo electrónico a{" "}
                      <strong className="text-gray-800">
                        {selectedSolicitud?.email ||
                          selectedSolicitud?.contactEmail ||
                          "correo no disponible"}
                      </strong>{" "}
                      con la razón del rechazo.
                    </p>
                  </div>

                  <div>
                    <label className="text-sm font-semibold text-gray-700 mb-2 block">
                      Razón del Rechazo *
                    </label>
                    <Textarea
                      classNames={{
                        input: "resize-y",
                      }}
                      minRows={4}
                      placeholder="Ingrese la razón del rechazo de la solicitud de evento..."
                      value={razonRechazo}
                      variant="bordered"
                      onValueChange={setRazonRechazo}
                    />
                  </div>

                  <div className="bg-yellow-50 border-l-4 border-yellow-400 p-4 rounded">
                    <div className="flex items-start gap-3">
                      <Icon
                        color="text-yellow-600"
                        name="bi bi-exclamation-triangle"
                        size={20}
                      />
                      <div className="text-sm text-yellow-800">
                        <p className="font-semibold mb-1">Importante:</p>
                        <p>
                          Esta acción no se puede deshacer. El solicitante será
                          notificado por correo electrónico.
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </ModalBody>
              <ModalFooter className="border-t">
                <Button color="default" variant="light" onPress={onClose}>
                  Cancelar
                </Button>
                <Button
                  color="danger"
                  isDisabled={!razonRechazo.trim()}
                  startContent={<Icon name="bi bi-send" size={18} />}
                  variant="shadow"
                  onPress={handleRechazar}
                >
                  Enviar Rechazo
                </Button>
              </ModalFooter>
            </>
          )}
        </ModalContent>
      </Modal>
    </div>
  );
}

Component.displayName = "SolicitudEventos";
