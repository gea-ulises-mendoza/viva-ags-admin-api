using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection;

namespace VivaAguascalientesAPI.DAO
{
    public class DataAccessModelService
    {
        protected readonly SqlConnection _connection;

        public DataAccessModelService()
        {
            _connection = DbContext.CreateConnection();
        }

        public List<string> GetResultJson<TObject>(string procedure, TObject actualModel)
        {
            List<string> responseModel = new List<string>();

            using (_connection)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = procedure;
                    command.CommandType = CommandType.StoredProcedure;

                    if (actualModel != null)
                        SetParameters<TObject>(command, actualModel);

                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            do
                            {
                                var readerSerealize = Serialize(reader);
                                responseModel.Add(JsonConvert.SerializeObject(readerSerealize));
                            } while (reader.NextResult());
                        }
                        catch {/* Ignore exceptions */ }
                    }
                }
            }
            return responseModel;
        }

        public TModel GetModel<TModel, TObject>(string procedure, TObject actualModel)
            where TModel : class, new()
        {
            var results = this.GetListModel<TModel, TObject>(procedure, actualModel);
            if (results.Count != 1)
            {
                return default(TModel);
            }
            return results.First();
        }

        public IList<TModel> GetListModel<TModel, TObject>(string procedure, TObject actualModel)
            where TModel : class, new()
        {
            var responseModel = new List<TModel>();
            using (_connection)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = procedure;
                    command.CommandType = CommandType.StoredProcedure;

                    if (actualModel != null)
                    {
                        SetParameters<TObject>(command, actualModel);
                    }
                    if (_connection.State != ConnectionState.Open) {
                        _connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        responseModel.AddRange(ReaderToList<TModel>(reader, null));
                    }
                }
            }
            return responseModel;
        }

        public int ExecuteNonQueryModel<TObject>(string procedure, TObject actualModel) where TObject : new()
        {
            using (_connection)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = procedure;
                    command.CommandType = CommandType.StoredProcedure;
                    SetParameters<TObject>(command, actualModel);
                    
                    SqlParameter returnParameter = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    try
                    {
                        _connection.Open();
                        command.ExecuteNonQuery();
                    } catch (Exception ex)
                    {
                        throw ex;
                    }
                    int returnValue = (int)returnParameter.Value;

                    return returnValue;
                }
            }
        }

        #region PrivateMethod

        private void SetParameters<TObject>(SqlCommand command, TObject model)
        {
            var inAttributes = model.GetType().GetProperties();

            foreach (var inAttribute in inAttributes)
            {
                if (inAttribute.PropertyType.Name == "String")
                {
                    if (inAttribute.GetValue(model) != null)
                    {
                        char[] value = ((string) inAttribute.GetValue(model)).ToCharArray();
                        if (value.Count() > 256)
                        {
                            command.Parameters.Add("@" + inAttribute.Name, SqlDbType.Text);
                        }
                        else
                        {
                            command.Parameters.Add("@" + inAttribute.Name, SqlDbType.NVarChar, 256);
                        }
                        command.Parameters["@" + inAttribute.Name].Value = inAttribute.GetValue(model);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@" + inAttribute.Name, inAttribute.GetValue(model) ?? DBNull.Value);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@" + inAttribute.Name, inAttribute.GetValue(model) ?? DBNull.Value);
                }
            }
        }

        private void SetParametersOutput<TOutput>(SqlCommand command, TOutput ParamOut)
        {
            var OutputAttributes = ParamOut.GetType().GetProperties();
            foreach (var outputAttribute in OutputAttributes)
            {

                var parameter = new SqlParameter("@" + outputAttribute.Name, outputAttribute.GetValue(ParamOut));
                parameter.Direction = ParameterDirection.Output;
                parameter.DbType = SqlHelper.GetDbType(outputAttribute.PropertyType);
                if (parameter.DbType == DbType.Decimal || parameter.DbType == DbType.Double)
                {
                    parameter.Precision = 4;
                }
                if (parameter.DbType == DbType.String)
                {
                    parameter.Size = -1;
                }
                command.Parameters.Add(parameter);


            }
        }

        private IEnumerable<TModel> ReaderToList<TModel>(SqlDataReader readerSQL, TModel model)
            where TModel : class, new()
        {
            var responseModel = new List<TModel>();
            do
            {
                bool errorCatch = true;

                while (readerSQL.Read())
                {
                    TModel modelo = new TModel();
                    var properties = modelo.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        try
                        {
                            var obj = GetNullableValue(readerSQL, property);
                            property.SetValue(modelo, obj);
                            errorCatch = false;
                        }
                        catch (Exception ex)
                        {
                            errorCatch = false;
                            continue;
                            /* Ignore exceptions */
                        }
                    }
                    if (!errorCatch)
                        responseModel.Add(modelo);
                }

            } while (readerSQL.NextResult());

            return responseModel;
        }

        private object GetNullableValue(SqlDataReader reader, PropertyInfo property)
        {
            var t = property.PropertyType;

            if (reader[property.Name] == null || reader[property.Name] is DBNull)
                return null;

            if (property.PropertyType.Name.Equals(typeof(Nullable<>).Name))
            {
                t = Nullable.GetUnderlyingType(t);
                return Convert.ChangeType(reader[property.Name], t);
            }

            return Convert.ChangeType(reader[property.Name], property.PropertyType);
        }

        private IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }

        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        #endregion

        #region Helper
        public static class SqlHelper
        {
            private static Dictionary<Type, DbType> typeMap;
            // Create and populate the dictionary in the static constructor
            static SqlHelper()
            {
                typeMap = new Dictionary<Type, DbType>();
                typeMap[typeof(string)] = DbType.String;
                typeMap[typeof(char[])] = DbType.String;
                typeMap[typeof(byte)] = DbType.Byte;
                typeMap[typeof(short)] = DbType.Int16;
                typeMap[typeof(int)] = DbType.Int32;
                typeMap[typeof(long)] = DbType.Int64;
                typeMap[typeof(byte[])] = DbType.Binary;
                typeMap[typeof(bool)] = DbType.Boolean;
                typeMap[typeof(DateTime)] = DbType.DateTime;
                typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
                typeMap[typeof(double)] = DbType.Double;
                typeMap[typeof(decimal)] = DbType.Double;
                typeMap[typeof(float)] = DbType.Double;
                typeMap[typeof(double)] = DbType.Double;
                typeMap[typeof(TimeSpan)] = DbType.Time;
                /* ... and so on ... */
            }
            // Non-generic argument-based method
            public static DbType GetDbType(Type giveType)
            {
                // Allow nullable types to be handled
                giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;
                if (typeMap.ContainsKey(giveType))
                {
                    return typeMap[giveType];
                }
                throw new ArgumentException($"{giveType.FullName} is not a supported .NET class");
            }
            // Generic version
            public static DbType GetDbType<T>()
            {
                return GetDbType(typeof(T));
            }
        }
        #endregion
    }
}
