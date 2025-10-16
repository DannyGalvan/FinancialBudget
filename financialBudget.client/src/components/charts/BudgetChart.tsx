import type { MonthlyUsage } from "../../types/Budget";

interface BudgetChartProps {
  monthlyUsage?: MonthlyUsage[];
  totalBudget: number;
}

export function BudgetChart({ monthlyUsage = [], totalBudget }: BudgetChartProps) {
  // Si no hay datos, mostrar mensaje
  if (monthlyUsage.length === 0) {
    return (
      <div className="h-64 flex items-center justify-center">
        <p className="text-gray-400">No hay datos disponibles</p>
      </div>
    );
  }

  // Calcular el valor máximo para escalar las barras
  const maxValue = Math.max(...monthlyUsage.map(m => m.amount), totalBudget * 0.1);
  
  // Abreviaturas de meses
  const monthAbbr: Record<string, string> = {
    'Enero': 'Ene', 'Febrero': 'Feb', 'Marzo': 'Mar', 'Abril': 'Abr',
    'Mayo': 'May', 'Junio': 'Jun', 'Julio': 'Jul', 'Agosto': 'Ago',
    'Septiembre': 'Sep', 'Octubre': 'Oct', 'Noviembre': 'Nov', 'Diciembre': 'Dic'
  };

  return (
    <div className="h-64 w-full p-4">
      <div className="flex items-end justify-between gap-1 h-full pb-8">
        {monthlyUsage.map((month, index) => {
          const percentage = maxValue > 0 ? (month.amount / maxValue) * 100 : 0;
          const isCurrentMonth = index === monthlyUsage.length - 1;
          
          return (
            <div key={index} className="flex flex-col items-center gap-2 flex-1">
              <div className="w-full flex flex-col justify-end items-center" style={{ height: '180px' }}>
                <div 
                  className={`w-full rounded-t-lg transition-all duration-500 ease-out flex items-end justify-center pb-1 ${
                    isCurrentMonth 
                      ? 'bg-gradient-to-t from-blue-600 to-blue-500' 
                      : 'bg-gradient-to-t from-purple-400 to-purple-300'
                  }`}
                  style={{ height: `${Math.max(percentage, 5)}%`, minHeight: '20px' }}
                  title={`${month.month}: Q${month.amount.toLocaleString()}`}
                >
                  {month.amount > 0 && percentage > 15 && (
                    <span className="text-[9px] font-semibold text-white">
                      {month.amount >= 1000 
                        ? `Q${(month.amount / 1000).toFixed(1)}K` 
                        : `Q${month.amount}`}
                    </span>
                  )}
                </div>
              </div>
              <span className="text-[10px] font-medium text-gray-600">
                {monthAbbr[month.month] || month.month.substring(0, 3)}
              </span>
            </div>
          );
        })}
      </div>
    </div>
  );
}
