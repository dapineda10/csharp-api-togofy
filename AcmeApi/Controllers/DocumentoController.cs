using AcmeApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcmeApi.Controllers
{
    public class DocumentoController : Controller
    {
        private const string TABLE = "documento";
        private const string ID = "id_documento";

        // GET: Documento/GetAll/
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

        // GET: Documento/Get/5
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

        // POST: Documento/Create
        [HttpPost]
        public JObject Create(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Documento documento = new Documento();
                respuesta["response"] = documento.Create(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Documento/Edit
        [HttpPost]
        public JObject Edit(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Documento documento = new Documento();
                respuesta["response"] = documento.Update(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Documento/Delete
        [HttpPost]
        public JObject Delete(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Documento documento = new Documento();
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