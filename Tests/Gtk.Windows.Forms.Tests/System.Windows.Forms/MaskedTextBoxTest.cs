//
// Copyright (c) 2007 Novell, Inc.
//
// Authors:
//      Rolf Bjarne Kvinge  (RKvinge@novell.com)
//

using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using Thread=System.Threading.Thread;
using System.Reflection;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class MaskedTextBoxTest : TestHelper
{
    [SetUp]
    protected override void SetUp () {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo ("en-US");
        base.SetUp ();
    }

    [Test]
    public void InitialProperties ()
    {
        var mtb = new MaskedTextBox ();
        Assert.AreEqual (0, mtb.Lines.Length, "#L1");
        Assert.AreEqual ("", mtb.Mask, "#M1");
        Assert.AreEqual (false, mtb.Multiline, "#M6");
        Assert.AreEqual ('\0', mtb.PasswordChar, "#P1");
        Assert.AreEqual (false, mtb.ReadOnly, "#R1");
        Assert.AreEqual (MaskFormat.IncludeLiterals, mtb.TextMaskFormat, "#T4");
        Assert.IsNull (mtb.ValidatingType, "#V1");
			
        mtb.Dispose ();
    }
		
    [Test]
    public void ValidatingTypeTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.IsNull (mtb.ValidatingType, "#V1");
        mtb.ValidatingType = typeof(int);
        Assert.IsNotNull (mtb.ValidatingType, "#V2");
        Assert.AreSame (typeof(int), mtb.ValidatingType, "#V3");
        mtb.Dispose ();
    }
		
    [Test]
    public void TextMaskFormatTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual (MaskFormat.IncludeLiterals, mtb.TextMaskFormat, "#T1");
        mtb.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
        Assert.AreEqual (MaskFormat.ExcludePromptAndLiterals, mtb.TextMaskFormat, "#T2");
        mtb.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
        Assert.AreEqual (MaskFormat.IncludePromptAndLiterals, mtb.TextMaskFormat, "#T3");
        mtb.TextMaskFormat = MaskFormat.IncludePrompt;
        Assert.AreEqual (MaskFormat.IncludePrompt, mtb.TextMaskFormat, "#T4");
        mtb.TextMaskFormat = MaskFormat.IncludeLiterals;
        Assert.AreEqual (MaskFormat.IncludeLiterals, mtb.TextMaskFormat, "#T4");
        mtb.Dispose ();
    }
		
    [Test]
    public void TextMaskFormatExceptionTestException ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            MaskedTextBox mtb;

            mtb = new MaskedTextBox();
            mtb.TextMaskFormat = (MaskFormat)123;
            mtb.Dispose();
        });
    }
		
    [Test]
    public void TextTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual ("", mtb.Text, "#T1");
        mtb.Text = "abc";
        Assert.AreEqual ("abc", mtb.Text, "#T2");
        mtb.Text = "ABC";
        Assert.AreEqual ("ABC", mtb.Text, "#T3");
        mtb.Mask = "abc";
        mtb.Text = "abc";
        Assert.AreEqual ("abc", mtb.Text, "#T4");
        mtb.Text = "ABC";
        Assert.AreEqual ("Abc", mtb.Text, "#T5");
        mtb.Text = "123";
        Assert.AreEqual ("1bc", mtb.Text, "#T6");
        mtb.Dispose ();
    }

    [Test]
    public void TextTest2 ()
    {
        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        mtb.Mask = "99 99";

        mtb.Text = "23 34";
        Assert.AreEqual ("23 34", mtb.Text, "#T1");

        mtb.Dispose ();
    }

    [Test]
    public void TextTest3 ()
    {
        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        mtb.Mask = "00-00";
        mtb.Text = "12 3";
        Assert.AreEqual ("12- 3", mtb.Text, "#T1");

        mtb.Text = "b31i4";
        Assert.AreEqual ("31-4", mtb.Text, "#T2");

        mtb.Text = "1234";
        Assert.AreEqual ("12-34", mtb.Text, "#T3");

        mtb.Dispose ();
    }

    [Test]
    public void ReadOnlyTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual (false, mtb.ReadOnly, "#R1");
        mtb.ReadOnly = true;
        Assert.AreEqual (true, mtb.ReadOnly, "#R2");
        mtb.Dispose ();
    }
		
    [Test]
    public void PasswordCharTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual ('\0', mtb.PasswordChar, "#P1");
        mtb.PasswordChar = '*';
        Assert.AreEqual ('*', mtb.PasswordChar, "#P2");
        mtb.Dispose ();
    }

    [Test]
    public void MultilineTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual (false, mtb.Multiline, "#M1");
        mtb.Multiline = true;
        Assert.AreEqual (false, mtb.Multiline, "#M2");
        mtb.Dispose ();
    }
		
    [Test]
    public void MaskTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual ("", mtb.Mask, "#M1");
        mtb.Mask = "abc";
        Assert.AreEqual ("abc", mtb.Mask, "#M2");
        mtb.Mask = "";
        Assert.AreEqual ("", mtb.Mask, "#M3");
        mtb.Mask = null;
        Assert.AreEqual ("", mtb.Mask, "#M4");
        mtb.Mask = "";
        Assert.AreEqual ("", mtb.Mask, "#M5");
        mtb.Dispose ();
    }

    [Test]
    public void LinesTest ()
    {

        MaskedTextBox mtb;

        mtb = new MaskedTextBox ();
        Assert.AreEqual (0, mtb.Lines.Length, "#L1");
        mtb.Text = "abc";
        Assert.AreEqual (1, mtb.Lines.Length, "#L2");
        Assert.AreEqual ("abc", mtb.Lines [0], "#L2a");
        mtb.Text = "abc\nabc";
        Assert.AreEqual (2, mtb.Lines.Length, "#L3");
        Assert.AreEqual ("abc", mtb.Lines [0], "#L3a");
        Assert.AreEqual ("abc", mtb.Lines [1], "#L3b");
        mtb.Dispose ();
    }

    [Test]
    public void CreateHandleTest ()
    {
        using var mtb = new MaskedTextBox ();
        Assert.AreEqual (false, mtb.IsHandleCreated, "#A1");
        typeof (MaskedTextBox).GetMethod ("CreateHandle", BindingFlags.Instance | BindingFlags.NonPublic).Invoke (mtb, new object [] { });
        Assert.AreEqual (true, mtb.IsHandleCreated, "#A2");
    }
		
    [Test]
    public void IsInputKeyTest ()
    {
        using var f = new Form ();
        using var mtb = new MaskedTextBox ();
        f.Controls.Add (mtb);
        f.Show ();
        var IsInputKey = typeof (MaskedTextBox).GetMethod ("IsInputKey", BindingFlags.NonPublic | BindingFlags.Instance);
				
        for (var i = 0; i <= 0xFF; i++) {
            var key = (Keys) i;
            var key_ALT = key | Keys.Alt;
            var key_SHIFT = key | Keys.Shift;
            var key_CTRL = key | Keys.Control;
            var key_ALT_SHIFT = key | Keys.Alt | Keys.Shift;
            var key_ALT_CTRL = key | Keys.Alt | Keys.Control;
            var key_SHIFT_CTLR = key | Keys.Shift | Keys.Control;
            var key_ALT_SHIFT_CTLR = key | Keys.Alt | Keys.Shift | Keys.Control;

            var is_input = false;
					
            switch (key) {
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Back:
                    is_input = true;
                    break;
            }

            Assert.AreEqual (is_input, (bool)IsInputKey.Invoke (mtb, new object [] { key }));
            Assert.AreEqual (false, (bool)IsInputKey.Invoke (mtb, new object [] { key_ALT }));
            Assert.AreEqual (is_input, (bool)IsInputKey.Invoke (mtb, new object [] { key_SHIFT }));
            Assert.AreEqual (is_input, (bool)IsInputKey.Invoke (mtb, new object [] { key_CTRL }));
            Assert.AreEqual (false, (bool)IsInputKey.Invoke (mtb, new object [] { key_ALT_SHIFT }));
            Assert.AreEqual (false, (bool)IsInputKey.Invoke (mtb, new object [] { key_ALT_CTRL }));
            Assert.AreEqual (is_input, (bool)IsInputKey.Invoke (mtb, new object [] { key_SHIFT_CTLR }));
            Assert.AreEqual (false, (bool)IsInputKey.Invoke (mtb, new object [] { key_ALT_SHIFT_CTLR }));
        }
    }
		
    [Test]
    public void ValidateTextTest ()
    {
        Assert.Ignore ("Pending implementation");
    }
		
    [Test]
    public void ToStringTest ()
    {
        Assert.Ignore ("Pending implementation");
    }
}