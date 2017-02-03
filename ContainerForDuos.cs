using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public struct Duo
    {
        public double value
        {
            set;
            get;
        }
        public int index
        {
            get;
            set;
        }
        public Duo(double V, int I)
        {
            value = V;
            index = I;
        }
    }
    public class ContainerForDuos : Container<Duo>
    {
        public int town(int index)
        {
            return container[index].index;
        }
        public ContainerForDuos(int s)
        {
            size = s;
            container = new List<Duo>();
        }
        public override void Add(Duo item)
        {
            if (container.Count < size)
                container.Add(item);
            else
                container[container.Count - 1] = item;

            int i = container.Count - 2;

            while (i >= 0 && container[i].value < container[i + 1].value)
            {
                Duo tmp = container[i];
                container[i] = container[i + 1];
                container[i + 1] = tmp;
                i--;
            }
        }
    }
}
