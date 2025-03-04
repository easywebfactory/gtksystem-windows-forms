
using System.ComponentModel;
using System.Drawing;
using Gtk;
using Image = System.Drawing.Image;
using Region = System.Drawing.Region;

namespace System.Windows.Forms;

/// <summary>
///  A non selectable ToolStrip item
/// </summary>
public class ToolStripItem : Component, IDropTarget, ISupportOleDropSource, IArrangedElement, IKeyboardToolTip
{
    public virtual string? UniqueKey { get; protected set; }
    public virtual Widget? Widget { get; protected set; }
    public virtual MenuItem? MenuItem { get; set; }
    public virtual bool Created { get; set; }
    public virtual bool Checked { get; set; }
    public virtual CheckState CheckState { get; set; }
    internal Gtk.Image defaultImage;
    public virtual Image? Image { get; set; }
    public ToolStripItem()
    {
        Application.Init();
        defaultImage = new Gtk.Image("image-missing", IconSize.Menu);
        dropDownItems = new ToolStripItemCollection(this);
    }

    protected ToolStripItem(string? text, Image? image, EventHandler? onClick) : this(text, image, onClick, "")
    {
    }
      
    protected ToolStripItem(string? text, Image? image, EventHandler? onClick, string? name) : this()
    {
        Name = name;
        Text = text;
        if (image is { PixbufData: not null })
            Image = image;

        if (onClick != null)
            Click += onClick;
    }
    public virtual void CreateControl() { }
    public virtual ToolStripItemCollection Items => dropDownItems;
    private readonly ToolStripItemCollection dropDownItems;

    //public virtual event EventHandler? Disposed;

    public virtual ToolStripItemCollection DropDownItems => dropDownItems;

    public virtual string? Name { get; set; }
    //public virtual string Text { get { return base.Label; } set { base.Label = value; } }
    public virtual string? Text { get; set; }
    public virtual Color ImageTransparentColor { get; set; }
    public virtual ToolStripItemDisplayStyle DisplayStyle { get; set; }
    //public virtual Size Size { get; set; }
    public virtual bool AutoToolTip { get; set; }

    public virtual Image? BackgroundImage { get; set; }

    public virtual ImageLayout BackgroundImageLayout { get; set; }

    //public virtual bool Enabled { get; set; }
    public virtual string? ToolTipText { get; set; }
    public virtual ContentAlignment ImageAlign { get; set; }
    public virtual int ImageIndex { get; set; }
    public virtual string? ImageKey { get; set; }
    public virtual ToolStripItemImageScaling ImageScaling { get; set; }

    public virtual TextImageRelation TextImageRelation { get; set; }

    public virtual ToolStripTextDirection TextDirection { get; set; }

    public virtual ContentAlignment TextAlign { get; set; }

    // public virtual bool Selected { get; }

    public virtual bool RightToLeftAutoMirrorImage { get; set; }

    public virtual bool Pressed { get; protected set; }
    public virtual ToolStripItemPlacement Placement { get; protected set; }
    public virtual ToolStripItemOverflow Overflow { get; set; }
    public virtual ToolStripItem? OwnerItem { get; protected set; }

    public virtual ToolStrip? Owner { get; set; }

    public virtual int MergeIndex { get; set; }
    public virtual MergeAction MergeAction { get; set; }



    public virtual bool Enabled { get; set; }

    //  public virtual bool Focused { get { return this.IsFocus; } }

    public virtual Font? Font { get; set; }

    public virtual Color ForeColor { get; set; }
    public virtual Color BackColor { get; set; }
    public virtual bool HasChildren { get; protected set; }

    public virtual int Height { get; set; }
    public virtual ImeMode ImeMode { get; set; }

    public virtual int Left
    {
        get;
        set;
    }

    //public override Padding Margin { get; set; }
    //public override Size MaximumSize { get; set; }
    //public override Size MinimumSize { get; set; }
    public virtual Padding Padding { get; set; }
    public virtual ToolStripItem? Parent { get; set; }
    public virtual Region? Region { get; set; }
    public virtual int Right { get; protected set; }

    public virtual RightToLeft RightToLeft { get; set; }
    //public virtual ISite Site { get; set; }
    public virtual Size Size { get; set; }

    public virtual object? Tag { get; set; }
    public virtual int Top
    {
        get;
        set;
    }
    public virtual void ResumeLayout()
    {
            
    }

    public virtual void ResumeLayout(bool performLayout)
    {
             
    }
    public virtual void SuspendLayout()
    {
            
    }

    public virtual void PerformLayout()
    {
            
    }

    public virtual void PerformLayout(Control affectedControl, string affectedProperty)
    {
            
    }

    public void SetBounds(Rectangle bounds, BoundsSpecified specified)
    {
        throw new NotImplementedException();
    }

    public Size GetPreferredSize(Size proposedSize)
    {
        throw new NotImplementedException();
    }

    void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string? propertyName)
    {
        throw new NotImplementedException();
    }

    public virtual bool UseWaitCursor { get; set; }
    public virtual int Width { get; set; }

    public Rectangle Bounds => throw new NotImplementedException();

    public Rectangle DisplayRectangle => throw new NotImplementedException();

    public bool ParticipatesInLayout => throw new NotImplementedException();

    PropertyStore IArrangedElement.Properties => throw new NotImplementedException();

    IArrangedElement IArrangedElement.Container => throw new NotImplementedException();

    public ArrangedElementCollection? Children => throw new NotImplementedException();

    public virtual event EventHandler? Click;
    public virtual event EventHandler? CheckedChanged;
    public virtual event EventHandler? CheckStateChanged;
    public virtual event ToolStripItemClickedEventHandler? DropDownItemClicked;
}