using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class Attribute:IEnumEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //public int Type { get; set; } //ToDo: Do we need this?
        public string Pattern { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
	public enum AttributeEnum
	{
		Title = 1,
		Relations,
		Location,
	};
}
