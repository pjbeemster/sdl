using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ExternalContentLibrary.V2;

namespace Tridion.Extensions.ECL.FotoWare.UnitTests.Mock
{
    class DisplayTypeMockup : IDisplayType
    {
        public DisplayTypeMockup(string id, string displayText, EclItemTypes itemType)
        {
            Id = id;
            DisplayText = displayText;
            ItemType = itemType;
        }
        public string DisplayText { get; private set; }

        public string Id { get; private set; }

        public EclItemTypes ItemType { get; private set; }
    }
}
