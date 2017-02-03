using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public abstract class Container<T> : IEnumerable<T>
    {
        public List<T> container;
        public int size;
        public T this[int index]
        {
            get
            {
                return container[index];
            }
        }
        public int Count
        {
            get
            {
                return container.Count;
            }
  
        }

        public IEnumerator<T> GetEnumerator()
        {
            return container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public abstract void Add(T item);

    }
}
