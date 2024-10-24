
using Gtk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace System.Windows.Forms
{
	[ListBindable(false)]
	public class TableLayoutControlCollection : ControlCollection
	{
		public TableLayoutPanel Container
		{
			[CompilerGenerated]
			get;
			protected set;
		}
        public TableLayoutControlCollection(Control owner, Gtk.Container ownerContainer) : base(owner, ownerContainer)
        {
            Container = owner as TableLayoutPanel;
            Container.self.Realized += Self_Realized;
        }
        public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
        {
            Container = container;
            Container.self.Realized += Self_Realized;
        }
        private bool SelfRealized = false;
        private void Self_Realized(object sender, EventArgs e)
        {
            if (SelfRealized == false)
            {
                SelfRealized = true;
                PerformLayout();
            }
        }

        public void PerformLayout()
        {
            int[] colsWidth = Container.GetColumnWidths();
            int[] rowsHeight = Container.GetRowHeights();
            int colLeft = 0;
            int rowTop = 0;
            for (int col = 0; col < Container.ColumnCount; col++)
            {
                rowTop = 0;
                colLeft += colsWidth[col];
                for (int row = 0; row < Container.ColumnCount; row++)
                {
                    rowTop += rowsHeight[row];
                    Gtk.Viewport viewport = new Gtk.Viewport();
                    viewport.BorderWidth = 0;
                    if (Container.CellBorderStyle == TableLayoutPanelCellBorderStyle.None)
                        viewport.ShadowType = Gtk.ShadowType.None;
                    else if (Container.CellBorderStyle == TableLayoutPanelCellBorderStyle.Single || Container.CellBorderStyle == TableLayoutPanelCellBorderStyle.Inset)
                        viewport.ShadowType = Gtk.ShadowType.In;
                    else if (Container.CellBorderStyle == TableLayoutPanelCellBorderStyle.Outset)
                        viewport.ShadowType = Gtk.ShadowType.Out;
                    else if (Container.CellBorderStyle == TableLayoutPanelCellBorderStyle.InsetDouble)
                    {
                        viewport.ShadowType = Gtk.ShadowType.In;
                        viewport.BorderWidth = 1;
                    }
                    else if (Container.CellBorderStyle == TableLayoutPanelCellBorderStyle.OutsetDouble)
                    {
                        viewport.ShadowType = Gtk.ShadowType.Out;
                        viewport.BorderWidth = 1;
                    }
                    else
                    {
                        viewport.ShadowType = Gtk.ShadowType.In;
                    }

                    viewport.Margin = 0;
                    viewport.MarginStart = 0;
                    viewport.MarginTop = 0;
                    viewport.WidthRequest = colsWidth[col];
                    viewport.HeightRequest = rowsHeight[row];
                    Gtk.Layout layout = new Gtk.Layout(new Adjustment(0, 0, 0, 1, 0, 0), new Adjustment(0, 0, 0, 1, 0, 0));
                    layout.Vexpand = true;
                    layout.Hexpand = true;
                    viewport.Child = layout;

                    if (this.GetCellControl(col, row) is TableLayoutControllCell cell)
                    {
                        cell.Control.Widget.MarginStart = 3;
                        cell.Control.Widget.MarginTop = 3;
                        layout.Add(cell.Control.Widget);
                    }

                    Container.self.Attach(viewport, colLeft, rowTop, 1, 1);
                }
            }
            Container.self.ShowAll();
        }
        public object GetCellControl(int column, int row)
        {
            foreach (TableLayoutControllCell cell in this)
            {
                if (cell.TableLayoutPanelCellPosition.Column == column && cell.TableLayoutPanelCellPosition.Row == row)
                    return cell;
            }
            return null;
        }

        public virtual void Add(Control control, int column, int row)
		{
            base.Add(new TableLayoutControllCell(control, new TableLayoutPanelCellPosition(column, row)));
        }

        public class TableLayoutControllCell
        {
            public TableLayoutControllCell(Control control, TableLayoutPanelCellPosition tableLayoutPanelCellPosition)
            {
                this.Control = control;
                this.TableLayoutPanelCellPosition = tableLayoutPanelCellPosition;
            }
            public Control Control { get; set; }
            public TableLayoutPanelCellPosition TableLayoutPanelCellPosition { get; set; }
        }
    }
}
