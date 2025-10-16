import { api } from "../configs/axios/interceptors";
import type { ApiResponse } from "../types/ApiResponse";
import type {
  CreateSolicitudRequest,
  Solicitud,
  SolicitudFilters,
  UpdateSolicitudRequest,
} from "../types/Solicitud";

// GET all solicitudes with pagination
export const getAllSolicitudes = async (
  pageNumber: number = 1,
  pageSize: number = 100,
) => {
  const response = await api.get<unknown, ApiResponse<Solicitud[]>>(
    `/Request?PageNumber=${pageNumber}&PageSize=${pageSize}&IncludeTotal=true&Include=origin,priority,requestStatus`,
  );
  return response;
};

// GET solicitudes by tipo (Mantenimiento = OriginId 1, Evento = OriginId 2)
export const getSolicitudesByTipo = async (
  tipo: "Mantenimiento" | "Evento",
  pageNumber: number = 1,
  pageSize: number = 100,
) => {
  // Mapear tipo a OriginId
  // 1 = Mantenimiento, 2 = Eventos (ajusta estos IDs según tu base de datos)
  const originIdMap = {
    Mantenimiento: "1",
    Evento: "2",
  };

  const originId = originIdMap[tipo];
  const filters = `OriginId:eq:${originId}`;

  const response = await api.get<unknown, ApiResponse<Solicitud[]>>(
    `/Request?Filters=${encodeURIComponent(filters)}&Include=origin,priority,requestStatus&PageNumber=${pageNumber}&PageSize=${pageSize}&IncludeTotal=true`,
  );
  return response;
};

// GET solicitud by ID
export const getSolicitudById = async (id: number) => {
  const response = await api.get<unknown, ApiResponse<Solicitud>>(
    `/Request/${id}?Include=origin,priority,requestStatus`,
  );
  return response;
};

// POST create new solicitud
export const createSolicitud = async (data: CreateSolicitudRequest) => {
  const response = await api.post<
    unknown,
    ApiResponse<Solicitud>,
    CreateSolicitudRequest
  >("/Request", data);
  return response;
};

// PUT update solicitud
export const updateSolicitud = async (data: UpdateSolicitudRequest) => {
  const response = await api.put<
    unknown,
    ApiResponse<Solicitud>,
    UpdateSolicitudRequest
  >(`/Request/${data.id}`, data);
  return response;
};

// DELETE solicitud
export const deleteSolicitud = async (id: number) => {
  const response = await api.delete<unknown, ApiResponse<void>>(
    `/Request/${id}`,
  );
  return response;
};

// GET solicitudes with filters
export const getSolicitudesFiltered = async (
  filters: SolicitudFilters,
  pageNumber: number = 1,
  pageSize: number = 100,
) => {
  const filterParts: string[] = [];

  if (filters.estado) {
    filterParts.push(`RequestStatusId:eq:${filters.estado}`);
  }
  if (filters.tipo) {
    filterParts.push(`OriginId:eq:${filters.tipo}`);
  }

  const filterString = filterParts.length > 0 ? filterParts.join(" AND ") : "";

  const params = new URLSearchParams();
  if (filterString) params.append("Filters", filterString);
  params.append("Include", "origin,priority,requestStatus");
  params.append("PageNumber", pageNumber.toString());
  params.append("PageSize", pageSize.toString());
  params.append("IncludeTotal", "true");

  const response = await api.get<unknown, ApiResponse<Solicitud[]>>(
    `/Request?${params.toString()}`,
  );
  return response;
};

// Rechazar solicitud con razón y envío de correo
export const rechazarSolicitud = async (
  solicitudId: number,
  razonRechazo: string,
): Promise<ApiResponse<Solicitud>> => {
  try {
    console.log("🔄 Rechazando solicitud:", solicitudId);
    console.log("📝 Razón:", razonRechazo);

    // Primero obtenemos la solicitud completa
    const solicitudResponse = await api.get<unknown, ApiResponse<Solicitud>>(
      `/Request/${solicitudId}`,
      {
        params: {
          Include: "origin,priority,requestStatus",
        },
      },
    );

    if (
      !solicitudResponse?.success ||
      !solicitudResponse.data ||
      Array.isArray(solicitudResponse.data)
    ) {
      throw new Error("No se pudo obtener la solicitud");
    }

    const solicitud = solicitudResponse.data;

    // Convertir fecha al formato ISO si es necesario
    let requestDate = solicitud.requestDate;
    if (typeof requestDate === "string" && requestDate.includes("/")) {
      // Si la fecha viene en formato DD/MM/YYYY, convertir a YYYY-MM-DD
      const [day, month, year] = requestDate.split("/");
      requestDate = `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`;
    }

    // Preparamos el payload
    // NO incluimos createdBy ni updatedBy porque el backend no permite modificarlos
    const payload = {
      id: solicitud.id,
      originId: solicitud.originId,
      requestAmount: solicitud.requestAmount,
      name: solicitud.name,
      reason: solicitud.reason,
      requestDate: requestDate,
      email: solicitud.email,
      priorityId: solicitud.priorityId,
      requestStatusId: 3, // 3 = Rechazada
      comments: razonRechazo, // Guardamos la razón del rechazo
    };

    console.log("📤 Payload que se enviará:", payload);
    console.log("📤 Solicitud original:", solicitud);

    // Actualizamos el estado a Rechazada (RequestStatusId = 3) usando PATCH
    // Enviamos el objeto completo con todos los campos requeridos
    const updateResponse = await api.patch<unknown, ApiResponse<Solicitud>>(
      `/Request`,
      payload,
      {
        headers: {
          "Content-Type": "application/json",
        },
      },
    );

    console.log("✅ Respuesta del servidor:", updateResponse);

    // Si hay errores de validación, mostrarlos en detalle
    if (!updateResponse.success) {
      console.error(
        "❌ Error de validación:",
        JSON.stringify(updateResponse.data, null, 2),
      );
      console.error("❌ Mensaje:", updateResponse.message);
      if (Array.isArray(updateResponse.data)) {
        updateResponse.data.forEach((error, index) => {
          console.error(`❌ Error ${index + 1}:`, error);
        });
      }
      return updateResponse;
    }

    // TODO: Implementar envío de correo cuando el endpoint esté disponible
    // El backend aún no tiene el endpoint /Email/SendRejection
    /*
    try {
      await api.post<unknown, ApiResponse<void>, { to: string; subject: string; body: string }>(
        `/Email/SendRejection`,
        {
          to: email,
          subject: `Solicitud #${solicitudId} Rechazada`,
          body: razonRechazo
        }
      );
    } catch (emailError) {
      console.warn("⚠️ No se pudo enviar el correo de notificación:", emailError);
    }
    */

    return updateResponse;
  } catch (error) {
    console.error("Error al rechazar solicitud:", error);
    throw error;
  }
};

// Aprobar solicitud
export const aprobarSolicitud = async (solicitudId: number) => {
  try {
    console.log("🔄 Aprobando solicitud:", solicitudId);

    // Primero obtenemos la solicitud completa
    const solicitudResponse = await api.get<unknown, ApiResponse<Solicitud>>(
      `/Request/${solicitudId}`,
      {
        params: {
          Include: "origin,priority,requestStatus",
        },
      },
    );

    if (
      !solicitudResponse?.success ||
      !solicitudResponse.data ||
      Array.isArray(solicitudResponse.data)
    ) {
      throw new Error("No se pudo obtener la solicitud");
    }

    const solicitud = solicitudResponse.data;

    // Convertir fecha al formato ISO si es necesario
    let requestDate = solicitud.requestDate;
    if (typeof requestDate === "string" && requestDate.includes("/")) {
      // Si la fecha viene en formato DD/MM/YYYY, convertir a YYYY-MM-DD
      const [day, month, year] = requestDate.split("/");
      requestDate = `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`;
    }

    // Preparamos el payload
    // NO incluimos createdBy ni updatedBy porque el backend no permite modificarlos
    const payload = {
      id: solicitud.id,
      originId: solicitud.originId,
      requestAmount: solicitud.requestAmount,
      name: solicitud.name,
      reason: solicitud.reason,
      requestDate: requestDate,
      email: solicitud.email,
      priorityId: solicitud.priorityId,
      requestStatusId: 2, // 2 = Aprobada
      comments: solicitud.comments || "Solicitud aprobada", // El backend requiere comments no vacío
    };

    console.log("📤 Payload que se enviará:", payload);
    console.log("📤 Solicitud original:", solicitud);

    // Actualizamos el estado a Aprobada (RequestStatusId = 2) usando PATCH
    // Enviamos el objeto completo con todos los campos requeridos
    const response = await api.patch<unknown, ApiResponse<Solicitud>>(
      `/Request`,
      payload,
    );

    console.log("✅ Respuesta del servidor:", response);

    // Si hay errores de validación, mostrarlos en detalle
    if (!response.success) {
      console.error(
        "❌ Error de validación:",
        JSON.stringify(response.data, null, 2),
      );
      console.error("❌ Mensaje:", response.message);
      if (Array.isArray(response.data)) {
        response.data.forEach((error, index) => {
          console.error(`❌ Error ${index + 1}:`, error);
        });
      }
    }

    return response;
  } catch (error) {
    console.error("Error al aprobar solicitud:", error);
    throw error;
  }
};
