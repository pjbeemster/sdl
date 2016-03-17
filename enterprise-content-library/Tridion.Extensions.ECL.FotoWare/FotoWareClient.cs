using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Tridion.Extensions.ECL.FotoWare
{
    internal class FotoWareClient : System.Net.Http.HttpClient
    {
        private FotoWareClient(HttpClientHandler handler, Uri baseUrl) : base(handler)
        {
            this.Handler = handler;
            this.BaseAddress = baseUrl;
        }

        internal string GetCookieValue(HttpResponseMessage response, string name)
        {
            string result = null;
            if (response != null)
            {
                var cookies = Handler.CookieContainer.GetCookies(response.RequestMessage.RequestUri);
                var cookie = cookies[name];
                if (cookie != null)
                {
                    result = cookie.Value;
                }
            }
            return result;
        }

        internal List<XElement> GetArchives()
        {
            List<XElement> result = new List<XElement>();

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseAddress, ArchiveAgentURL));
            var response = SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var xdoc = XDocument.Parse(content);
                result.AddRange(xdoc.Descendants(XName.Get("Archive", FotoWareXmlNamespace)));
            }
            return result;
        }
        internal List<XElement> GetSearchResults(int? archive, string searchTerm, out bool? hasMoreResults)
        {
            return GetSearchResults(archive, searchTerm, DefaultPageSize, DefaultSkip, DefaultIncludeFileInfo, DefaultIncludeMetadata, out hasMoreResults);
        }
        internal List<XElement> GetSearchResults(int? archive, string searchTerm, int numberOfResults, int skip, bool includeFileInfo, bool includeMetadata, out bool? hasMoreResults)
        {
            return GetSearchResults(archive, searchTerm, numberOfResults, skip, includeFileInfo, includeMetadata, null, out hasMoreResults);
        }
        internal List<XElement> GetSearchResults(int? archive, string searchTerm, int numberOfResults, int skip, bool includeFileInfo, bool includeMetadata, IEnumerable<int> previewSizes, out bool? hasMoreResults)
        {
            hasMoreResults = null;

            // get one additional hit than requested, except if causing overflow
            // used to determine whether there are more search results (hasMoreResults)
            int maxHits = int.MaxValue.Equals(numberOfResults) ? numberOfResults : numberOfResults + 1;

            List<int> archiveIds = new List<int>();
            if (archive.HasValue)
            {
                // only search specified archive
                archiveIds.Add(archive.Value);
            }
            else
            {
                // aggregate results from all archives
                archiveIds.AddRange(GetArchivesContainingResults(searchTerm));
            }
            
            var result = new List<XElement>(maxHits * archiveIds.Count()); // limit result list capacity to minimize memory footprint

            foreach (int archiveId in archiveIds)
            {
                var requestURL = BaseAddress.Append(ArchiveAgentURL, archiveId.ToString(), API.Methods.Search);
                var builder = new UriBuilder(requestURL);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query[API.QueryStringArguments.Search] = searchTerm;
                query[API.QueryStringArguments.MaxHits] = Convert.ToString(maxHits);
                query[API.QueryStringArguments.Skip] = Convert.ToString(skip);
                query[API.QueryStringArguments.FileInfo] = includeFileInfo ? "1" : "0";
                query[API.QueryStringArguments.Metadata] = includeMetadata ? "1" : "0";
                builder.Query = query.ToString();

                if (previewSizes != null)
                {
                    // preview size is a repeating argument, so cannot be added using name value collection
                    char[] separator = new[] { '?' };
                    foreach (int size in previewSizes)
                    {
                        if (size > 0)
                        {
                            builder.Query = string.Format("{0}&{1}={2}", builder.Query, API.QueryStringArguments.PreviewSize, size).TrimStart(separator);
                        }
                    }
                }

                var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
                var response = SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var xdoc = XDocument.Parse(content);
                    result.AddRange(xdoc.Descendants(XName.Get("File", FotoWareXmlNamespace)));
                }
            }
            if(result.Count > numberOfResults)
            {
                // query returned more results than requested - remove excess entry
                hasMoreResults = true;
                result = result.GetRange(0, numberOfResults);
            }
            return result;
        }

        private List<int> GetArchivesContainingResults(string searchTerm)
        {
            List<int> result = new List<int>();

            var requestURL = BaseAddress.Append(ArchiveAgentURL, API.Methods.Search);
            var builder = new UriBuilder(requestURL);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query[API.QueryStringArguments.Search] = searchTerm;
            query[API.QueryStringArguments.FileInfo] = "0";
            query[API.QueryStringArguments.Metadata] = "0";
            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            var response = SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var xdoc = XDocument.Parse(content);
                var hits = xdoc.Descendants(XName.Get("Archive", FotoWareXmlNamespace)).Where(x => int.Parse(x.Attribute("Hits").Value) > 0);
                foreach(var archive in hits)
                {
                    int archiveId = int.Parse(archive.Attribute("Id").Value);
                    result.Add(archiveId);
                }
            }

            return result;
        }

        internal bool Login(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(BaseAddress, LoginURL));
            request.Content = new StringContent(
                content: string.Format("u={0}&p={1}", 
                    HttpUtility.UrlEncode(username),
                    HttpUtility.UrlEncode(password)
                ), 
                encoding: Encoding.UTF8,
                mediaType: API.MediaTypes.Forms.UrlEncoded
            );
            var response = SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
            AuthenticationToken = GetCookieValue(response, API.Cookies.AuthenticationToken);
            SessionId = GetCookieValue(response, API.Cookies.SessionId);
            return !string.IsNullOrWhiteSpace(AuthenticationToken);
        }

        internal HttpClientHandler Handler { get; private set; }
        internal string AuthenticationToken { get; private set; }
        internal string SessionId { get; private set; }

        internal static FotoWareClient GetInstance(Uri baseUrl)
        {
            return GetInstance(baseUrl, null, null, null);
        }
        internal static FotoWareClient GetInstance(Uri baseUrl, string username, string password)
        {
            return GetInstance(baseUrl, username, password, null);
        }
        internal static FotoWareClient GetInstance(Uri baseUrl, IWebProxy proxy)
        {
            return GetInstance(baseUrl, null, null, proxy);
        }
        internal static FotoWareClient GetInstance(Uri baseUrl, string username, string password, IWebProxy proxy)
        {
            baseUrl = AssertTrailingSlash(baseUrl);

            var cookieJar = new CookieContainer() { MaxCookieSize = 4096 };
            var handler = new HttpClientHandler() {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = cookieJar,
                Proxy = proxy,
                UseCookies = true,
                UseProxy = proxy!=null
            };
            var instance = new FotoWareClient(handler, baseUrl);
            instance.DefaultRequestHeaders.ConnectionClose = true;
            if(!string.IsNullOrWhiteSpace(username))
            {
                instance.Login(username, password);
            }

            return instance;
        }

        private static Uri AssertTrailingSlash(Uri uri)
        {
            return uri.ToString().EndsWith("/") ? uri : new Uri(uri, "/");
        }

        internal static readonly List<int> DefaultPreviewSizes = new List<int>() { 64, 200, 800, 1600 };
        internal static readonly int DefaultPageSize = 25;
        internal static readonly int DefaultSkip = 0;
        internal static readonly bool DefaultIncludeFileInfo = false;
        internal static readonly bool DefaultIncludeMetadata = false;

        internal static readonly string FotoWareXmlNamespace = string.Empty;
        internal static readonly string ArchiveAgentURL = "fwbin/fotoweb_isapi.dll/ArchiveAgent/";
        internal static readonly string LoginURL = "cmdrequest/Login.fwx";

    }
}
