using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpObjectPool
{
   public interface IObjectPool<T>
    {
        /// <summary>
        /// 分配
        /// </summary>
        /// <returns></returns>
        T Spawn();

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="item"></param>
        void Recycle(T item);
    }
}
