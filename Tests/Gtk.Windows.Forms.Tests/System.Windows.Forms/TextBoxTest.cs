//
// Copyright (c) 2005 Novell, Inc.
//
// Authors:
//      Ritvik Mayank (mritvik@novell.com)
//

using System.Drawing;
using System.Windows.Forms;
using GtkTests.Helpers;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class TextBoxTest : TestHelper
{
    TextBox textBox;
    int _changed;
    int _invalidated;
    int _paint;

    [TearDown]
    public void TearDown()
    {
        textBox.Dispose();
    }

    [SetUp]
    protected override void SetUp () {
        textBox = new TextBox();
        textBox.Invalidated += TextBox_Invalidated;
        textBox.Paint += TextBox_Paint;
        textBox.TextChanged += TextBox_TextChanged;
        Reset ();
        base.SetUp ();
    }

    [Test]
    public void TextBoxBasePropertyTest ()
    {
        textBox.Multiline = true;
        Assert.AreEqual (true, textBox.AutoSize, "#2");
        Assert.AreEqual (null, textBox.BackgroundImage, "#4a");
        var gif = TestResourceHelper.GetFullPathOfResource ("Test/resources/M.gif");
        textBox.BackgroundImage = Image.FromFile (gif);
        // comparing image objects fails on MS .Net so using Size property
        Assert.AreEqual (Image.FromFile(gif, true).Size, textBox.BackgroundImage.Size, "#4b");
			
        Assert.AreEqual (BorderStyle.Fixed3D, textBox.BorderStyle, "#5");
        Assert.AreEqual (0, textBox.Lines.Length, "#9");
        Assert.AreEqual (true, textBox.Multiline, "#12a");
        Assert.AreEqual (true, textBox.Multiline, "#12b");
        Assert.AreEqual (true, textBox.Multiline, "#12c");
        Assert.AreEqual (false, textBox.ReadOnly, "#14");
        textBox.Text = "sample TextBox";
        Assert.AreEqual ("sample TextBox", textBox.Text, "#18");
    }

    [Test]
    public void TextBoxPropertyTest ()
    {
        Assert.AreEqual ('\0', textBox.PasswordChar, "#23");
        textBox.PasswordChar = '*';
        Assert.AreEqual ('*', textBox.PasswordChar, "#23b");
    }

    [Test]
    public void AppendTextTest ()
    {
        var f = new Form (); 
        f.ShowInTaskbar = false;
        f.Visible = true;
        textBox.Visible = true;
        textBox.Text = "TextBox1";
        var textBox2 = new TextBox ();
        textBox2.Visible = true;
        f.Controls.Add (textBox);
        f.Controls.Add (textBox2);
        textBox2.AppendText (textBox.Text);
        Assert.AreEqual ("TextBox1", textBox2.Text, "#27");
        f.Dispose ();
    }

    [Test]
    public void AppendTextTest2 ()
    {
        var textBox2 = new TextBox ();
        textBox2.AppendText ("hi");
        textBox2.AppendText ("ho");
        Assert.AreEqual ("hiho", textBox2.Text, "#1");
        Assert.IsNotNull (textBox2.Lines, "#2");
        Assert.AreEqual (1, textBox2.Lines.Length, "#3");
        Assert.AreEqual ("hiho", textBox2.Lines [0], "#4");
    }

    [Test]
    public void AppendText_Multiline_CRLF ()
    {
        var textBox = new TextBox ();
        textBox.Text = "ha";
        textBox.AppendText ("hi\r\n\r\n");
        textBox.AppendText ("ho\r\n");
        Assert.AreEqual ("hahi\r\n\r\nho\r\n", textBox.Text, "#A1");
        Assert.IsNotNull (textBox.Lines, "#A2");
        Assert.AreEqual (4, textBox.Lines.Length, "#A3");
        Assert.AreEqual ("hahi", textBox.Lines [0], "#A4");
        Assert.AreEqual (string.Empty, textBox.Lines [1], "#A5");
        Assert.AreEqual ("ho", textBox.Lines [2], "#A6");
        Assert.AreEqual (string.Empty, textBox.Lines [3], "#A7");

        textBox.Multiline = true;

        textBox.Text = "ha";
        textBox.AppendText ("hi\r\n\r\n");
        textBox.AppendText ("ho\r\n");
        Assert.AreEqual ("hahi\r\n\r\nho\r\n", textBox.Text, "#B1");
        Assert.IsNotNull (textBox.Lines, "#B2");
        Assert.AreEqual (4, textBox.Lines.Length, "#B3");
        Assert.AreEqual ("hahi", textBox.Lines [0], "#B4");
        Assert.AreEqual (string.Empty, textBox.Lines [1], "#B5");
        Assert.AreEqual ("ho", textBox.Lines [2], "#B6");
        Assert.AreEqual (string.Empty, textBox.Lines [3], "#B7");
    }

    [Test]
    public void AppendText_Multiline_LF ()
    {
        var textBox = new TextBox ();

        textBox.Text = "ha";
        textBox.AppendText ("hi\n\n");
        textBox.AppendText ("ho\n");
        Assert.AreEqual ("hahi\n\nho\n", textBox.Text, "#A1");
        Assert.IsNotNull (textBox.Lines, "#A2");
        Assert.AreEqual (4, textBox.Lines.Length, "#A3");
        Assert.AreEqual ("hahi", textBox.Lines [0], "#A4");
        Assert.AreEqual (string.Empty, textBox.Lines [1], "#A5");
        Assert.AreEqual ("ho", textBox.Lines [2], "#A6");
        Assert.AreEqual (string.Empty, textBox.Lines [3], "#A7");

        textBox.Multiline = true;

        textBox.Text = "ha";
        textBox.AppendText ("hi\n\n");
        textBox.AppendText ("ho\n");
        Assert.AreEqual ("hahi\n\nho\n", textBox.Text, "#B1");
        Assert.IsNotNull (textBox.Lines, "#B2");
        Assert.AreEqual (4, textBox.Lines.Length, "#B3");
        Assert.AreEqual ("hahi", textBox.Lines [0], "#B4");
        Assert.AreEqual (string.Empty, textBox.Lines [1], "#B5");
        Assert.AreEqual ("ho", textBox.Lines [2], "#B6");
        Assert.AreEqual (string.Empty, textBox.Lines [3], "#B7");
    }

    [Test]
    public void BackColorTest ()
    {
        Assert.AreEqual (GtkSystemColors.Window, textBox.BackColor, "#A1");
        textBox.BackColor = Color.Red;
        Assert.AreEqual (Color.Red, textBox.BackColor, "#A2");
        textBox.BackColor = Color.White;
        Assert.AreEqual (Color.White, textBox.BackColor, "#A3");
        Assert.AreEqual (0, _invalidated, "#A4");
        Assert.AreEqual (0, _paint, "#A5");

        var form = new Form ();
        form.ShowInTaskbar = false;
        form.Controls.Add (textBox);
        form.Show ();

        _invalidated = 0;
        _paint = 0;
			
        Assert.AreEqual (Color.White, textBox.BackColor, "#B1");
        Assert.AreEqual (0, _invalidated, "#B2");
        Assert.AreEqual (0, _paint, "#B3");
        textBox.BackColor = Color.Red;
        Assert.AreEqual (Color.Red, textBox.BackColor, "#B4");
        Assert.AreEqual (1, _invalidated, "#B5");
        Assert.AreEqual (0, _paint, "#B6");
        textBox.BackColor = Color.Red;
        Assert.AreEqual (Color.Red, textBox.BackColor, "#B7");
        Assert.AreEqual (1, _invalidated, "#B8");
        Assert.AreEqual (0, _paint, "#B9");
        textBox.BackColor = Color.Blue;
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#B10");
        Assert.AreEqual (2, _invalidated, "#B11");
        Assert.AreEqual (0, _paint, "#B12");
        textBox.BackColor = Color.Empty;
        Assert.AreEqual (GtkSystemColors.Window, textBox.BackColor, "#B13");
        Assert.AreEqual (3, _invalidated, "#B14");
        Assert.AreEqual (0, _paint, "#B15");
			
        form.Close ();
    }

    [Test] // bug #80626
    [Ignore ("Depends on default font height")]
    public void BorderStyle_None ()
    {
        textBox.BorderStyle = BorderStyle.None;
        Assert.AreEqual (20, textBox.Height, "#1");
        textBox.CreateControl ();
        Assert.AreEqual (13, textBox.Height, "#2");
    }

    [Test]
    public void ClearTest ()
    {
        textBox.Text = "TextBox1";
        Assert.AreEqual ("TextBox1", textBox.Text, "#28a" );
        textBox.Clear ();
        Assert.AreEqual ("", textBox.Text, "#28b");
    }

    [Test] // bug #80620
    [Ignore ("Depends on default font height")]
    public void ClientRectangle_Borders ()
    {
        textBox.CreateControl ();
        Assert.AreEqual (textBox.ClientRectangle, new TextBox ().ClientRectangle);
    }

    [Test]		
    public void ForeColorTest ()
    {
        Assert.AreEqual (GtkSystemColors.WindowText, textBox.ForeColor, "#A1");
        textBox.ForeColor = Color.Red;
        Assert.AreEqual (Color.Red, textBox.ForeColor, "#A2");
        textBox.ForeColor = Color.White;
        Assert.AreEqual (Color.White, textBox.ForeColor, "#A3");
        Assert.AreEqual (0, _invalidated, "#A4");
        Assert.AreEqual (0, _paint, "#A5");

        var form = new Form ();
        form.ShowInTaskbar = false;
        form.Controls.Add (textBox);
        form.Show ();

        Assert.AreEqual (Color.White, textBox.ForeColor, "#B1");
        Assert.AreEqual (0, _invalidated, "#B2");
        Assert.AreEqual (0, _paint, "#B3");
        textBox.ForeColor = Color.Red;
        Assert.AreEqual (Color.Red, textBox.ForeColor, "#B4");
        Assert.AreEqual (1, _invalidated, "#B5");
        Assert.AreEqual (0, _paint, "#B6");
        textBox.ForeColor = Color.Red;
        Assert.AreEqual (Color.Red, textBox.ForeColor, "#B7");
        Assert.AreEqual (1, _invalidated, "#B8");
        Assert.AreEqual (0, _paint, "#B9");
        textBox.ForeColor = Color.Blue;
        Assert.AreEqual (Color.Blue, textBox.ForeColor, "#B10");
        Assert.AreEqual (2, _invalidated, "#B11");
        Assert.AreEqual (0, _paint, "#B12");

        form.Close ();
    }

    [Test]
    public void ReadOnly_BackColor_NotSet ()
    {
        textBox.ReadOnly = true;
        Assert.IsTrue (textBox.ReadOnly, "#A1");
        Assert.AreEqual (GtkSystemColors.Control, textBox.BackColor, "#A2");

        var form = new Form ();
        form.ShowInTaskbar = false;
        form.Controls.Add (textBox);
        form.Show ();

        Assert.IsTrue (textBox.ReadOnly, "#B1");
        Assert.AreEqual (GtkSystemColors.Control, textBox.BackColor, "#B2");

        textBox.ResetBackColor ();
        Assert.IsTrue (textBox.ReadOnly, "#C1");
        Assert.AreEqual (GtkSystemColors.Control, textBox.BackColor, "#C2");

        textBox.ReadOnly = false;
        Assert.IsFalse (textBox.ReadOnly, "#D1");
        Assert.AreEqual (GtkSystemColors.Window, textBox.BackColor, "#D2");

        textBox.ReadOnly = true;
        Assert.IsTrue (textBox.ReadOnly, "#E1");
        Assert.AreEqual (GtkSystemColors.Control, textBox.BackColor, "#E2");

        textBox.BackColor = Color.Red;
        Assert.IsTrue (textBox.ReadOnly, "#F1");
        Assert.AreEqual (Color.Red, textBox.BackColor, "#F2");

        textBox.ReadOnly = false;
        Assert.IsFalse (textBox.ReadOnly, "#G1");
        Assert.AreEqual (Color.Red, textBox.BackColor, "#G2");

        textBox.ReadOnly = true;
        Assert.IsTrue (textBox.ReadOnly, "#H1");
        Assert.AreEqual (Color.Red, textBox.BackColor, "#H2");

        textBox.ResetBackColor ();
        Assert.IsTrue (textBox.ReadOnly, "#I1");
        Assert.AreEqual (GtkSystemColors.Control, textBox.BackColor, "#I2");

        form.Close ();
    }

    [Test]
    public void ReadOnly_BackColor_Set ()
    {
        textBox.BackColor = Color.Blue;
        textBox.ReadOnly = true;
        Assert.IsTrue (textBox.ReadOnly, "#A1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#A2");

        var form = new Form ();
        form.ShowInTaskbar = false;
        form.Controls.Add (textBox);
        form.Show ();

        Assert.IsTrue (textBox.ReadOnly, "#B1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#B2");

        textBox.ReadOnly = false;
        Assert.IsFalse (textBox.ReadOnly, "#C1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#C2");

        textBox.ReadOnly = true;
        Assert.IsTrue (textBox.ReadOnly, "#D1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#D2");

        textBox.BackColor = Color.Red;
        Assert.IsTrue (textBox.ReadOnly, "#E1");
        Assert.AreEqual (Color.Red, textBox.BackColor, "#E2");

        textBox.ReadOnly = false;
        Assert.IsFalse (textBox.ReadOnly, "#F1");
        Assert.AreEqual (Color.Red, textBox.BackColor, "#F2");

        textBox.ReadOnly = true;
        textBox.ResetBackColor ();
        Assert.IsTrue (textBox.ReadOnly, "#G1");
        Assert.AreEqual (GtkSystemColors.Control, textBox.BackColor, "#G2");

        form.Dispose ();

        textBox = new TextBox ();
        textBox.ReadOnly = true;
        textBox.BackColor = Color.Blue;
        Assert.IsTrue (textBox.ReadOnly, "#H1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#H2");

        form = new Form ();
        form.ShowInTaskbar = false;
        form.Controls.Add (textBox);
        form.Show ();

        Assert.IsTrue (textBox.ReadOnly, "#I1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#I2");

        textBox.ReadOnly = false;
        Assert.IsFalse (textBox.ReadOnly, "#J1");
        Assert.AreEqual (Color.Blue, textBox.BackColor, "#J2");

        textBox.ResetBackColor ();
        Assert.IsFalse (textBox.ReadOnly, "#K1");
        Assert.AreEqual (GtkSystemColors.Window, textBox.BackColor, "#K2");
			
        form.Close ();
    }

    [Test]
    public void ToStringTest ()
    {
        Assert.AreEqual ("System.Windows.Forms.TextBox, Text: ", textBox.ToString(), "#35");
    }

    [Test] // bug #79851
    public void WrappedText ()
    {
        var text = "blabla blablabalbalbalbalbalbal blabla blablabl bal " +
                   "bal bla bal balajkdhfk dskfk ersd dsfjksdhf sdkfjshd f";

        textBox.Multiline = true;
        textBox.Size = new Size (30, 168);
        textBox.Text = text;

        var form = new Form ();
        form.Controls.Add (textBox);
        form.ShowInTaskbar = false;
        form.Show ();

        Assert.AreEqual (text, textBox.Text);
			
        form.Close ();
    }

    [Test] // bug #79909
    public void MultilineText ()
    {
        var text = "line1\n\nline2\nline3\r\nline4";

        textBox.Size = new Size (300, 168);
        textBox.Text = text;

        var form = new Form ();
        form.Controls.Add (textBox);
        form.ShowInTaskbar = false;
        form.Show ();

        Assert.AreEqual (text, textBox.Text, "#1");

        text = "line1\n\nline2\nline3\r\nline4\rline5\r\n\nline6\n\n\nline7";

        textBox.Text = text;

        form.Visible = false;
        form.Show ();

        Assert.AreEqual (text, textBox.Text, "#2");
			
        form.Close ();
    }

    [Test]
    public void Bug82749 ()
    {
        var f = new Form ();
        f.ShowInTaskbar = false;

        var _textBox = new TextBox ();
        _textBox.Dock = DockStyle.Top;
        _textBox.Height = 100;
        _textBox.Multiline = true;
        f.Controls.Add (_textBox);
			
        f.Show ();
        Assert.AreEqual (100, _textBox.Height, "A1");
			
        // Font dependent, but should be less than 30.
        _textBox.Multiline = false;
        Assert.IsTrue (_textBox.Height < 30, "A2");

        _textBox.Multiline = true;
        Assert.AreEqual (100, _textBox.Height, "A3");
			
        f.Close ();
        f.Dispose ();
    }
		
    [Test]
    public void Bug6357 ()
    {
        var f = new Form (); 
        f.ShowInTaskbar = false;
        f.Visible = true;
        f.ClientSize = new Size (300, 130);
        textBox.Visible = true;
        textBox.AppendText(
            "Achtung! Passwort für URL angepasst! Anführungszeichen im Passwort funktionieren in URL nur mit Escape.\r\n" +
            "\r\n" +
            "{S:fileFilepath} -> {S:##volumeDriveLetter}:\\\r\n" +
            "\r\n" +
            "Verschlüsselter Kontainer (VeraCrypt).\r\n" +
            "\r\n" +
            "URL-Anmerkungen:\r\n" +
            "- nur für Windows\r\n" +
            "- volumeDriveLetter muss frei sein\r\n" +
            "\r\n" +
            "veracrypt --mount /media/NAS_container_flo/test.vc -p '1 1' --fs-options=X-mount.mkdir=0700 /media/vera\r\n" +
            "\r\n" +
            "cmd://veracrypt --mount {S:fFilepath} -p '{PASSWORD}' --pim='{S:#pim}' --fs-options=X-mount.mkdir=0700 {S:mPoint}" +
            "\r\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\r\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        textBox.Multiline = true;
        f.Dispose ();
    }


    bool modified_changed_fired;

    void TextBox_ModifiedChanged (object sender, EventArgs e)
    {
        modified_changed_fired = true;
    }

    void TextBox_TextChanged (object sender, EventArgs e)
    {
        _changed++;
    }

    void TextBox_Invalidated (object sender, InvalidateEventArgs e)
    {
        _invalidated++;
    }

    void TextBox_Paint (object sender, PaintEventArgs e)
    {
        _paint++;
    }

    void Reset ()
    {
        _changed = 0;
        _invalidated = 0;
        _paint = 0;
    }

}
