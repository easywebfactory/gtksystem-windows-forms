// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Gtk;

namespace System.Windows.Forms
{
    public partial class TreeNode
    {
        public void EnsureVisible()
        {
            if (TreeView != null && !TreeIter.Zero.Equals(this.TreeIter))
            {
                TreePath thispath = TreeView.Store.GetPath(this.TreeIter);
                TreeView.self.TreeView.ExpandToPath(thispath);
                if (thispath != null)
                    TreeView.self.TreeView.ScrollToCell(thispath, TreeView.self.TreeView.Columns[0], false, 0, 0);
            }
        }
    }
}