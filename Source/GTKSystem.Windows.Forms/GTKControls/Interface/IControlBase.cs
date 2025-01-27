// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

namespace System.Windows.Forms
{
    public interface IControlBase
    {
        string AccessibleDefaultActionDescription { get; set; }
        string AccessibleDescription { get; set; }
        string AccessibleName { get; set; }
        AccessibleRole AccessibleRole { get; set; }
        bool AllowDrop { get; set; }
        AnchorStyles Anchor { get; set; }
        Point AutoScrollOffset { get; set; }
        bool AutoSize { get; set; }
        Color BackColor { get; set; }
        Image BackgroundImage { get; set; }
        ImageLayout BackgroundImageLayout { get; set; }
        BindingContext BindingContext { get; set; }
        int Bottom { get; }
        bool CausesValidation { get; set; }
        DockStyle Dock { get; set; }

        Color ForeColor { get; set; }

        Point Location { get; set; }
        Padding Margin { get; set; }
        Size Size { get; set; }
        string Name { get; set; }
        string Text { get; set; }
        Padding Padding { get; set; }

        int TabIndex { get; set; }
        bool TabStop { get; set; }
        object Tag { get; set; }
        
        bool Enabled { get; set; }
        bool Visible { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        //void SuspendLayout();
        //void ResumeLayout(bool resume);
        //void PerformLayout();
    }
}