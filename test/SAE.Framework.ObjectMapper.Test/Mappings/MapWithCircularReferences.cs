using SAE.Framework.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings
{
    public class MapWithCircularReferences : MappingBase
    {
        [Fact]
        public void Map_Node_CircularReferences_Success()
        {
            var source = new Node
            {
                Id = "1",
                Next = new Node
                {
                    Id = "2",
                    Next = new Node
                    {
                        Id = "3",
                        Child = new[]
                        {
                            new Node
                            {
                                Id = " 123 1"
                            },
                            new Node
                            {
                                Id = "123 2"
                            }
                        }

                    }
                },
                Child = new[]
                {
                    new Node
                    {
                        Id = "1 1"
                    },
                    new Node
                    {
                        Id = "1 2"
                    }
                }
            };

            _tinyMapper.Bind<Node, Node>();

            var target = _tinyMapper.Map<Node, Node>(source);

            XAssert.Equal(source.Id, target.Id);
            XAssert.Equal(source.Next.Id, target.Next.Id);
            XAssert.Equal(source.Next.Next.Id, target.Next.Next.Id);
            XAssert.Equal(source.Next.Next.Id, target.Next.Next.Id);

            XAssert.Equal(source.Next.Next.Child, target.Next.Next.Child);

            XAssert.Equal(source.Child, target.Child);
        }

    }

    public class Node
    {
        public string Id;
        public Node Next;
        public Node[] Child;

        protected bool Equals(Node other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
