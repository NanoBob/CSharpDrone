using Drone.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core
{
    public class WebServer
    {
        readonly HttpListener listener;
        private bool running;
        private readonly Dictionary<string, Func<HttpListenerContext, dynamic>> actions;
        private IAuthorizationHandler authorizationHandler;

        public WebServer(string protocol = "http", string domain = "*", int port = 80)
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"{protocol}://{domain}:{port}/");

            this.running = false;
            this.actions = new Dictionary<string, Func<HttpListenerContext, dynamic>>();
        }

        public void Start()
        {
            listener.Start();

            running = true;

            Task.Run(async () =>
            {
                while (running)
                {
                    var context = await listener.GetContextAsync();

                    if (context.Request.HttpMethod == "OPTIONS")
                    {
                        context.Response.AddHeader("Access-Control-Allow-Headers", "*");
                        context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH");
                        context.Response.AddHeader("Access-Control-Max-Age", "1728000");
                        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                        context.Response.Close();
                    }
                    else if(authorizationHandler != null && !authorizationHandler.IsAuthorized(context))
                    {
                        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                        context.Response.StatusCode = 401;
                        context.Response.Close();
                    }
                    else
                    {
                        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

                        _ = Task.Run(() =>
                        {
                            string method = context.Request.HttpMethod;
                            string route = context.Request.Url.LocalPath.ToString();
                            string identifier = method.ToUpper() + route.ToLower();

                            if (actions.ContainsKey(identifier))
                            {
                                string response;
                                try
                                {
                                    response = JsonConvert.SerializeObject(actions[identifier](context));
                                }
                                catch (Exception e)
                                {
                                    response = JsonConvert.SerializeObject(e);
                                    context.Response.StatusCode = 500;
                                }


                                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                                {
                                    writer.Write(response);
                                    context.Response.ContentLength64 = response.Length;
                                    context.Response.ContentType = "Application/json";
                                }
                                context.Response.Close();
                            }
                            else
                            {
                                context.Response.StatusCode = 404;
                                context.Response.Close();
                            }
                        });
                    }
                }
            });
        }

        public void AddAuthorizationMethod(IAuthorizationHandler authorizationMethod)
        {
            this.authorizationHandler = authorizationMethod;
        }

        public void Stop()
        {
            running = false;
            listener.Stop();
        }

        public void AddAction(string method, string route, Func<HttpListenerContext, dynamic> action)
        {
            //Console.WriteLine($"{method.ToUpper() + route.ToLower()} registered");
            actions[method.ToUpper() + route.ToLower()] = action;
        }
    }
}
