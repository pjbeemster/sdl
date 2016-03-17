using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Tridion.Extensions.ECL.FotoWare
{
    internal partial class Configuration
    {
        internal Configuration(XElement source)
        {
            var configuredUrl = source.Descendants(XName.Get("EndpointUrl", Configuration.XmlNamespace)).FirstOrDefault();
            var configuredUid = source.Descendants(XName.Get("UserName", Configuration.XmlNamespace)).FirstOrDefault();
            var configuredPwd = source.Descendants(XName.Get("Password", Configuration.XmlNamespace)).FirstOrDefault();
            var providerIcon = source.Descendants(XName.Get("ProviderIcon", Configuration.XmlNamespace)).FirstOrDefault();
            var archiveIcon = source.Descendants(XName.Get("ArchiveIcon", Configuration.XmlNamespace)).FirstOrDefault();
            var assetIcon = source.Descendants(XName.Get("AssetIcon", Configuration.XmlNamespace)).FirstOrDefault();

            Debug.Assert(configuredUrl != null, "Failed to load Fotoware URL from configuration.");
            Debug.Assert(configuredUid != null, "Failed to load Fotoware username from configuration.");
            Debug.Assert(configuredPwd != null, "Failed to load Fotoware password from configuration.");

            Uri baseURL;
            if(!Uri.TryCreate(configuredUrl.Value, UriKind.Absolute, out baseURL))
            {
                throw new ConfigurationErrorsException("Please configure a valid absolute base URL for FotoWeb (application setting FotoWeb:BaseURL in web.config)");
            }

            this.BaseUrl = baseURL;
            this.UserName = configuredUid.Value;
            this.Password = configuredPwd.Value;
            this.ProdiverIcon = providerIcon != null ? providerIcon.Value : null;
            this.ArchiveIcon = archiveIcon != null ? archiveIcon.Value : null;
            this.AssetIcon = assetIcon != null ? assetIcon.Value : null;
        }

        public Uri BaseUrl { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public string ProdiverIcon { get; private set; }
        public string ArchiveIcon { get; private set; }
        public string AssetIcon { get; private set; }

        internal const string XmlNamespace = "http://sdl.com/ecl/fotoware";
    }
}
