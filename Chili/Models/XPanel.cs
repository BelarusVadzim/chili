using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chili.Models
{
    class XPanel
    {

        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        public float Width { get; set; }

        public float Heigth { get; set; }

        public float HingeOffset { get; set; }

        public int AttachedToSide { get; set; }

        public string ParentPanelId
        {
            get
            {
                return ParentPanel != null? ParentPanel.Id : "" ;
            }
            
        }

        public XPanel ParentPanel { get; set; }

        public List<XPanel> ChildXPanelsList { get; set; }

        public int Orientation 
        {
            get 
            {
                int parentPanelOrientation = ParentPanel != null ? ParentPanel.Orientation : -2;
                switch (AttachedToSide)
                {
                    case 0:
                        return CutOrientation( 2 + parentPanelOrientation );
                    case 2:
                        return CutOrientation( parentPanelOrientation );
                    default:
                        return CutOrientation( AttachedToSide + parentPanelOrientation );
                }
            }
        }
        

        #endregion

        #region Constructor
        public XPanel()
        {

        }
     #endregion

        private int CutOrientation(int Orientation)
        {
            if(Orientation >= 4)
            {
                Orientation = Orientation - 4;
                CutOrientation(Orientation);
            }
            return Orientation;
        }

    }
}
