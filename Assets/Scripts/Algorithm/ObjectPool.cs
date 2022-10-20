using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class ObjectPool<T> where T : new()
    {
        public Queue<T> Pool;

        public List<T> InUse;

        protected int MaxSize;
        
        public ObjectPool(int size)
        {
            MaxSize = size;

            Pool = new Queue<T>();

            InUse = new  List<T>();
        }

        public virtual T Get()
        {
            T res;
            if (Pool.Count > 0)
            {
                res = Pool.Peek();
                
                InUse.Add(Pool.Dequeue());
            }
            else
            {
                res = new T();
                
                MaxSize++;
                
                InUse.Add(res);
            }

            return res;
        }

        public virtual bool Return(T ob)
        {
            bool res;
            if (InUse.Contains(ob))
            {
                res = true;
                Pool.Enqueue(ob);
                InUse.Remove(ob);
            }
            else
            {
                res = false;
            }

            return res;
        }
    }
}