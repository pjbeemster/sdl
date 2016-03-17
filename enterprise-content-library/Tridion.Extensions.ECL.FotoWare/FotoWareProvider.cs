using System;
using System.AddIn;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare
{
    [AddIn("FotoWareProvider", Version = "1.0.0.0")]
    public class FotoWareProvider : IContentLibrary
    {
        public IList<IDisplayType> DisplayTypes
        {
            get
            {
                return new[]
                {
                    HostServices.CreateDisplayType(Model.Archive.DisplayTypeIdentifier, "FotoWare Archive", EclItemTypes.Folder),
                    HostServices.CreateDisplayType(Model.Asset.DisplayTypeIdentifier, "FotoWare Asset", EclItemTypes.File)
                };
            }
        }

        public IContentLibraryContext CreateContext(IEclSession session)
        {
            HostServices.LogMessage(LoggingSeverity.Debug, string.Format("Creating context for session user {0} ({1})", session.TridionUser.FullName, session.TridionUser.Id));
            return new FotoWareContext(session);
        }

        public byte[] GetIconImage(string theme, string iconIdentifier, int iconSize)
        {
            switch (iconIdentifier)
            {
                case Model.Archive.DisplayTypeIdentifier:
                case Model.Asset.DisplayTypeIdentifier:
                default:
                    break;
            }
            return null;
        }

        public void Initialize(string mountPointId, string configurationXmlElement, IHostServices hostServices)
        {
            var configSource = System.Xml.Linq.XElement.Parse(configurationXmlElement);
            _config = new Configuration(configSource);
            HostServices = hostServices;
            MountpointId = mountPointId;
            Client = FotoWareClient.GetInstance(_config.BaseUrl, _config.UserName, _config.Password);

            if(HostServices!=null)
            {
                HostServices.LogMessage(LoggingSeverity.Debug,
                    string.Format("FotoWare client logged in as {0} with authentication token {1}", _config.UserName, Client.AuthenticationToken)
                );
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Client != null) { Client.Dispose(); }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        private Configuration _config;
        internal static string MountpointId;
        internal static IHostServices HostServices;
        internal static FotoWareClient Client;

    }
}
