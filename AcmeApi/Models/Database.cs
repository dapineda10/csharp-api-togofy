using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Configuration;

namespace AcmeApi.Models
{
    public class Database
    {
        string conexion = "";
        private SQLiteConnection cadenaconexion = null;

        public SQLiteConnection Cadenaconexion { get => cadenaconexion; set => cadenaconexion = value; }

        public Database() {

            conexion = WebConfigurationManager.AppSettings["path"];
        }

        public void OpenConnection() {
            cadenaconexion = new SQLiteConnection($"Data Source={conexion};foreign keys=true");
            cadenaconexion.Open();
        }

        public void CloseConnection() {
            cadenaconexion.Close();
        }

        public IEnumerable<Dictionary<string, object>> SearchById(string table, string field,int id) {

            OpenConnection();
            try
            {
                var results = new List<Dictionary<string, object>>();
                string consulta = $"select * from {table} where {field} = {id};";
                var cols = new List<string>();
                SQLiteCommand comando = new SQLiteCommand(consulta, cadenaconexion);
                SQLiteDataReader datos = comando.ExecuteReader();

                string ss = datos.GetName(1);
                for (var i = 0; i < datos.FieldCount; i++)
                    cols.Add(datos.GetName(i));

                while (datos.Read())
                    results.Add(SerializeRow(cols, datos));

                return results;
            }
            catch (Exception)
            {
                throw new ArgumentException("No fue posible consultar la base de datos");
            }
            finally {
                cadenaconexion.Close();
            }
        }

        public Dictionary<string, object> SerializeRow(IEnumerable<string> cols,SQLiteDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        public IEnumerable<Dictionary<string, object>> GetAllValues(string table)
        {
            OpenConnection();
            try
            {
                var results = new List<Dictionary<string, object>>();
                string consulta = $"select * from {table};";
                var cols = new List<string>();
                SQLiteCommand comando = new SQLiteCommand(consulta, cadenaconexion);
                SQLiteDataReader datos = comando.ExecuteReader();

                string ss = datos.GetName(1);
                for (var i = 0; i < datos.FieldCount; i++)
                    cols.Add(datos.GetName(i));

                while (datos.Read())
                    results.Add(SerializeRow(cols, datos));

                return results;
            }
            catch (Exception e)
            {
                throw new ArgumentException("No fue posible consultar la base de datos " + e);
            }
            finally
            {
                cadenaconexion.Close();
            }
        }

        public string Delete(string table,string field,int id)
        {
            Database db = new Database();
            
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                cmd.CommandText = $"select count(*) from {table} where {field} = {id}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    cmd.CommandText = $"DELETE FROM {table} where {field} = {id}";
                    cmd.ExecuteNonQuery();
                    return "Se Eliminó correctamente el registro";
                }
                else
                {
                    return "No existe un registro con ese valor";
                }


            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No fue posible eliminar el registro, la actualización generó el siguiente error {ex.Message}");
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public bool CheckIfValueExist(string table, string field, int id)
        {

            OpenConnection();
            try
            {
                Database db = new Database();
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Consulta la sala y si no está ocupada
                cmd.CommandText = $"select count(*) from {table} where {field} = {id}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("No fue posible si el valor existe");
            }
            finally
            {
                cadenaconexion.Close();
            }
        }

    }
}