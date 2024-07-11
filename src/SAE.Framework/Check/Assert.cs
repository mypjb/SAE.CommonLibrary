using System.Runtime.CompilerServices;

namespace SAE.Framework.Extension
{
    /// <summary>
    /// 断言
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// 构建一个<see cref="IAssert{TAssert}"/>对象
        /// </summary>
        /// <typeparam name="TAssert"></typeparam>
        /// <param name="assert"></param>
        /// <param name="name">参数名称</param>
        /// <returns><see cref="IAssert{TAssert}"/></returns>
        public static IAssert<TAssert> Build<TAssert>(TAssert assert, [CallerMemberName]string name = "")
                                       => new Assert<TAssert>(assert, name);
    }
    /// <summary>
    /// 断言泛型实现
    /// </summary>
    /// <typeparam name="TAssert"></typeparam>
    internal class Assert<TAssert> : IAssert<TAssert>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="name">参数名称</param>
        public Assert(TAssert assert,string name)
        {
            this.Current = assert;
            this.Name = string.IsNullOrWhiteSpace(name) ? "未知的参数" : name;
        }
        /// <inheritdoc/>
        public string Name { get; }
        /// <inheritdoc/>
        public TAssert Current { get; }

        
    }
}
