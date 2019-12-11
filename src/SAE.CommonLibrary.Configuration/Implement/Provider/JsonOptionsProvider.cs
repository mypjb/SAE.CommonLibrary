using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Implement
{
    public class JsonOptionsProvider : IOptionsProvider
    {
        private const string Suffix = ".json";
        public async Task HandleAsync(OptionsContext context)
        {
            var path = Utils.Path.Config($"{context.Name}{Suffix}");

            if (path.ExistFile())
            {
                var json = await File.ReadAllTextAsync(path, Constant.Encoding);
                context.Options = json.ToObject(context.Type);
                context.Provider = this;
            }
        }

        public async Task SaveAsync(string name, object options)
        {
            var path = Utils.Path.Config($"{name}{Suffix}");

            Constant.Path.Config.CreateDirectory();

            using (var fileStream = new FileStream(path,
                                                   FileMode.Create, 
                                                   FileAccess.Write, 
                                                   FileShare.Read))
            {
                await fileStream.WriteAsync(options.ToJsonString()
                                                   .ToBytes()
                                                   .ToArray());
            }

        }
    }
}
