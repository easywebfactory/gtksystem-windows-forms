//此文件主要是为了覆盖原生System.ComponentModel.ComponentResourceManager类，原生类不支持读取项目资源图像文件。
//GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取。
//如果项目里没有使用资源图像文件，可以不用新建此文件

using System.Globalization;

namespace System.ComponentModel
{
    public class ComponentResourceManager:GTKSystem.ComponentModel.ComponentResourceManager
    {
        public ComponentResourceManager(Type form):base(form)
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

