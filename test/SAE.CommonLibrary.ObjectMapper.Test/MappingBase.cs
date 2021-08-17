using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.ObjectMapper.Test
{
    public abstract class MappingBase
    {
        protected readonly TinyMapper _tinyMapper;

        public MappingBase()
        {
            _tinyMapper = new TinyMapper(new[] { new ObjectMapperBuilder(null) });
        }
    }
}
