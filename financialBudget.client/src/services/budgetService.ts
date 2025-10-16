import { api } from "../configs/axios/interceptors";
import type { ApiResponse } from "../types/ApiResponse";
import type { Budget, DashboardStats, PendingSolicitudesStats, TransactionHistory } from "../types/Budget";

/**
 * Obtiene el presupuesto actual
 */
export const getCurrentBudget = async () => {
  try {
    console.log('🔄 Obteniendo presupuesto actual desde /Budget...');
    const response = await api.get<unknown, ApiResponse<Budget[]>>(
      `/Budget?PageNumber=1&PageSize=1&IncludeTotal=true`
    );
    console.log('📦 Respuesta de /Budget:', response);
    if (response.success && response.data && response.data.length > 0 && !Array.isArray(response.data[0])) {
      const budget = response.data[0] as Budget;
      console.log('💰 Presupuesto obtenido:', {
        authorizedAmount: budget.authorizedAmount,  // Total autorizado
        committedAmount: budget.committedAmount,    // Usado
        availableAmount: budget.availableAmount,    // Disponible
        period: budget.period
      });
    }
    return response;
  } catch (error) {
    console.error("❌ Error al obtener presupuesto:", error);
    throw error;
  }
};

/**
 * Obtiene las estadísticas del dashboard
 * Incluye uso del mes pasado y mes actual
 */
export const getDashboardStats = async (): Promise<ApiResponse<DashboardStats>> => {
  try {
    console.log('📊 Iniciando obtención de estadísticas del dashboard...');
    
    // Obtener el presupuesto actual
    const budgetResponse = await getCurrentBudget();
    
    if (!budgetResponse.success || !budgetResponse.data || budgetResponse.data.length === 0) {
      console.error('❌ No se pudo obtener el presupuesto o está vacío');
      return {
        success: false,
        message: "No se pudo obtener el presupuesto",
        data: null,
        totalResults: 0
      };
    }

    const budget = budgetResponse.data[0] as Budget;
    console.log('✅ Budget obtenido correctamente:', budget);
    
    // Calcular fechas del mes pasado y actual
    const now = new Date();
    const currentMonthStart = new Date(now.getFullYear(), now.getMonth(), 1);
    const lastMonthStart = new Date(now.getFullYear(), now.getMonth() - 1, 1);
    const lastMonthEnd = new Date(now.getFullYear(), now.getMonth(), 0);
    
    console.log('📅 Fechas calculadas:', {
      currentMonthStart: currentMonthStart.toISOString(),
      lastMonthStart: lastMonthStart.toISOString(),
      lastMonthEnd: lastMonthEnd.toISOString()
    });
    
    // Obtener solicitudes aprobadas del mes pasado
    const lastMonthFilters = `RequestStatusId:eq:2 AND RequestDate:gte:${lastMonthStart.toISOString()} AND RequestDate:lte:${lastMonthEnd.toISOString()}`;
    const lastMonthResponse = await api.get<unknown, ApiResponse<any[]>>(
      `/Request?Filters=${encodeURIComponent(lastMonthFilters)}&Include=requestStatus&PageNumber=1&PageSize=1000&IncludeTotal=true`
    );
    
    // Obtener solicitudes aprobadas del mes actual
    const currentMonthFilters = `RequestStatusId:eq:2 AND RequestDate:gte:${currentMonthStart.toISOString()}`;
    const currentMonthResponse = await api.get<unknown, ApiResponse<any[]>>(
      `/Request?Filters=${encodeURIComponent(currentMonthFilters)}&Include=requestStatus&PageNumber=1&PageSize=1000&IncludeTotal=true`
    );
    
    const lastMonthUsage = lastMonthResponse.data?.reduce((sum, req) => sum + (req.requestAmount || 0), 0) || 0;
    const currentMonthUsage = currentMonthResponse.data?.reduce((sum, req) => sum + (req.requestAmount || 0), 0) || 0;
    
    console.log('📈 Uso calculado:', {
      lastMonthUsage,
      currentMonthUsage
    });
    
    // Calcular el total usado: sumar TODAS las solicitudes aprobadas (sin filtro de fecha)
    const allApprovedFilters = `RequestStatusId:eq:2`; // Solo aprobadas
    const allApprovedResponse = await api.get<unknown, ApiResponse<any[]>>(
      `/Request?Filters=${encodeURIComponent(allApprovedFilters)}&Include=requestStatus&PageNumber=1&PageSize=10000&IncludeTotal=true`
    );
    
    const totalUsedFromRequests = allApprovedResponse.data?.reduce((sum, req) => sum + (req.requestAmount || 0), 0) || 0;
    
    console.log('💰 Total usado calculado desde solicitudes aprobadas:', totalUsedFromRequests);
    console.log('📊 Total de solicitudes aprobadas:', allApprovedResponse.data?.length || 0);
    
    // Usar los campos correctos del backend
    const totalBudget = budget.authorizedAmount || 0;      // Total autorizado
    const usedBudget = totalUsedFromRequests;              // Calcular desde solicitudes aprobadas (no usar committedAmount)
    const availableBudget = totalBudget - usedBudget;      // Calcular disponible
    
    const stats: DashboardStats = {
      totalBudget: totalBudget,
      usedBudget: usedBudget,
      availableBudget: availableBudget,
      usagePercentage: totalBudget > 0 ? (usedBudget / totalBudget) * 100 : 0,
      lastMonthUsage,
      currentMonthUsage
    };

    console.log('✅ Estadísticas finales del dashboard:', stats);

    return {
      success: true,
      message: "Estadísticas obtenidas exitosamente",
      data: stats,
      totalResults: 1
    };
  } catch (error) {
    console.error("Error al obtener estadísticas del dashboard:", error);
    return {
      success: false,
      message: "Error al obtener estadísticas",
      data: null,
      totalResults: 0
    };
  }
};

/**
 * Obtiene las estadísticas de solicitudes pendientes por departamento
 */
export const getPendingSolicitudesStats = async (): Promise<ApiResponse<PendingSolicitudesStats[]>> => {
  try {
    // Obtener todas las solicitudes pendientes (RequestStatusId = 1 para Pendiente)
    const pendingFilters = `RequestStatusId:eq:1`;
    const pendingResponse = await api.get<unknown, ApiResponse<any[]>>(
      `/Request?Filters=${encodeURIComponent(pendingFilters)}&Include=origin,requestStatus&PageNumber=1&PageSize=1000&IncludeTotal=true`
    );

    // Obtener todas las solicitudes aprobadas (RequestStatusId = 2 para Aprobada)
    const approvedFilters = `RequestStatusId:eq:2`;
    const approvedResponse = await api.get<unknown, ApiResponse<any[]>>(
      `/Request?Filters=${encodeURIComponent(approvedFilters)}&Include=origin,requestStatus&PageNumber=1&PageSize=1000&IncludeTotal=true`
    );

    if (!pendingResponse.success || !pendingResponse.data) {
      return {
        success: false,
        message: "No se pudieron obtener las solicitudes pendientes",
        data: null,
        totalResults: 0
      };
    }

    // Agrupar por origin (departamento)
    const statsMap = new Map<number, PendingSolicitudesStats>();
    
    // Primero, contar las pendientes
    pendingResponse.data.forEach(solicitud => {
      const originId = solicitud.originId || 0;
      const originName = solicitud.origin?.name || 'Desconocido';

      if (statsMap.has(originId)) {
        const existing = statsMap.get(originId)!;
        existing.totalPending += 1;
      } else {
        statsMap.set(originId, {
          originId,
          originName,
          totalPending: 1,
          totalUsed: 0 // Inicializar en 0
        });
      }
    });

    // Luego, sumar el monto de las aprobadas (USADO)
    if (approvedResponse.success && approvedResponse.data) {
      approvedResponse.data.forEach(solicitud => {
        const originId = solicitud.originId || 0;
        const originName = solicitud.origin?.name || 'Desconocido';
        const amount = solicitud.requestAmount || 0;

        if (statsMap.has(originId)) {
          const existing = statsMap.get(originId)!;
          existing.totalUsed += amount;
        } else {
          // Si no hay pendientes pero sí aprobadas, crear entrada
          statsMap.set(originId, {
            originId,
            originName,
            totalPending: 0,
            totalUsed: amount
          });
        }
      });
    }

    const stats = Array.from(statsMap.values());

    return {
      success: true,
      message: "Estadísticas de solicitudes pendientes obtenidas exitosamente",
      data: stats,
      totalResults: stats.length
    };
  } catch (error) {
    console.error("Error al obtener estadísticas de solicitudes pendientes:", error);
    return {
      success: false,
      message: "Error al obtener estadísticas",
      data: null,
      totalResults: 0
    };
  }
};

/**
 * Obtiene el historial de transacciones (solicitudes aprobadas o rechazadas)
 */
export const getTransactionHistory = async (limit: number = 10): Promise<ApiResponse<TransactionHistory[]>> => {
  try {
    // Obtener solicitudes aprobadas (2) o rechazadas (3)
    const filters = `RequestStatusId:eq:2 OR RequestStatusId:eq:3`;
    const response = await api.get<unknown, ApiResponse<any[]>>(
      `/Request?Filters=${encodeURIComponent(filters)}&Include=origin,requestStatus&PageNumber=1&PageSize=${limit}&IncludeTotal=true`
    );

    if (!response.success || !response.data) {
      return {
        success: false,
        message: "No se pudo obtener el historial de transacciones",
        data: null,
        totalResults: 0
      };
    }

    const transactions: TransactionHistory[] = response.data.map(req => ({
      id: req.id,
      requestNumber: req.requestNumber,
      name: req.name,
      originId: req.originId,
      originName: req.origin?.name,
      priorityId: req.priorityId,
      priorityName: req.priority?.name,
      requestDate: req.requestDate,
      requestAmount: req.requestAmount,
      statusName: req.requestStatus?.name
    }));

    return {
      success: true,
      message: "Historial obtenido exitosamente",
      data: transactions,
      totalResults: transactions.length
    };
  } catch (error) {
    console.error("Error al obtener historial de transacciones:", error);
    return {
      success: false,
      message: "Error al obtener historial",
      data: null,
      totalResults: 0
    };
  }
};
