using System;

namespace AgileTrace.Entity
{
    public class App:IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SecurityKey { get; set; }
    }
}
