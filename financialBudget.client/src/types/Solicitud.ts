export interface Solicitud {
  id: number;
  name?: string;
  reason?: string;
  requestNumber?: string;
  requestDescription?: string;
  requestDate?: string;
  requestAmount?: number;
  originId?: number;
  priorityId?: number;
  requestStatusId?: number;
  email?: string;
  contactEmail?: string;
  approvedDate?: string;
  rejectionReason?: string;
  authorizedReason?: string;
  comments?: string;
  state?: number;
  createdBy?: number;
  updatedBy?: number;
  createdAt?: string;
  updatedAt?: string;
  
  // Relaciones incluidas
  origin?: {
    id: number;
    name: string;
  };
  priority?: {
    id: number;
    name: string;
  };
  requestStatus?: {
    id: number;
    name: string;
  };
  
  // Campos computados para compatibilidad con la UI
  descripcion?: string;
  numero?: string;
  fecha?: string;
  estado?: "Pendiente" | "Completada" | "Rechazada" | "Nueva";
  tipo?: "Mantenimiento" | "Evento";
  prioridad?: string;
  monto?: number;
}

export interface CreateSolicitudRequest {
  requestDescription: string;
  originId: number;
  priorityId?: number;
  requestAmount?: number;
}

export interface UpdateSolicitudRequest {
  id: number;
  requestDescription?: string;
  requestStatusId?: number;
  priorityId?: number;
  requestAmount?: number;
}

export interface SolicitudFilters {
  estado?: string;
  tipo?: string;
  fechaInicio?: string;
  fechaFin?: string;
}

export interface RechazarSolicitudRequest {
  solicitudId: number;
  razonRechazo: string;
  email: string;
}
