using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockSwitch
{
    class Size
    { 
        public double x { get; private set; }
        public double y { get; private set; }

        public Size(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
    }
}
