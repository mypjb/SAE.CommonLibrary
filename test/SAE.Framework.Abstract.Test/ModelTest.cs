using System;
using System.Linq;
using SAE.Framework.Abstract.Model;
using SAE.Framework.Extension;
using SAE.Framework.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.Framework.Abstract.Test
{
    public class ModelTest : BaseTest
    {
        public ModelTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void PagingSerialize()
        {
            var pagedList = PagedList.Build(Enumerable.Range(0, 10), new Paging
            {
                PageIndex = 5,
                PageSize = 100
            });

            this.WriteLine(pagedList.ToJsonString());
        }
    }
}