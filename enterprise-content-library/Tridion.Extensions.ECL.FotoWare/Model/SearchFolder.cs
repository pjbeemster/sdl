using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.Model
{
    public class SearchFolder : IContentLibraryListItem
    {
        public bool CanGetUploadMultimediaItemsUrl { get; internal set; }

        public bool CanSearch { get; internal set; }

        public string DisplayTypeId { get; internal set; }

        public string IconIdentifier { get; internal set; }

        public IEclUri Id { get; internal set; }

        public bool IsThumbnailAvailable { get; internal set; }

        public DateTime? Modified { get; internal set; }

        public string ThumbnailETag { get; internal set; }

        public string Title { get; set; }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotImplementedException();
        }
    }
}
