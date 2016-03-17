using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.Model
{
    public class Archive : IContentLibraryListItem
    {
        internal Archive()
        {
            CanGetUploadMultimediaItemsUrl = false;
            CanSearch = true;
            DisplayTypeId = DisplayTypeIdentifier;
            IconIdentifier = "fwx-archive";
            IsThumbnailAvailable = false;
            ThumbnailETag = null;
        }

        public bool CanGetUploadMultimediaItemsUrl { get; internal set; }

        public bool CanSearch { get; internal set; }

        public string DisplayTypeId { get; internal set; }

        public string IconIdentifier { get; internal set; }

        internal string InternalId { get; set; }

        public IEclUri Id { get; internal set; }

        public bool IsThumbnailAvailable { get; internal set; }

        public DateTime? Modified { get; internal set; }

        public string ThumbnailETag { get; internal set; }

        public string Title { get; set; }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotImplementedException();
        }

        internal static Converter<XElement, Archive> FromXml = delegate(XElement source) {
            var item = new Archive()
            {
                InternalId = source.Attribute("Id").Value,
                Title = source.Attribute("Name").Value
            };
            return item;
        };

        internal const string DisplayTypeIdentifier = "archive";
    }
}
