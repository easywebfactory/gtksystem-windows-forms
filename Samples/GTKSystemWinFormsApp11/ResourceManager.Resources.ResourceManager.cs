// ReSharper disable CheckNamespace

using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace System.Resources
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class ResourceManager : Windows.Forms.Resources.ResourceManager
    {
        public ResourceManager(System.Type resourceSource) : base(null, null, resourceSource)
        {

        }
        public ResourceManager(string? baseName, Assembly? assembly) : base(baseName, assembly, null)
        {

        }
        public ResourceManager(string? baseName, Assembly? assembly, System.Type? resourceSource) : base(baseName, assembly, resourceSource)
        {

        }
        protected ResourceManager()
        {

        }
    }
}