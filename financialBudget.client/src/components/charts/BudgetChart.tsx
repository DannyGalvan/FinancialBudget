interface BudgetChartProps {
  lastMonthUsage: number;
  currentMonthUsage: number;
  totalBudget: number;
}

export function BudgetChart({ lastMonthUsage, currentMonthUsage, totalBudget }: BudgetChartProps) {
  const maxValue = Math.max(lastMonthUsage, currentMonthUsage, totalBudget * 0.5);
  const lastMonthPercentage = (lastMonthUsage / maxValue) * 100;
  const currentMonthPercentage = (currentMonthUsage / maxValue) * 100;

  const months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];
  const currentMonth = new Date().getMonth();
  const lastMonth = currentMonth === 0 ? 11 : currentMonth - 1;

  return (
    <div className="h-64 flex flex-col justify-end">
      <div className="flex items-end justify-center gap-12 h-full pb-4">
        {/* Last Month Bar */}
        <div className="flex flex-col items-center gap-2" style={{ width: '100px' }}>
          <div className="w-full flex flex-col justify-end items-center" style={{ height: '200px' }}>
            <div 
              className="w-full bg-gradient-to-t from-purple-400 to-purple-300 rounded-t-lg transition-all duration-500 ease-out flex items-end justify-center pb-2"
              style={{ height: `${lastMonthPercentage}%`, minHeight: '30px' }}
            >
              <span className="text-xs font-semibold text-white">
                Q{(lastMonthUsage / 1000).toFixed(1)}K
              </span>
            </div>
          </div>
          <span className="text-sm font-medium text-gray-600">{months[lastMonth]}</span>
        </div>

        {/* Current Month Bar */}
        <div className="flex flex-col items-center gap-2" style={{ width: '100px' }}>
          <div className="w-full flex flex-col justify-end items-center" style={{ height: '200px' }}>
            <div 
              className="w-full bg-gradient-to-t from-blue-600 to-blue-500 rounded-t-lg transition-all duration-500 ease-out flex items-end justify-center pb-2"
              style={{ height: `${currentMonthPercentage}%`, minHeight: '30px' }}
            >
              <span className="text-xs font-semibold text-white">
                Q{(currentMonthUsage / 1000).toFixed(1)}K
              </span>
            </div>
          </div>
          <span className="text-sm font-medium text-gray-600">{months[currentMonth]}</span>
        </div>
      </div>
    </div>
  );
}
