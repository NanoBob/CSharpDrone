using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Drone.Core.Interfaces
{
    public interface IAuthorizationHandler
    {
        bool IsAuthorized(HttpListenerContext context);
    }
}
