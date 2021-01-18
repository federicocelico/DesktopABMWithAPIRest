using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Activities;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Datos
{
    public class Request
    {
        public async Task<string> GetProducts()
        {
            try
            {
                string url = "http://localhost:55086/api/Productos";
                WebRequest oRequest = WebRequest.Create(url);
                WebResponse oResponse = oRequest.GetResponse();
                StreamReader sr = new StreamReader(oResponse.GetResponseStream());
                return await sr.ReadToEndAsync();
            }
            catch (Exception)
            {

                throw;
            }
       
        }

        public bool ABMProducts<T>(T producto, string method)
        {
            string url = "";
            try
            { 
                if(method.Equals("PUT") || method.Equals("DELETE"))
                {
                    System.Type type = producto.GetType();
                    int idArticulo = (int)type.GetProperty("idArticulo").GetValue(producto, null);
                    url = "http://localhost:55086/api/Productos/" + idArticulo;
                }
                else
                {
                    url = "http://localhost:55086/api/Productos";
                }
               

                JavaScriptSerializer js = new JavaScriptSerializer();

              
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(producto);

            
                WebRequest request = WebRequest.Create(url);
              
                request.Method = method;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                   
                  streamReader.ReadToEnd();
                }
            
                    return true;

            }
            catch (Exception e)
            {

                return false;

            }

    
        }
          
    }

    
}
