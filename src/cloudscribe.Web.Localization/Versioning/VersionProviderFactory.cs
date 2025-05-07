using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace cloudscribe.Web.Localization.Versioning
{
    public class VersionProviderFactory : IVersionProviderFactory
    {
        public VersionProviderFactory(
            ILogger<VersionProviderFactory> logger,
            IEnumerable<IVersionProvider> versionProviders = null)
        {
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            log = logger;
            if (versionProviders != null)
            {
                VersionProviders = versionProviders;
            }
            else
            {
                VersionProviders = new List<IVersionProvider>();
                log.LogWarning("IEnumerable<IVersionProvider> was null, make sure any needed IVersionProviders have been added to DI");
            }
        }

        private ILogger log;

        public IEnumerable<IVersionProvider> VersionProviders { get; private set; }

        public IVersionProvider Get(string name)
        {
            foreach (IVersionProvider provider in VersionProviders)
            {
                if (provider.Name == name) { return provider; }
            }

            return null;
        }

    }
}