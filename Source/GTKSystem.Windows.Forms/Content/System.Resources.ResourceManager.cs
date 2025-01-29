//This file is mainly to overwrite the native System.Resources.ResourceManager class. The native class does not support reading project resource image files.
//GTKSystem.Resources.ResourceManager implements reading of project resource files and image files.
//If the resource image file is not used in the project, there is no need to create this file.

using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace System.Resources
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ResourceManager : GTKSystem.Resources.ResourceManager
    {
        public ResourceManager(System.Type resourceSource) : base(null, null, resourceSource)
        {

        }
        public ResourceManager(string baseName, Assembly assembly) : base(baseName, assembly, null)
        {

        }
        public ResourceManager(string baseName, Assembly assembly, System.Type? resourceSource) : base(baseName, assembly, resourceSource)
        {

        }
        protected ResourceManager()
        {

        }
    }
}