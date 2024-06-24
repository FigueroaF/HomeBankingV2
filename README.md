# Homebanking Backend

## Descripción

El backend de esta aplicación de homebanking está desarrollado en .NET y C#, ofreciendo una solución robusta y segura para la gestión de operaciones bancarias. Esta aplicación permite a los usuarios realizar una variedad de transacciones financieras y gestionar sus cuentas y tarjetas con facilidad. 

### Funcionalidades Principales

1. **Gestión de Clientes y Cuentas:**
   - **Crear nuevos clientes:** Permite registrar clientes con información detallada.
   - **Gestión de cuentas bancarias:** Cada cliente puede tener múltiples cuentas asociadas, facilitando la organización y el seguimiento de sus finanzas.

2. **Tarjetas de Crédito y Débito:**
   - **Emisión de tarjetas:** Los usuarios pueden solicitar nuevas tarjetas de crédito y débito.
   - **Gestión de tarjetas:** Posibilidad de consultar detalles de las tarjetas, activar o desactivar tarjetas, y reportar pérdidas.

3. **Transferencias Bancarias:**
   - **Realización de transferencias:** Los usuarios pueden transferir fondos entre sus cuentas o hacia cuentas de otros usuarios de manera segura y eficiente.
   - **Historial de transferencias:** Registro detallado de todas las transferencias realizadas para un seguimiento preciso.

4. **Préstamos:**
   - **Solicitud de préstamos:** Los usuarios pueden solicitar préstamos, especificando la cantidad y el plazo de reembolso.
   - **Gestión de préstamos:** Seguimiento de la solicitud, aprobación y reembolso de préstamos.

### Tecnología y Dependencias

El proyecto utiliza **Entity Framework Core** para interactuar con una base de datos SQL Server, facilitando la gestión de datos mediante operaciones CRUD. Las principales dependencias incluyen:
- **Microsoft.EntityFrameworkCore v8.0.1:** Provee la funcionalidad principal de ORM (Mapeo de Objetos Relacionales).
- **Microsoft.EntityFrameworkCore.Tools v8.0.1:** Herramientas para la gestión de migraciones y otras tareas de desarrollo.
- **Microsoft.EntityFrameworkCore.SqlServer v8.0.1:** Proveedor de EF Core para SQL Server.
- **Microsoft.VisualStudio.Web.CodeGeneration.Design v8.0.0:** Herramientas de generación de código para aplicaciones web.
- **Swashbuckle.AspNetCore v6.5.0:** Integra Swagger para la documentación y prueba de la API.

### API y Documentación

La API expone varios endpoints para manejar las operaciones mencionadas. La integración con Swashbuckle.AspNetCore permite la generación automática de documentación de la API a través de Swagger, lo que facilita a los desarrolladores y usuarios la exploración y prueba de los endpoints disponibles.

### Seguridad y Mejores Prácticas

El backend implementa medidas de seguridad robustas para garantizar la protección de datos sensibles y prevenir accesos no autorizados. Esto incluye autenticación, autorización y encriptación de datos. Además, se siguen las mejores prácticas de desarrollo seguro y mantenimiento continuo del código para asegurar que la aplicación se mantenga protegida contra vulnerabilidades.
