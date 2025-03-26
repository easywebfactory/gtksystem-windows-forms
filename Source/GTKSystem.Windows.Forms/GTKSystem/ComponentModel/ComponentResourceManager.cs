using System.Globalization;
using System.Resources;

namespace System.ComponentModel;

/// <summary>
/// This is the image retrieval implementation of System.ComponentModel.ComponentResourceManager
/// How to use: Create a new System.ComponentModel.ComponentResourceManager under the form project and inherit GTKSystem.ComponentModel.ComponentResourceManager.
/// </summary>
public class GtkComponentResourceManager : ComponentResourceManager
{
    private readonly Type formtype;
    private string formName;
    public GtkComponentResourceManager(Type form) : base(form)
    {
        formtype = form;
        formName = form.Name;
    }

    public override object? GetObject(string name, CultureInfo culture)
    {
        var temp = new GtkResourceManager(formtype.FullName, formtype.Assembly);
        return temp.GetObject(name, culture);
    }
    public override object? GetObject(string name)
    {
        var temp = new GtkResourceManager(formtype.FullName, formtype.Assembly);
        return temp.GetObject(name);
    }
    public override string? GetString(string name)
    {
        var temp = new GtkResourceManager(formtype.FullName, formtype.Assembly);
        return temp.GetString(name);
    }
}