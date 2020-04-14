using ChakraHost.Hosting;
using SeasideResearch.LibCurlNet;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ckv_aspnet
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        #region [ CURL ]

        static string curl_get_raw(object p)
        {
            if (p == null) return string.Empty;

            string url = p.ToString(); 

            try
            {
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                Easy easy = new Easy();
                bi.Clear();
                Easy.WriteFunction wf = new Easy.WriteFunction((buf, size, nmemb, extraData) =>
                {
                    string si = Encoding.UTF8.GetString(buf);
                    bi.Append(si);
                    return size * nmemb;
                });

                easy.SetOpt(CURLoption.CURLOPT_URL, url);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                easy.Perform();
                //easy.Cleanup();
                easy.Dispose();

                Curl.GlobalCleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return bi.ToString();
        }

        static string curl_get_text(object p)
        {
            string html = curl_get_raw_https(p);
            string s = html;

            //s = new Regex(@"<script[^>]*>[\s\S]*?</script>").Replace(s, string.Empty);
            //s = new Regex(@"<style[^>]*>[\s\S]*?</style>").Replace(s, string.Empty);
            //s = new Regex(@"<noscript[^>]*>[\s\S]*?</noscript>").Replace(s, string.Empty);
            //s = Regex.Replace(s, @"<meta(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<link(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<use(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<figure(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<!DOCTYPE(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<!--(.|\n)*?-->", string.Empty, RegexOptions.Singleline);

            string title = Regex.Match(html, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value.Trim();
            s = Regex.Replace(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", "[TITLE] " + title, RegexOptions.Singleline);

            s = Regex.Replace(s, @"<a(.|\n)*?>", string.Empty, RegexOptions.Singleline);

            //### Remove any tags but not there content "<p>bob<span> johnson</span></p>" -> "bob johnson"
            // Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
            s = Regex.Replace(s, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", string.Empty, RegexOptions.Singleline);

            //### Remove multi break lines
            //s = Regex.Replace(s, @"[\r\n]+", "<br />");
            //s = Regex.Replace(s, @"[\r\n]{2,}", "<br />");
            s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");

            return s.Trim();
        }

        static StringBuilder bi = new StringBuilder();
        static bool curl_inited = false;
        static string curl_get_raw_https(object p)
        {
            if (p == null) return string.Empty;

            string url = p.ToString();
            try
            {
                if (curl_inited == false)
                {
                    Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);
                    curl_inited = true;
                }

                Easy easy = new Easy();

                bi.Clear();
                Easy.WriteFunction wf = new Easy.WriteFunction((buf, size, nmemb, extraData) =>
                {
                    string si = Encoding.UTF8.GetString(buf);
                    bi.Append(si);
                    return size * nmemb;
                });
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                Easy.SSLContextFunction sf = new Easy.SSLContextFunction((ctx, extraData) => CURLcode.CURLE_OK);
                easy.SetOpt(CURLoption.CURLOPT_SSL_CTX_FUNCTION, sf);

                easy.SetOpt(CURLoption.CURLOPT_URL, url);
                //easy.SetOpt(CURLoption.CURLOPT_CAINFO, "ca-bundle.crt");
                easy.SetOpt(CURLoption.CURLOPT_CAINFO, @"D:\cache_redis\ckv_aspnet\bin\ca-bundle.crt");

                easy.Perform();
                easy.Dispose();

                //Curl.GlobalCleanup();

                string s = bi.ToString();

                //string title = Regex.Match(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                s = new Regex(@"<script[^>]*>[\s\S]*?</script>").Replace(s, string.Empty);
                s = new Regex(@"<style[^>]*>[\s\S]*?</style>").Replace(s, string.Empty);
                s = new Regex(@"<noscript[^>]*>[\s\S]*?</noscript>").Replace(s, string.Empty);
                s = Regex.Replace(s, @"<meta(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<link(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<use(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<figure(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<!DOCTYPE(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<!--(.|\n)*?-->", string.Empty, RegexOptions.Singleline);

                s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");

                return s;
            }
            catch (Exception ex)
            {
                return "ERR: " + ex.Message;
            }
        }

        static void curl_stop()
        {
            Curl.GlobalCleanup();
        }

        #endregion

        static string js_chakra_run(string body_function)
        {
            string script = "(()=>{return \'Hello world!\';})()";
            //string script = "(()=>{ try{ " + body_function + " }catch(e){ return 'ERR:'+e.message; } })()";
            //var result = JavaScriptContext.RunScript(script, js_currentSourceContext++, string.Empty);
            //string v = result.ConvertToString().ToString();

            JavaScriptRuntime runtime;
            JavaScriptContext context;
            JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
            JavaScriptValue result;

            // Create a runtime. 
            Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime);

            // Create an execution context. 
            Native.JsCreateContext(runtime, out context);

            // Now set the execution context as being the current one on this thread.
            Native.JsSetCurrentContext(context);

            //Native.js ("ChakraBridge");

            // Run the script.
            Native.JsRunScript(script, currentSourceContext++, "", out result);

            // Convert your script result to String in JavaScript; redundant if your script returns a String
            JavaScriptValue resultJSString;
            Native.JsConvertValueToString(result, out resultJSString);

            // Project script result in JS back to C#.
            IntPtr resultPtr;
            UIntPtr stringLength;
            Native.JsStringToPointer(resultJSString, out resultPtr, out stringLength);

            string v = Marshal.PtrToStringUni(resultPtr);

            // Dispose runtime
            Native.JsSetCurrentContext(JavaScriptContext.Invalid);
            Native.JsDisposeRuntime(runtime);

            //JavaScriptValue result;
            //Native.JsRunScript(script, js_currentSourceContext++, "", out result);
            //string v = result.ConvertToString().ToString();
            return v;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string s = "";
            if (Request.Url.AbsolutePath == "/api")
            {
                Response.Clear();
                Response.Write(DateTime.Now.ToString());
                Response.End();
            }
            else if (Request.Url.AbsolutePath == "/js_chakra")
            {
                s = js_chakra_run("https://vnexpress.net/trump-noi-gian-voi-truyen-thong-my-4084322.html");
                Response.Clear();
                Response.Write(s);
                Response.End();
            }
            else if (Request.Url.AbsolutePath == "/curl-https")
            {
                s = curl_get_raw_https("https://vnexpress.net/trump-noi-gian-voi-truyen-thong-my-4084322.html");
                Response.Clear();
                Response.Write(s);
                Response.End();
            }
            else if (Request.Url.AbsolutePath == "/curl-http")
            {
                s = curl_get_raw("http://localhost:8085/_cms/2000/");
                Response.Clear();
                Response.Write(s);
                Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}