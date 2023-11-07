using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.SharedKernel.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayValueAttribute : Attribute
    {
        public string Value { get; set; }
        public int Id { get; set; }
    }


}
