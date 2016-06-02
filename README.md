# cloudscribe.Web.Localization - more flexible localization for ASP.NET Core

I developed this project to meet my localization goals for [cloudscribe.Core](https://github.com/joeaudette/cloudscribe) and [cloudscribe.SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent). However it has no dependencies on other cloudscribe components and can be used by anyone who wants the provided functionality.

## Rationale

My vision for composing web applications is to build separate components for each feature or purpose in separate class library projects, packaged as nugets that one can bring into their main web application to add functionality. The main web application may or may not have its own features that it implements, but much or most of the functionality will come from class library projects that are pulled in as nuget dependencies.

The [new localization system for ASP.NET Core](https://docs.asp.net/en/latest/fundamentals/localization.html) allows you to configure a folder where resx files can be dropped in to localize for different languages. However this new system currently [doesn't play nice with class libraries](https://github.com/aspnet/Localization/issues/157), it only works well for localizing things that are part of the main web application.

Given my vision for building apps from class library/nuget components this new system doesn't really do what I want even if the bugs affecting class libraries were fixed. I want to be able to drop resx files into the main application to localize any of my components no matter whether they are pulled in from nuget or baked right into the main web application, I want something more like the old App_GlobalResources folder. I want to be able to embed localized resources into my class library projects of course, but if I don't have an embedded resx for a particular language I want it to be possible to drop one into the main application and have it work so that it is easy for people who speak other languages to create and add their own resx files, and ideally share them back with me so I can share them with others who may need them.

In fact, when it comes to strings for buttons and labels, people often want to customize them in addition to localizing them, so if someone wants different labels or doesn't like the current translations I have embedded, they should be able to override them by dropping in their own custom resx file.

To make this possible I have implemented [GlobalResourceManagerStringLocalizerFactory](https://github.com/joeaudette/cloudscribe.Web.Localization/blob/master/src/cloudscribe.Web.Localization/GlobalResourceManagerStringLocalizerFactory.cs), and [GlobalResourceManagerStringLocalizer](https://github.com/joeaudette/cloudscribe.Web.Localization/blob/master/src/cloudscribe.Web.Localization/GlobalResourceManagerStringLocalizer.cs). By default this will check the global resources in the application resource folder first for a localized string and if not found it will fall back to checking for embedded resources in the class library. If you don't care about customizing, it is also possible by configuration to make it use embedded resources first and fall back to global resources. For the main web application it only checks the configured application resource folder, so it has the same behavior as the standard ResourceManagerStringLocalizerFactory when it comes to resources within the main web application.

For those who may not want global resources like I do but who just want a workaround for the [rc2 class library bugs in ResourceManagerStringLocalizerFactory](https://github.com/aspnet/Localization/issues/157), this project also includes [PatchedResourceManagerStringLocalizerFactory](https://github.com/joeaudette/cloudscribe.Web.Localization/blob/master/src/cloudscribe.Web.Localization/PatchedResourceManagerStringLocalizerFactory.cs)

## Installation

In your project.json file add a dependency

    "cloudscribe.Web.Localization": "1.0.0-*"
	
In your Startup.cs in ConfigureServices add this:

    // these 2 lines are the only part unique to this project and must be added first
    services.Configure<GlobalResourceOptions>(Configuration.GetSection("GlobalResourceOptions"));
    services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();

	// you must configure a folder where the resx files will live, it can be named as you like
	services.AddLocalization(options => options.ResourcesPath = "Resources" ); 
	services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("fr"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //  // My custom request culture logic
                //  return new ProviderCultureResult("en");
                //}));
            });
			
In the main Configure method of Startup.cs you need this:

    // this gets the options we configured above
    var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
	// make sure this is before app.UseMvc
    app.UseRequestLocalization(locOptions.Value);
	
If you don't want to use global resources in preference to embedded resources in class libraries, but only want fallback to global resources change this to false in your appSettings.json, the default is true to allow customization in addition to localization.

    "GlobalResourceOptions": {
        "TryGlobalFirst": "true"
    }
	
Now if you develop class library projects with Controllers that you want to localize, you can add resx files into the main web application to localize your controllers even though they live in a class library.

To more fully understand ASP.NET Core localization be sure to [read the docs](https://docs.asp.net/en/latest/fundamentals/localization.html), to more fully understand cloudscribe.Web.Localization, study the localization.WebApp project in this repository which has examples showing how to localize a class library and how to override the class library localization from the main web application. 

## Update 2016-06-02

Unfortunately this solution does not do exactly what I want because when you publish the web application, the resx files get compiled and there seems no way around it at the moment. I can get it to include the raw resx files in the published output, but editing or adding new resx files after deployment does not work, so there is no point in publishing or deploying the raw resx files.

It is still possible to add new resx files and override resx files from class libraries, but it requires re-publish the main web app if you do that. This is not as ideal as I would like it to be, but I guess adding a new language is an infrequent activity so having to re-publish is probably not a huge burden in most cases, and it makes sense that the framework is not optimized for infrequent activities. The end result is that resx files are all pre-compiled which means they need no JIT compilation and therefore adding more languages should not impact app startup performance like it did in the old days with App_GlobalResources folder.
