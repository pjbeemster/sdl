using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.UnitTests.Mock
{
    class TridionUserMockup : ITridionUser
    {
        public string FullName { get; internal set; }

        public IList<ITridionGroup> GroupMemberships { get; internal set; }

        public string Id { get; internal set; }

        public bool IsAdministrator { get; internal set; }

        public bool IsTrustedReadOnlyMode { get; internal set; }

        public string UserName { get; internal set; }

        public string GetSamlToken(string appliesTo)
        {
            throw new NotImplementedException();
        }

        public string GetSamlToken(string appliesTo, out byte[] proofTokenBytes, out DateTime validFrom, out DateTime validTo, out string internalTokenReferenceAssertionId, out string externalTokenReferenceAssertionId)
        {
            throw new NotImplementedException();
        }

        public bool IsInGroup(string groupUri, string publicationUri)
        {
            throw new NotImplementedException();
        }
    }
}
