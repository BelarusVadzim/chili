using Chili.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chili
{
    class Parser
    {
        string XMLFilePath = "";
        public RenderData Parse()
        {

            RenderData RData = new RenderData();
            var xml = XDocument.Load(XMLFilePath);
            RData.RootX = LoadRootXFromXML(xml);
            RData.RootY = LoadRootYFromXML(xml);
            RData.OriginalDocumentHeigth = LoadOriginalDocumentHeigthFromXML(xml);
            RData.OriginalDocumentWidth= LoadOriginalDocumentWidthFromXML(xml);
            IEnumerable<XElement> elements = xml.Root.Element("panels").Elements("item");
            List<XPanel> panels = new List<XPanel>();
            foreach (XElement item in elements)
            {
                AddDataToListOfPanel(item, null, panels);
            }
            RData.ListOfXPanel = panels;
            return RData;

        }

        public Parser(string XMLFilePath)
        {
            this.XMLFilePath = XMLFilePath;
        }

        #region Private methods
        private void AddDataToListOfPanel(XElement xelement, XPanel parentPanel, List<XPanel> panels)
        {
            XPanel panel = LoadDataToPanelFromXMLElement(xelement, parentPanel);
            panels.Add(panel);
            IEnumerable<XElement> elements = xelement.Element("attachedPanels").Elements("item");

            foreach (XElement element in elements)
            {   
                AddDataToListOfPanel(element, panel, panels);
            }

        }
        private XPanel LoadDataToPanelFromXMLElement(XElement xelement, XPanel parentPanel)
        {
            XPanel panel = new XPanel()
            {
                Id = xelement.Attribute("panelId").Value,
                Name = xelement.Attribute("panelName").Value,
                Width = Double.Parse(xelement.Attribute("panelWidth").Value),
                Heigth = Double.Parse(xelement.Attribute("panelHeight").Value),
                AttachedToSide = int.Parse(xelement.Attribute("attachedToSide").Value),
                HingeOffset = Double.Parse(xelement.Attribute("hingeOffset").Value),
                ParentPanel = parentPanel
            };
            return panel;
        }
        private Double LoadRootXFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("rootX").Value;
            return double.Parse(value);
        }
        private Double LoadRootYFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("rootY").Value;
            return double.Parse(value);
        }
        private Double LoadOriginalDocumentWidthFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("originalDocumentWidth").Value;
            return double.Parse(value);
        }
        private Double LoadOriginalDocumentHeigthFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("originalDocumentHeight").Value;
            return double.Parse(value);
        }
        #endregion
    }
}
