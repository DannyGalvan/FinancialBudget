import { Icon } from "../../components/icons/Icon";

export function Dashboard() {
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
                <p className="text-sm text-gray-400 mb-1">Limite De Presupuesto</p>
                <div className="w-80 bg-gray-200 rounded-full h-2.5 mb-2">
                  <div className="bg-blue-600 h-2.5 rounded-full" style={{ width: '21%' }}></div>
                </div>
                <p className="text-sm font-semibold text-neutral-700">Q35,009.60 / Q54,000</p>
              </div>
            </div>
            
            {/* Chart Placeholder */}
            <div className="h-64 bg-white rounded-2xl flex items-center justify-center border border-gray-100">
              <div className="text-center">
                <Icon name="bi bi-bar-chart-line" size={48} color="#197BBD" />
                <p className="text-gray-400 mt-4">Gráfico de Fondos</p>
                <p className="text-sm text-gray-300 mt-1">Esta vista requiere un gráfico dinámico</p>
              </div>
            </div>
            
            <div className="flex justify-center gap-6 mt-4">
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-blue-500"></div>
                <span className="text-xs text-gray-600">This Month</span>
              </div>
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-purple-300"></div>
                <span className="text-xs text-gray-600">Last Month</span>
              </div>
            </div>
          </div>

          {/* Historial De Transacciones Recientes */}
          <div className="bg-white rounded-3xl shadow-lg p-6">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-xl font-semibold text-neutral-700">
                Historial De Transacciones Recientes
              </h2>
              <Icon name="bi bi-arrow-right" size={20} color="#404040" />
            </div>
            
            {/* Table Headers */}
            <div className="grid grid-cols-4 gap-4 text-sm text-gray-400 mb-4 pb-2">
              <div>Solicitud</div>
              <div>Area</div>
              <div>Date</div>
              <div className="text-right">Monto</div>
            </div>
            
            {/* Table Rows */}
            <div className="space-y-4">
              <div className="grid grid-cols-4 gap-4 items-center py-3 border-b border-gray-100">
                <div className="flex items-center gap-3">
                  <div className="w-5 h-5 bg-gray-100 rounded"></div>
                  <span className="text-sm text-neutral-700">Reparacion De Calle</span>
                </div>
                <span className="text-sm text-gray-300">Mantenimiento</span>
                <span className="text-sm text-gray-300">13 Dec 2025</span>
                <span className="text-sm font-bold text-neutral-700 text-right">Q100,052.00</span>
              </div>
              
              <div className="grid grid-cols-4 gap-4 items-center py-3">
                <div className="flex items-center gap-3">
                  <div className="w-5 h-5 bg-gray-100 rounded"></div>
                  <span className="text-sm text-neutral-700">Dia del nino</span>
                </div>
                <span className="text-sm text-gray-300">Eventos Com</span>
                <span className="text-sm text-gray-300">10 Oct 2025</span>
                <span className="text-sm font-bold text-neutral-700 text-right">Q250.00</span>
              </div>
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
              <div className="bg-white rounded-3xl shadow-md p-5">
                <p className="text-2xl font-semibold text-gray-600 mb-1">Q 2200</p>
                <p className="text-sm text-gray-400 mb-2">12/20/20</p>
                <p className="text-lg text-neutral-700">Mantenimiento</p>
              </div>
              
              <div className="bg-white rounded-3xl shadow-md p-5">
                <p className="text-2xl font-semibold text-gray-600 mb-1">Q 8200</p>
                <p className="text-sm text-gray-400 mb-2">12/20/20</p>
                <p className="text-lg text-neutral-700">Eventos Com.</p>
              </div>
            </div>
          </div>

          {/* Solicitudes Pendientes */}
          <div>
            <h2 className="text-xl font-semibold text-neutral-700 mb-4">
              Solicitudes Pendientes
            </h2>
            
            <div className="space-y-4">
              {/* Mantenimiento */}
              <div className="flex items-center gap-4">
                <div className="w-11 h-11 bg-green-50 rounded-sm shadow-md flex items-center justify-center">
                  <Icon name="bi bi-wrench" size={20} color="#2BC255" />
                </div>
                <div className="flex-1">
                  <p className="text-sm text-gray-400 mb-1">Mantenimiento</p>
                  <div className="w-full bg-gray-200 rounded-full h-2.5">
                    <div className="bg-gradient-to-r from-green-600 to-green-400 h-2.5 rounded-full" style={{ width: '16%' }}></div>
                  </div>
                </div>
                <span className="text-2xl font-semibold text-gray-600">8</span>
              </div>
              
              {/* Eventos Comunitarios */}
              <div className="flex items-center gap-4">
                <div className="w-11 h-11 bg-blue-50 rounded-sm shadow-md flex items-center justify-center">
                  <Icon name="bi bi-calendar-event" size={20} color="#70A6E8" />
                </div>
                <div className="flex-1">
                  <p className="text-sm text-gray-400 mb-1">Eventos Comunitarios</p>
                  <div className="w-full bg-gray-200 rounded-full h-2.5">
                    <div className="bg-blue-400 h-2.5 rounded-full" style={{ width: '76%' }}></div>
                  </div>
                </div>
                <span className="text-2xl font-semibold text-gray-600">12</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
