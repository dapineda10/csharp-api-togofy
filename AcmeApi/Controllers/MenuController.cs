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
    public class MenuController : Controller
    {
        private const string TABLE= "menu";
        private const string ID = "id_menu";
        // GET: Menu/GetAll/
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

        // GET: Menu/Get/5
        public JArray Get(int id)
        {
            JArray resp = new JArray();
            JObject respuesta = new JObject();
            try
            {
                JObject a = new JObject();
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

        // POST: Menu/Create
        [HttpPost]
        public JObject Create(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Menu menu = new Menu();
                respuesta["response"] = menu.Create(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Menu/Edit
        [HttpPost]
        public JObject Edit(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Menu menu = new Menu();
                respuesta["response"] = menu.Update(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
}

        // POST: Menu/Delete
        [HttpPost]
        public JObject Delete(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Menu menu = new Menu();
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
