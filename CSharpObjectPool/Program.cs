using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpObjectPool
{
    class Program
    {
        static void Main(string[] args)
        {
            //对象池
            //对象池管理类应该包含：初始化[Initialize] 分配功能[Spawn]，回收功能[Recycle],清理功能[Release]
            //可回收对象应当继承接口 IPoolRecycleItem,并实现[OnRecycled]回收方法
            //下面是性能差异对比
            //### 未使用对象池
            //开始性能分析[没有使用对象池]
            //没有使用对象池耗费时间: 3817,占用632MB
            //请按任意键继续. . .
            //### 使用对象池
            //开始性能分析[使用对象池]
            //使用对象池耗费时间: 1764,占用122MB
            //请按任意键继续. . .

            //首先是[count]个对象

            int count = 10000000;
            int l = count / 2;
            Console.WriteLine("开始性能分析[没有使用对象池]");
            List<HomeWork> array = new List<HomeWork>();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < count; i++)
            {
                array.Add(new HomeWork()
                {
                    Name = i.ToString(),
                    Work = new object()
                });
            }

            sw.Stop();
            Console.WriteLine($"没有使用对象池耗费时间:{sw.ElapsedMilliseconds},占用{GetMemory()}MB");

            sw.Reset();
            GC.Collect();

            Console.WriteLine("开始性能分析[使用对象池]");
            //使用对象池，先创建l个对象，后面l个复用,测试不严谨
            sw.Start();
            var pool = new ObjectPool<HomeWork>();
            pool.Initialize(l);

            for (int i = 0; i < l; i++)
            {
                var val = pool.Spawn();
                val.Name = i.ToString();
                val.Work = new object();
                pool.Recycle(val);
            }

            sw.Stop();
            Console.WriteLine($"使用对象池耗费时间:{sw.ElapsedMilliseconds},占用{GetMemory()}MB");
        }

        public static long GetMemory()
        {
            Process proc = Process.GetCurrentProcess();
            long b = proc.PrivateMemorySize64;
            for (int i = 0; i < 2; i++)
            {
                b /= 1024;
            }
            return b;
        }

        private class HomeWork : IPoolItem
        {
            public string Name;

            public object Work;


            public void OnRecycled()
            {
                Name = "";
                Work = new object();
            }

            public void OnSpawned()
            {

            }
        }
    }
}
