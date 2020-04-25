﻿// https://github.com/prabirshrestha/simple-owin
// Compatible with OWIN 1.0 and WebSocket 0.4.0

namespace ckv_site
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Routing;
    using System.Security.Cryptography.X509Certificates;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    /// <summary>
    /// SimpleOwin ASP.NET Router Handler
    /// </summary>
    public class SimpleOwinAspNetRouteHandler : IRouteHandler
    {
        private readonly SimpleOwinAspNetHandler simpleOwinAspNetHandler;

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetRouteHandler"/>.
        /// </summary>
        /// <param name="app">The owin app.</param>
        public SimpleOwinAspNetRouteHandler(AppFunc app)
            : this(app, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetRouteHandler"/>.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="root">The root path.</param>
        public SimpleOwinAspNetRouteHandler(AppFunc app, string root)
        {
            this.simpleOwinAspNetHandler = new SimpleOwinAspNetHandler(app, root);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetRouteHandler"/>.
        /// </summary>
        /// <param name="apps">The owin apps.</param>
        public SimpleOwinAspNetRouteHandler(IEnumerable<Func<AppFunc, AppFunc>> apps)
            : this(apps, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetRouteHandler"/>.
        /// </summary>
        /// <param name="apps">The owin apps.</param>
        /// <param name="root">The root path.</param>
        public SimpleOwinAspNetRouteHandler(IEnumerable<Func<AppFunc, AppFunc>> apps, string root)
        {
            this.simpleOwinAspNetHandler = new SimpleOwinAspNetHandler(apps, root);
        }

        /// <summary>
        /// Gets the SimpleOwin ASP.NET http handler.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns>The SimpleOwin ASP.NET http handler.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return simpleOwinAspNetHandler;
        }
    }

    /// <summary>
    /// The SimpleOwin ASP.NET http async handler.
    /// </summary>
    public class SimpleOwinAspNetHandler : IHttpAsyncHandler
    {
        private readonly AppFunc appFunc;
        private readonly string root;

        private static readonly Task CompletedTask;

        static SimpleOwinAspNetHandler()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.TrySetResult(0);
            CompletedTask = tcs.Task;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetHandler"/>.
        /// </summary>
        /// <param name="app">The owin app.</param>
        public SimpleOwinAspNetHandler(AppFunc app)
            : this(app, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetHandler"/>.
        /// </summary>
        /// <param name="app">The owin app.</param>
        /// <param name="root">The root path.</param>
        public SimpleOwinAspNetHandler(AppFunc app, string root)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            this.appFunc = app;
            if (!string.IsNullOrWhiteSpace(root))
            {
                if (!root.StartsWith("/"))
                {
                    this.root += "/" + root;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetHandler"/>.
        /// </summary>
        /// <param name="apps">The owin apps.</param>
        public SimpleOwinAspNetHandler(IEnumerable<Func<AppFunc, AppFunc>> apps)
            : this(apps, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleOwinAspNetHandler"/>.
        /// </summary>
        /// <param name="apps">The owin apps.</param>
        /// <param name="root">The root path.</param>
        public SimpleOwinAspNetHandler(IEnumerable<Func<AppFunc, AppFunc>> apps, string root)
            : this(ToOwinApp(apps), root)
        {
        }

        /// <summary>
        /// Converts apps to owin app.
        /// </summary>
        /// <param name="apps">The apps.</param>
        /// <returns>The owin app.</returns>
        public static AppFunc ToOwinApp(IEnumerable<Func<AppFunc, AppFunc>> apps)
        {
            if (apps == null)
                throw new ArgumentNullException("apps");

            return
                env =>
                {
                    var enumerator = apps.GetEnumerator();
                    AppFunc next = null;
                    next = env2 => enumerator.MoveNext() ? enumerator.Current(env3 => next(env3))(env2) : CompletedTask;
                    return next(env);
                };
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotSupportedException();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object state)
        {
            return BeginProcessRequest(new HttpContextWrapper(context), callback, state);
        }

        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.
        /// </returns>
        /// <param name="context">An <see cref="T:System.Web.HttpContextBase"/> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback"/> to call when the asynchronous method call is complete. If <paramref name="callback"/> is null, the delegate is not called. </param>
        /// <param name="state">Any extra data needed to process the request. </param>
        public IAsyncResult BeginProcessRequest(HttpContextBase context, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<Action>(state);
            if (callback != null)
            {
                tcs.Task.ContinueWith(task => callback(task), TaskContinuationOptions.ExecuteSynchronously);
            }

            var request = context.Request;
            var response = context.Response;
            response.BufferOutput = false;

            var pathBase = request.ApplicationPath;
            if (pathBase == "/" || pathBase == null)
            {
                pathBase = string.Empty;
            }

            if (root != null)
            {
                pathBase += root;
            }

            var path = request.Path;
            if (path.StartsWith(pathBase))
            {
                path = path.Substring(pathBase.Length);
            }

            var serverVarsToAddToEnv = request.ServerVariables.AllKeys
                .Where(key => !key.StartsWith("HTTP_") && !string.Equals(key, "ALL_HTTP") && !string.Equals(key, "ALL_RAW"))
                .Select(key => new KeyValuePair<string, object>(key, request.ServerVariables.Get(key)));

            var env = new Dictionary<string, object>();
            env[OwinConstants.Version] = "1.0";
            env[OwinConstants.RequestMethod] = request.HttpMethod;
            env[OwinConstants.RequestScheme] = request.Url.Scheme;
            env[OwinConstants.RequestPathBase] = pathBase;
            env[OwinConstants.RequestPath] = path;
            env[OwinConstants.RequestQueryString] = request.ServerVariables["QUERY_STRING"];
            env[OwinConstants.RequestProtocol] = request.ServerVariables["SERVER_PROTOCOL"];
            env[OwinConstants.RequestBody] = request.InputStream;
            env[OwinConstants.RequestHeaders] = request.Headers.AllKeys
                    .ToDictionary(x => x, x => request.Headers.GetValues(x), StringComparer.OrdinalIgnoreCase);

            if (request.ClientCertificate != null && request.ClientCertificate.Certificate.Length != 0)
            {
                env[OwinConstants.ClientCertificate] = new X509Certificate(request.ClientCertificate.Certificate);
            }
            
            env[OwinConstants.CallCancelled] = CancellationToken.None;

            env[OwinConstants.ResponseHeaders] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

            int? responseStatusCode = null;

            env[OwinConstants.ResponseBody] =
                new TriggerStream(response.OutputStream)
                {
                    OnFirstWrite = () =>
                    {
                        responseStatusCode = Get<int>(env, OwinConstants.ResponseStatusCode, 200);
                        response.StatusCode = responseStatusCode.Value;

                        object reasonPhrase;
                        if (env.TryGetValue(OwinConstants.ResponseReasonPhrase, out reasonPhrase))
                            response.StatusDescription = Convert.ToString(reasonPhrase);

                        var responseHeaders = Get<IDictionary<string, string[]>>(env, OwinConstants.ResponseHeaders, null);
                        if (responseHeaders != null)
                        {
                            foreach (var responseHeader in responseHeaders)
                            {
                                foreach (var headerValue in responseHeader.Value)
                                    response.AddHeader(responseHeader.Key, headerValue);
                            }
                        }
                    }
                };

            SetEnvironmentServerVariables(env, serverVarsToAddToEnv);

            env[OwinConstants.HttpContextBase] = context;


            try
            {
                appFunc(env)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            tcs.TrySetException(t.Exception.InnerExceptions);
                        }
                        else if (t.IsCanceled)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            tcs.TrySetResult(() => { });
                        }

                    });
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }

        public void EndProcessRequest(IAsyncResult asyncResult)
        {
            var task = ((Task<Action>)asyncResult);
            if (task.IsFaulted)
            {
                var exception = task.Exception;
                exception.Handle(ex => ex is HttpException);
            }
            else if (task.IsCompleted)
            {
                task.Result.Invoke();
            }
        }

        [DebuggerStepThrough]
        private static void SetEnvironmentServerVariables(Dictionary<string, object> env, IEnumerable<KeyValuePair<string, object>> serverVarsToAddToEnv)
        {
            foreach (var kv in serverVarsToAddToEnv)
                env["server." + kv.Key] = kv.Value;
        }

        private static T Get<T>(IDictionary<string, object> env, string key, T defaultValue)
        {
            object value;
            return env.TryGetValue(key, out value) && value is T ? (T)value : defaultValue;
        }

        /// <summary>
        /// Gets the owin startup properties.
        /// </summary>
        /// <returns>The owin statup properties.</returns>
        public static IDictionary<string, object> GetStartupProperties()
        {
            var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            properties[OwinConstants.Version] = "1.0";

            return properties;
        }


        /// <remarks>TriggerStream pulled from Gate source code.</remarks>
        private class TriggerStream : System.IO.Stream
        {
            public TriggerStream(System.IO.Stream innerStream)
            {
                InnerStream = innerStream;
            }

            public System.IO.Stream InnerStream { get; set; }

            public Action OnFirstWrite { get; set; }

            private bool IsStarted { get; set; }

            public override bool CanRead
            {
                get { return InnerStream.CanRead; }
            }

            public override bool CanWrite
            {
                get { return InnerStream.CanWrite; }
            }

            public override bool CanSeek
            {
                get { return InnerStream.CanSeek; }
            }

            public override bool CanTimeout
            {
                get { return InnerStream.CanTimeout; }
            }

            public override int WriteTimeout
            {
                get { return InnerStream.WriteTimeout; }
                set { InnerStream.WriteTimeout = value; }
            }

            public override int ReadTimeout
            {
                get { return InnerStream.ReadTimeout; }
                set { InnerStream.ReadTimeout = value; }
            }

            public override long Position
            {
                get { return InnerStream.Position; }
                set { InnerStream.Position = value; }
            }

            public override long Length
            {
                get { return InnerStream.Length; }
            }

            public override void Close()
            {
                InnerStream.Close();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    InnerStream.Dispose();
                }
            }

            public override string ToString()
            {
                return InnerStream.ToString();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return InnerStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                InnerStream.SetLength(value);
            }

            public override int ReadByte()
            {
                return InnerStream.ReadByte();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return InnerStream.Read(buffer, offset, count);
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                return InnerStream.BeginRead(buffer, offset, count, callback, state);
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                return InnerStream.EndRead(asyncResult);
            }

            public override void WriteByte(byte value)
            {
                Start();
                InnerStream.WriteByte(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                Start();
                InnerStream.Write(buffer, offset, count);
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                Start();
                return InnerStream.BeginWrite(buffer, offset, count, callback, state);
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                InnerStream.EndWrite(asyncResult);
            }

            public override void Flush()
            {
                Start();
                InnerStream.Flush();
            }

            private void Start()
            {
                if (!IsStarted)
                {
                    IsStarted = true;
                    if (OnFirstWrite != null)
                    {
                        OnFirstWrite();
                    }
                }
            }
        }

        private static class OwinConstants
        {
            public const string Version = "owin.Version";
            public const string RequestMethod = "owin.RequestMethod";
            public const string RequestScheme = "owin.RequestScheme";
            public const string RequestPathBase = "owin.RequestPathBase";
            public const string RequestPath = "owin.RequestPath";
            public const string RequestQueryString = "owin.RequestQueryString";
            public const string RequestProtocol = "owin.RequestProtocol";
            public const string RequestBody = "owin.RequestBody";
            public const string RequestHeaders = "owin.RequestHeaders";
            public const string CallCancelled = "owin.CallCancelled";
            public const string ResponseHeaders = "owin.ResponseHeaders";
            public const string ResponseBody = "owin.ResponseBody";
            public const string ResponseStatusCode = "owin.ResponseStatusCode";
            public const string ResponseReasonPhrase = "owin.ResponseReasonPhrase";

            public const string HttpContextBase = "System.Web.HttpContextBase";

            public const string WebSocketVersion = "websocket.Version";
            public const string WebSocketAcceptKey = "websocket.Accept";
            public const string SecWebSocketProtocol = "Sec-WebSocket-Protocol";
            public const string WebSocketSubProtocolKey = "websocket.SubProtocol";
            public const string WebSocketSendAsyncKey = "websocket.SendAsync";
            public const string WebSocketReceiveAsyncKey = "websocket.ReceiveAsync";
            public const string WebSocketCloseAsyncKey = "websocket.CloseAsync";
            public const string WebSocketCallCancelled = "websocket.CallCancelled";

            public const string WebSocketContext = "System.Net.WebSockets.WebSocketContext";

            public const string ClientCertificate = "ssl.ClientCertificate";
        }
    }
}