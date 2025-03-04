using Gtk;
using Image = System.Drawing.Image;

namespace System.Windows.Forms;

public class ToolStripItemCollection : List<ToolStripItem>
{
    private readonly ToolStripItem? owner;
    private readonly ToolStrip? toolStrip;
    private readonly StatusStrip? statusStrip;
    private Menu? menu;
    private readonly bool isToolStrip;
    private readonly bool isStatusStrip;
    private readonly bool isMenuStrip;
    private readonly bool isToolStripDropDown;

    public ToolStripItemCollection(ToolStrip? owner) 
    {
        toolStrip = owner;
        isToolStrip = true;
    }
    public ToolStripItemCollection(ToolStrip? toolStrip, string owner)
    {
        this.toolStrip = toolStrip;
        isMenuStrip = owner == "MenuStrip";
    }
    public ToolStripItemCollection(StatusStrip? owner)
    {
        statusStrip = owner;
        isStatusStrip = true;
    }
    public ToolStripItemCollection(ToolStripDropDown owner)
    {
        menu = owner.Widget as Menu;
        isToolStripDropDown = true;
    }
    public ToolStripItemCollection(ToolStripItem? owner)
    {
        this.owner = owner;
        if(owner?.Widget is Menu gmenu)
        {
            menu = gmenu;
            isToolStripDropDown = true;
        }
    }
    internal ToolStripItemCollection(ToolStripItem? owner, bool itemsCollection)
        : this(owner, itemsCollection, isReadOnly: false)
    {
    }

    internal ToolStripItemCollection(ToolStripItem? owner, bool itemsCollection, bool isReadOnly)
    {
        this.owner = owner;
        isToolStrip = false;

    }
    public ToolStripItemCollection(ToolStripItem? owner, ToolStripItem[] value)
    {
        this.owner = owner;
        isToolStrip = false;
        AddRange(value);
    }
    public ToolStripItem Add(string text)
    {
        return Add(text, null, null);
    }

    public ToolStripItem Add(Image image)
    {
        return Add(null, image, null);
    }


    public ToolStripItem Add(string text, Image image)
    {
        return Add(text, image, null);
    }

    public ToolStripItem Add(string? text, Image? image, EventHandler? onClick)
    {
        ToolStripItem toolStripItem = new ToolStripLabel();
        AddMemu(toolStripItem);
        return toolStripItem;
    }

    public int AddMemu(ToolStripItem item)
    {
        item.Parent = owner;
        if (isToolStrip)
        {
            toolStrip?.self.Add(item.Widget);
        }
        else if (isStatusStrip)
        {
            statusStrip?.self.Add(item.Widget);
        }
        else if (isMenuStrip)
        {
            toolStrip?.self.Add(item.Widget);
        }
        else if (isToolStripDropDown)
        {
            menu?.Add(item.Widget);
        }
        else
        {
            if (owner?.MenuItem?.Submenu == null)
            {
                menu = new Menu();
                if (owner is { MenuItem: not null })
                {
                    owner.MenuItem.Submenu = menu;
                }
            }
            menu?.Add(item.Widget);
        }

        base.Add(item);
        return Count;
    }

    public void AddRange(ToolStripItem[] toolStripItems)
    {
        for (var i = 0; i < toolStripItems.Length; i++)
        {
            AddMemu(toolStripItems[i]);
        }
    }

    public void AddRange(ToolStripItemCollection toolStripItems)
    {
        var count = toolStripItems.Count;
        for (var i = 0; i < count; i++)
        {
            AddMemu(toolStripItems[i]);
        }
    }
    //-------------------
    public new ToolStripItem this[int index]
    {
        get => base[index];
        set { menu?.Insert(value.Widget, index); base[index] = value; }
    }
}