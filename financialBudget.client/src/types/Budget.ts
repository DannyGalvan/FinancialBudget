export interface Budget {
  id: number;
  authorizedAmount?: number;  // Total autorizado (presupuesto total)
  committedAmount?: number;   // Monto comprometido/usado
  availableAmount?: number;   // Monto disponible
  period?: number;            // Período (año)
  state?: number;             // Estado del presupuesto
  createdBy?: number;
  updatedBy?: number | null;
  createdAt?: string;
  updatedAt?: string | null;
  
  // Campos legacy (deprecados, se mantienen por compatibilidad)
  name?: string;
  description?: string;
  totalAmount?: number;
  usedAmount?: number;
  year?: number;
  month?: number;
  startDate?: string;
  endDate?: string;
  budgetStatusId?: number;
  budgetStatus?: {
    id: number;
    name: string;
  };
}

export interface BudgetUsage {
  departmentId: number;
  departmentName: string;
  totalUsed: number;
  lastMonth: number;
  currentMonth: number;
}

export interface DashboardStats {
  totalBudget: number;
  usedBudget: number;
  availableBudget: number;
  usagePercentage: number;
  lastMonthUsage: number;
  currentMonthUsage: number;
}

export interface PendingSolicitudesStats {
  originId: number;
  originName: string;
  totalPending: number;
  totalUsed: number;
}

export interface TransactionHistory {
  id: number;
  requestNumber?: string;
  name?: string;
  originId?: number;
  originName?: string;
  priorityId?: number;
  priorityName?: string;
  requestDate?: string;
  requestAmount?: number;
  statusName?: string;
}

