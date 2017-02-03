using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public class ContainerForInds : Container<Individual>
    {
        public ContainerForInds(int s)
        {
            size = s;
            container = new List<Individual>();
        }
        public override void Add(Individual item)
        {
            if (container.Count < size)
                container.Add(item);
            else
                container[container.Count - 1] = item;

            int i = container.Count - 2;

            while (i >= 0 && container[i].fitness < container[i + 1].fitness)
            {
                Individual tmp = container[i];
                container[i] = container[i + 1];
                container[i + 1] = tmp;
                i--;
            }
        }
    }
}
