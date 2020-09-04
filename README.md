# DotVVM + ASP.NET MVC in one application

This repo shows how to use [DotVVM](https://github.com/riganti/dotvvm) together with ASP.NET MVC application in the same application.

## Prerequisites

* Make sure you have installed [DotVVM for Visual Studio](https://www.dotvvm.com/install)

## How to run the sample

1. [Open the GitHub repo in Visual Studio](git-client://clone/?repo=https%3A%2F%2Fgithub.com%2Friganti%2Fdotvvm-samples-combo-with-mvc)
or 
`git clone https://github.com/riganti/dotvvm-samples-combo-with-mvc.git`

2. Open `AspNetCore/DotvvmMvcIntegration/DotvvmMvcIntegration.sln` (ASP.NET Core) or `Owin/DotvvmMvcIntegration/DotvvmMvcIntegration.sln` (.NET Framework with OWIN)

3. Right-click the `DotvvmMvcIntegration` project and select **View > View in Browser**

## What you can learn in the sample

* How to use DotVVM together with other frameworks like ASP.NET MVC in the same app

---

# Steps Required to Add DotVVM in Existing ASP.NET MVC App

## OWIN (.NET Framework)

1. Install the `DotVVM.Owin` NuGet package.

2. Install the `Microsoft.Owin.Host.SystemWeb` package if you already don't have it in your project.

3. Add the [OWIN Startup class](Owin/DotvvmMvcIntegration/DotvvmMvcIntegration/Startup.cs) and call `app.UseDotVVM...`.

4. Unload the project, edit the `.csproj` file and add the DotVVM Project guid (`94EE71E2-EE2A-480B-8704-AF46D2E58D94`) 
as a first one in the `<ProjectTypeGuids>` element. 

_This is only needed to get full editing experience in Visual Studio. If you don't have the [DotVVM for Visual Studio](https://www.dotvvm.com/landing/dotvvm-for-visual-studio-extension) installed, do not add this GUID otherwise you won't be able to load the project._

It should look like this:

    ```
    <ProjectTypeGuids>{94EE71E2-EE2A-480B-8704-AF46D2E58D94};{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>
    ```

5. Make sure you have told the IIS to run all managed modules for all HTTP requests in the `web.config`:

    ```
      <system.webServer>
        <modules runAllManagedModulesForAllRequests="true" />
        <validation validateIntegratedModeConfiguration="false" />
      </system.webServer>
    ```

6. Create your [DotvvmStartup](Owin/DotvvmMvcIntegration/DotvvmMvcIntegration/DotvvmStartup.cs) file and register your DotVVM routes.
Any request that doesn't match any DotVVM route, will be passed to the ASP.NET MVC handlers.

## ASP.NET Core

1. Install the `DotVVM.AspNetCore` NuGet package.

2. Register the DotVVM services in the `ConfigureServices` method in the [Startup.cs](AspNetCore/DotvvmMvcIntegration/DotvvmMvcIntegration/Startup.cs) file:

```
public void ConfigureServices(IServiceCollection services)
{
	...
	
	services.AddDotVVM<DotvvmStartup>();
	
	...
}
```

3. Install the DotVVM middleware in the HTTP request pipeline in the `Configure` method in the [Startup.cs](AspNetCore/DotvvmMvcIntegration/DotvvmMvcIntegration/Startup.cs) file:

```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
	...

	app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);
	
	...
}
```

4. Create your [DotvvmStartup](AspNetCore/DotvvmMvcIntegration/DotvvmMvcIntegration/DotvvmStartup.cs) file and register your DotVVM routes.
Any request that doesn't match any DotVVM route, will be passed to the next middleware.

_To separate DotVVM views from the MVC views, we have placed DotVVM views in a folder called `DotVVM`. However, it is not necessary. 
DotVVM doesn't care about where you have your views, so you can have views from both DotVVM and MVC in one folder._

---

## Other resources

* [Gitter Chat](https://gitter.im/riganti/dotvvm)
* [DotVVM Official Website](https://www.dotvvm.com)
* [DotVVM Documentation](https://www.dotvvm.com/docs)
* [DotVVM GitHub](https://github.com/riganti/dotvvm)
* [Twitter @dotvvm](https://twitter.com/dotvvm)
* [Samples](https://www.dotvvm.com/samples)
