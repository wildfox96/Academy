using System.Collections;

namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;

    public class JQTreeView
    {
        public JQTreeView()
        {
            this.ID = "";
            this.DataUrl = "";
            this.DragAndDropUrl = "";
            this.Width = Unit.Empty;
            this.Height = Unit.Empty;
            this.HoverOnMouseOver = true;
            this.CheckBoxes = false;
            this.MultipleSelect = false;
            this.NodeTemplateID = "";
            this.ClientSideEvents = new TreeViewClientSideEvents();
            this.DragAndDrop = false;
        }

        public JsonResult DataBind(List<JQTreeNode> nodes)
        {
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new JavaScriptSerializer().Serialize(this.SerializeNodes(nodes)) };
        }

        public List<JQTreeNode> GetAllNodesFlat(List<JQTreeNode> nodes)
        {
            List<JQTreeNode> result = new List<JQTreeNode>();
            foreach (JQTreeNode node in nodes)
            {
                result.Add(node);
                if (node.Nodes.Count > 0)
                {
                    this.GetNodesFlat(node.Nodes, result);
                }
            }
            return result;
        }

        public JQTreeNodeDropEventArgs GetDragDropInfo()
        {
            JQTreeNodeDropEventArgs args = new JQTreeNodeDropEventArgs();
            NameValueCollection form = HttpContext.Current.Request.Form;
            return args;
        }

        private void GetNodesFlat(List<JQTreeNode> nodes, List<JQTreeNode> result)
        {
            foreach (JQTreeNode node in nodes)
            {
                result.Add(node);
                if (node.Nodes.Count > 0)
                {
                    this.GetNodesFlat(node.Nodes, result);
                }
            }
        }

        private List<Hashtable> SerializeNodes(List<JQTreeNode> nodes)
        {
            List<Hashtable> list = new List<Hashtable>();
            foreach (JQTreeNode node in nodes)
            {
                list.Add(node.ToHashtable());
            }
            return list;
        }

        public bool CheckBoxes { get; set; }

        public TreeViewClientSideEvents ClientSideEvents { get; set; }

        public string DataUrl { get; set; }

        public bool DragAndDrop { get; set; }

        public string DragAndDropUrl { get; set; }

        public Unit Height { get; set; }

        public bool HoverOnMouseOver { get; set; }

        public string ID { get; set; }

        public bool MultipleSelect { get; set; }

        public string NodeTemplateID { get; set; }

        public Unit Width { get; set; }
    }
}

