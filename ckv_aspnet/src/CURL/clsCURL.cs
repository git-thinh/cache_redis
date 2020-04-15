using SeasideResearch.LibCurlNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ckv_aspnet
{
    public class clsCURL
    {
        public static string get_raw_http(object p)
        {
            if (p == null)
                p = "http://localhost/";

            string url = p.ToString();
            StringBuilder bi = new StringBuilder();

            try
            {
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                Easy easy = new Easy();
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

        public static string get_text(object p)
        {
            string html = get_raw_https(p);
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

        //static bool curl_inited = false;
        public static string get_raw_https(object p)
        {
            //if (p == null || string.IsNullOrWhiteSpace(p.ToString())) return string.Empty;

            if (p == null)
                p = "https://vnexpress.net/ha-noi-de-xuat-keo-dai-cach-ly-xa-hoi-den-30-4-4084947.html";

            string url = p.ToString();
            try
            {
                //if (curl_inited == false)
                //{
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);
                //    curl_inited = true;
                //}

                Easy easy = new Easy();

                StringBuilder bi = new StringBuilder();
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

        static void stop()
        {
            Curl.GlobalCleanup();
        }
    }
}