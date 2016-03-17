using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.UnitTests.Mock
{
    class EclSessionMockup : IEclSession
    {
        internal EclSessionMockup(IHostServices hostServices, ITridionUser user)
        {
            HostServices = hostServices;
            TridionUser = user;
        }
        public IHostServices HostServices { get; internal set; }

        public ITridionUser TridionUser { get; internal set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IList<IEclUri> GetContententLibraryUris(string publicationTcmUri)
        {
            throw new NotImplementedException();
        }

        public IList<IEclUri> GetContententLibraryUris(int publicationId)
        {
            throw new NotImplementedException();
        }

        public IContentLibraryContext GetContentLibrary(IEclUri eclUri)
        {
            throw new NotImplementedException();
        }

        public string GetOrCreateTcmUriFromEclUri(IEclUri eclUri)
        {
            throw new NotImplementedException();
        }

        public IEclUri TryGetEclUriFromTcmUri(string tcmUri)
        {
            throw new NotImplementedException();
        }

        public string TryGetTcmUriFromEclUri(IEclUri eclUri)
        {
            throw new NotImplementedException();
        }
    }
}
