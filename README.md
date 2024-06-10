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
1.  项目工程框架选择“window应用程序”改配置UseWindowsForms为false或“控制台应用程序”，框架.netcore3.1或.net6及以上版本
2.  NulGet安装GtkSharp(3.24.24.95)、GTKSystem.Windows.Forms、GTKSystem.Windows.FormsDesigner
3.  检查form表单是否有使用图像资源，如使用需新建System.Resources.ResourceManager和System.ComponentModel.ComponentResourceManager，具体请看下面内容。
4.  按默认配置编译发布测试运行
5.  linux和macos上执行命令：dotnet demo_app.dll
6.  编译工程，执行本项目的开发插件菜单“修复窗体设计器”，或者手动在obj目录下创建.designer.runtimeconfig.json，请看下面第5点。

注意：安装GtkSharp后，编译你的工程项目时，会自动下载$(LOCALAPPDATA)\Gtk\3.24.24\gtk.zip配置Gtk环境，目前国内网络限制，可能会出现无法下载的错误，可以尝试安装fastgithub软件解决，或者其它vpn软件。
如果无法自动下载，本项目提供下载 [/Dependencies/gtk-3.24.24.zip](/Dependencies/gtk-3.24.24.zip)。
也可以下载[https://github.com/GtkSharp/Dependencies](https://github.com/GtkSharp/Dependencies)，把文件解压后放到$(LOCALAPPDATA)\Gtk\3.24.24\gtk.zip目录即可。

ps:$(LOCALAPPDATA)为电脑的AppData\Local文件夹,如：C:\Users\chj\AppData\Local\Gtk\3.24.24

linux安装gtk环境：
```
 sudo apt install libgtk-3-dev
 或
 sudo apt-get install libgtk3*
```
linux安装dotnet环境：
```
  安装方法可以查看微软官网教程：https://learn.microsoft.com/zh-cn/dotnet/core/install/linux-scripted-manual
```
#### VisualStudio插件安装

工具一、从NuGet上安装GTKSystem.Windows.FormsDesigner类库，此类库可以在编译工程时修正窗体设计器。

工具二、下载本插件工具，关闭visual studio，直接双击GTKWinformVSIXProject.vsix文件安装（本框架下的工程，Studio没有添加Form模板项，需要安装此插件）

插件会安装两个功能：

1、新建项的Form窗体模板、用户控件模板。

2、工程右键菜单。

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
如以下配置：
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

#### 默认风格效果
![demo效果](/pic/native-2.png)

#### 默认风格工具栏菜单
![工具栏菜单](/pic/native-1.png)

#### 图文窗口
![图文窗口](/pic/listview.png)

### 支持各种主题风格界面（windows xp、vista、7、8、10，macOS系列，等等）

#### 主题风格，windows7风格界面
![windows7风格界面](/pic/windows7-1.png)
![windows7风格界面](/pic/windows7-0.png)
#### 主题风格，macOS风格界面
![macOS风格界面](/pic/macOS-1.png)

#### 支持GTKSystem，获取技术服务
 ![支持GTKSystem](/pic/love_reward_qrcode_.png)

#### 添加GTKSystem微信，更多服务帮助
 ![联系GTKSystem](/pic/contact_weixin.png)

| ￥299基础服务一   | ￥2999套餐服务二    | ￥9999套餐服务三     |
| :---               |    :----           |          :---   |
| 帮助开发环境安装       | 包括套餐一服务        |   包含所有套餐服务  |
| 运行环境gtk\dotnet安装 | 提供二次开发技术支持  |   提供二次开发技术支持    |
| 上手开发疑难解答       | 为用户开发协商指定的功能或接口至少3项 | 为用户开发功能或接口不少于9项  |
| 帮助解决程序异常      | 提供一套定制的主题风格界面样式和跨平台webview控件    |  提供至少三套定制的主题风格界面样式和跨平台webview控件                              |

#### 交流/合作/商务/赞助
QQ群：236066073
邮箱：438865652@qq.com <br/>
#### 常见问题
1、为什么Form窗体设计器打不开？
答：检查runtimeconfig的确保配置正确，NuGet安装GTKSystem.Windows.FormsDesigner，然后按以下流程操作：
    1、编译一下
    3、打开Form窗体
    （如果不能打开窗体，执行下面流程）
    4、关闭Form窗体，编译一下
    5、重启vs
    6、打开Form窗体设计器



#### 参与贡献

1. https://gitee.com/easywebfactory
2. https://github.com/easywebfactory

#### 更新日志
 ## 2024/6/10
   1. 修改控件背景图显示方式，让大部分控件支持圆角和背景图圆角显示（重要）
   2. 修改优化了很多控件的功能和性能
   3. 修改优化了控件的样式显示程序，以支持风格主题换肤机制
   4. 修正发现的一些功能或程序错误
   5. DataGridView增加网址图片异步加载功能，优化DataGridView数据显示性能
 ## 2024/5/30
   1. (重要)修正多线程界面更新的invoker同步方法，Timer执行与UI同步
   2. ListBox、ListView、RichTextBox背景色修正
   3. 增强项目功能演示案例，添加变化滚动数据演示
 ## 2024/5/28
   1. 修正和增加了控件的一些方法
   2. 添加GTK多线程UI更新程序
 ## 2024/5/21
   1. combobox\listbox功能
   2. ToolStripSeparator修正
 ## 2024/5/17
   1. datetimepicker增加时间数据和format模式
 ## 2024/5/16
   1. 修正form窗口有透明边线的问题
   2. 增加了几个控件常用属性
   3. combobox控件增加DropDown或DropDownList可选模式
   4. 修改了VisualStudio开发插件的功能错误，提高了安装适配兼容性
 ## 2024/5/11
   1. 修正form启动时窗口大小异常
   2. button增加image属性图片
   3. 修正控件背景位置
 ## 2024/5/6
   1. 完善treeview、listview功能
   2. 新增开发工具GTKSystem.Windows.FormsDesigner.dll(NuGet安装)，编译时自动检查并修正窗体设计器配置
   3. 修正datagridview的取数错误
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
