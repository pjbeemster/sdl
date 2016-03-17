using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.UnitTests.Mock
{
    class EclUriMockup : IEclUri
    {
        public bool IsNullUri { get; internal set; }

        public string ItemId { get; internal set; }

        public EclItemTypes ItemType { get; internal set; }

        public string MountPointId { get; internal set; }

        public int PublicationId { get; internal set; }

        public string SubType { get; internal set; }

        public int? Version { get; internal set; }

        public IEclUri GetInPublication(int publicationId)
        {
            throw new NotImplementedException();
        }

        public IEclUri GetMountPointUri()
        {
            throw new NotImplementedException();
        }

        public IEclUri GetVersionlessUri()
        {
            throw new NotImplementedException();
        }

        public IEclUri GetWithVersion(int? version)
        {
            throw new NotImplementedException();
        }
    }
}
