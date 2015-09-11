
[![AppVeyor](https://ci.appveyor.com/api/projects/status/github/dd4t/DD4T.DI.Ninject?branch=master&svg=true&passingText=master)](https://ci.appveyor.com/project/DD4T/dd4t-di-ninject)

[![AppVeyor](https://ci.appveyor.com/api/projects/status/github/dd4t/DD4T.DI.Ninject?branch=develop&svg=true&passingText=develop)](https://ci.appveyor.com/project/DD4T/dd4t-di-ninject)

# dd4t-di-ninject

Ninject Dependency injection container



## How to 

1. Install Nuget package: `Install-Package DD4T.DI.Ninject` [http://www.nuget.org/packages/DD4T.DI.Ninject](http://www.nuget.org/packages/DD4T.DI.Ninject "DD4T.DI.Ninject")
2. Add `DD4T.DI.Ninject` namespace to your usings;
3. Call the `UseDD4T` method on your Ninject `Ninject.IKernel` interface.

>     IKernel kernel = new StandardKernel();
>     //set all your custom apllication binding here.
>     
>     kernel.UseDD4T();



UseDD4T will Register all default class provided by the DD4T framework.

If you need to override the default classes: (i.e. the DefaultPublicationResovler) Register your class before the method call `UseDD4T`

>     IKernel kernel = new StandardKernel();
>     //set all your custom apllication binding here.
>     kernel.Bind<IPublicationResolver>().To<MyCustomPublicationResovler>().InSingletonScope();
>     kernel.UseDD4T();
