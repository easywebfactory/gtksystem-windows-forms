using System.ComponentModel;
using System.Reflection;
using System.Xml;

namespace GtkTests.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ResourceManager : GTKSystem.Resources.ResourceManager
    {
        public ResourceManager(Type resourceSource) : base(null, null, resourceSource)
        {

        }
        public ResourceManager(string baseName, Assembly assembly) : base(baseName, assembly, null)
        {

        }
        public ResourceManager(string baseName, Assembly assembly, Type resourceSource) : base(baseName, assembly, resourceSource)
        {

        }
        protected ResourceManager()
        {

        }
    }
}