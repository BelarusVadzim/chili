using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chili.Models
{
    class RenderData
    {
        public List<XPanel> ListOfXPanel { get; set; }
        public double RootX { get; set; }
        public double RootY { get; set; }
        public double OriginalDocumentWidth { get; set; }
        public double OriginalDocumentHeigth { get; set; }
    }
}
