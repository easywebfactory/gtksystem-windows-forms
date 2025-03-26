﻿using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms;

public class ToolStripSplitButton : ToolStripDropDownItem
{
 
    public ToolStripSplitButton() { }

    public ToolStripSplitButton(string? text) : base(text, null) { }

    public ToolStripSplitButton(Image? image) : base("", image) { }

    public ToolStripSplitButton(string? text, Image? image) : base(text, image) { }

    public ToolStripSplitButton(string? text, Image? image, EventHandler? onClick) : base(text, image, onClick) { }

    public ToolStripSplitButton(string? text, Image? image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems) { }

    public ToolStripSplitButton(string? text, Image? image, EventHandler? onClick, string? name) : base(text, image, onClick, name)
    {

    }
    [Browsable(false)]
    public bool DropDownButtonSelected { get; protected set; }
       
    [Browsable(false)]
    public bool DropDownButtonPressed { get; protected set; }
       
    [Browsable(false)]
    public Rectangle DropDownButtonBounds { get; protected set; }
      
    [Browsable(false)]
    [DefaultValue(null)]
    public ToolStripItem? DefaultItem { get; set; }
        
    [Browsable(false)]
    public bool ButtonSelected { get; protected set; }
      
    [Browsable(false)]
    public bool ButtonPressed { get; protected set; }
      
    [Browsable(false)]
    public Rectangle ButtonBounds { get; protected set; }
       
    //[DefaultValue(true)]
    //public bool AutoToolTip { get; set; }
    //
       
    public int DropDownButtonWidth { get; set; }
       
    [Browsable(false)]
    public Rectangle SplitterBounds { get; protected set; }
       
    protected  bool DefaultAutoToolTip { get; set; }
       
    protected internal bool DismissWhenClicked { get; set; }

}