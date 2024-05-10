//此文件主要是为了覆盖原生System.Resources.ResourceManager类，原生类不支持读取项目资源图像文件。
//GTKSystem.Resources.ResourceManager实现了项目资源文件和图像文件读取。
//如果项目里没有使用资源图像文件，可以不用新建此文件

using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace System.Resources
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ResourceManager: GTKSystem.Resources.ResourceManager
    {
        public ResourceManager(System.Type resourceSource) : base(null, null, resourceSource)
        {

        }
        public ResourceManager(string baseName, Assembly assembly) : base(baseName, assembly, null)
        {

        }
        public ResourceManager(string baseName, Assembly assembly, System.Type resourceSource) : base(baseName, assembly, resourceSource)
        {

        }
        protected ResourceManager()
        {

        }
    }
}