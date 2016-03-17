using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.UnitTests.Mock
{
    class HostServicesMockup : IHostServices
    {
        public IContentResult CreateContentResult(Stream stream, long contentLength, string contentType)
        {
            throw new NotImplementedException();
        }

        public IDateFieldDefinition CreateDateFieldDefinition(string xmlElementName, string label, int minOccurs = 0, int? maxOccurs = 1, DateTime? minValue = default(DateTime?), bool isMinValueExclusive = false, DateTime? maxValue = default(DateTime?), bool isMaxValueExclusive = false, string pattern = null, FieldListType listType = FieldListType.Select, int listHeight = 1, string customUrl = null)
        {
            throw new NotImplementedException();
        }

        public IDisplayType CreateDisplayType(string id, string displayText, EclItemTypes itemType)
        {
            return new Mock.DisplayTypeMockup(id, displayText, itemType);
        }

        public IEclUri CreateEclUri(string eclUriStringRepresentation)
        {
            throw new NotImplementedException();
        }

        public IEclUri CreateEclUri(int publicationId, string mountPointId)
        {
            return CreateEclUri(publicationId, mountPointId, "undefined", "undefined", EclItemTypes.Undefined);
        }

        public IEclUri CreateEclUri(int publicationId, string mountPointId, string itemId, string subType, EclItemTypes itemType)
        {
            return CreateEclUri(publicationId, mountPointId, itemId, subType, itemType, null);
        }

        public IEclUri CreateEclUri(int publicationId, string mountPointId, string itemId, string subType, EclItemTypes itemType, int? version)
        {
            return new Mock.EclUriMockup()
            {
                PublicationId = publicationId,
                MountPointId = mountPointId,
                ItemId = itemId,
                SubType = subType,
                ItemType = itemType,
                Version = version
            };
        }

        public IFieldGroupDefinition CreateFieldGroupDefinition(string xmlElementName, string label, int minOccurs = 0, int? maxOccurs = 1, string customUrl = null)
        {
            throw new NotImplementedException();
        }

        public IFolderContent CreateFolderContent(IEclUri parentUri, IList<IContentLibraryListItem> childItems, bool canGetUploadMultimediaItemsUrl, bool canSearch)
        {
            throw new NotImplementedException();
        }

        public IPaginatedFolderContent CreateFolderContent(IEclUri parentUri, int pageIndex, bool isLastPage, IList<IContentLibraryListItem> childItems, bool canGetUploadMultimediaItemsUrl, bool canSearch)
        {
            throw new NotImplementedException();
        }

        public IPaginatedFolderContent CreateFolderContent(IEclUri parentUri, int pageIndex, int totalNumberOfPages, IList<IContentLibraryListItem> childItems, bool canGetUploadMultimediaItemsUrl, bool canSearch)
        {
            throw new NotImplementedException();
        }

        public IMultiLineTextFieldDefinition CreateMultiLineTextFieldDefinition(string xmlElementName, string label, int minOccurs = 0, int? maxOccurs = 1, int height = 10, string customUrl = null)
        {
            throw new NotImplementedException();
        }

        public INumberFieldDefinition CreateNumberFieldDefinition(string xmlElementName, string label, int minOccurs = 0, int? maxOccurs = 1, double? minValue = default(double?), bool isMinValueExclusive = false, double? maxValue = default(double?), bool isMaxValueExclusive = false, int? totalDigits = default(int?), int? fractionDigits = 0, string pattern = null, FieldListType listType = FieldListType.Select, int listHeight = 1, string customUrl = null)
        {
            throw new NotImplementedException();
        }

        public ISchemaDefinition CreateSchemaDefinition(string rootElementName, string xmlNamespaceUrl)
        {
            throw new NotImplementedException();
        }

        public ISingleLineTextFieldDefinition CreateSingleLineTextFieldDefinition(string xmlElementName, string label, int minOccurs = 0, int? maxOccurs = 1, int minLength = 0, int? maxLength = default(int?), string pattern = null, FieldListType listType = FieldListType.Select, int listHeight = 1, string customUrl = null)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateThumbnailImage(int width, int height, Stream imageStream, IList<IThumbnailOverlay> thumbnailOverlays)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateThumbnailImage(int width, int height, Stream imageStream, int maxImageWidth, int maxImageHeight, IList<IThumbnailOverlay> thumbnailOverlays)
        {
            throw new NotImplementedException();
        }

        public IThumbnailOverlay CreateThumbnailOverlay(int x, int y, Stream imageStream)
        {
            throw new NotImplementedException();
        }

        public IThumbnailOverlay CreateThumbnailOverlay(int x, int y, int width, int height, Stream imageStream)
        {
            throw new NotImplementedException();
        }

        public IXhtmlFieldDefinition CreateXhtmlTextFieldDefinition(string xmlElementName, string label, int minOccurs = 0, int? maxOccurs = 1, int height = 10, string filterXslt = null, string customUrl = null)
        {
            throw new NotImplementedException();
        }

        public byte[] GetIcon(string basePath, string theme, string iconName, int size)
        {
            throw new NotImplementedException();
        }

        public byte[] GetIcon(string basePath, string theme, string iconName, int size, out int actualSize)
        {
            throw new NotImplementedException();
        }

        public byte[] GetTransactionPropagationToken()
        {
            throw new NotImplementedException();
        }

        public bool IsNullOrNullEclUri(IEclUri eclUri)
        {
            throw new NotImplementedException();
        }

        public void LogMessage(LoggingSeverity severity, string message)
        {
            System.Diagnostics.Debug.WriteLine(severity.ToString() + " : " + message);
        }

        public IEclUri TryCreateEclUri(string eclUriStringRepresentation)
        {
            throw new NotImplementedException();
        }
    }
}
