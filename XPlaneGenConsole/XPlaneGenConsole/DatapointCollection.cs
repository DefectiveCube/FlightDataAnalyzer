using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class DatapointCollection<T> : ICollection<T> where T : Datapoint<T>
    {
        public void Add(T item)
        {

        }

        public void Clear() { }

        public bool Contains(T item)
        {
            return false;
        }

        public void CopyTo(T[] array, int index)
        {

        }

        public int Count { get { return 0; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(T item)
        {
            return false;
        }
       
        public virtual IEnumerator<T> GetEnumerator()
        {
            throw new Exception();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception();
        }
    }
}
