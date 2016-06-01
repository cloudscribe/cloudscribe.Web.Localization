
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static RazorViewEngineOptions AddEmbeddedViewsForWebLib(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(WebLib.Controllers.FooController).GetTypeInfo().Assembly,
                    "WebLib"
                ));

            return options;
        }
    }
}
