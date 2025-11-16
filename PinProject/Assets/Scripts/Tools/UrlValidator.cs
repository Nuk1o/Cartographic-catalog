using System;

namespace DefaultNamespace.Tools
{
    public static class UrlValidator
    {
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            url = url.Trim();

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
                return false;

            return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
        }
    }
}