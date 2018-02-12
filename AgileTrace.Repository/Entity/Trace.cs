using System;
using System.Collections.Generic;
using System.Text;

namespace AgileTrace.Repository.Entity
{
    public class Trace
    {
        public string Id { get; set; }

        public string Topic { get; set; }
        public string Level { get; set; }

        public string Stacktrace { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public string AppId { get; set; }
    }
}
