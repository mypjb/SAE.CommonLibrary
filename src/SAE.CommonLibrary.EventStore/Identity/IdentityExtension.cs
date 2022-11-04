using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// <see cref="IIdentity"/>扩展
    /// </summary>
    public static class IdentityExtension
    {
        /// <summary>
        /// string 转 <see cref="IIdentity"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IIdentity ToIdentity(this string id)
        {
            Assert.Build(id,nameof(id))
                  .NotNull();
            return IdentityGenerator.Build(id);
        }

    }
}
