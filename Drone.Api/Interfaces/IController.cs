﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Interfaces
{
    public interface IController
    {
        void AddRoutes(HttpServer webserver, Drone drone);
    }
}
