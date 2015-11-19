namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class JQTreeNodeDropEventArgs
    {
        public JQTreeNodeDropEventArgs()
        {
        }

        public JQTreeNodeDropEventArgs(List<JQTreeNode> draggedNodes, JQTreeNode destinationNode, string sourceTreeViewID)
        {
            this.DraggedNodes = draggedNodes;
            this.DestinationNode = destinationNode;
            this.SourceTreeViewID = this.SourceTreeViewID;
        }

        public JQTreeNode DestinationNode { get; set; }

        public List<JQTreeNode> DraggedNodes { get; set; }

        public string SourceTreeViewID { get; set; }
    }
}

