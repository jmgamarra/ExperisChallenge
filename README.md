# ExperisChallenge
UserStory 
Gestionar Productos 
Cada jefe de almacen tendra acceso a lo sgte (crear , editar , eliminar y leer) el stock de productos que gestiona.
Criterios de Aceptacion:
- Crear producto > el sistema permitira la creacion de un producto registrando el nombre , precio y cantidad
- Editar producto > el sistema permitira la edicion de producto por los campos nombre , precio y cantidad
- Eliminar producto > el sistema permitira la eliminacion de un producto , conociendo el identificador unido de producto ID que es generado por sistema
- Leer producto > el sistema permitira ver el listado completo de productos .
Se asume :
- un solo jefe por almacen
- un solo almacen
Validaciones:
- todos los productos deben tener nombre .
- El precio y cantidad por defecto sera 0 , no puede ser un valor negativo
- Se usa TDD
- No se permite el uso de EF.
 
