//This file is mainly to overwrite the native System.ComponentModel.ComponentResourceManager class. The native class does not support reading project resource image files.
//GTKSystem.ComponentModel.ComponentResourceManager implements reading of project resource files and image files.
//If the resource image file is not used in the project, there is no need to create this file.

using System.Globalization;

namespace System.ComponentModel
{
    public class ComponentResourceManager : GTKSystem.ComponentModel.ComponentResourceManager
    {
        public ComponentResourceManager(Type form) : base(form)
        {

        }
        public new object GetObject(string name, CultureInfo culture)
        {

            return GetObject(name);
        }

        public new object GetObject(string name)
        {
            return base.GetObject(name);
        }
    }

}

