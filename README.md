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
2.  NulGet安装GtkSharp(3.24.24.95)、GTKSystem.Windows.Forms，或引用GTKSystem.Windows.Forms.dll
3.  检查form表单是否有使用图像资源，如使用需新建System.Resources.ResourceManager和System.ComponentModel.ComponentResourceManager，具体请看下面内容。
4.  按默认配置编译发布测试运行
5.  linux和macos上执行命令：dotnet demo_app.dll
6.  编译工程，执行本项目的开发插件菜单“修复窗体设计器”，或者手动在obj目录下创建.designer.runtimeconfig.json，请看下面第5点。
 
linux安装gtk环境：
```
 sudo apt install libgtk-3-dev
 或
 sudo apt-get install libgtk3*
```

#### VisualStudio插件安装

工具一、从NuGet上安装GTKSystem.Windows.FormsDesigner类库，此类库可以在编译工程时修正窗体设计器。

工具二、下载本插件工具，关闭visual studio，直接双击GTKWinformVSIXProject.vsix文件安装

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
配置UseWindowsForms为false，目标OS设置为“(空)”，或者使用控制台应用程序(在控制台框架下会显示控制台窗口，不建议这种方式)
```
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <UseWindowsForms>false</UseWindowsForms>
```

4、引用GTKSystem.Windows.Forms、System.Resources.Extensions <br/>
GTKSystem.Windows.Forms是必须引用<br/>
System.Resources.Extensions是空程序dll，不是必须引用，只有VS在窗体设计器出现相关异常提示时使用

5、GTKWinFormsApp\obj\Debug\net8.0\GTKWinFormsApp.designer.runtimeconfig.json
GTKWinFormsApp\obj\Debug\net8.0\GTKWinFormsApp.runtimeconfig.json
将name设置为Microsoft.WindowsDesktop.App， **用于VS支持可视化窗体设计器，重新加载工程或重启VS** 
以下的是我配置：
GTKWinFormsApp.designer.runtimeconfig.json
```
{
  "runtimeOptions": {
    "tfm": "net8.0",
    "framework": {
      "name":"Microsoft.WindowsDesktop.App",
      "version": "8.0.0"
    },
    "additionalProbingPaths": [
      "C:\\Users\\chj\\.dotnet\\store\\|arch|\\|tfm|", 
      "C:\\Users\\chj\\.nuget\\packages",
      "C:\\Program Files (x86)\\Microsoft Visual Studio\\Shared\\NuGetPackages",
      "C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder"
    ],
    "configProperties": {
      "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false,
      "Microsoft.NETCore.DotNetHostPolicy.SetAppPaths": true
    }
  }
}
```

GTKWinFormsApp.runtimeconfig.json
```
{
  "runtimeOptions": {
    "tfm": "net8.0",
    "framework": {
      "name": "Microsoft.WindowsDesktop.App",
      "version": "8.0.0"
    },
    "configProperties": {
      "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false
    }
  }
}
```

#### demo效果
![输入图片说明](/pic/run.jpg)

#### 工具栏菜单
![输入图片说明](/pic/toolstrip.jpeg)

#### 窗口背景
![输入图片说明](/pic/backgroundimage.jpg)

#### 交流/合作/商务/赞助
QQ群：236066073
邮箱：438865652@qq.com

#### 常见问题
1、为什么Form窗体设计器打不开？
答：
    正确的使用流程：
    1、编译一下
    2、执行“修复窗体设计器”
    3、打开Form窗体
    （如果不能打开窗体，执行下面流程）
    4、关闭Form窗体，执行“修复窗体设计器”
    5、重启vs
    6、打开Form窗体设计器


#### 参与贡献

1. https://gitee.com/easywebfactory
2. https://github.com/easywebfactory

#### 更新日志
 ## 2024/5/1
  1. 重大更新！重构控件的结构程序，优化了很多控件功能和性能，修正一些错误
  2. 优化了绘图、控件背景功能程序，绘制背景图不再覆盖子控件
  3. 特别优化Form界面程序和性能

 ## 2024/4/20
 1. 修正graphic绘图的位置
 2. 实现graphicpath绘图、渐变色
 3. 实现控件的BeginInvoke和EndInvoke方法
 4. 修改DataGridView、ListBox的数据加载程序，修正不能在窗口启动加载数据的问题
 ## 2024/3/27
 1. 改正usercontrol在窗体设计器上打开出现异常的问题（还无法显示控件）
 2. 实现graphics上的椭圆绘画
 ## 2024/3/19
 1. panel内容溢出显示滚动条，窗口缩放程序优化
 ## 2024/3/14
 1. 修正TreeView数据程序加载
 ## 2024/3/6
 1. 修正一些窗口配置问题、binding
 ## 2024/3/2
 1. 修正label文本的字体大小问题、增加对齐属性
 2. 实现imagelist兼容使用窗体设计器
 ## 2024/2/29
 1. grahpics增加曲线和多边形绘图，优化文字绘图程序
 2. 修正一些隐性异常 
 ## 2024/2/23
 1. 实现和修正DataGridView单元控件数据编辑、取数功能
