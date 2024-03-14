namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <inheritdoc/>
    public class Identity : IIdentity
    {
        /// <summary>
        /// 具体值
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// ctor
        /// </summary>
        public Identity():this(Utils.GenerateId())
        {
        }
        /// <summary>
        /// 根据<paramref name="id"/>构造一个<see cref="IIdentity"/>
        /// </summary>
        /// <param name="id">字符串形式的标识</param>
        public Identity(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                id = Utils.GenerateId();
            this.Id = id;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Id;
        }

        /// <summary>
        /// 字符串隐式转换为<see cref="Identity"/>
        /// </summary>
        /// <param name="identity">标识</param>
        public static implicit operator Identity(string identity)
        {
            return new Identity(identity);
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="identity">标识</param>
        public static implicit operator string(Identity identity)
        {
            return identity.ToString();
        }
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            return this.ToString().Equals(obj.ToString());
        }
        /// <inheritdoc/>
        public static bool operator ==(Identity left, Identity right)
        {
            if ((object)left == null && (object)right == null) return true;
            return left.Equals(right);
        }


        /// <inheritdoc/>
        public static bool operator !=(Identity left, Identity right)
        {
            return !(left == right);
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
