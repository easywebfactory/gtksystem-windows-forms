namespace System.Windows.Forms;

public class Clipboard
{
    public static string GetText()
    {
        var clipboard = Gtk.Clipboard.Get(Gdk.Selection.Clipboard);
        var value = string.Empty;
        clipboard.RequestText((clip, text) =>
        {
            if (!string.IsNullOrEmpty(text))
            {
                value = text; // Set clipboard text in Entry
            }
        });
        return value;
    }
}