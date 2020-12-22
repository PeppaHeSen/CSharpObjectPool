namespace CSharpObjectPool
{
    public interface IPoolItem
    {
        /// <summary>
        /// 当对象回收时
        /// </summary>
        void OnRecycled();

        /// <summary>
        /// 被分配了
        /// </summary>
        void OnSpawned();
    }
}
