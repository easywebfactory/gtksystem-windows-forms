using System.Globalization;
using System.Windows.Forms.Resources;

namespace GTKSystem.ComponentModel;

/// <summary>
/// 此为System.ComponentModel.ComponentResourceManager的取图实现
/// 使用方法：在form项目下新建System.ComponentModel.ComponentResourceManager并继承GTKSystem.ComponentModel.ComponentResourceManager即可
/// </summary>
public class ComponentResourceManager : System.ComponentModel.ComponentResourceManager
{
    private readonly Type formtype;
    private string formName;
    public ComponentResourceManager(Type form) : base(form)
    {
        formtype = form;
        formName = form.Name;
    }

    public override object? GetObject(string name, CultureInfo culture)
    {
        return GetObject(name);
    }
    public override object? GetObject(string name)
    {
        var temp = new ResourceManager(formtype.FullName, formtype.Assembly);
        return temp.GetObject(name);
    }
    public override string? GetString(string name)
    {
        var temp = new ResourceManager(formtype.FullName, formtype.Assembly);
        return temp.GetString(name);
    }
}