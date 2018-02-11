using System;
using System.Collections.Generic;
using System.Text;

namespace AgileTrace.Repository.Entity
{
    public class Trace
    {
        public string Topic { get; set; }

        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }
    }
}
