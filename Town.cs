using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public class Town
    {
        public double x
        {
            set;
            get;
        }
        public double y
        {
            set;
            get;
        }
        public double profit
        {
            set;
            get;
        }
        public Town(double px, double py, double p)
        {
            x = px;
            y = py;
            profit = p;
        }
        public int calculateDistanceToOtherTown(Town t1)
        {
            return (int)Math.Round(Math.Sqrt((x - t1.x)* (x - t1.x) + (y - t1.y)*(y-t1.y)));
        }
    }
}
