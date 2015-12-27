using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Providers;
using DD4T.ContentModel.Factories;
using DD4T.Factories;
using DD4T.Utils;
using DD4T.Utils.Caching;
using DD4T.ContentModel.Contracts.Caching;

using DD4T.Utils.Resolver;
using DD4T.Utils.Logging;
using System.Reflection;
using System.IO;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Reflection;
using DD4T.ViewModels;
using DD4T.DI.Ninject.Exceptions;

namespace DD4T.DI.Ninject
{
    public static class Bootstrap
    {
        public static void UseDD4T(this IKernel kernel)
        {
            //not all dll's are loaded in the app domain. we will load the assembly in the appdomain to be able map the mapping
            var binDirectory = string.Format(@"{0}\bin\", AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(binDirectory))
                return;

            var file = Directory.GetFiles(binDirectory, "DD4T.Providers.*").FirstOrDefault();
            if (file == null)
                throw new ProviderNotFoundException();

            var load = Assembly.LoadFile(file);

            kernel.BindProviders();
            kernel.BindFactories();
            kernel.BindMvc();
            kernel.BindRestProvider();
            kernel.BindResolvers();
            kernel.BindViewModels();

            if (kernel.TryGet<IDD4TConfiguration>() == null)
                kernel.Bind<IDD4TConfiguration>().To<DD4TConfiguration>().InSingletonScope();

            if (kernel.TryGet<ILogger>() == null)
                kernel.Bind<ILogger>().To<DefaultLogger>().InSingletonScope();

            if (kernel.TryGet<ICacheAgent>() == null)
                kernel.Bind<ICacheAgent>().To<DefaultCacheAgent>();

            //caching JMS
            if (kernel.TryGet<IMessageProvider>() == null)
                kernel.Bind<IMessageProvider>().To<JMSMessageProvider>().InSingletonScope();

         
        }

    }
}
