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
            XPanel P = new XPanel();
            foreach (XElement item in elements)
            {
                AddDataToListOfPanel(item, null, panels);
                P = LoadDataToPanelFromXMLElement2(item);
            }
            RData.ListOfXPanel = panels;
            RData.RootXPanel = P;
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
                Width = float.Parse(xelement.Attribute("panelWidth").Value),
                Heigth = float.Parse(xelement.Attribute("panelHeight").Value),
                AttachedToSide = int.Parse(xelement.Attribute("attachedToSide").Value),
                HingeOffset = float.Parse(xelement.Attribute("hingeOffset").Value),
                ParentPanel = parentPanel
            };
            return panel;
        }

        private XPanel LoadDataToPanelFromXMLElement2(XElement xelement)
        {
            XPanel xPanel = new XPanel()
            {
                Id = xelement.Attribute("panelId").Value,
                Name = xelement.Attribute("panelName").Value,
                Width = float.Parse(xelement.Attribute("panelWidth").Value),
                Heigth = float.Parse(xelement.Attribute("panelHeight").Value),
                AttachedToSide = int.Parse(xelement.Attribute("attachedToSide").Value),
                HingeOffset = float.Parse(xelement.Attribute("hingeOffset").Value)
            };

            var childPanels = xelement.Element("attachedPanels").Elements("item");

            if (childPanels.Count() > 0)
                xPanel.ChildXPanelsList = new List<XPanel>();

            foreach (var item in childPanels)
            {
                XPanel P = LoadDataToPanelFromXMLElement2(item);
                P.ParentPanel = xPanel;
                xPanel.ChildXPanelsList.Add(P);
            }
            return xPanel;
        }

        private float LoadRootXFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("rootX").Value;
            return float.Parse(value);
        }

        private float LoadRootYFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("rootY").Value;
            return float.Parse(value);
        }

        private float LoadOriginalDocumentWidthFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("originalDocumentWidth").Value;
            return float.Parse(value);
        }

        private float LoadOriginalDocumentHeigthFromXML(XDocument xml)
        {
            string value = xml.Root.Attribute("originalDocumentHeight").Value;
            return float.Parse(value);
        }
        #endregion
    }
}
