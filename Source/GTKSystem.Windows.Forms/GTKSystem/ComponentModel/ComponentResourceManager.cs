using System;
using System.Globalization;


namespace GTKSystem.ComponentModel
{
    /// <summary>
    /// This is the image retrieval implementation of System.ComponentModel.ComponentResourceManager
    /// How to use: Create a new System.ComponentModel.ComponentResourceManager under the form project and inherit GTKSystem.ComponentModel.ComponentResourceManager.
    /// </summary>
    public class ComponentResourceManager : System.ComponentModel.ComponentResourceManager
    {
        private Type formtype;
        private string formName;
        public ComponentResourceManager(Type form) : base(form)
        {
            formtype = form;
            formName = form.Name;
        }

        public override object GetObject(string name, CultureInfo culture)
        {
            return GetObject(name);
        }
        public override object GetObject(string name)
        {
            GTKSystem.Resources.ResourceManager temp = new GTKSystem.Resources.ResourceManager(formtype.FullName, formtype.Assembly);
            return temp.GetObject(name);
        }
        public override string GetString(string name)
        {
            GTKSystem.Resources.ResourceManager temp = new GTKSystem.Resources.ResourceManager(formtype.FullName, formtype.Assembly);
            return temp.GetString(name);
        }
    }

}

