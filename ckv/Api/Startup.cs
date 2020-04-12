using Jose;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ckv
{

    public class Startup
    {
        static string readFileFromManifestResourceStream(string fileName)
        { 
            string comName = fileName.Split(',')[0];
            string resourceName = @"UI\" + comName + ".dll";
            var assembly = Assembly.GetExecutingAssembly();
            resourceName = typeof(App).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    byte[] buffer = new byte[stream.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            ms.Write(buffer, 0, read);
                        buffer = ms.ToArray();
                    }
                    string s = System.Text.Encoding.UTF8.GetString(buffer).Trim();
                    return s;
                }
            }
            return string.Empty;
        }

        public void Configuration(IAppBuilder app)
        {
            //app.Map("/", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("OK: " + DateTime.Now.ToString());
            //    });
            //});

            #region [ UI ]

            //app.Map("/index.html", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/w2ui.min.css", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/w2ui.min.js", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/style.css", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/index.js", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            #endregion

            app.Map("/token", (iab) =>
            {
                iab.Run(context =>
                {
                    context.Response.ContentType = "text/plain";
                    var payload = new Dictionary<string, object>()
                    {
                        { "sub", "mr.thinh@iot.vn" },
                        { "exp", 1300819380 }
                    };
                    var secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };
                    string token = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
                    return context.Response.WriteAsync(token);
                });
            });
            app.Map("/admin", (iab) =>
            {
                iab.Run(context =>
                {
                    context.Response.ContentType = "text/plain";
                    return context.Response.WriteAsync("This is admin page");
                });
            });


            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("OK: " + DateTime.Now.ToString());
            });

            //app.UseFileServer(true);
            //var options = new FileServerOptions
            //{
            //    EnableDirectoryBrowsing = true,
            //    EnableDefaultFiles = true,
            //    DefaultFilesOptions = { DefaultFileNames = { "index.html" } },
            //    FileSystem = new PhysicalFileSystem("ui"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //};
            //app.UseFileServer(options);
            //app.UseFileServer(new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //    RequestPath = new PathString("/valid_add"),
            //    FileSystem = new PhysicalFileSystem("valid_add"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //});
            //app.UseFileServer(new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //    RequestPath = new PathString("/schema"),
            //    FileSystem = new PhysicalFileSystem("schema"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //});
            //app.UseFileServer(new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //    RequestPath = new PathString("/sql"),
            //    FileSystem = new PhysicalFileSystem(@"config\sql"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //});
        }

    }



}
