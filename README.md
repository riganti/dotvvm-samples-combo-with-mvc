# DotVVM + ASP.NET MVC in one application

This repo is a demo of how to combine DotVVM with existing ASP.NET MVC application.

We have a sample for both **OWIN (classic .NET Framework)** and new **ASP.NET Core** project types.

<br />

## OWIN + .NET 4.5: Steps Required to Add DotVVM in Existing ASP.NET MVC App

1. Install the `DotVVM.Owin` NuGet package.

2. Install the `Microsoft.Owin.Host.SystemWeb` package if you already don't have it in your project.

3. Add the [OWIN Startup class](Owin/DotvvmMvcIntegration/DotvvmMvcIntegration/Startup.cs) and call `app.UseDotVVM...`.

4. Unload the project, edit the `.csproj` file and add the DotVVM Project guid (`94EE71E2-EE2A-480B-8704-AF46D2E58D94`) 
as a first one in the `<ProjectTypeGuids>` element. (**optional** - only for Visual Studio Extension, DotVVM runtime will work correctly without this step. Also note that project will want to load without DotVVM for Visual Studio installed, so don't forget to warn your colleagues)
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

<br />

## ASP.NET Core: Steps Required to Add DotVVM in Existing ASP.NET MVC Core App

1. Install the `DotVVM.AspNetCore` NuGet package.

2. Register the DotVVM services in the `ConfigureServices` method in the [Startup.cs](AspNetCore/DotvvmMvcIntegration/DotvvmMvcIntegration/Startup.cs) file:

```
public void ConfigureServices(IServiceCollection services)
{
	...
	
	services.AddDotVVM(options =>
	{
		options.AddDefaultTempStorages("Temp");
	});
	
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

__To separate DotVVM views from the MVC views, we have placed DotVVM views in a folder called `DotVVM`. However, it is not necessary. 
DotVVM doesn't care about where you have your views, so you can have views from both DotVVM and MVC in one folder.__
