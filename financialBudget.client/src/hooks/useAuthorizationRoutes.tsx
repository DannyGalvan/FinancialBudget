import { createBrowserRouter } from "react-router";

import { nameRoutes } from "../configs/constants";
import { Root } from "../containers/Root";
import { ErrorRoutes } from "../routes/ErrorRoutes";
import { PublicRoutes } from "../routes/PublicRoutes";
import { useAuth } from "./useAuth";
import LoadingPage from "../pages/public/LoadingPage";
import type { RouteObject } from "react-router";

// Mapeo de rutas a componentes de páginas
const pageComponentMap: Record<string, () => Promise<any>> = {
  "/request/maintenance": () => import("../pages/solicitudes/SolicitudMantenimiento"),
  "/request/events": () => import("../pages/solicitudes/SolicitudEventos"),
  "/solicitudes/historial": () => import("../pages/solicitudes/HistorialTransacciones"),
  // Agrega más mapeos aquí según las rutas que tengas del backend
  // "/report/financial": () => import("../pages/reportes/ReporteFinanciero"),
  // "/report/summarycitizenship": () => import("../pages/reportes/ResumenCiudadania"),
  // "/budget/update": () => import("../pages/presupuesto/ActualizarPresupuesto"),
  // "/budgetitem/update": () => import("../pages/presupuesto/ActualizarItemPresupuesto"),
};

export const useAuthorizationRoutes = () => {
  const { allOperations } = useAuth();

  // Crear rutas dinámicas desde las operaciones
  const dynamicRoutes: RouteObject[] = allOperations
    .filter((operation) => operation.isVisible)
    .map((operation) => {
      const path = operation.path.startsWith("/") ? operation.path : `/${operation.path}`;
      const pathKey = path.toLowerCase();
      const pathKeyWithoutSlash = pathKey.replace(/^\//, '');
      
      // Buscar el componente con o sin slash
      const component = pageComponentMap[pathKey] || pageComponentMap[pathKeyWithoutSlash];
      
      console.log('🔍 Ruta del backend:', operation.path, '→ Path procesado:', path, '→ Componente encontrado:', !!component);
      
      return {
        path: path,
        lazy: component,
        hydrateFallbackElement: <LoadingPage />,
      };
    })
    .filter((route) => route.lazy !== undefined); // Solo incluir rutas que tengan componente mapeado

  console.log('📋 Rutas dinámicas creadas:', dynamicRoutes.map(r => r.path));

  // Rutas estáticas adicionales (no vienen del backend)
  const staticRoutes: RouteObject[] = [
    {
      path: "/solicitudes/historial",
      lazy: () => import("../pages/solicitudes/HistorialTransacciones"),
      hydrateFallbackElement: <LoadingPage />,
    },
  ];

  const routes = createBrowserRouter([
    {
      path: nameRoutes.root,
      element: <Root />,
      children: [...PublicRoutes, ...staticRoutes, ...dynamicRoutes, ...ErrorRoutes],
    },
  ]);

  return routes;
};
