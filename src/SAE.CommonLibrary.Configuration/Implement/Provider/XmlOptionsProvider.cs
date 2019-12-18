using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SAE.CommonLibrary.Configuration.Implement
{
    public class XmlOptionsProvider : IOptionsProvider
    {
        public XmlOptionsProvider()
        {

        }
        private const string Suffix = ".config";

        public event Func<Task> OnChange;

        public Task HandleAsync(OptionsContext context)
        {
            var path = Utils.Path.Config($"{context.Name}{Suffix}");

            if (path.ExistFile())
            {
                var document = new XmlDocument();
                document.Load(path);
                context.SetOption(document.ToObject(context.Type));
                context.Provider = this;
            }
            return Task.CompletedTask;
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
                await options.ToXml().SaveAsync(fileStream,
                                                SaveOptions.None,
                                                CancellationToken.None);
            }

        }
    }
}
