using AcmeApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcmeApi.Controllers
{
    public class ResponsableController : Controller
    {

        private const string TABLE = "responsable";
        private const string ID = "id_responsable";

        // GET: Responsable/GetAll/
        public JArray GetAll()
        {
            JArray resp = new JArray();
            JObject respuesta = new JObject();
            try
            {
                Database db = new Database();
                var values = db.GetAllValues(TABLE);
                JValue valores = (JValue)JsonConvert.SerializeObject(values);

                if (valores.Value.ToString() == "[]")
                {
                    respuesta["response"] = "No hay información en base de datos";
                    resp.Add(respuesta);
                    return resp;
                }
                else
                {
                    return JArray.Parse(valores.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                resp.Add(respuesta);
                return resp;
            }
        }

        // GET: Responsable/Get/5
        public JArray Get(int id)
        {
            JArray resp = new JArray();
            JObject respuesta = new JObject();
            try
            {
                Database db = new Database();
                var values = db.SearchById(TABLE, ID, id);
                JValue valores = (JValue)JsonConvert.SerializeObject(values);
                if (valores.Value.ToString() == "[]")
                {
                    respuesta["response"] = "No hay información en base de datos";
                    resp.Add(respuesta);
                    return resp;
                }
                else
                {
                    return JArray.Parse(valores.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                resp.Add(respuesta);
                return resp;
            }
        }

        // POST: Responsable/Create
        [HttpPost]
        public JObject Create(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Responsable responsable = new Responsable();
                respuesta["response"] = responsable.Create(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Responsable/Edit
        [HttpPost]
        public JObject Edit(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Responsable responsable = new Responsable();
                respuesta["response"] = responsable.Update(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Responsable/Delete
        [HttpPost]
        public JObject Delete(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Responsable responsable = new Responsable();
                Database db = new Database();
                JObject json = JObject.Parse(data);
                respuesta["response"] = db.Delete(TABLE, ID, Convert.ToInt32(json[$"{ID}"]));
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }

        }
    }
}
