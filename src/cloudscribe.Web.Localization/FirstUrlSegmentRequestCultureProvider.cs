using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Web.Localization
{
    public class FirstUrlSegmentRequestCultureProvider : RequestCultureProvider
    {
        public FirstUrlSegmentRequestCultureProvider(
            IList<CultureInfo> supportedUICultures,
            IList<CultureInfo> supportedCultures = null
            )
        {
            _supportedUICultures = supportedUICultures;
            _supportedCultures = supportedCultures;
        }

        private readonly IList<CultureInfo> _supportedUICultures;
        private readonly IList<CultureInfo> _supportedCultures;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var pathStartingSegment = GetStartingSegment(httpContext.Request.Path);

            if (!string.IsNullOrWhiteSpace(pathStartingSegment))
            {
                var matchingUICulture = _supportedUICultures.Where(x => x.Name.Equals(pathStartingSegment, System.StringComparison.InvariantCultureIgnoreCase) 
                || x.TwoLetterISOLanguageName.Equals(pathStartingSegment, System.StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                CultureInfo mainCulture = null;
                if (_supportedCultures != null)
                {
                    mainCulture = _supportedCultures.Where(x => x.Name.Equals(pathStartingSegment, System.StringComparison.InvariantCultureIgnoreCase) 
                    || x.TwoLetterISOLanguageName.Equals(pathStartingSegment, System.StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                }
                if (matchingUICulture != null)
                {
                    if (mainCulture != null)
                    {
                        return Task.FromResult(new ProviderCultureResult(mainCulture.Name, matchingUICulture.Name));
                    }
                    return Task.FromResult(new ProviderCultureResult(matchingUICulture.Name, matchingUICulture.Name));
                }
            }

            //nothing matched
            return NullProviderCultureResult;

        }

        private string GetStartingSegment(string requestPath)
        {
            if (string.IsNullOrEmpty(requestPath)) return requestPath;
            if (!requestPath.Contains("/")) return requestPath;

            var segments = SplitOnCharAndTrim(requestPath, '/');
            return segments.FirstOrDefault();
        }

        private List<string> SplitOnCharAndTrim(string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrWhiteSpace(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrWhiteSpace(item)) { list.Add(item.Trim()); }
            }


            return list;
        }
    }
}
