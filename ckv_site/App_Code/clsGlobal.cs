using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ckv_site
{
    public class clsGlobal : System.Web.HttpApplication
    {
        static ICache m_cache;
        static IApi m_api;
        protected void Application_Start(object sender, EventArgs e)
        {
            _CONFIG._init();
            m_cache = new clsCache();
            m_api = new clsApi(m_cache);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Uri uri = Request.Url;

            string path = uri.AbsolutePath.Substring(1);
            if (path == "favicon.ico") { Response.End(); return; }
            string[] a = path.Split('/');

            string domain = uri.Host;
            if (domain == "localhost" || domain == "127.0.0.1") domain = _CONFIG.DOMAIN_LOCALHOST;

            //[1] api/{cache_name}/{do_action}/{id}
            if (a[0] == "api" && a.Length > 2)
            {
                Response.ContentType = "application/json";
                if (m_api.cache_exist(a[1]))
                {
                    Response.Write(JsonConvert.SerializeObject(oResult.Error("Cannot found cache name: " + a[1])));
                    Response.End();
                    return;
                }

                oResult rv = null;
                oResult rp = get_api_parameters();
                if (rp.ok)
                    rv = m_api.execute(a[1], a[2], string.Empty, (Dictionary<string, object>)rp.data);
                else
                    rv = rp;

                Response.Write(JsonConvert.SerializeObject(rv));
                Response.End();
                return;
            }

            //[2] Files
            string file = string.Empty;
            if (path.Length == 0) path = "index.html";
            if (path.EndsWith(".html")) path = "_site\\" + domain + "\\" + path;
            if (path == "admin") path = "admin.html";

            string contentType = MimeMapping.GetMimeMapping(path);
            if (path[0] != '_' // break dirs: _lib, _view,..
                && contentType != "text/html" // break files *.html
                && Request.UrlReferrer != null) // Ref from other request as request on page html
                path = "_site\\" + domain + "\\" + path;

            file = Path.Combine(_CONFIG.PATH_ROOT, path).Replace("/", "\\");

            if (File.Exists(file))
            {
                Response.ContentType = contentType;
                Response.TransmitFile(file);
            }
            else Response.StatusCode = 404;

            Response.End();
        }

        oResult get_api_parameters()
        {
            oResult r = oResult.Error();

            string path = Request.Url.AbsolutePath.Substring(1).ToLower();
            Dictionary<string, object> para = new Dictionary<string, object>() {
                { "___domain", Request.Url.Host },
                { "___port", Request.Url.Port },
                { "___url", path },
                { "___method", Request.HttpMethod },
                { "___token", string.Empty },
            };

            try
            {
                var a = Request.QueryString.Keys.Cast<string>().ToArray();
                foreach (var key in a) if (!para.ContainsKey(key)) para.Add(key, Uri.UnescapeDataString(Request.QueryString[key]));

                if (Request.HttpMethod == "POST")
                {
                    string s = new StreamReader(Request.InputStream).ReadToEnd();
                    if (s == null || string.IsNullOrWhiteSpace(s))
                        return oResult.Error("Body of POST is not null or emtpy");
                    else
                    {
                        s = s.Trim();
                        if (s.Length > 1 && s[0] == '{' && s[s.Length - 1] == '}')
                        {
                            try
                            {
                                var d = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
                                foreach (var kv in d) para.Add(kv.Key, kv.Value);
                            }
                            catch (Exception e)
                            {
                                return oResult.Error("Convert to json of body error: " + e.Message);
                            }
                        }
                        else
                        {
                            para.Add("___BODY", s);
                        }
                    }
                }

                r.ok = true;
                r.data = para;
            }
            catch (Exception e)
            {
                r.error = "ERROR[clsRouter.get_api_parameters()] " + e.Message;
            }

            return r;
        }
    }
}