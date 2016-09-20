using PilotKit.Infrastructure.CrossCutting.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Infrastructure.CrossCutting.Utilities
{
    public static class ConfigManager
    {
        public static AppSettings AppSettings { get; set; }

        public static ConnectionStrings ConnectionStrings { get; set; }
    }
}
