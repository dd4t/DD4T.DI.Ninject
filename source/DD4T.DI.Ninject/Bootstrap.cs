﻿using Ninject;
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

namespace DD4T.DI.Ninject
{
    public static class Bootstrap
    {
        public static void UseDD4T(this IKernel kernel)
        {
            //not all dll's are loaded in the app domain. we will load the assembly in the appdomain to be able map the mapping
            var location = string.Format(@"{0}\bin\", AppDomain.CurrentDomain.BaseDirectory);
            var file = Directory.GetFiles(location, "DD4T.Providers.*").FirstOrDefault();
            var load = Assembly.LoadFile(file);

            var provider = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.StartsWith("DD4T.Providers")).FirstOrDefault();
            var providerTypes = provider.GetTypes();
            var pageprovider = providerTypes.Where(a => typeof(IPageProvider).IsAssignableFrom(a)).FirstOrDefault();
            var cpProvider = providerTypes.Where(a => typeof(IComponentPresentationProvider).IsAssignableFrom(a)).FirstOrDefault();
            var linkProvider = providerTypes.Where(a => typeof(ILinkProvider).IsAssignableFrom(a)).FirstOrDefault();
            var commonServices = providerTypes.Where(a => typeof(IProvidersCommonServices).IsAssignableFrom(a)).FirstOrDefault();
            var binaryProvider = providerTypes.Where(a => typeof(IBinaryProvider).IsAssignableFrom(a)).FirstOrDefault();
            var componentProvider = providerTypes.Where(a => typeof(IComponentProvider).IsAssignableFrom(a)).FirstOrDefault();


            if (kernel.TryGet<IDD4TConfiguration>() == null)
                kernel.Bind<IDD4TConfiguration>().To<DD4TConfiguration>().InSingletonScope();

            if (kernel.TryGet<ILogger>() == null)
                kernel.Bind<ILogger>().To<DefaultLogger>().InSingletonScope();

            if (kernel.TryGet<IPublicationResolver>() == null)
                kernel.Bind<IPublicationResolver>().To<DefaultPublicationResolver>().InSingletonScope();

            if (kernel.TryGet<ICacheAgent>() == null)
                kernel.Bind<ICacheAgent>().To<DefaultCacheAgent>();

            //providers
            if (binaryProvider != null && kernel.TryGet<IBinaryProvider>() == null)
                kernel.Bind<IBinaryProvider>().To(binaryProvider);

            if (componentProvider != null && kernel.TryGet<IComponentProvider>() == null)
                kernel.Bind<IComponentProvider>().To(componentProvider);

            if (pageprovider != null && kernel.TryGet<IPageProvider>() == null)
                kernel.Bind<IPageProvider>().To(pageprovider);

            if (cpProvider != null && kernel.TryGet<IComponentPresentationProvider>() == null)
                kernel.Bind<IComponentPresentationProvider>().To(cpProvider);

            if (linkProvider != null && kernel.TryGet<ILinkProvider>() == null)
                kernel.Bind<ILinkProvider>().To(linkProvider);

            if (commonServices != null && kernel.TryGet<IProvidersCommonServices>() == null)
                kernel.Bind<IProvidersCommonServices>().To(commonServices);

            //factories
            if (kernel.TryGet<IPageFactory>() == null)
                kernel.Bind<IPageFactory>().To<PageFactory>();

            if (kernel.TryGet<IComponentPresentationFactory>() == null)
                kernel.Bind<IComponentPresentationFactory>().To<ComponentPresentationFactory>();

            if (kernel.TryGet<ILinkFactory>() == null)
                kernel.Bind<ILinkFactory>().To<LinkFactory>();

            if (kernel.TryGet<IBinaryFactory>() == null)
                kernel.Bind<IBinaryFactory>().To<BinaryFactory>();

            if(kernel.TryGet<IComponentFactory>() ==  null)
                kernel.Bind<IComponentFactory>().To<ComponentFactory>();

            if (kernel.TryGet<IFactoryCommonServices>() == null)
                kernel.Bind<IFactoryCommonServices>().To<FactoryCommonServices>();

        }

    }
}
