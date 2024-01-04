/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System.ComponentModel;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TreeView : WidgetControl<Gtk.TreeView>
    {
        private Gtk.TreeStore _store;
        internal TreeNode root;
        internal Gtk.TreeStore Store { get { return _store; } }
        private Gtk.TreeViewColumn textcolumn;
        private Gtk.TreeViewColumn checkboxcolumn;
        public TreeView()
        {
            Widget.StyleContext.AddClass("TreeView");
            base.Control.BorderWidth = 0;
            base.Control.Expand = true;
            base.Control.HeadersVisible = false;
            base.Control.ActivateOnSingleClick = true;
            root = new TreeNode(this);
            root.Name = "root";

            Gtk.CellRendererText renderertext = new Gtk.CellRendererText();
            renderertext.IsExpanded = true;
            renderertext.PlaceholderText = "---";
            textcolumn = new Gtk.TreeViewColumn(" 菜单列表", renderertext, "text", 0);

            CellRendererToggle renderercheckbox = new CellRendererToggle();
            renderercheckbox.Activatable = true;
            renderercheckbox.IsExpanded = true;
            renderercheckbox.Toggled += CellName_Toggled;
            checkboxcolumn = new Gtk.TreeViewColumn("", renderercheckbox, "active", 1);
            
            _store = new Gtk.TreeStore(typeof(string), typeof(bool));
            base.Control.Model = _store;
            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            base.Control.AppendColumn(textcolumn);
            if (CheckBoxes == true)
                base.Control.AppendColumn(checkboxcolumn);

            //Console.Write("TreeView_Shown"+ root.Nodes.Count);
            foreach (TreeNode child in root.Nodes)
            {
                child.Index = ++index;
                TreeIter ti = Store.InsertWithValues(child.Index, child.Text, false);
                loadNodeChildrenValue(child, ti);
            }
            base.Control.ExpandAll();
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            //Console.WriteLine("CellRendererToggle CellName_Toggled");
            TreePath path = new TreePath(args.Path);
            var model = _store;
            model.GetIter(out TreeIter iter, path);
            bool val = (bool)(model.GetValue(iter, 1));
            model.SetValue(iter, 1, val == false);
          
        }

        int index = 0;
        private void loadNodeChildrenValue(TreeNode node, TreeIter parent)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Index = ++index;
                TreeIter ti = Store.InsertWithValues(parent, child.Index, child.Text, false);
                loadNodeChildrenValue(child, ti);
            }
        }
        public TreeNodeCollection Nodes
        {
            get
            {
                return root.Nodes;
            }
        }
       
        public bool CheckBoxes { get; set; }
        public bool ShowLines { get; set; } = true;
        public bool ShowNodeToolsTips { get; set; }
        public bool ShowPlusMinus { get; set; } = true;
        public bool ShowRootLines { get; set; } = true;

        TreeViewCancelEventArgs cancelEventArgs = null;
        public event TreeViewCancelEventHandler BeforeSelect
        {
            add
            {
                base.Control.Selection.Changed += (object sender, EventArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        if (base.Control.Selection.GetSelected(out TreeIter iter))
                        {
                            TreePath[] paths = base.Control.Selection.GetSelectedRows();
                            GetNodeChild(root, paths[0].Indices[0], out TreeNode result);
                            cancelEventArgs = new TreeViewCancelEventArgs(result, false, TreeViewAction.ByMouse);
                            value.Invoke(sender, cancelEventArgs);
                        }
                    }
                };
            }
            remove
            {
                base.Control.Selection.Changed -= (object sender, EventArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        if (base.Control.Selection.GetSelected(out TreeIter iter))
                        {
                            TreePath[] paths = base.Control.Selection.GetSelectedRows();
                            GetNodeChild(root, paths[0].Indices[0], out TreeNode result);
                            cancelEventArgs = new TreeViewCancelEventArgs(result, false, TreeViewAction.ByMouse);
                            value.Invoke(sender, cancelEventArgs);
                        }
                    }
                };
            }
        }
        public event TreeViewEventHandler AfterSelect
        {
            add
            {
                base.Control.RowActivated += (object sender, Gtk.RowActivatedArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        if (cancelEventArgs == null || cancelEventArgs.Cancel == false)
                        {
                            GetNodeChild(root, e.Path.Indices[0], out TreeNode result);
                            value.Invoke(this, new TreeViewEventArgs(result));
                        }
                    }
                };
            }
            remove
            {
                base.Control.RowActivated -= (object sender, Gtk.RowActivatedArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        if (cancelEventArgs == null || cancelEventArgs.Cancel == false)
                        {
                            GetNodeChild(root, e.Path.Indices[0], out TreeNode result);
                            value.Invoke(this, new TreeViewEventArgs(result));
                        }
                    }
                };
            }
        }

        public event TreeViewEventHandler AfterCollapse
        {
            add
            {
                base.Control.RowCollapsed += (object sender, Gtk.RowCollapsedArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        GetNodeChild(root, e.Path.Indices[0], out TreeNode result);
                        value.Invoke(this, new TreeViewEventArgs(result));
                    }
                };
            }
            remove
            {
                base.Control.RowCollapsed -= (object sender, Gtk.RowCollapsedArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        GetNodeChild(root, e.Path.Indices[0], out TreeNode result);
                        value.Invoke(this, new TreeViewEventArgs(result));
                    }
                };
            }
        }

        public event TreeViewEventHandler AfterExpand
        {
            add
            {
                base.Control.RowExpanded += (object sender, Gtk.RowExpandedArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        GetNodeChild(root, e.Path.Indices[0], out TreeNode result);
                        value.Invoke(this, new TreeViewEventArgs(result));
                    }
                };
            }
            remove
            {
                base.Control.RowExpanded -= (object sender, Gtk.RowExpandedArgs e) =>
                {
                    if (base.Control.IsRealized)
                    {
                        GetNodeChild(root, e.Path.Indices[0], out TreeNode result);
                        value.Invoke(this, new TreeViewEventArgs(result));
                    }
                };
            }
        }

        private void GetNodeChild(TreeNode node, int index,out TreeNode result)
        {
            result = null;
            foreach (TreeNode child in node.Nodes)
            {
                if(child.Index == index)
                {
                    result = child;
                }
                else
                {
                    GetNodeChild(child, index,out result);
                }
            }
        }
    }
}
