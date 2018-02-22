using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AgileTrace.Configuration
{
    public class Config
    {
        public static IConfiguration Configuration { get; set; }
    }
}
