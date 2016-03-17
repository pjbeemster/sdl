using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare
{
    public class FotoWareContext : IContentLibraryContext
    {
        internal FotoWareContext(IEclSession session)
        {
            this._session = session;
        }

        public string IconIdentifier { get; private set; }

        public bool CanGetUploadMultimediaItemsUrl(int publicationId)
        {
            return false;
        }

        public bool CanSearch(int publicationId)
        {
            return true;
        }

        public string Dispatch(string command, string payloadVersion, string payload, out string responseVersion)
        {
            throw new NotImplementedException();
        }

        public IList<IContentLibraryListItem> FindItem(IEclUri eclUri)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("Finding item {0}", eclUri)
            );
            throw new NotImplementedException();
        }

        public IFolderContent GetFolderContent(IEclUri parentFolderUri, int pageIndex, EclItemTypes itemTypes)
        {
            FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug,
                string.Format("Getting folder contents for {0}, page index {1} for item types {2}",
                    parentFolderUri.ToString(),
                    pageIndex,
                    itemTypes.ToString()
                )
            );

            List<IContentLibraryListItem> result = new List<IContentLibraryListItem>();

            if(parentFolderUri.ItemType == EclItemTypes.MountPoint && itemTypes.HasFlag(EclItemTypes.Folder))
            {
                result.AddRange(GetRootFolderContent(parentFolderUri));
            }

            bool canUpload = CanGetUploadMultimediaItemsUrl(parentFolderUri.PublicationId);
            bool canSearch = CanSearch(parentFolderUri.PublicationId);
            return FotoWareProvider.HostServices.CreateFolderContent(parentFolderUri, result, canUpload, canSearch);
        }

        private List<IContentLibraryListItem> GetRootFolderContent(IEclUri parentFolderUri)
        {
            FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug, "Getting root folder content.");

            List<IContentLibraryListItem> result = new List<IContentLibraryListItem>();
            var archives = FotoWareProvider.Client.GetArchives();

            FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug, string.Format("Retrieved {0} archives from FotoWeb", archives.Count));
            result.AddRange(archives.ConvertAll(Model.Archive.FromXml));
            FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug, string.Format("Converted {0} archives to ECL folders", result.Count));

            foreach (Model.Archive archive in result)
            {
                archive.Id = FotoWareProvider.HostServices.CreateEclUri(
                    publicationId: parentFolderUri.PublicationId,
                    mountPointId: parentFolderUri.MountPointId,
                    itemId: archive.InternalId,
                    subType: Model.Archive.DisplayTypeIdentifier,
                    itemType: EclItemTypes.Folder
                );
                FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug, 
                    string.Format("Archive {0} ({1}) has ECL URI {2}", 
                        archive.Title, 
                        archive.InternalId,
                        archive.Id)
                );
            }
            return result;
        }

        public IContentLibraryItem GetItem(IEclUri eclUri)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("Getting item {0}", eclUri)
            );

            IContentLibraryItem result = null;
            switch(eclUri.SubType)
            {
                case Model.Asset.DisplayTypeIdentifier:

                    // get first asset with supplied unique document ID
                    string uniqueId = GetUniqueDocumentIdFromEclUri(eclUri);
                    var assets = GetAssetsByMetadataValue(API.SearchFields.Metadata.UniqueDocumentId, uniqueId);
                    if(assets!=null)
                    {
                        var asset = assets.FirstOrDefault() as Model.Asset;
                        if(asset!=null)
                        {
                            asset.Id = eclUri;
                            result = asset;
                        }
                    }
                    break;

                case Model.Archive.DisplayTypeIdentifier:
                    break;

                default:
                    break;
            }

            return result;
        }

        private List<Model.Asset> GetAssetsByMetadataValue(string field, string value)
        {
            return GetAssetsByMetadataValue(field, value, null);
        }
        private List<Model.Asset> GetAssetsByMetadataValue(string field, string value, IEnumerable<int> previewSizes)
        {
            List<Model.Asset> results = null;
            string searchTerm = string.Format("({0} contains ({1}))", field, value);
            bool? hasMoreResults;

            var assets = FotoWareProvider.Client.GetSearchResults(
                archive: null,
                searchTerm: searchTerm,
                numberOfResults: FotoWareClient.DefaultPageSize,
                skip: 0,
                includeFileInfo: true,
                includeMetadata: true,
                previewSizes: previewSizes,
                hasMoreResults: out hasMoreResults);

            if(assets!=null && assets.Count > 0)
            {
                results = assets.ConvertAll(Model.Asset.FromXml);
            }

            return results;
        }
        private string GetUniqueDocumentIdFromEclUri(IEclUri uri)
        {
            string result = null;
            if (uri != null && Model.Asset.DisplayTypeIdentifier.Equals(uri.SubType))
            {
                result = uri.ItemId.Replace(Model.Asset.WhitespaceReplacement, " ");
            }
            return result;
        }

        public IList<IContentLibraryItem> GetItems(IList<IEclUri> eclUris)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("GetItems called for items {0}", eclUris)
            );
            throw new NotImplementedException();
        }

        public byte[] GetThumbnailImage(IEclUri eclUri, int maxWidth, int maxHeight)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("Getting thumbnail for item {0}", eclUri)
            );

            byte[] result = null;
            Uri thumbnailURL = null;

            switch(eclUri.ItemType)
            {
                case EclItemTypes.File:
                    string uniqueId = GetUniqueDocumentIdFromEclUri(eclUri);
                    var previewSizes = new int[] { maxWidth };
                    var assets = GetAssetsByMetadataValue(API.SearchFields.Metadata.UniqueDocumentId, uniqueId);
                    if(assets!=null && assets.Count > 0)
                    {
                        var asset = assets.FirstOrDefault();
                        var preview = asset != null && asset.Previews != null ? asset.Previews.FirstOrDefault() : null;
                        thumbnailURL = preview != null ? preview.Url : null;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            if(thumbnailURL!=null)
            {
                result = FotoWareProvider.Client.GetByteArrayAsync(thumbnailURL).Result;
            }
            return result;
        }

        public string GetUploadMultimediaItemsUrl(IEclUri parentFolderUri)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("Getting upload URL for folder {0}", parentFolderUri)
            );
            throw new NotImplementedException();
        }

        public string GetViewItemUrl(IEclUri eclUri)
        {
            FotoWareProvider.HostServices.LogMessage(
                LoggingSeverity.Debug,
                string.Format("Getting preview URL for item {0}", eclUri)
            );
            string result = null;
            switch (eclUri.SubType)
            {
                case Model.Asset.DisplayTypeIdentifier:
                    var uniqueId = GetUniqueDocumentIdFromEclUri(eclUri);
                    var previewSize = new int[] { 2048 };
                    var assets = GetAssetsByMetadataValue(API.SearchFields.Metadata.UniqueDocumentId, uniqueId, previewSize);
                    if(assets!=null && assets.Count > 0)
                    {
                        var asset = assets.FirstOrDefault();
                        var preview = asset != null && asset.Previews != null ? asset.Previews.FirstOrDefault() : null;
                        result = preview != null ? preview.Url.AbsoluteUri : null;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

        public IFolderContent Search(IEclUri contextUri, string searchTerm, int pageIndex, int numberOfItems)
        {
            // expand query to include unqiue document id in results
            // used to generate ECL URI
            searchTerm = string.Format("({0}) AND NOT ({1} contains (xnoword))",
                searchTerm,
                API.SearchFields.Metadata.UniqueDocumentId
            );

            FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug,
                string.Format("Searching archive {0} for search term: {1}", contextUri, searchTerm)
            );

            List<IContentLibraryListItem> results = new List<IContentLibraryListItem>();

            int? archive = null;
            bool isLastPage = false;
            bool? hasMoreResults;
            bool includeFileInfo = false;
            bool includeMetadata = true; // required for unique document
            bool canUpload = CanGetUploadMultimediaItemsUrl(contextUri.PublicationId);
            bool canSearch = CanSearch(contextUri.PublicationId);

            if(contextUri.ItemType.HasFlag(EclItemTypes.MountPoint))
            {
                // for mountpoints, aggregate results from archives by passing a null reference
                archive = null;
            }
            else
            {
                // for archives, search only within the archive
                int parentId;
                if(int.TryParse(contextUri.ItemId, out parentId))
                {
                    archive = parentId;
                }

                // TODO: process auto-generated search folders

            }

            var items = FotoWareProvider.Client.GetSearchResults(archive, searchTerm, numberOfItems, pageIndex, includeFileInfo, includeMetadata, out hasMoreResults);
            if (hasMoreResults.HasValue)
            {
                isLastPage = !hasMoreResults.Value;
            }
            results.AddRange(items.ConvertAll(Model.Asset.FromXml));

            foreach(Model.Asset asset in results)
            {
                asset.ParentId = contextUri;
                asset.Id = FotoWareProvider.HostServices.CreateEclUri(
                    publicationId: contextUri.PublicationId,
                    mountPointId: contextUri.MountPointId,
                    itemId: asset.UniqueId,
                    subType: Model.Asset.DisplayTypeIdentifier,
                    itemType: EclItemTypes.File
                );
                FotoWareProvider.HostServices.LogMessage(LoggingSeverity.Debug,
                    string.Format("Asset {0} ({1}) has ECL URI {2}",
                        asset.Title,
                        asset.InternalId,
                        asset.Id)
                );
            }
            return FotoWareProvider.HostServices.CreateFolderContent(contextUri, pageIndex, isLastPage, results, canUpload, canSearch);
        }

        public void StubComponentCreated(IEclUri eclUri, string tcmUri)
        {
            //TODO
        }

        public void StubComponentDeleted(IEclUri eclUri, string tcmUri)
        {
            //TODO
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

        private IEclSession _session;
    }
}
