using System;
using System.Collections.Generic;
using System.Data;

namespace VivaAguascalientesAPI.DTO.RequestDTO
{
    public class CreateAtractivoRequest
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Horarios { get; set; }
        public string Costos { get; set; }
        public string Notas { get; set; } 
        public string NombreEn { get; set; }
        public string DescripcionEn { get; set; }
        public string DireccionEn { get; set; }
        public string HorariosEn { get; set; }
        public string CostosEn { get; set; }
        public string NotasEn { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public IList<TelefonoRequest> ListaTelefonos { get; set; }
        public IList<EnlaceRequest> ListaEnlaces { get; set; }
        public IList<FotosRequest> ListaFotos { get; set; }
        public IList<AtractivoCategoriaRequest> ListaCategorias { get; set; }
        public IList<EtiquetaRequest> ListaEtiquetas { get; set; }
        public IList<MunicipioAtractivoRequest> ListaMunicipios { get; set; }
    }

    public class TelefonoRequest
    {
        public int? Id { get; set; }
        public long? Telefono{ get; set; }
        public int? Extension { get; set; }
        public Boolean Eliminar { get; set; }

        public static DataTable ToDataTable(IList<TelefonoRequest> telefonos)
        {
            DataTable dataTable = new DataTable("ListaTelefonos");
            var columnId = new DataColumn("id", typeof(int));
            columnId.AllowDBNull = true;
            dataTable.Columns.Add(columnId);
            dataTable.Columns.Add(new DataColumn("telefono", typeof(Int64)));
            dataTable.Columns.Add(new DataColumn("extension", typeof(Int32)));
            dataTable.Columns.Add(new DataColumn("eliminar", typeof(Boolean)));

            foreach (TelefonoRequest telefono in telefonos)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = (object) telefono.Id ?? DBNull.Value;
                dataRow["telefono"] = telefono.Telefono;
                if (telefono.Extension != null)
                {
                    dataRow["extension"] = telefono.Extension;
                }
                dataRow["eliminar"] = telefono.Eliminar;
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class EnlaceRequest
    {
        public int? Id { get; set; }
        public string Liga { get; set; }
        public int IdEnlace { get; set; }
        public Boolean Eliminar { get; set; }

        public static DataTable ToDataTable(IList<EnlaceRequest> enlaces)
        {
            DataTable dataTable = new DataTable("ListaEnlaces");
            var columnId = new DataColumn("id", typeof(int));
            columnId.AllowDBNull = true;
            dataTable.Columns.Add(columnId);
            dataTable.Columns.Add(new DataColumn("liga", typeof(string)));
            dataTable.Columns.Add(new DataColumn("id_enlace", typeof(Int32)));
            dataTable.Columns.Add(new DataColumn("eliminar", typeof(Boolean)));

            foreach (EnlaceRequest enlace in enlaces)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = (object) enlace.Id ?? DBNull.Value;
                dataRow["liga"] = enlace.Liga;
                dataRow["id_enlace"] = enlace.IdEnlace;
                dataRow["eliminar"] = enlace.Eliminar;
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class FotosRequest
    {
        public int? Id { get; set; }
        public byte[] Imagen { get; set; }
        public string Ruta { get; set; }
        public string Descripcion { get; set; }
        public Boolean Portada { get; set; }
        public Boolean isDeleted { get; set; }
        public Boolean Eliminar { get; set; }
        public static DataTable ToDataTable(IList<FotosRequest> fotos)
        {
            DataTable dataTable = new DataTable("ListaFotos");

            var columnId = new DataColumn("id", typeof(int));
            columnId.AllowDBNull = true;
            dataTable.Columns.Add(columnId);

            dataTable.Columns.Add(new DataColumn("ruta", typeof(string)));
            dataTable.Columns.Add(new DataColumn("descripcion", typeof(string)));
            dataTable.Columns.Add(new DataColumn("portada", typeof(Boolean)));
            dataTable.Columns.Add(new DataColumn("eliminar", typeof(Boolean)));

            foreach (FotosRequest foto in fotos)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = (object) foto.Id ?? DBNull.Value;
                dataRow["ruta"] = foto.Ruta;
                dataRow["descripcion"] = foto.Descripcion;
                dataRow["portada"] = foto.Portada;
                dataRow["eliminar"] = foto.Eliminar;
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class AtractivoCategoriaRequest
    {
        public int? Id { get; set; }
        public int IdCategoria { get; set; }
        public bool Recomendado { get; set; }
        public Boolean Eliminar { get; set; }
        public static DataTable ToDataTable(IList<AtractivoCategoriaRequest> categorias)
        {
            DataTable dataTable = new DataTable("ListaCategorias");
            var columnId = new DataColumn("id", typeof(int));
            columnId.AllowDBNull = true;
            dataTable.Columns.Add(columnId);
            
            dataTable.Columns.Add(new DataColumn("id_categoria", typeof(int)));
            dataTable.Columns.Add(new DataColumn("recomendado", typeof(Boolean)));
            dataTable.Columns.Add(new DataColumn("eliminar", typeof(Boolean)));

            foreach (AtractivoCategoriaRequest categoria in categorias)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = (object) categoria.Id ?? DBNull.Value;
                dataRow["id_categoria"] = categoria.IdCategoria;
                dataRow["recomendado"] = categoria.Recomendado;
                dataRow["eliminar"] = categoria.Eliminar;
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
    
    public class EtiquetaRequest
    {
        public int? Id { get; set; }
        public string Etiqueta { get; set; }
        public Boolean Eliminar { get; set; }

        public static DataTable ToDataTable(IList<EtiquetaRequest> etiquetas)
        {
            DataTable dataTable = new DataTable("AT_ListaEtiquetas");
            var columnId = new DataColumn("id", typeof(int));
            columnId.AllowDBNull = true;
            dataTable.Columns.Add(columnId);
            dataTable.Columns.Add(new DataColumn("etiqueta", typeof(string)));
            dataTable.Columns.Add(new DataColumn("eliminar", typeof(Boolean)));

            foreach (EtiquetaRequest etiqueta in etiquetas)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = (object) etiqueta.Id ?? DBNull.Value;
                dataRow["etiqueta"] = etiqueta.Etiqueta;
                dataRow["eliminar"] = etiqueta.Eliminar;
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class MunicipioAtractivoRequest
    {
        public int? Id { get; set; }
        public int IdMunicipio { get; set; }
        public Boolean Eliminar { get; set; }

        public static DataTable ToDataTable(IList<MunicipioAtractivoRequest> municipios)
        {
            DataTable dataTable = new DataTable("AT_ListaMunicipios");
            var columnId = new DataColumn("id", typeof(int));
            columnId.AllowDBNull = true;
            dataTable.Columns.Add(columnId);
            dataTable.Columns.Add(new DataColumn("id_municipio", typeof(int)));
            dataTable.Columns.Add(new DataColumn("eliminar", typeof(Boolean)));

            if (municipios == null)
            {
                return dataTable;
            }

            foreach (MunicipioAtractivoRequest municipio in municipios)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = (object)municipio.Id ?? DBNull.Value;
                dataRow["id_municipio"] = municipio.IdMunicipio;
                dataRow["eliminar"] = municipio.Eliminar;
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class UpdateAtractivoRequest: CreateAtractivoRequest
    {
        public int Id { get; set; }
        
    }
}
