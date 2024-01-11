using Cairo;
using GLib;
using Gtk;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace GTKSystem.ComponentModel
{
    /// <summary>
    /// 此为System.ComponentModel.ComponentResourceManager的取图实现
    /// 使用方法：在form项目下新建System.ComponentModel.ComponentResourceManager并继承GTKSystem.ComponentModel.ComponentResourceManager即可
    /// </summary>
    public class ComponentResourceManager
    {
        private Type formtype;
        private string formName;
        public ComponentResourceManager(Type form)
        {
            formtype = form;
            formName = form.Name;
        }

        public virtual object GetObject(string name, CultureInfo culture)
        {
            return GetObject(name);
        }
        public virtual object GetObject(string name)
        {
            GTKSystem.Resources.ResourceManager temp = new GTKSystem.Resources.ResourceManager(formtype.FullName, formtype.Assembly);
            return temp.GetObject(name);
        }
        public virtual string GetString(string name)
        {
            GTKSystem.Resources.ResourceManager temp = new GTKSystem.Resources.ResourceManager(formtype.FullName, formtype.Assembly);
            return temp.GetString(name);
        }
    }

}

