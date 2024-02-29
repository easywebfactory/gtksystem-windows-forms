# GTKSystem.Windows.Forms

#### 介绍
**Visual Studio原生开发，无需学习，一次编译，跨平台运行**.
C#桌面应用程序跨平台（windows、linux、macos）界面开发组件，基于GTK组件开发，该组件的核心优势是使用C#的原生winform表单控件窗体设计器，相同的属性方法，C#原生开发即可，无需学习。一次编译，跨平台运行。
便于开发跨平台winform软件，便于将C#升级为跨平台软件。

作者博客请看 [https://www.cnblogs.com/easywebfactory/p/17803567.html](https://www.cnblogs.com/easywebfactory/p/17803567.html)

目前功能持续更新中，将优先完善常用功能。

#### 软件架构

使用GTK3.24.24.95作为表单UI重写C#的System.Windows.Forms组件，在应用时，兼容原生C#程序组件。

#### 安装教程
1.  把项目工程改为配置UseWindowsForms为false或“控制台应用程序”，框架.net6或以上版本
2.  NulGet安装GtkSharp(3.24.24.95)、GTKSystem.Windows.Forms
3.  引用GTKSystem.Windows.Forms.dll
4.  检查form表单是否有使用图像资源，如使用需新建System.Resources.ResourceManager和System.ComponentModel.ComponentResourceManager，具体请看下面内容。
4.  按默认配置编译发布测试运行
5.  linux和macos上执行命令：dotnet doemo_app.dll

 （注：如果出现打开visual studio的Form窗体设计器出现“设计器”相关异常，可自建一个空类，命名为System.Resources.Extensions.dll，引用）。

#### VisualStudio插件安装

下载本插件工具，关闭visual studio，直接双击GTKWinformVSIXProject.vsix文件安装

插件会安装两个功能，都是在右键菜单和工具菜单上添加：

1、新建项的Form窗体模板

2、菜单增加设置/修复窗体设计器

![输入图片说明](pic/vs_vsix.jpeg)

#### 使用说明

以下配置在你的项目工程里操作：

1、新建System.Resources.ResourceManager类<br/>
在项目下新建System.Resources.ResourceManager类，继承GTKSystem.Resources.ResourceManager，用于覆盖原生System.Resources.ResourceManager类。
GTKSystem.Resources.ResourceManager实现了项目资源文件和图像文件读取。
如果项目里没有使用资源图像文件，可以不用新建此文件。

2、新建System.ComponentModel.ComponentResourceManager类<br/>
在项目下新建System.ComponentModel.ComponentResourceManager类，继承GTKSystem.ComponentModel.ComponentResourceManager，用于覆盖原生System.ComponentModel.ComponentResourceManager类。<br/>
GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取（调用GTKSystem.Resources.ResourceManager）。
如果项目里没有使用资源图像文件，可以不用新建此文件。

3、GTKWinFormsApp.csproj<br/>
配置UseWindowsForms为false，或者使用控制台应用程序
```
<UseWindowsForms>false</UseWindowsForms>
```

4、引用GTKSystem.Windows.Forms、System.Resources.Extensions <br/>
System.Resources.Extensions是空程序dll，VS加载Form界面时验证需要此dll.

5、GTKWinFormsApp\obj\Debug\net6.0\GTKWinFormsApp.designer.runtimeconfig.json
GTKWinFormsApp\obj\Release\net6.0\GTKWinFormsApp.designer.runtimeconfig.json
将name设置为Microsoft.WindowsDesktop.App， **用于VS支持可视化窗体设计器，重新加载工程或重启VS** 

```
   "runtimeOptions": {
     "framework": {
      "name": "Microsoft.WindowsDesktop.App"
    },

```

#### demo效果
![输入图片说明](/pic/run.jpg)

#### 工具栏菜单
![输入图片说明](/pic/toolstrip.jpeg)

#### 窗口背景
![输入图片说明](/pic/backgroundimage.jpg)

#### 交流
QQ群：236066073

#### 参与贡献

1. https://gitee.com/easywebfactory
2. https://github.com/easywebfactory

#### 更新日志
 ## 2024/2/29
 1. grahpics增加曲线和多边形绘图，优化文字绘图程序
 2. 修正一些隐性异常 
 ## 2024/2/23
 1. 实现和修正DataGridView单元控件数据编辑、取数功能
