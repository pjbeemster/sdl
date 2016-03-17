using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tridion.Extensions.ECL.FotoWare.API
{
    internal class MediaTypes
    {
        internal class Json
        {
            internal const string ApiDescriptor = "application/vnd.fotoware.full-api-descriptor+json";
            internal const string AssetUpdate = "application/vnd.fotoware.assetupdate+json";
            internal const string CollectionList = "application/vnd.fotoware.collectionlist+json";
        }

        internal class Forms
        {
            internal const string UrlEncoded = "application/x-www-form-urlencoded";
            internal const string Multipart = "multipart/form-data";
        }
    }
}
