using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UchebPr
{
    public partial class Department
    {
        public override string ToString()
        {
            return name;
        }
    }
    public partial class KancTovari
    {
        public override string ToString()
        {
            return "Наименование - " + name;
        }
    }
}
