using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;


namespace GTKSystem.ComponentModel
{
    /// <summary>
    /// 此为System.ComponentModel.ComponentResourceManager的取图实现
    /// 使用方法：在form项目下新建System.ComponentModel.ComponentResourceManager并继承GTKSystem.ComponentModel.ComponentResourceManager即可
    /// </summary>
    public class ComponentResourceManager : System.ComponentModel.ComponentResourceManager
    {
        private Type formtype;
        private string formName;
        private GTKSystem.Resources.ResourceManager resource;
        public ComponentResourceManager([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type form) : base(form)
        {
            formtype = form;
            formName = form.Name;
            resource = new GTKSystem.Resources.ResourceManager(formtype.FullName, formtype.Assembly);
        }

        public override object GetObject(string name, CultureInfo culture)
        {
            return GetObject(name);
        }
        public override object GetObject(string name)
        {
            return resource.GetObject(name);
        }
        public override string GetString(string name)
        {
            return resource.GetString(name);
        }
    }

}

