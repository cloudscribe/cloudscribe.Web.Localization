using System.Collections.Generic;

namespace cloudscribe.Web.Localization.Versioning
{
    public interface IVersionProviderFactory
    {
        IEnumerable<IVersionProvider> VersionProviders { get; }
        IVersionProvider Get(string name);
    }
}
