using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chili.Models
{
    class Render
    {
        #region Properties
        private List<XPanel> ListOfXPanel { get; set; }
        private float RootX { get; set; }
        private float RootY { get; set; }
        private float OriginalDocumentWidth { get; set; }
        private float OriginalDocumentHeigth { get; set; }
        private XPanel RootXPanel { get; set; }
        #endregion

        #region Constructor

        public Render(RenderData RData)
        {
            this.ListOfXPanel = RData.ListOfXPanel;
            this.RootX = RData.RootX;
            this.RootY = RData.RootY;
            this.OriginalDocumentWidth = RData.OriginalDocumentWidth;
            this.OriginalDocumentHeigth = RData.OriginalDocumentHeigth;
            this.RootXPanel = RData.RootXPanel;
        }
        #endregion

        #region Public methods
        public void RenderAll()
        {
            List<RenderedPanel> listOfRenderedXPanel = TransformXPanelToListOfRenderedPanel(RootXPanel, RootX, RootY);
            Bitmap canvas = PrepareCanvas();
            DrawListOfRenderedPanel(canvas, listOfRenderedXPanel);
            canvas.Save("./image.png");
        }
        #endregion

        #region Private methods

        private List<RenderedPanel> TransformXPanelToListOfRenderedPanel(XPanel XPanel, float ParentX, float ParentY)
        {
            List<RenderedPanel> result = new List<RenderedPanel>();
            RenderedPanel RXPanel = new RenderedPanel
            {
                Text = XPanel.Name,
                Color = Color.Black,
                Recatngle = GetRectangleFOfXPanel(XPanel, ParentX, ParentY)
            };
            result.Add(RXPanel);
            if (XPanel.ChildXPanelsList != null)
            {
                foreach (var item in XPanel.ChildXPanelsList)
                {
                    foreach (var renderedXpanel in TransformXPanelToListOfRenderedPanel(item, RXPanel.Recatngle.X, RXPanel.Recatngle.Y))
                    {
                        result.Add(renderedXpanel);
                    }
                }
            }
            return result;
        }

        private RectangleF GetRectangleFOfXPanel(XPanel XPanel, float ParentX, float ParentY)
        {
            RectangleF rec = new Rectangle();
            switch (XPanel.Orientation)
            {
                case 1:
                case 3:
                    rec.Width = XPanel.Heigth;
                    rec.Height = XPanel.Width;
                    break;
                default:
                    rec.Height = XPanel.Heigth;
                    rec.Width = XPanel.Width;
                    break;
            }

            switch (XPanel.Orientation)
            {
                case 1:
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.X = XPanel.ParentPanel.Width + ParentX;
                        rec.Y = ParentY + (XPanel.ParentPanel.Heigth / 2) - rec.Height / 2 + XPanel.HingeOffset;
                    }
                    else
                    {
                        rec.X = (int)XPanel.ParentPanel.Heigth + ParentX;
                        rec.Y = ParentY + (XPanel.ParentPanel.Width / 2) - rec.Height / 2 + XPanel.HingeOffset;
                    }
                    break;
                case 2:
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.Y = ParentY + XPanel.ParentPanel.Heigth;
                        rec.X = ParentX + (XPanel.ParentPanel.Width / 2) - (XPanel.Width / 2) + XPanel.HingeOffset;
                    }
                    else
                    {
                        rec.Y = ParentY + XPanel.ParentPanel.Width;
                        rec.X = ParentX + (XPanel.ParentPanel.Heigth / 2) - (XPanel.Width / 2) - XPanel.HingeOffset;
                    }
                    break;
                case 3:
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.Y = ParentY + (XPanel.ParentPanel.Heigth / 2) - rec.Height / 2 - XPanel.HingeOffset;
                        rec.X = ParentX - XPanel.Heigth;
                    }
                    else
                    {
                        rec.Y = ParentY + (XPanel.ParentPanel.Width / 2) - rec.Height / 2 - XPanel.HingeOffset;
                        rec.X = ParentX - XPanel.Heigth;
                    }
                    break;
                case 0:
                    rec.Y = ParentY - XPanel.Heigth;
                    if (XPanel.ParentPanel != null)
                    {
                        if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                        {
                            rec.X = ParentX + (XPanel.ParentPanel.Width / 2) - (XPanel.Width / 2) + XPanel.HingeOffset;
                        }
                        else
                        {
                            rec.X = ParentX + (XPanel.ParentPanel.Heigth / 2) - (XPanel.Width / 2) + XPanel.HingeOffset;
                        }
                    }
                    else
                    {
                        rec.X = ParentX - XPanel.Width / 2 + XPanel.HingeOffset;
                    }


                    break;
                default:
                    break;
            }
            return rec;
        }

        private void DrawListOfRenderedPanel(Bitmap canvas, List<RenderedPanel> listOfrenderedXPanel)
        {
            using (Graphics gfx = Graphics.FromImage(canvas))
            {
                Pen pen;
                Font F;
                SolidBrush shadowBrush = new SolidBrush(Color.Black);
                foreach (var item in listOfrenderedXPanel)
                {
                    pen = new Pen(item.Color, 3);
                    gfx.DrawRectangle(pen, item.Recatngle.X, item.Recatngle.Y, 
                        item.Recatngle.Width, item.Recatngle.Height);
                    F = new Font("Arial", 10);
                    gfx.DrawString(item.Text, F, shadowBrush, item.Recatngle.X + 5,
                        item.Recatngle.Y + 5);
                }
            }
        }

        private Bitmap PrepareCanvas()
        {
            Bitmap B = new Bitmap((int)OriginalDocumentWidth, (int)OriginalDocumentHeigth);
            using (Graphics gfx = Graphics.FromImage(B))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, (float)OriginalDocumentWidth, (float)OriginalDocumentHeigth);
            }
            return B;
        }


        #endregion
    }
}
