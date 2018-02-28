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
        private List<RenderedXpanel> ListOfRenderedXPanel { get; set; }
        private double RootX { get; set; }
        private double RootY { get; set; }
        private double OriginalDocumentWidth { get; set; }
        private double OriginalDocumentHeigth { get; set; }
        #endregion

        #region Constructor

        public Render(RenderData RData)
        {
            this.ListOfXPanel = RData.ListOfXPanel;
            this.RootX = RData.RootX - ListOfXPanel[0].Width / 2;
            this.RootY = RData.RootY- ListOfXPanel[0].Heigth;
            this.OriginalDocumentWidth = RData.OriginalDocumentWidth;
            this.OriginalDocumentHeigth = RData.OriginalDocumentHeigth;
    }
        #endregion

        #region Public methods
        public void RenderAll()
        {

            RenderXPanel(ListOfXPanel[0]);
            Bitmap canvas = PrepareCanvas();
            foreach(var item in ListOfRenderedXPanel)
            {
                DrawXPanel(canvas, item);
            }
            canvas.Save("./image.png");

        }
        #endregion



        #region Privte methods




        private void RenderChildXPanel(XPanel ParentXPanel, int ParentPanelX, int ParentPanelY)
        {

            var ChildXPanels = ListOfXPanel.Where(item => item.ParentPanelId == ParentXPanel.Id);
            foreach (var item in ChildXPanels)
            {
                RenderXPanel(item, ParentPanelX, ParentPanelY);
            }
        }
        
        private void RenderXPanel(XPanel XPanel)
        {
            ListOfRenderedXPanel = new List<RenderedXpanel>();
            RenderedXpanel RXPanel = new RenderedXpanel();
            RXPanel.XRecatngle = GetRectangleOfRenderedXPanel(XPanel);
            RXPanel.Text = "Root panel";
            RXPanel.XColor = Color.Black;
            //render XPanel
            ListOfRenderedXPanel.Add(RXPanel);
            RenderChildXPanel(XPanel, RXPanel.XRecatngle.X, RXPanel.XRecatngle.Y);
        }

        private void RenderXPanel(XPanel XPanel, int ParentPanelX, int ParentPanelY)
        {
            RenderedXpanel RXPanel = new RenderedXpanel()
            {
                XRecatngle = GetRectangleOfRenderedXPanel(XPanel, ParentPanelX, ParentPanelY)
            };
            RXPanel.Text = XPanel.Name;
            RXPanel.XColor = Color.Black;
            ListOfRenderedXPanel.Add(RXPanel);
            RenderChildXPanel(XPanel, RXPanel.XRecatngle.X, RXPanel.XRecatngle.Y);
        }

        private Rectangle GetRectangleOfRenderedXPanel(XPanel XPanel, int ParentPanelX, int ParentPanelY)
        {
            Rectangle rec = new Rectangle();
            switch (XPanel.Orientation)
            {
                case 1:
                case 3:
                    rec.Width = (int)XPanel.Heigth;
                    rec.Height = (int)XPanel.Width;
                    break;
                default:
                    rec.Height = (int)XPanel.Heigth;
                    rec.Width = (int)XPanel.Width;
                    break;
            }

            switch (XPanel.Orientation)
            {
                case 1:
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.X = (int)XPanel.ParentPanel.Width + ParentPanelX;
                        rec.Y = ParentPanelY + (int)(XPanel.ParentPanel.Heigth / 2) - rec.Height / 2 + (int)XPanel.HingeOffset;
                    }
                    else
                    {
                        rec.X = (int)XPanel.ParentPanel.Heigth + ParentPanelX;
                        rec.Y = ParentPanelY + (int)(XPanel.ParentPanel.Width / 2) - rec.Height / 2 + (int)XPanel.HingeOffset;
                    }
                    break;
                case 2:
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.Y = ParentPanelY + (int)XPanel.ParentPanel.Heigth;
                        rec.X = ParentPanelX + (int)(XPanel.ParentPanel.Width / 2) - (int)(XPanel.Width / 2) + (int)XPanel.HingeOffset;
                    }
                    else
                    {
                        rec.Y = ParentPanelY + (int)XPanel.ParentPanel.Width;
                        rec.X = ParentPanelX + (int)(XPanel.ParentPanel.Heigth / 2) - (int)(XPanel.Width / 2) - (int)XPanel.HingeOffset;
                    }
                    break;
                case 3:
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.Y = ParentPanelY + (int)(XPanel.ParentPanel.Heigth / 2) - rec.Height / 2 - (int)XPanel.HingeOffset;
                        rec.X = ParentPanelX - (int)XPanel.Width;
                    }
                    else
                    {
                        rec.Y = ParentPanelY + (int)(XPanel.ParentPanel.Width / 2) - rec.Height / 2 - (int)XPanel.HingeOffset;
                        rec.X = ParentPanelX - (int)XPanel.Heigth;
                    }
                    break;
                case 0:
                    rec.Y = ParentPanelY - (int)XPanel.Heigth;
                    if (XPanel.ParentPanel.Orientation == 0 || XPanel.ParentPanel.Orientation == 2)
                    {
                        rec.X = ParentPanelX + (int)(XPanel.ParentPanel.Width / 2) - (int)(XPanel.Width / 2) + (int)XPanel.HingeOffset;
                    }
                    else
                    {
                        rec.X = ParentPanelX + (int)(XPanel.ParentPanel.Heigth / 2) - (int)(XPanel.Width / 2) + (int)XPanel.HingeOffset;
                    }

                    break;
                default:
                    break;
            }
            return rec;
        }

        private Rectangle GetRectangleOfRenderedXPanel(XPanel XPanel)
        {
            Rectangle rec = new Rectangle()
            {
                Height = (int)XPanel.Heigth,
                Width = (int)XPanel.Width,
                X = (int)RootX,
                Y = (int)RootY
            };
            return rec;
        }

        private void DrawRootXPanel(Bitmap bitmap)
        {
            //using (Graphics gfx = Graphics.FromImage(bitmap))
            //using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            //{
            //    Color customColor = Color.FromArgb(50, Color.Gray);
            //    SolidBrush shadowBrush = new SolidBrush(customColor);
            //    Pen p = new Pen(Color.Black, 2);

            //    gfx.DrawRectangle(p, 20, 20, 300, 300);
            //}
        }

        private void DrawChildXPanel(Bitmap bitmap)
        {
            //using (Graphics gfx = Graphics.FromImage(bitmap))
            //using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            //{
            //    Color customColor = Color.FromArgb(50, Color.Gray);
            //    SolidBrush shadowBrush = new SolidBrush(customColor);
            //    Pen p = new Pen(Color.Black, 2);

            //    gfx.DrawRectangle(p, 20, 20, 300, 300);
            //}
        }

        private void DrawXPanel(Bitmap bitmap, RenderedXpanel renderedXPanel)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                Pen p = new Pen(renderedXPanel.XColor, 3);
                gfx.DrawRectangle(p, renderedXPanel.XRecatngle);
                Font F = new Font("Arial", 10);
                SolidBrush shadowBrush = new SolidBrush(Color.Black);
                gfx.DrawString(renderedXPanel.Text, F, shadowBrush, renderedXPanel.XRecatngle.X + 5, renderedXPanel.XRecatngle.Y + 5);
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
