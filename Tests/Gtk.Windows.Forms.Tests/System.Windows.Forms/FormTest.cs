//
// FormTest.cs: Test cases for Form.
//
// Author:
//   Ritvik Mayank (mritvik@novell.com)
//
// (C) 2005 Novell, Inc. (http://www.novell.com)
//

using GtkTests.System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CategoryAttribute = NUnit.Framework.CategoryAttribute;
using Timer = System.Windows.Forms.Timer;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class FormTest : TestHelper
{
    [Test]
    public void bug_82358 ()
    {
        //Console.WriteLine ("Starting bug_82358");
        int sizeable_factor;
        int title_bar;
        int tool_bar;
        int tool_border;
        int d3;
        int d2;

        // WinXP, default theme
        sizeable_factor = 2;
        title_bar = 26;
        tool_bar = 18;
        tool_border = 6;
        d3 = 10;
        d2 = 6;

        // WinXP, Win32 theme:
        sizeable_factor = 2;
        title_bar = 19;
        tool_bar = 16;
        tool_border = 6;
        d3 = 10;
        d2 = 6;


        Size size = new Size (200, 200);
			
        // Universal theme??
        using (Form f = new Form ()) {
            f.FormBorderStyle = FormBorderStyle.FixedSingle;
            f.Visible = true;
            d2 = f.Size.Width - f.ClientSize.Width;
            title_bar = f.Size.Height - f.ClientSize.Height - d2;
        }
        using (Form f = new Form ()) {
            f.FormBorderStyle = FormBorderStyle.Sizable;
            f.Visible = true;
            sizeable_factor = f.Size.Width - f.ClientSize.Width - d2;
        }
        using (Form f = new Form ()) {
            f.ClientSize = size;
            f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            //f.Visible = true;
            tool_border = f.Size.Width - f.ClientSize.Width;
            tool_bar = f.Size.Height - f.ClientSize.Height - tool_border;
        }
        using (Form f = new Form ()) {
            f.FormBorderStyle = FormBorderStyle.Fixed3D;
            f.Visible = true;
            d3 = f.Size.Width - f.ClientSize.Width; 
        }			
		
        FormBorderStyle style;
			
			
        //Console.WriteLine ("Universal theme says: d2={0}, d3={1}, title_bar={2}, sizeable_factor={3}, tool_border={4}, tool_bar={5}", d2, d3, title_bar, sizeable_factor, tool_border, tool_bar);
			
        // Changing client size, then FormBorderStyle.
        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedToolWindow;
            //Console.WriteLine ("Created form, size: {0}, clientsize: {1}", f.Size, f.ClientSize);
            f.ClientSize = size;
            //Console.WriteLine ("Changed ClientSize, size: {0}, clientsize: {1}", f.Size, f.ClientSize);
            f.FormBorderStyle = style;
            //Console.WriteLine ("Changed FormBorderStyle, size: {0}, clientsize: {1}", f.Size, f.ClientSize);
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (new Size (size.Width + tool_border, size.Height + tool_border + tool_bar).ToString (), f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            //Console.WriteLine ("Made visible, size: {0}, clientsize: {1}", f.Size, f.ClientSize);
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (new Size (size.Width + tool_border, size.Height + tool_border + tool_bar).ToString (), f.Size.ToString (), style.ToString () + "-A4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.SizableToolWindow;
            f.ClientSize = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (new Size (size.Width + tool_border + sizeable_factor, size.Height + tool_border + tool_bar + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (new Size (size.Width + tool_border + sizeable_factor, size.Height + tool_border + tool_bar + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-A4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Fixed3D;
            f.ClientSize = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (new Size (size.Width + d3, size.Height + title_bar + d3).ToString () , f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (new Size (size.Width + d3, size.Height + title_bar + d3).ToString (), f.Size.ToString (), style.ToString () + "-A4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedDialog;
            f.ClientSize = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-A4");
			
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedSingle;
            f.ClientSize = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-A4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.None;
            f.ClientSize = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-A4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Sizable;
            f.ClientSize = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A1");
            Assert.AreEqual (new Size (size.Width + d2 + sizeable_factor, size.Height + title_bar + d2 + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-A2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-A3");
            Assert.AreEqual (new Size (size.Width + d2 + sizeable_factor, size.Height + title_bar + d2 + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-A4");
        }
			
			
        // Changing size, then FormBorderStyle.
        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedToolWindow;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (new Size (size.Width - tool_border, size.Height - tool_border - tool_bar).ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.SizableToolWindow;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (new Size (size.Width - tool_border - sizeable_factor, size.Height - tool_border - tool_bar - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Fixed3D;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (new Size (size.Width - d3, size.Height - title_bar - d3).ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedDialog;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (new Size (size.Width - d2, size.Height - title_bar - d2).ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");

        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedSingle;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (new Size (size.Width - d2, size.Height - title_bar - d2).ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.None;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Sizable;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-B3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-B4");
        }



        // Changing FormBorderStyle, then client size
        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedToolWindow;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (new Size (size.Width + tool_border, size.Height + tool_border + tool_bar).ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (new Size (size.Width + tool_border, size.Height + tool_border + tool_bar).ToString (), f.Size.ToString (), style.ToString () + "-C4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.SizableToolWindow;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (new Size (size.Width + tool_border + sizeable_factor, size.Height + tool_border + tool_bar + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (new Size (size.Width + tool_border + sizeable_factor, size.Height + tool_border + tool_bar + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-C4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Fixed3D;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (new Size (size.Width + d3, size.Height + title_bar + d3).ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (new Size (size.Width + d3, size.Height + title_bar + d3).ToString (), f.Size.ToString (), style.ToString () + "-C4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedDialog;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-C4");

        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedSingle;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (new Size (size.Width + d2, size.Height + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-C4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.None;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-C4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Sizable;
            f.FormBorderStyle = style;
            f.ClientSize = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C1");
            Assert.AreEqual (new Size (size.Width + d2 + sizeable_factor, size.Height + title_bar + d2 + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-C2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-C3");
            Assert.AreEqual (new Size (size.Width + d2 + sizeable_factor, size.Height + title_bar + d2 + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-C4");
        }


        // Changing FormBorderStyle, then size
        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedToolWindow;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (new Size (size.Width - tool_border, size.Height - tool_border - tool_bar).ToString (), f.ClientSize.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (new Size (size.Width - tool_border, size.Height - tool_border - tool_bar).ToString (), f.ClientSize.ToString (), style.ToString () + "-D4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.SizableToolWindow;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (new Size (size.Width - tool_border - sizeable_factor, size.Height - tool_border - tool_bar - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (new Size (size.Width - tool_border - sizeable_factor, size.Height - tool_border - tool_bar - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-D4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Fixed3D;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (new Size (size.Width - d3, size.Height - title_bar - d3).ToString (), f.ClientSize.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (new Size (size.Width - d3, size.Height - title_bar - d3).ToString (), f.ClientSize.ToString (), style.ToString () + "-D4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedDialog;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (new Size (size.Width - d2, size.Height - title_bar - d2).ToString (), f.ClientSize.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (new Size (size.Width - d2, size.Height - title_bar - d2).ToString (), f.ClientSize.ToString (), style.ToString () + "-D4");

        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedSingle;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (new Size (size.Width - d2, size.Height - title_bar - d2).ToString (), f.ClientSize.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (new Size (size.Width - d2, size.Height - title_bar - d2).ToString (), f.ClientSize.ToString (), style.ToString () + "-D4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.None;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.ClientSize.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Sizable;
            f.FormBorderStyle = style;
            f.Size = size;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-D2");
            f.Visible = true;
            Assert.AreEqual (size.ToString (), f.Size.ToString (), style.ToString () + "-D3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-D4");
        }



        // Set clientsize, then change size, then FormBorderStyle.
        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedToolWindow;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            // Here we subtract the Sizable borders (default) then add FixedToolWindow's border.
            // Note how now the sizes doesn't change when creating the handle.
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + tool_border, size.Height - title_bar - d2 - sizeable_factor + tool_border + tool_bar).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + tool_border, size.Height - title_bar - d2 - sizeable_factor + tool_border + tool_bar).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.SizableToolWindow;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + tool_border + sizeable_factor, size.Height - title_bar - d2 - sizeable_factor + tool_border + tool_bar + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + tool_border + sizeable_factor, size.Height - title_bar - d2 - sizeable_factor + tool_border + tool_bar + sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Fixed3D;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d3, size.Height - title_bar - d2 - sizeable_factor + title_bar + d3).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d3, size.Height - title_bar - d2 - sizeable_factor + title_bar + d3).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedDialog;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d2, size.Height - title_bar - d2 - sizeable_factor + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d2, size.Height - title_bar - d2 - sizeable_factor + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");

        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.FixedSingle;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d2, size.Height - title_bar - d2 - sizeable_factor + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d2, size.Height - title_bar - d2 - sizeable_factor + title_bar + d2).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.None;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");
        }

        using (Form f = new Form ()) {
            style = FormBorderStyle.Sizable;
            f.ClientSize = f.ClientSize;
            f.Size = size;
            f.FormBorderStyle = style;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d2 + sizeable_factor, size.Height - title_bar - d2 - sizeable_factor + d2 + sizeable_factor + title_bar).ToString (), f.Size.ToString (), style.ToString () + "-E1");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E2");
            f.Visible = true;
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor + d2 + sizeable_factor, size.Height - title_bar - d2 - sizeable_factor + d2 + sizeable_factor + title_bar).ToString (), f.Size.ToString (), style.ToString () + "-E3");
            Assert.AreEqual (new Size (size.Width - d2 - sizeable_factor, size.Height - title_bar - d2 - sizeable_factor).ToString (), f.ClientSize.ToString (), style.ToString () + "-E4");
        }




    }

    [Test] // bug 81969
    public void StartPositionClosedForm ()
    {
        using (Form form = new Form ()) {
            form.StartPosition = FormStartPosition.CenterParent;
            form.Load += new EventHandler (CenterDisposedForm_Load);
            form.Show ();
        }

        using (Form form = new Form ()) {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Load += new EventHandler (CenterDisposedForm_Load);
            form.Show ();
        }


        using (Form form = new Form ()) {
            form.StartPosition = FormStartPosition.Manual;
            form.Load += new EventHandler (CenterDisposedForm_Load);
            form.Show ();
        }


        using (Form form = new Form ()) {
            form.StartPosition = FormStartPosition.WindowsDefaultBounds;
            form.Load += new EventHandler (CenterDisposedForm_Load);
            form.Show ();
        }

        using (Form form = new Form ()) {
            form.StartPosition = FormStartPosition.WindowsDefaultLocation;
            form.Load += new EventHandler (CenterDisposedForm_Load);
            form.Show ();
        }
    }

    private void CenterDisposedForm_Load (object sender, EventArgs e)
    {
        ((Form) sender).Close ();
    }

    void Form_VisibleChanged1 (object sender, EventArgs e)
    {
        TimeBombedForm f = (TimeBombedForm) sender;
        f.Reason = "VisibleChanged";
        f.Visible = false;
    }

    void Form_VisibleChanged2 (object sender, EventArgs e)
    {
        TimeBombedForm f = (TimeBombedForm) sender;
        f.Reason = "VisibleChanged";
        f.Visible = false;
        f.DialogResult = DialogResult.OK;
        Assert.IsFalse (f.Visible);
    }

    [Test]
    [Category ("NotWorking")]
    public void FormStartupPositionChangeTest ()
    {
        using (Form frm = new Form ())
        {
            frm.ShowInTaskbar = false;
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point (0, 0);
            frm.Show ();

            // On X there seem to be pending messages in the queue aren't processed
            // before Show returns, so process them. Otherwise the Location returns
            // something like (5,23)
            Application.DoEvents ();
				
            Assert.AreEqual ("{X=0,Y=0}", frm.Location.ToString (), "#01");

            frm.StartPosition = FormStartPosition.CenterParent;
            Assert.AreEqual ("{X=0,Y=0}", frm.Location.ToString (), "#02");

            frm.StartPosition = FormStartPosition.CenterScreen;
            Assert.AreEqual ("{X=0,Y=0}", frm.Location.ToString (), "#03");

            frm.StartPosition = FormStartPosition.Manual;
            Assert.AreEqual ("{X=0,Y=0}", frm.Location.ToString (), "#04");

            frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
            Assert.AreEqual ("{X=0,Y=0}", frm.Location.ToString (), "#05");

            frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            Assert.AreEqual ("{X=0,Y=0}", frm.Location.ToString (), "#06");
        }
    }
		
    [Test]
    public void FormStartupPositionTest ()
    {
        CreateParams cp;
			
        using (Form frm = new Form ())
        {
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$01");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#01");

            frm.StartPosition = FormStartPosition.CenterParent;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.CenterParent, frm.StartPosition, "$01");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#02");

            frm.StartPosition = FormStartPosition.CenterScreen;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.CenterScreen, frm.StartPosition, "$01");

            frm.StartPosition = FormStartPosition.Manual;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.Manual, frm.StartPosition, "$01");
            Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#04");

            frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, frm.StartPosition, "$01");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#05");

            frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$01");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#06");
				
        }


        using (Form frm = new Form ()) {
            frm.Location = new Point (23, 45);

            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$A1");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#A1");

            frm.StartPosition = FormStartPosition.CenterParent;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.CenterParent, frm.StartPosition, "$A2");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#A2");

            frm.StartPosition = FormStartPosition.CenterScreen;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.CenterScreen, frm.StartPosition, "$A3");

            frm.StartPosition = FormStartPosition.Manual;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.Manual, frm.StartPosition, "$A4");
            Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A4");

            frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, frm.StartPosition, "$A5");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#A5");

            frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            cp = TestHelper.GetCreateParams (frm);
            Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$A6");
            Assert.AreEqual (new Point (int.MinValue, int.MinValue).ToString (), new Point (cp.X, cp.Y).ToString (), "#A6");
        }
    }
		
    [Test]
    public void ParentedFormStartupPositionTest ()
    {
        CreateParams cp;
        using (Form Main = new Form ()) {
            Main.ShowInTaskbar = false;
            Main.Show ();

            using (Form frm = new Form ()) {
                Main.Controls.Add (frm);
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$01");
                Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#01");

                frm.StartPosition = FormStartPosition.CenterParent;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterParent, frm.StartPosition, "$02");
                Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#02");

                frm.StartPosition = FormStartPosition.CenterScreen;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterScreen, frm.StartPosition, "$03");
                Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#03");

                frm.StartPosition = FormStartPosition.Manual;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.Manual, frm.StartPosition, "$04");
                Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#04");

                frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, frm.StartPosition, "$05");
                Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#05");

                frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$06");
                Assert.AreEqual (new Point (0, 0).ToString (), new Point (cp.X, cp.Y).ToString (), "#06");
                frm.Show ();
            }

            using (Form frm = new Form ()) {
                Main.Controls.Add (frm);
                frm.Location = new Point (23, 45);

                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$A1");
                Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A1");

                frm.StartPosition = FormStartPosition.CenterParent;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterParent, frm.StartPosition, "$A2");
                Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A2");

                frm.StartPosition = FormStartPosition.CenterScreen;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterScreen, frm.StartPosition, "$A3");
                Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A3");

                frm.StartPosition = FormStartPosition.Manual;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.Manual, frm.StartPosition, "$A4");
                Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A4");

                frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, frm.StartPosition, "$A5");
                Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A5");

                frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$A6");
                Assert.AreEqual (new Point (23, 45).ToString (), new Point (cp.X, cp.Y).ToString (), "#A6");

                frm.Show ();
            }

            using (Form frm = new Form ()) {
                Main.Controls.Add (frm);
                frm.Location = new Point (34, 56);

                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$B1");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#B1");

                frm.StartPosition = FormStartPosition.CenterParent;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterParent, frm.StartPosition, "$B2");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#B2");

                frm.StartPosition = FormStartPosition.CenterScreen;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterScreen, frm.StartPosition, "$B3");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#B3");

                frm.StartPosition = FormStartPosition.Manual;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.Manual, frm.StartPosition, "$B4");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#B4");

                frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, frm.StartPosition, "$B5");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#B5");

                frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$B6");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#B6");

                frm.Show ();
            }

            Main.Size = new Size (600, 600);
            using (Form frm = new Form ()) {
                Main.Controls.Add (frm);
                frm.Location = new Point (34, 56);

                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$C1");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#C1");

                frm.StartPosition = FormStartPosition.CenterParent;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterParent, frm.StartPosition, "$C2");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#C2");

                frm.StartPosition = FormStartPosition.CenterScreen;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.CenterScreen, frm.StartPosition, "$C3");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#C3");

                frm.StartPosition = FormStartPosition.Manual;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.Manual, frm.StartPosition, "$C4");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#C4");

                frm.StartPosition = FormStartPosition.WindowsDefaultBounds;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, frm.StartPosition, "$C5");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#C5");

                frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                cp = TestHelper.GetCreateParams (frm);
                Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, frm.StartPosition, "$C6");
                Assert.AreEqual (new Point (34, 56).ToString (), new Point (cp.X, cp.Y).ToString (), "#C6");

                frm.Show ();
            }
        }
    }
		
    [Test]
    public void UnparentForm ()
    {
        Form f1 = new Form ();
        f1.Show ();

        Form f2 = new Form ();
        f2.Parent = f1;
        Assert.AreSame (f1, f2.Parent, "#1");
        f2.Show ();
        f2.Parent = null;
        Assert.IsNull (f2.Parent, "#2");

        f1.Close();
        f2.Close();
    }

    [Test] // bug #80791
    public void ClientSizeTest ()
    {
        Form form = new Form ();
        Assert.IsFalse (form.ClientSize == form.Size);
    }

    [Test] // bug #80574
    [Category ("NotWorking")]
    public void FormBorderStyleTest ()
    {
        Form form = new Form ();
        Rectangle boundsBeforeBorderStyleChange = form.Bounds;
        Rectangle clientRectangleBeforeBorderStyleChange = form.ClientRectangle;
        form.FormBorderStyle = FormBorderStyle.None;
        Assert.AreEqual (form.Bounds, boundsBeforeBorderStyleChange, "#A1");
        Assert.AreEqual (form.ClientRectangle, clientRectangleBeforeBorderStyleChange, "#A2");

        form.Visible = true;
        form.FormBorderStyle = FormBorderStyle.Sizable;
        boundsBeforeBorderStyleChange = form.Bounds;
        clientRectangleBeforeBorderStyleChange = form.ClientRectangle;
        form.FormBorderStyle = FormBorderStyle.None;
        Assert.IsFalse (form.Bounds == boundsBeforeBorderStyleChange, "#B1");
        Assert.AreEqual (form.ClientRectangle, clientRectangleBeforeBorderStyleChange, "#B2");

        form.Visible = false;
        form.FormBorderStyle = FormBorderStyle.Sizable;
        boundsBeforeBorderStyleChange = form.Bounds;
        clientRectangleBeforeBorderStyleChange = form.ClientRectangle;
        form.FormBorderStyle = FormBorderStyle.None;
        Assert.IsFalse (form.Bounds == boundsBeforeBorderStyleChange, "#C1");
        Assert.AreEqual (form.ClientRectangle, clientRectangleBeforeBorderStyleChange, "#C2");
    }

    [Test]
    [NUnit.Framework.Category ("NotWorking")]
    public void FormCreateParamsStyleTest ()
    {
        Form frm;
			
        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles) TestHelper.GetCreateParams (frm).Style), "#01-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles) TestHelper.GetCreateParams (frm).ExStyle), "#01-ExStyle");
        }

        using (frm = new Form ()) {
            frm.AllowDrop = !frm.AllowDrop;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#02-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#02-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#03-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_LAYERED, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#03-ExStyle");
        }

        using (frm = new Form ()) {
            frm.Opacity = 0.50;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#04-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_LAYERED, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#04-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#05-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_LAYERED, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#05-ExStyle");
        }
			
        using (frm = new Form ()) {
            frm.CausesValidation = !frm.CausesValidation;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#06-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#06-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_GROUP | WindowStyles.WS_THICKFRAME | WindowStyles.WS_BORDER | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#07-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#07-ExStyle");
        }

        using (frm = new Form ()) {
            frm.Enabled = true;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#08-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#08-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.Fixed3D;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_GROUP | WindowStyles.WS_SYSMENU | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#10-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CLIENTEDGE | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#10-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_GROUP | WindowStyles.WS_SYSMENU | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#11-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_DLGMODALFRAME | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#11-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_GROUP | WindowStyles.WS_SYSMENU | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#12-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#12-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_GROUP | WindowStyles.WS_SYSMENU | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#13-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_TOOLWINDOW | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#13-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.None;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#14-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#14-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.Sizable;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#15-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#15-ExStyle");
        }

        using (frm = new Form ()) {
            frm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#16-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_TOOLWINDOW | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#16-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#17-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#17-ExStyle");
        }

        using (frm = new Form ()) {
            frm.Icon = null;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#18-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#18-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#19-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#19-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#20-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#20-ExStyle");
        }

        using (frm = new Form ()) {
            frm.MaximizeBox = !frm.MaximizeBox;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_GROUP | WindowStyles.WS_THICKFRAME | WindowStyles.WS_SYSMENU | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#21-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#21-ExStyle");
        }

        using (frm = new Form ()) {
            frm.MinimizeBox = !frm.MinimizeBox;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_THICKFRAME | WindowStyles.WS_SYSMENU | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#22-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#22-ExStyle");
        }

        using (frm = new Form ()) {
            frm.ShowIcon = !frm.ShowIcon;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#23-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_DLGMODALFRAME | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#23-ExStyle");
        }

        using (frm = new Form ()) {
            frm.ShowInTaskbar = !frm.ShowInTaskbar;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#24-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#24-ExStyle");
        }


        using (frm = new Form ()) {
            frm.TabStop = !frm.TabStop;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#25-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#25-ExStyle");
        }

        using (frm = new Form ()) {
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS | WindowStyles.WS_CHILD, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#26-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#26-ExStyle");
        }

        using (frm = new Form ()) {
            frm.Visible = !frm.Visible;
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TILEDWINDOW | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#27-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#27-ExStyle");
        }

        using (frm = new Form ()) {
            frm.Text = "";
            Assert.AreEqual (WindowStyles.WS_TILED | WindowStyles.WS_TABSTOP | WindowStyles.WS_GROUP | WindowStyles.WS_THICKFRAME | WindowStyles.WS_BORDER | WindowStyles.WS_CLIPCHILDREN, ((WindowStyles)TestHelper.GetCreateParams (frm).Style), "#28-Style");
            Assert.AreEqual (WindowExStyles.WS_EX_LEFT | WindowExStyles.WS_EX_RIGHTSCROLLBAR | WindowExStyles.WS_EX_CONTROLPARENT | WindowExStyles.WS_EX_APPWINDOW, ((WindowExStyles)TestHelper.GetCreateParams (frm).ExStyle), "#28-ExStyle");
        }
    }
		
    [Test]
    public void FormPropertyTest ()
    {
        Form myform = new Form ();
        myform.Visible = true;
        myform.Text = "NewForm";
        myform.Name = "FormTest";
        Assert.AreEqual (DialogResult.None, myform.DialogResult, "#12");
        Assert.AreEqual (FormBorderStyle.Sizable, myform.FormBorderStyle, "#13");
        Assert.AreEqual ("System.Drawing.Icon", myform.Icon.GetType ().ToString (), "#15");
        Assert.IsTrue (myform.MaximizeBox, "#19");
        Assert.AreEqual (0, myform.MaximumSize.Height, "#20a");
        Assert.AreEqual (0, myform.MaximumSize.Width, "#20b");
        Assert.IsTrue (myform.MinimizeBox, "#25");
        Assert.AreEqual (0, myform.MinimumSize.Height, "#26a");
        Assert.AreEqual (0, myform.MinimumSize.Width, "#26b");
        Assert.IsTrue (myform.MinimumSize.IsEmpty, "#26c");
        Assert.AreEqual (1, myform.Opacity, "#28");
        Assert.IsTrue (myform.ShowInTaskbar, "#31");
        Assert.AreEqual (300, myform.Size.Height, "#32a");
        Assert.AreEqual (300, myform.Size.Width, "#32b");
        Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, myform.StartPosition, "#34");
        Assert.AreEqual (FormWindowState.Normal, myform.WindowState, "#38");
        Assert.AreEqual (ImeMode.NoControl, myform.ImeMode, "#39");
        myform.Dispose ();
    }

    [Test]
    [Category ("NotWorking")]
    public void ActivateTest ()
    {
        Form myform = new Form ();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        myform.Text = "NewForm";
        myform.Name = "FormTest";
        myform.Activate ();
        Assert.AreEqual (true, myform.Focus (), "#40");
        myform.Dispose ();
    }

    [Test]
    public void SetDialogResultOutOfRange ()
    {
        Form myform = new Form ();
        myform.ShowInTaskbar = false;
        try {
            myform.DialogResult = (DialogResult) (-1);
            Assert.Fail ("#48");
        } catch (InvalidEnumArgumentException) {
        }

        try {
            myform.DialogResult = (DialogResult) ((int) DialogResult.No + 1);
            Assert.Fail ("#49");
        } catch (InvalidEnumArgumentException) {
        }
        myform.Dispose ();
    }

    void myform_set_dialogresult (object sender, EventArgs e)
    {
        Form f = (Form)sender;

        f.DialogResult = DialogResult.OK;
    }

    void myform_close (object sender, EventArgs e)
    {
        Form f = (Form)sender;

        f.Close();
    }

    [Test]
    public void SetDialogResult ()
    {
        Form myform = new Form ();
        myform.ShowInTaskbar = false;
        myform.Visible = true;

        myform.DialogResult = DialogResult.Cancel;

        Assert.IsTrue (myform.Visible, "A1");
        Assert.IsFalse (myform.IsDisposed, "A2");

        myform.Close ();

        Assert.IsFalse (myform.Visible, "A3");
        Assert.IsTrue (myform.IsDisposed, "A4");

        DialogResult result;

        myform = new Form ();
        myform.ShowInTaskbar = false;
        myform.VisibleChanged += new EventHandler (myform_set_dialogresult);
        result = myform.ShowDialog ();

        Assert.AreEqual (result, DialogResult.OK, "A5");
        Assert.IsFalse (myform.Visible, "A6");
        Assert.IsFalse (myform.IsDisposed, "A7");
        myform.Dispose ();
			
        myform = new Form ();
        myform.ShowInTaskbar = false;
        myform.VisibleChanged += new EventHandler (myform_close);
        result = myform.ShowDialog ();

        Assert.AreEqual (result, DialogResult.Cancel, "A8");
        Assert.IsFalse (myform.Visible, "A9");
        Assert.IsFalse (myform.IsDisposed, "A10");
			
        myform.Dispose ();
    }

    [Test] // bug #80052
    [Category ("NotWorking")]
    public void Location ()
    {
        // 
        // CenterParent
        // 

        Form formA = new Form ();
        formA.ShowInTaskbar = false;
        formA.StartPosition = FormStartPosition.CenterParent;
        formA.Location = new Point (151, 251);
        formA.Show ();

        Assert.AreEqual (FormStartPosition.CenterParent, formA.StartPosition, "#A1");
        Assert.IsFalse (formA.Location.X == 151, "#A2");
        Assert.IsFalse (formA.Location.Y == 251, "#A3");

        formA.Location = new Point (311, 221);

        Assert.AreEqual (FormStartPosition.CenterParent, formA.StartPosition, "#A4");
        Assert.AreEqual (311, formA.Location.X, "#A5");
        Assert.AreEqual (221, formA.Location.Y, "#A6");

        formA.Dispose ();

        // 
        // CenterScreen
        // 

        Form formB = new Form ();
        formB.ShowInTaskbar = false;
        formB.StartPosition = FormStartPosition.CenterScreen;
        formB.Location = new Point (151, 251);
        formB.Show ();

        Assert.AreEqual (FormStartPosition.CenterScreen, formB.StartPosition, "#B1");
        Assert.IsFalse (formB.Location.X == 151, "#B2");
        Assert.IsFalse (formB.Location.Y == 251, "#B3");

        formB.Location = new Point (311, 221);

        Assert.AreEqual (FormStartPosition.CenterScreen, formB.StartPosition, "#B4");
        Assert.AreEqual (311, formB.Location.X, "#B5");
        Assert.AreEqual (221, formB.Location.Y, "#B6");

        formB.Dispose ();

        // 
        // Manual
        // 

        Form formC = new Form ();
        formC.ShowInTaskbar = false;
        formC.StartPosition = FormStartPosition.Manual;
        formC.Location = new Point (151, 251);
        formC.Show ();

        Assert.AreEqual (FormStartPosition.Manual, formC.StartPosition, "#C1");
        Assert.AreEqual (151, formC.Location.X, "#C2");
        Assert.AreEqual (251, formC.Location.Y, "#C3");

        formC.Location = new Point (311, 221);

        Assert.AreEqual (FormStartPosition.Manual, formC.StartPosition, "#C4");
        Assert.AreEqual (311, formC.Location.X, "#C5");
        Assert.AreEqual (221, formC.Location.Y, "#C6");

        formC.Dispose ();

        // 
        // WindowsDefaultBounds
        // 

        Form formD = new Form ();
        formD.ShowInTaskbar = false;
        formD.StartPosition = FormStartPosition.WindowsDefaultBounds;
        formD.Location = new Point (151, 251);
        formD.Show ();

        Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, formD.StartPosition, "#D1");
        Assert.IsFalse (formD.Location.X == 151, "#D2");
        Assert.IsFalse (formD.Location.Y == 251, "#D3");

        formD.Location = new Point (311, 221);

        Assert.AreEqual (FormStartPosition.WindowsDefaultBounds, formD.StartPosition, "#D4");
        Assert.AreEqual (311, formD.Location.X, "#D5");
        Assert.AreEqual (221, formD.Location.Y, "#D6");

        formD.Dispose ();

        // 
        // WindowsDefaultLocation
        // 

        Form formE = new Form ();
        formE.ShowInTaskbar = false;
        formE.Location = new Point (151, 251);
        formE.Show ();

        Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, formE.StartPosition, "#E1");
        Assert.IsFalse (formE.Location.X == 151, "#E2");
        Assert.IsFalse (formE.Location.Y == 251, "#E3");

        formE.Location = new Point (311, 221);

        Assert.AreEqual (FormStartPosition.WindowsDefaultLocation, formE.StartPosition, "#E4");
        Assert.AreEqual (311, formE.Location.X, "#E5");
        Assert.AreEqual (221, formE.Location.Y, "#E6");

        formE.Dispose ();
    }

    [Test]
    public void Opacity ()
    {
        Form frm;
        using (frm = new Form ()) {
            Assert.AreEqual (1.0f, frm.Opacity, "#01-opacity");
            frm.Opacity = 0.50;
            Assert.AreEqual (0.50f, frm.Opacity, "#02-opacity");
            frm.Opacity = -0.1f;
            Assert.AreEqual (0, frm.Opacity, "#03-opacity");
            frm.Opacity = 1.1f;
            Assert.AreEqual (1, frm.Opacity, "#04-opacity");
        }
    }

    [Test]
    public void AccessDisposedForm ()
    {
        Assert.Throws<ObjectDisposedException>(() =>
        {
            Form myform = new Form();
            myform.ShowInTaskbar = false;

            myform.Show();
            myform.Close(); // this should result in the form being disposed
            myform.Show(); // and this line should result in the ODE being thrown
        });
    }

    int handle_destroyed_count;
    void handle_destroyed (object sender, EventArgs e)
    {
        handle_destroyed_count++;
    }

    [Test]
    public void FormClose ()
    {
        Form myform = new Form ();
        myform.ShowInTaskbar = false;

        Assert.IsFalse (myform.Visible, "A1");
        Assert.IsFalse (myform.IsDisposed, "A2");

        myform.Close ();

        Assert.IsTrue (myform.IsDisposed, "A3");
    }

    [Test]
    public void FormClose2 ()
    {
        WMCloseWatcher f = new WMCloseWatcher ();
        f.ShowInTaskbar = false;

        f.close_count = 0;
        Assert.IsFalse (f.Visible, "A1");
        f.Close ();
        Assert.AreEqual (0, f.close_count, "A2");
        Assert.IsTrue (f.IsDisposed, "A3");
    }

    class WMCloseWatcher : Form {
        public int close_count;

        protected override void WndProc (ref Message msg) {
            if (msg.Msg == 0x0010 /* WM_CLOSE */) {
                close_count ++;
            }

            base.WndProc (ref msg);
        }
    }

    [Test]
    public void ShowWithOwnerIOE ()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            using (Form f = new Form())
            {
                f.Show(f);
            }
        });
    }
		
    [Test]	// Bug #79959, #80574, #80791
    [Category ("NotWorking")]
    public void BehaviorResizeOnBorderStyleChanged ()
    {
        // Marked NotWorking because the ClientSize is dependent on the WM.
        // The values below match XP Luna to make sure our behavior is the same.
        Form f = new Form ();
        f.ShowInTaskbar = false;
        f.Show ();

        Assert.AreEqual (true, f.IsHandleCreated, "A0");

        Assert.AreEqual (new Size (300, 300), f.Size, "A1");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A2");

        f.FormBorderStyle = FormBorderStyle.Fixed3D;
        Assert.AreEqual (new Size (302, 302), f.Size, "A3");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A4");

        f.FormBorderStyle = FormBorderStyle.FixedDialog;
        Assert.AreEqual (new Size (298, 298), f.Size, "A5");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A6");

        f.FormBorderStyle = FormBorderStyle.FixedSingle;
        Assert.AreEqual (new Size (298, 298), f.Size, "A7");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A8");

        f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Assert.AreEqual (new Size (298, 290), f.Size, "A9");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A0");

        f.FormBorderStyle = FormBorderStyle.None;
        Assert.AreEqual (new Size (292, 266), f.Size, "A11");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A12");

        f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Assert.AreEqual (new Size (300, 292), f.Size, "A13");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A14");

        f.FormBorderStyle = FormBorderStyle.Sizable;
        Assert.AreEqual (new Size (300, 300), f.Size, "A15");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A16");
			
        f.Close ();
    }

    [Test]  // Bug #80574, #80791
    [Category ("NotWorking")]
    public void BehaviorResizeOnBorderStyleChangedNotVisible ()
    {
        // Marked NotWorking because the ClientSize is dependent on the WM.
        // The values below match XP Luna to make sure our behavior is the same.
        Form f = new Form ();
        f.ShowInTaskbar = false;

        Assert.AreEqual (false, f.IsHandleCreated, "A0");
			
        Assert.AreEqual (new Size (300, 300), f.Size, "A1");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A2");

        f.FormBorderStyle = FormBorderStyle.Fixed3D;
        Assert.AreEqual (new Size (300, 300), f.Size, "A3");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A4");

        f.FormBorderStyle = FormBorderStyle.FixedDialog;
        Assert.AreEqual (new Size (300, 300), f.Size, "A5");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A6");

        f.FormBorderStyle = FormBorderStyle.FixedSingle;
        Assert.AreEqual (new Size (300, 300), f.Size, "A7");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A8");

        f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Assert.AreEqual (new Size (300, 300), f.Size, "A9");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A0");

        f.FormBorderStyle = FormBorderStyle.None;
        Assert.AreEqual (new Size (300, 300), f.Size, "A11");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A12");

        f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Assert.AreEqual (new Size (300, 300), f.Size, "A13");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A14");

        f.FormBorderStyle = FormBorderStyle.Sizable;
        Assert.AreEqual (new Size (300, 300), f.Size, "A15");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A16");
    }

    [Test]  // Bug #80574, #80791
    [Category ("NotWorking")]
    public void MoreBehaviorResizeOnBorderStyleChangedNotVisible ()
    {
        // Marked NotWorking because the ClientSize is dependent on the WM.
        // The values below match XP Luna to make sure our behavior is the same.
        Form f = new Form ();
        f.ShowInTaskbar = false;

        f.Show ();
        f.Hide ();

        Assert.AreEqual (true, f.IsHandleCreated, "A0");

        f.FormBorderStyle = FormBorderStyle.Sizable;
        Assert.AreEqual (new Size (300, 300), f.Size, "A1");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A2");
        f.FormBorderStyle = FormBorderStyle.None;
        Assert.AreEqual (new Size (292, 266), f.Size, "A3");
        Assert.AreEqual (new Size (292, 266), f.ClientSize, "A4");
    }

    [Test]  // bug #438866
    public void MinMaxSize ()
    {
        Form f = new Form ();
			
        f.MinimumSize = new Size (200, 200);
        f.MaximumSize = new Size (150, 150);

        Assert.AreEqual (new Size (150, 150), f.MinimumSize, "A1");
        Assert.AreEqual (new Size (150, 150), f.MaximumSize, "A2");
			
        f.MinimumSize = new Size (200, 200);

        Assert.AreEqual (new Size (200, 200), f.MinimumSize, "A3");
        Assert.AreEqual (new Size (200, 200), f.MaximumSize, "A4");
			
        f.Dispose ();
    }

    [Test]
    public void MinSizeIssue ()
    {
        Form f = new Form ();

        f.MinimumSize = new Size (100, 100);

        f.Show ();

        Assert.AreEqual (new Size (300, 300), f.Size, "A1");

        f.Dispose ();
    }
		
    void tv_GotFocus (object sender, EventArgs e)
    {
        //Console.WriteLine (Environment.StackTrace);
    }

    [Test]
    public void Bug82470 ()
    {
        Form f = new Form ();
        f.Load += new EventHandler (Form_LoadAndHide);
        f.Show ();
			
        Assert.AreEqual (true, f.Visible, "A1");
			
        f.Dispose ();
    }

    private void Form_LoadAndHide (object sender, EventArgs e)
    {
        ((Form)sender).Visible = false;
    }

    [Test]
    public void Bug686486 ()
    {
        using (Form f = new Bug686486Form ())
        {
            try
            {
                f.ShowDialog ();
            }
            catch (StackOverflowException)
            {
                Assert.Fail ("Setting DialogResult in FormClosing Event causes endless loop: StackOverflowException");
            }
        }
    }

    private class Bug686486Form : Form
    {
        public Bug686486Form ()
        {
            this.FormClosing += SetDialogResultOK;
            this.Load += SetDialogResultOK;
        }

        private void SetDialogResultOK (object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }

    [Test]
    public void AutoSizeGrowOnly ()
    {
        Form f = new Form ();
        f.ShowInTaskbar = false;
        f.AutoSize = true;

        Button b = new Button ();
        b.Size = new Size (200, 200);
        b.Location = new Point (200, 200);
        f.Controls.Add (b);

        f.Show ();

        Assert.AreEqual (new Size (403, 403), f.ClientSize, "A1");
			
        f.Controls.Remove (b);
        Assert.AreEqual (new Size (403, 403), f.ClientSize, "A2");
		
        f.Dispose ();
    }

    [Test]
    public void AutoSizeReset ()
    {
        Form f = new Form ();
        f.ShowInTaskbar = false;

        Button b = new Button ();
        b.Size = new Size (200, 200);
        b.Location = new Point (200, 200);
        f.Controls.Add (b);

        f.Show ();

        Size start_size = f.ClientSize;

        f.AutoSize = true;
        Assert.AreEqual (new Size (403, 403), f.ClientSize, "A1");

        f.AutoSize = false;
        Assert.AreEqual (start_size, f.ClientSize, "A2");
        f.Close ();
    }

    [Test]
    public void AutoSizeGrowAndShrink ()
    {
        Form f = new Form ();
        f.ShowInTaskbar = false;
        f.AutoSize = true;

        f.Show ();

        // Make sure form shrunk
        Assert.IsTrue (f.ClientSize.Width < 150, "A1");
        Assert.IsTrue (f.ClientSize.Height < 150, "A1-2");

        Button b = new Button ();
        b.Size = new Size (200, 200);
        b.Location = new Point (0, 0);
        f.Controls.Add (b);

        Assert.AreEqual (new Size (203, 203), f.ClientSize, "A2");
        f.Dispose ();
    }


    [Test]
    public void SettingIconToNull ()
    {
        Form form = new Form ();
        Assert.IsNotNull (form.Icon, "1");
        form.Icon = null;
        Assert.IsNotNull (form.Icon, "2");
    }

}

public class TimeBombedForm : Form
{
    public Timer timer;
    public bool CloseOnPaint;
    public string Reason;
    public TimeBombedForm ()
    {
        timer = new Timer ();
        timer.Interval = 500;
        timer.Tick += new EventHandler (timer_Tick);
        timer.Start ();
    }

    void timer_Tick (object sender, EventArgs e)
    {
        Reason = "Bombed";
        Close ();
    }

    protected override void OnPaint (PaintEventArgs pevent)
    {
        base.OnPaint (pevent);
        if (CloseOnPaint) {
            Reason = "OnPaint";
            Close ();
        }
    }
}