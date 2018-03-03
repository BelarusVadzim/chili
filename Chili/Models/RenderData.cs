using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chili.Models
{
    class RenderData
    {
        //public List<XPanel> ListOfXPanel { get; set; }
        public float RootX { get; set; }
        public float RootY { get; set; }
        public float OriginalDocumentWidth { get; set; }
        public float OriginalDocumentHeigth { get; set; }
        public XPanel RootXPanel { get; set; }
    }
}
