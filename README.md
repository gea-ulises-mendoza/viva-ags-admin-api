### Cambios en base de datos
Tablas creadas
```
AT_RefreshToken
AT_Usuarios
```

Tablas modificadas
```
AT_Atractivo_Enlace
liga: varchar(500)

AT_Cat_Enlaces
cambiar decripcion a descripcion: varchar(30)
cambiar icono: varchar(30)
cambiar prefijo: varchar(50)

AT_Categorias
cambiar nombre: varchar(50)
cambiar imagen: varchar(200)
cambiar pin: varchar(200)
cambiar icono: varchar(200)
cambiar descripcion: varchar(MAX)
cambiar tag: varchar(20)

AT_Galeria
cambiar ruta: varchar(250)
cambiar descripcion: varchar(200)
```

Procedimientos creados
```
sp_AT_clabAtractivoLeer
sp_AT_clabAtractivoCrearTodo
sp_AT_clabAtractivoBorrar
sp_AT_ObtenCategoriasAltaAtractivo
sp_AT_clabCategorias
sp_AT_ObtenCategoriasPorAtractivo
sp_AT_clabCategoriaCrear
sp_AT_clabCategoriaActualizar
sp_AT_clabCategoriaBorrar
sp_AT_clabCategoriasOrdenar
sp_AT_Login
sp_AT_ObtenCategoriasPorAtractivo
sp_AT_ObtenRedes
sp_AT_RefreshToken
sp_AT_RegistroUsuario
sp_AT_ValidaToken
```

Procedimientos actualizados?
```
sp_AT_ObtenEnlacesPorAtractivo
sp_AT_ObtenTelefonos
```

Procedimientos no usados
```
sp_AT_clabAtractivoActualizar
sp_AT_clabAtractivoCrear
sp_AT_clabAtractivosPaginacion
sp_AT_clabCategoriaOrdenar
sp_AT_crear_categoria
sp_AT_ObtenCategoriasParaAtractivos
```


Tipos de datos creados
```
ListaCategorias
ListaEnlaces
ListaFotos
ListaTelefonos
```

BD Desarrollo
```
"BD_Atractivos": "Data Source=10.1.111.173;Initial Catalog=BD_Moviles;User ID=usuMoviles_Des;Password=MOV%u5ud; Connect Timeout=1800"
```

BD Producción
```
"BD_Atractivos": "Data Source=10.1.111.58\\SQLSAEDB;Initial Catalog=BD_Moviles;User ID=usuMoviles;Password=u5u%67MoV; Connect Timeout=1800"
```