using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tridion.Extensions.ECL.FotoWare.Model
{
    internal class Preview
    {
        private Preview() { }
        internal int Size { get; set; }
        internal int Id { get; set; }
        internal Uri Url { get; set; }

        internal static Converter<XElement, Preview> FromXml = delegate (XElement source)
        {
            Uri uri = null;
            int id;
            int size;

            Uri.TryCreate(source.Value, UriKind.RelativeOrAbsolute, out uri);
            int.TryParse(source.Attribute("Id").Value, out id);
            int.TryParse(source.Attribute("Size").Value, out size);

            return new Preview()
            {
                Id = id,
                Size = size,
                Url = uri
            };
        };
    }
}
