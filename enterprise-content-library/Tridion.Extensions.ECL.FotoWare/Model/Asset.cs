using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.Model
{
    internal class Asset : IContentLibraryMultimediaItem
    {
        internal Asset()
        {
            // default values
            CanGetUploadMultimediaItemsUrl = false;
            CanGetViewItemUrl = false;
            CanSearch = false;
            CanUpdateMetadataXml = false;
            CanUpdateTitle = false;
            DisplayTypeId = DisplayTypeIdentifier;
        }

        #region IContentLibraryMultimediaItem
        public bool CanGetUploadMultimediaItemsUrl { get; internal set; }

        public bool CanGetViewItemUrl { get; internal set; }

        public bool CanSearch { get; internal set; }

        public bool CanUpdateMetadataXml { get; internal set; }

        public bool CanUpdateTitle { get; internal set; }

        public DateTime? Created { get; internal set; }

        public string CreatedBy { get; internal set; }

        public string DisplayTypeId { get; internal set; }

        public string Filename { get; internal set; }

        public int? Height { get; internal set; }

        public string IconIdentifier { get; internal set; }

        public IEclUri Id { get; internal set; }

        public bool IsThumbnailAvailable { get; internal set; }

        public string MetadataXml { get; set; }

        public ISchemaDefinition MetadataXmlSchema { get; internal set; }

        public string MimeType { get; internal set; }

        public DateTime? Modified { get; internal set; }

        public string ModifiedBy { get; internal set; }

        public IEclUri ParentId { get; internal set; }

        public string ThumbnailETag { get; internal set; }

        public string Title { get; set; }

        public int? Width { get; internal set; }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotImplementedException();
        }

        public IContentResult GetContent(IList<ITemplateAttribute> attributes)
        {
            IContentResult result = null;
            string url = GetDirectLinkToPublished(attributes);
            if (!string.IsNullOrWhiteSpace(url))
            {
                using (var stream = FotoWareProvider.Client.GetStreamAsync(url).Result)
                {
                    result = FotoWareProvider.HostServices.CreateContentResult(stream, stream.Length, MimeType);

                }
            }
            return result;
        }

        public string GetDirectLinkToPublished(IList<ITemplateAttribute> attributes)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("Getting direct link to published for asset {0} with attributes:", UniqueId)
            );
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    FotoWareProvider.HostServices.LogMessage(
                        LoggingSeverity.Debug,
                        string.Format("   {0}:{1} = {2}", attribute.NamespaceUri, attribute.Name, attribute.Value)
                    );
                }
            }

            string result = null;
            if (Previews != null && Previews.Count() > 0)
            {
                int largest = Previews.Max(p => p.Size);
                var preview = Previews.First(p => p.Size == largest);
                if (preview != null && preview.Url != null)
                {
                    result = preview.Url.ToString();
                }
            }
            return result;
        }

        public string GetTemplateFragment(IList<ITemplateAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IContentLibraryItem Save(bool readback)
        {
            return readback ? this : null;
        }

        #endregion

        internal int ArchiveId { get; set; }
        internal string InternalId { get; set; }
        internal IEnumerable<Preview> Previews { get; set; }
        internal string UniqueId { get; set; }

        internal static Converter<XElement, Asset> FromXml = delegate (XElement source)
        {
            XElement metadata = source.Descendants("MetaData").FirstOrDefault();
            XElement fileinfo = source.Descendants("FileInfo").FirstOrDefault();
            XElement previewLinks = source.Descendants("PreviewLinks").FirstOrDefault();

            XElement created = null;
            XElement modified = null;
            XElement width = null;
            XElement height = null;
            XElement thumbnail = null;
            XElement mimetype = null;
            XElement uniqueId = null;

            if (fileinfo != null)
            {
                created = fileinfo.Descendants("Created").FirstOrDefault();
                modified = fileinfo.Descendants("LastModified").FirstOrDefault();
                mimetype = fileinfo.Descendants("MimeType").FirstOrDefault();
            }
            if (metadata != null)
            {
                string fieldId = API.SearchFields.Metadata.GetIptcId(
                    API.SearchFields.Metadata.UniqueDocumentId
                );
                uniqueId = metadata.Descendants("Field").Where(f => f.Attribute("Id").Value.Equals(fieldId)).FirstOrDefault();
                width = metadata.Descendants("PixelWidth").FirstOrDefault();
                height = metadata.Descendants("PixelHeight").FirstOrDefault();
            }

            var item = new Asset()
            {
                Filename = source.Attribute("Name").Value,
                Height = height != null ? int.Parse(height.Value) : new int?(),
                InternalId = source.Attribute("Id").Value,
                IsThumbnailAvailable = thumbnail != null,
                // MetadataXml = metadata != null ? metadata.ToString() : null,
                MetadataXml = null,
                Title = source.Attribute("Name").Value,
                ThumbnailETag = source.Attribute("Id").Value,
                UniqueId = uniqueId != null ? uniqueId.Value.Replace(" ", WhitespaceReplacement) : null,
                Width = width != null ? int.Parse(width.Value) : new int?()
            };

            if (previewLinks != null)
            {
                var previewURLs = previewLinks.Descendants("PreviewUrl").ToList();
                var previews = new List<Preview>(previewURLs.Count());
                previews.AddRange(previewURLs.ConvertAll(Preview.FromXml));
                item.Previews = previews;
            }

            DateTime dateModified;
            DateTime dateCreated;
            if (modified != null && DateTime.TryParse(modified.Value, out dateModified))
            {
                item.Modified = dateModified;
            }
            if (created != null && DateTime.TryParse(created.Value, out dateCreated))
            {
                item.Created = dateCreated;
            }

            return item;
        };

        internal const string DisplayTypeIdentifier = "asset";
        internal const string WhitespaceReplacement = "_";

    }
}
