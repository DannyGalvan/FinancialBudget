# 🎯 FinancialBudget API - Complete Postman Collection

## 📁 Archivos incluidos

- **`FinancialBudget-API.postman_collection.json`** - 🚀 COLECCIÓN COMPLETA con TODOS los endpoints
- **`FinancialBudget.postman_environment.json`** - 🔧 ENVIRONMENT UNIVERSAL para Mock/Local/Production
- **`README.md`** - Esta guía de uso

## 🎉 **1 COLECCIÓN + 1 ENVIRONMENT = TODO CUBIERTO**

### ✅ **Endpoints incluidos (COMPLETOS):**

#### 🔐 **Authentication**
- `POST /api/v1/Auth` - Login y obtención de JWT

#### 💰 **Budget Management** (CRUD Completo)
- `GET /api/v1/Budget` - Listar presupuestos
- `GET /api/v1/Budget/{id}` - Obtener presupuesto por ID
- `POST /api/v1/Budget` - Crear presupuesto
- `PUT /api/v1/Budget` - Actualizar presupuesto
- `DELETE /api/v1/Budget/{id}` - Eliminar presupuesto

#### 📊 **Budget Items** (CRUD Completo)
- `GET /api/v1/BudgetItem` - Listar items de presupuesto
- `GET /api/v1/BudgetItem/{id}` - Obtener item por ID
- `POST /api/v1/BudgetItem` - Crear item
- `PUT /api/v1/BudgetItem` - Actualizar item
- `DELETE /api/v1/BudgetItem/{id}` - Eliminar item

#### 📋 **Request Management** (CRUD Completo)
- `GET /api/v1/Request` - Listar solicitudes
- `GET /api/v1/Request/{id}` - Obtener solicitud por ID
- `POST /api/v1/Request` - Crear solicitud
- `PUT /api/v1/Request` - Actualizar solicitud
- `DELETE /api/v1/Request/{id}` - Eliminar solicitud

#### 📚 **Catalogue Management** (CRUD Completo)
- `GET /api/v1/Catalogue/{catalogue}` - Listar items de catálogo
- `GET /api/v1/Catalogue/{catalogue}/{id}` - Obtener item por ID
- `POST /api/v1/Catalogue/{catalogue}` - Crear item de catálogo
- `PUT /api/v1/Catalogue/{catalogue}` - Actualizar item
- `DELETE /api/v1/Catalogue/{catalogue}/{id}` - Eliminar item

## 🔄 **Cambiar entre Mock Server y Backend Real**

### **Environment Variables:**
- `baseUrl` - 🎯 **LA QUE USAS** (cambia esta)
- `mockServerUrl` - URL del Mock Server (referencia)
- `localServerUrl` - `http://localhost:5175` (referencia)
- `productionUrl` - URL de producción (referencia)
- `token` - JWT token (auto-generado)
- `currentMode` - Indicador del modo actual

### **🔧 Modo 1: Mock Server** (Sin backend)
```
1. En Postman: Collection → Create Mock Server
2. Copia la URL generada
3. Environment → baseUrl = "URL-del-mock"
4. currentMode = "mock"
5. ¡Listo! Datos ficticios consistentes
```

### **🔧 Modo 2: Backend Local** (Con tu API corriendo)
```
1. Terminal: dotnet run (en FinancialBudget.Server)
2. Environment → baseUrl = "http://localhost:5175"
3. currentMode = "local"  
4. ¡Listo! Datos reales de tu base de datos
```

### **🔧 Modo 3: Producción**
```
1. Environment → baseUrl = "https://tu-api-production.com"
2. currentMode = "production"
3. ¡Listo! API en vivo
```

## ⚡ **Características Avanzadas**

### 🔐 **Auto-Authentication**
- El token se guarda automáticamente después del login
- Todos los endpoints usan automáticamente `Bearer {{token}}`
- Script pre-request automático para authorization

### 🧪 **Tests Automáticos**
- Validación automática de códigos de respuesta
- Verificación de estructura de respuesta
- Tests de propiedades `success` y `message`

### 📝 **Respuestas Mock Realistas**
- Datos de ejemplo con estructura real
- IDs, fechas, montos realistas  
- Respuestas de éxito y error
- Estructura consistent con tu API

### 🎨 **Organización Clara**
- Carpetas por módulo con emojis
- Nombres descriptivos
- Headers preconfigurados
- Query parameters de ejemplo

## 🚀 **Pasos para usar:**

### **1. Importar en Postman**
```
1. Postman → Import
2. Selecciona ambos archivos JSON
3. ✅ Colección importada
4. ✅ Environment importado
```

### **2. Configurar Mock Server (Opcional)**
```
1. Click derecho en colección → Mock Collection
2. Copia URL generada
3. Environment → mockServerUrl = "URL-copiada"
4. baseUrl = {{mockServerUrl}}
```

### **3. Probar endpoints**
```
1. Selecciona environment "FinancialBudget - Universal Environment"
2. Ejecuta Login para obtener token
3. Prueba cualquier endpoint
4. Cambia baseUrl para alternar entre mock/local/prod
```

## 💡 **Tips de Uso**

- 👁️ **Quick Edit**: Usa el ojo del environment para cambiar URLs rápido
- 🔄 **Alternancia rápida**: Solo cambia `baseUrl` entre las URLs de referencia
- 🧪 **Tests**: Los endpoints se validan automáticamente
- 📊 **Console**: Revisa la consola de Postman para logs detallados
- 🎯 **Mock vs Real**: Mismo código, diferentes fuentes de datos

## ✅ **¿Todo funcionando?**

✅ Login devuelve token  
✅ Token se guarda automáticamente  
✅ Otros endpoints usan el token  
✅ Respuestas tienen estructura correcta  
✅ Puedes cambiar entre mock y real fácilmente  

## 🆘 **Troubleshooting**

**❌ 404 Not Found en Mock:**
- Verifica que creaste el Mock Server
- Asegúrate de usar la URL correcta del mock

**❌ 401 Unauthorized:**
- Ejecuta Login primero
- Verifica que el token se guardó en environment

**❌ Connection refused en Local:**
- Ejecuta `dotnet run` en FinancialBudget.Server
- Verifica que el puerto es correcto (5175)

**❌ Endpoints no aparecen:**
- Reimporta la colección
- Verifica que tienes la versión "Complete"

---

🎉 **¡Listo para probar tu API completa!** 🎉