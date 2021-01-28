using System.Runtime.CompilerServices;

namespace SAE.CommonLibrary.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// 构建一个<see cref="IAssert{TAssert}"/>对象
        /// </summary>
        /// <typeparam name="TAssert"></typeparam>
        /// <param name="assert"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IAssert<TAssert> Build<TAssert>(TAssert assert, [CallerMemberName]string name = "")
                                       => new Assert<TAssert>(assert, name);
    }
    internal class Assert<TAssert> : IAssert<TAssert>
    {
        public Assert(TAssert assert,string name)
        {
            this.Current = assert;
            this.Name = string.IsNullOrWhiteSpace(name) ? "未知的参数" : name;
        }

        public string Name { get; }

        public TAssert Current { get; }

        
    }
}
