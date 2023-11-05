# GTKSystem.Windows.Forms

#### 介绍
**Visual Studio原生开发，无需学习，一次编译，跨平台运行**.
这是基于GTK3.24.24框架组件开发的跨平台（windows和linux）C#桌面应用程序表单界面组件，该组件的核心优势是使用C#的原生winform表单控件和属性方法，C#原生开发即可，无需学习。

作者博客请看 [https://www.cnblogs.com/easywebfactory/p/17803567.html](https://www.cnblogs.com/easywebfactory/p/17803567.html)

#### 软件架构

使用GTK3.24.24做为GDI，重写C#的System.Windows.Forms组件，在应用时，兼容原生C#程序组件。

#### 安装教程

首先必须是.net core3.1及以上版本的项目工程。
1.  把项目工程改为“控制台应用程序”或者配置UseWindowsForms为false
2.  NulGet安装GtkSharp(3.24.24.34)
3.  引用Libs目录下的GTKSystem.Windows.Forms.dll和System.Resources.Extensions.dll
4.  编译发布测试运行

#### 使用说明

以下配置在你的项目工程里操作：
1、新建System.Resources.ResourceManager类
在项目下新建System.Resources.ResourceManager类，继承GTKSystem.Resources.ResourceManager，用于覆盖原生System.Resources.ResourceManager类。
GTKSystem.Resources.ResourceManager实现了项目资源文件和图像文件读取。
如果项目里没有使用资源图像文件，可以不用新建此文件。

2、新建System.ComponentModel.ComponentResourceManager类
在项目下新建System.ComponentModel.ComponentResourceManager类，继承GTKSystem.ComponentModel.ComponentResourceManager，用于覆盖原生System.ComponentModel.ComponentResourceManager类。
GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取（调用GTKSystem.Resources.ResourceManager）。
如果项目里没有使用资源图像文件，可以不用新建此文件。

3、GTKWinFormsApp.csproj
配置UseWindowsForms为false，或者使用控制台应用程序
<UseWindowsForms>false</UseWindowsForms>

4、引用GTKSystem.Windows.Forms、System.Resources.Extensions
System.Resources.Extensions是空程序dll，VS加载Form界面时验证需要此dll.

5、GTKWinFormsApp\obj\Debug\netcoreapp3.1\GTKWinFormsApp.designer.runtimeconfig.json
GTKWinFormsApp\obj\Release\netcoreapp3.1\GTKWinFormsApp.designer.runtimeconfig.json
将name设置为Microsoft.WindowsDesktop.App，用于VS支持可视化Form表单，重新加载工程或重启VS
  "runtimeOptions": {
    "framework": {
      "name": "Microsoft.WindowsDesktop.App"
    },

#### demo效果
<img>https://gitee.com/easywebfactory/gtksystem-windows-forms/blob/master/pic/2023-11-06%20065348.jpg<img>

#### 参与贡献

1. https://gitee.com/easywebfactory/


