using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpObjectPool
{
    /// <summary>
    /// 池管理器
    /// </summary>
    public class ObjectPool<T> : IObjectPool<T>
    {
        private static Stack<T> unusedStack;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(int count = 20)
        {
            unusedStack = new Stack<T>(count);
            for (int i = 0; i < count; i++)
            {
                unusedStack.Push(CreateNewInstance());
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            unusedStack.Clear();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public T Spawn()
        {
            T val;

            if (unusedStack.Count > 0)
            {
                val = unusedStack.Pop();
            }
            else
            {
                val = CreateNewInstance();
            }

            (val as IPoolItem)?.OnSpawned();

            return val;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"></param>
        public void Recycle(T item)
        {
            (item as IPoolItem)?.OnRecycled();
            unusedStack.Push(item);
        }

        /// <summary>
        /// 创建一个新的实例
        /// </summary>
        /// <returns></returns>
        private T CreateNewInstance()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
