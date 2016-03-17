using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tridion.Extensions.ECL.FotoWare.API
{
    internal class SearchFields
    {
        internal class Metadata
        {
            internal const string ActivityType = "IPTC543";
            internal const string AssetCategory = "IPTC530";
            internal const string AssetType = "IPTC531";
            internal const string BusinessArea = "IPTC561";
            internal const string Brand = "IPTC541";
            internal const string ColorGroup = "IPTC548";
            internal const string CustomerName = "IPTC543";
            internal const string EditStatus = "IPTC007";
            internal const string ItemSeason = "IPTC552";
            internal const string LindexFileName = "IPTC521";
            internal const string MarketDescription = "IPTC545";
            internal const string SerialNumber = "IPTC520";
            internal const string ShortDocumentId = "IPTC186";
            internal const string SizeRange = "IPTC550";
            internal const string TargetGroup = "IPTC551";
            internal const string ThemeBusinessArea = "IPTC554";
            internal const string StyleNumber = "IPTC544";
            internal const string UniqueDocumentId = "IPTC187";

            internal static string GetIptcId(string field)
            {
                string iptc = field.Replace("IPTC", "IPTC2:");
                string[] segments = iptc.Split(new char[] { ':' });
                return string.Format("IPTC2:{0}", int.Parse(segments[1]));
            }
        }
        internal class System
        {
            internal const string ImageColor = "FQYIC";
            internal const string FileType = "FQYIC";
            internal const string ImageResolution = "FQYIC";
            internal const string ImageWidth = "FQYIW";
            internal const string ImageHeight = "FQYIH";
            internal const string PixelWidth = "FQYPW";
            internal const string PixelHeight = "FQYPH";
            internal const string FileSize = "FQYFS";
            internal const string ImageSize = "FQYIS";
            internal const string FileName = "FQYFN";
            internal const string FolderName = "FQYFLN";
            internal const string FullPath = "FQYFP";
            internal const string Format = "FQYFM";
            internal const string OfflineStatus = "FQYOL";
            internal const string FileDate = "FQYFD";
            internal const string IptcDate = "FQYID";
            internal const string ReleaseDate = "FQYRD";
        }
    }
}
