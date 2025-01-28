## 一、增加存放资源文件夹Resources
此目录不是必须的，只有在使用了相关资源才可能需要，具体请看下面的【全局共享资源Properties/Resources.resx】和【窗体独占资源Form.resx】的使用方法。
一般情况下，单个图片资源可以直接兼容原生使用，可以无需额外配置，图片组资源则必须把图片存放到Resources文件夹下。

#### 创建方法如下：
在项目下和编译输出目录下创建Resources文件夹，把Resources资源存放的图片、Form.resx文件复制到Resources文件夹，此文件夹和文件全部生成到工程项目编译输出目录下。


## 二、使用全局共享资源Properties/Resources.resx
### 新建System.Resources.ResourceManager类<br/>
在项目下新建System.Resources.ResourceManager类，继承GTKSystem.Resources.ResourceManager，用于覆盖原生System.Resources.ResourceManager类。
GTKSystem.Resources.ResourceManager实现了项目资源文件和图像文件读取。
如果项目里没有使用资源图像文件，可以不用新建此文件。

### 新建System.ComponentModel.ComponentResourceManager类<br/>
在项目下新建System.ComponentModel.ComponentResourceManager类，继承GTKSystem.ComponentModel.ComponentResourceManager，用于覆盖原生System.ComponentModel.ComponentResourceManager类。<br/>
GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取（调用GTKSystem.Resources.ResourceManager）。
如果项目里没有使用资源图像文件，可以不用新建此文件。

## 三、使用窗体独占资源Form.resx
### 新建System.ComponentModel.ComponentResourceManager类<br/>
在项目下新建System.ComponentModel.ComponentResourceManager类，继承GTKSystem.ComponentModel.ComponentResourceManager，用于覆盖原生System.ComponentModel.ComponentResourceManager类。<br/>
GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取（调用GTKSystem.Resources.ResourceManager）。
如果项目里没有使用资源图像文件，可以不用新建此文件。

### 图片组资源的使用
由于GTKSystem无法读取图片组(ImageList)，需要把图片组的图片存入到项目的Resources文件夹下，如：
```
Form2.Designer.cs的配置程序如下：
 imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
 imageList1.TransparentColor = System.Drawing.Color.Transparent;
 imageList1.Images.SetKeyName(0, "010.jpg");
 imageList1.Images.SetKeyName(1, "timg2.jpg");

那么需要把图片010.jpg和timg2.jpg复制到文件夹Resources或Resources/imageList1。
```


