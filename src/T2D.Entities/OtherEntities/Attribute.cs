using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class Attribute:IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Pattern { get; set; }
        public long? MinValue { get; set; }
        public long? MaxValue { get; set; }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
