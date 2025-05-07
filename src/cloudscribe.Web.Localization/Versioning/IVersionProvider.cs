using System;

namespace cloudscribe.Web.Localization.Versioning
{
    public interface IVersionProvider
    {
        string Name { get; }
        Guid ApplicationId { get; }
        Version CurrentVersion { get; }
    }
}
