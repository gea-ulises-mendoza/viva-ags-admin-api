USE [BD_Moviles]
GO
/****** Object:  StoredProcedure [dbo].[sp_AT_clabAtractivoCrearTodo]    Script Date: 12/03/2026 12:53:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_AT_clabAtractivoCrearTodo]
       @ListaTelefonos AS dbo.ListaTelefonos READONLY,
       @ListaEnlaces AS dbo.ListaEnlaces READONLY,
       @ListaFotos AS dbo.ListaFotos READONLY,
       @ListaCategorias AS dbo.ListaCategorias READONLY,
       @ListaEtiquetas AS dbo.AT_ListaEtiquetas READONLY,
       @nombre varchar(100),
       @descripcion varchar(max),
       @direccion varchar(250),
       @horarios varchar(500),
       @costos varchar(max),
       @notas varchar(max),
       @latitud float,
       @longitud float,
       @id int = NULL,
       @nombreEn varchar(100) = null,
       @descripcionEn varchar(max) = null,
       @direccionEn varchar(250) = null,
       @horariosEn varchar(500) = null,
       @costosEn varchar(max) = null,
       @notasEn varchar(max) = null,
       @id_etiqueta int = NULL

AS
BEGIN

       declare @telefono bigint
       declare @extension int
       declare @liga nvarchar(500)
       declare @id_enlace int
       declare @ruta varchar(250)
       declare @descripcionFoto varchar(200)
       declare @portada bit
       declare @idGenerico int 
       declare @recomendado bit
       declare @activo bit
       declare @eliminado bit
       declare @id_categoria int
       declare @etiqueta varchar(50)

       IF @id IS NULL
BEGIN
INSERT INTO AT_Atractivos (nombre, descripcion, direccion, horarios, costos, notas, latitud, longitud, activo, nombre_en, descripcion_en, direccion_en, horarios_en, costos_en, notas_en)
    OUTPUT INSERTED.ID as id
VALUES (@nombre, @descripcion, @direccion, @horarios, @costos, @notas, @latitud, @longitud, 1, @nombreEn, @descripcionEn, @direccionEn, @horariosEn, @costosEn, @notasEn);
SET @id = @@IDENTITY

END
ELSE
BEGIN
UPDATE AT_Atractivos
SET nombre = @nombre, descripcion = @descripcion, direccion = @direccion, horarios = @horarios,
    costos = @costos, notas = @notas, latitud = @latitud, longitud = @longitud, activo = 1,
    nombre_en = @nombreEn, descripcion_en = @descripcionEn, direccion_en = @direccionEn,
    horarios_en = @horariosEn, costos_en = @costosEn, notas_en = @notasEn
    OUTPUT INSERTED.ID as id
WHERE id = @id
END

       -- Inserta las categorias
       DECLARE cursorCategorias CURSOR FOR
SELECT id, id_categoria, recomendado, eliminado FROM @ListaCategorias

    OPEN cursorCategorias

       FETCH NEXT FROM cursorCategorias INTO @idGenerico, @id_categoria, @recomendado, @eliminado

    WHILE @@FETCH_STATUS = 0
BEGIN

       IF @idGenerico IS NULL
BEGIN

INSERT INTO AT_Atractivo_Categoria (id_atractivo, id_categoria, recomendado)
VALUES (@id, @id_categoria, @recomendado)

END
ELSE
BEGIN

                    IF @eliminado = 1
BEGIN

DELETE FROM AT_Atractivo_Categoria WHERE id = @idGenerico

END
ELSE
BEGIN

UPDATE AT_Atractivo_Categoria
SET id_atractivo = @id, id_categoria = @id_categoria, recomendado = @recomendado
WHERE id = @idGenerico

END

END

FETCH NEXT FROM cursorCategorias INTO @idGenerico, @id_categoria, @recomendado, @eliminado

END
CLOSE cursorCategorias
    DEALLOCATE cursorCategorias

-- Inserta los enlaces
DECLARE cursorEnlaces CURSOR FOR
SELECT id, liga, id_enlace, eliminado FROM @ListaEnlaces

    OPEN cursorEnlaces

       FETCH NEXT FROM cursorEnlaces INTO @idGenerico, @liga, @id_enlace, @eliminado

    WHILE @@FETCH_STATUS = 0
BEGIN

       IF @idGenerico IS NULL
BEGIN

INSERT INTO AT_Atractivo_Enlace (id_atractivo, id_enlace, liga)
VALUES (@id, @id_enlace, @liga)

END
ELSE
BEGIN
                    
                    IF @eliminado = 1
BEGIN

DELETE FROM AT_Atractivo_Enlace WHERE id = @idGenerico

END
ELSE
BEGIN

UPDATE AT_Atractivo_Enlace
SET id_atractivo = @id, id_enlace = @id_enlace,
    liga = @liga
WHERE id = @idGenerico
END

END

FETCH NEXT FROM cursorEnlaces INTO @idGenerico, @liga, @id_enlace, @eliminado

END
CLOSE cursorEnlaces
    DEALLOCATE cursorEnlaces

-- Inserta los fotos
DECLARE cursorFotos CURSOR FOR
SELECT id, ruta, descripcion, portada, eliminado
FROM   @ListaFotos

    OPEN cursorFotos

    FETCH NEXT FROM cursorFotos INTO @idGenerico, @ruta, @descripcionFoto, @portada, @eliminado

    WHILE @@FETCH_STATUS = 0
BEGIN

       IF @idGenerico IS NULL
BEGIN

INSERT INTO AT_Galeria (id_atractivo, ruta, descripcion, portada)
VALUES (@id, @ruta, @descripcionFoto, @portada)

END
ELSE
BEGIN

                    IF @eliminado = 1
BEGIN

DELETE FROM AT_Galeria WHERE id = @idGenerico

END
ELSE
BEGIN

UPDATE AT_Galeria
SET id_atractivo = @id, ruta = @ruta,
    descripcion = @descripcionFoto, portada = @portada
WHERE id = @idGenerico
END

END

FETCH NEXT FROM cursorFotos INTO @idGenerico, @ruta, @descripcionFoto, @portada, @eliminado
END
CLOSE cursorFotos
    DEALLOCATE cursorFotos

-- Inserta los Teléfonos
DECLARE cursorTelefonos CURSOR FOR
SELECT id, telefono, extension, eliminado
FROM   @ListaTelefonos

    OPEN cursorTelefonos

    FETCH NEXT FROM cursorTelefonos INTO @idGenerico, @telefono, @extension, @eliminado

    WHILE @@FETCH_STATUS = 0
BEGIN

             IF @idGenerico IS NULL
BEGIN

INSERT INTO AT_Telefonos (id_atractivo, telefono, extension)
VALUES (@id, @telefono, @extension)

END
ELSE
BEGIN

                    IF @eliminado = 1
BEGIN

DELETE FROM AT_Telefonos WHERE id = @idGenerico

END
ELSE
BEGIN

UPDATE AT_Telefonos
SET id_atractivo = @id,
    telefono = @telefono, extension = @extension
WHERE id = @idGenerico

END

END

FETCH NEXT FROM cursorTelefonos INTO @idGenerico, @telefono, @extension, @eliminado

END
CLOSE cursorTelefonos
    DEALLOCATE cursorTelefonos

-- Inserta las Etiquetas
DECLARE cursorEtiquetas CURSOR FOR
SELECT id, etiqueta, eliminado
FROM   @ListaEtiquetas

    OPEN cursorEtiquetas

    FETCH NEXT FROM cursorEtiquetas INTO @idGenerico, @etiqueta, @eliminado

    WHILE @@FETCH_STATUS = 0
BEGIN

             IF @idGenerico IS NULL
BEGIN

INSERT INTO AT_Etiqueta(etiqueta)
VALUES (@etiqueta)
    SET @id_etiqueta = @@IDENTITY

INSERT INTO AT_Etiqueta_Atractivo (id_etiqueta, id_atractivo) VALUES (@id_etiqueta, @id)

END
ELSE
BEGIN
                    

                    IF @eliminado = 1
BEGIN

DELETE FROM AT_Etiqueta_Atractivo WHERE id_etiqueta = @idGenerico and id_atractivo = @id
END
ELSE
BEGIN
                          IF NOT EXISTS (SELECT id from AT_Etiqueta_Atractivo WHERE id_etiqueta = @idGenerico AND id_atractivo = @id)
BEGIN
INSERT INTO AT_Etiqueta_Atractivo (id_etiqueta, id_atractivo) VALUES (@idGenerico, @id)
END
END

END

FETCH NEXT FROM cursorEtiquetas INTO @idGenerico, @etiqueta, @eliminado

END
CLOSE cursorEtiquetas
    DEALLOCATE cursorEtiquetas

END
