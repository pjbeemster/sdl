using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.UnitTests
{
    [TestClass]
    public class ApiClientTests
    {
        public const string BaseURL = "https://fotoweb.lindex.com/fotoweb/";
        public const string UserName = "Edling-Dev";
        public const string Password = "Hämta bilder nu";

        public const int PublicationId = 126;
        public const string MountpountId = "fwx";


        private IContentLibraryContext GetContext(IContentLibrary provider)
        {
            IHostServices services = new Mock.HostServicesMockup();
            ITridionUser user = new Mock.TridionUserMockup()
            {
                FullName = "Unit Test",
                UserName = "unittest",
                Id = "tcm:0-1-65536",
                IsAdministrator = true,
                IsTrustedReadOnlyMode = true
            };
            IEclSession session = new Mock.EclSessionMockup(services, user);
            var context = provider.CreateContext(session);
            return context;
        }

        private FotoWareProvider GetFotoWareProvider()
        {
            IHostServices hostServices = new Mock.HostServicesMockup();
            string configXML = Properties.Resources.ExternalContentLibrary_FotoWare;
            string moutpointId = "UnitTestID";

            FotoWareProvider provider = new FotoWareProvider();
            provider.Initialize(moutpointId, configXML, hostServices);
            return provider;
        }

        private FotoWareClient GetFotoWareClient()
        {
            var baseUri = new Uri(BaseURL);
            var client = FotoWareClient.GetInstance(baseUri, UserName, Password);
            return client;
        }

        [TestMethod]
        public void Configuration()
        {
            var configXML = XDocument.Parse(Tridion.Extensions.ECL.FotoWare.Properties.Resources.ExternalContentLibrary_FotoWare);
            var mountpointXName = XName.Get("MountPoint", "http://www.sdltridion.com/ExternalContentLibrary/Configuration");
            var configSource = configXML.Descendants(mountpointXName).FirstOrDefault();

            var config = new Configuration(configSource);
            var baseURL = config.BaseUrl;
            var uid = config.UserName;
            var pwd = config.Password;

            Assert.IsFalse(string.IsNullOrWhiteSpace(uid));
            Assert.IsFalse(string.IsNullOrWhiteSpace(pwd));
            Assert.IsNotNull(baseURL);

            Assert.IsTrue(baseURL.ToString().StartsWith("https://fotoweb.lindex.com/fotoweb/"));
            Assert.IsTrue(string.Equals(UserName, uid));
            Assert.IsTrue(string.Equals(Password, pwd));
        }

        [TestMethod]
        public void Initialization()
        {
            var provider = GetFotoWareProvider();
        }
        [TestMethod]
        public void ClientGetRootFolders()
        {
            var client = GetFotoWareClient();
            var archives = client.GetArchives();
            var rootFolders = archives.ConvertAll(Model.Archive.FromXml);

            Assert.IsFalse(archives.Count == 0, "Failed to read top-level archives.");
            Assert.IsFalse(rootFolders.Count == 0, "Failed to convert archives to ECL data model.");
        }
        [TestMethod]
        public void ClientBasicSearch()
        {
            var client = GetFotoWareClient();
            bool? hasMoreItems = null;

            int? archiveId = null; // 5051; // Pack shots
            var searchTerm = string.Format(
                "({0} contains ({1}))",
                API.SearchFields.Metadata.Brand,
                "Lindex"
            );
            var results = client.GetSearchResults(archiveId, searchTerm, out hasMoreItems);

            Assert.IsFalse(results.Count == 0);

        }

        [TestMethod]
        public void EclGetRootFolders()
        {
            IHostServices hostservices = new Mock.HostServicesMockup();
            var provider = new FotoWareProvider();
            var context = GetContext(provider);

            IEclUri rootUri = hostservices.CreateEclUri(126, "fwx", "root", "mp", EclItemTypes.MountPoint);
            context.GetFolderContent(rootUri, 0, EclItemTypes.Folder);
        }

        [TestMethod]
        public void EclBasicSearch()
        {
            var hostServices = new Mock.HostServicesMockup();
            var provider = GetFotoWareProvider();
            var context = GetContext(provider);
            var searchTerm = string.Format(
                "({0} contains ({1}))",
                API.SearchFields.Metadata.Brand,
                "Lindex"
            );
            var rootUri = hostServices.CreateEclUri(
                publicationId: PublicationId,
                mountPointId: MountpountId,
                itemId: "root",
                subType: "mp",
                itemType: EclItemTypes.MountPoint
            );
            var results = context.Search(rootUri, searchTerm, 0, 25);

        }

        [TestMethod]
        public void EclGetItem()
        {
            var hostServices = new Mock.HostServicesMockup();
            var provider = GetFotoWareProvider();
            var context = GetContext(provider);

            // ecl:117-fwx-E9FDDEFC6D0B45E0_B6B5D9E227D8D432-asset-file
            string uniqueId = "E9FDDEFC6D0B45E0_B6B5D9E227D8D432";
            var summerDressURI = hostServices.CreateEclUri(
                publicationId: 117,
                mountPointId: MountpountId,
                itemId: uniqueId,
                subType: Model.Asset.DisplayTypeIdentifier,
                itemType: EclItemTypes.File
            );

            var dress = context.GetItem(summerDressURI);

            Assert.IsNotNull(dress, "Failed to retrieve asset by unique document id: " + uniqueId);
            Assert.IsNotNull(dress.Id);
            Assert.AreEqual(dress.Id.ItemId, uniqueId);
            Assert.AreEqual(dress.Title, "S0000007401168_PS_B.jpg");


            var previewUrl = ((IContentLibraryMultimediaItem)dress).GetDirectLinkToPublished(null);
        }
    }
}
