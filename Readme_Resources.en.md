## 1. Adding a Resources Folder for Stored Assets

This folder is not mandatory. It is only required when relevant resources are used. For details on how to use the "Global Shared Resources (Properties/Resources.resx)" and "Form-Exclusive Resources (Form.resx)", refer to the instructions below.

In general, individual image resources can be directly embedded without additional configuration. However, for image lists, the images must be stored in the `Resources` folder.

### How to Create:
Create a `Resources` folder under the project and its build output directory. Copy the resource images and `Form.resx` files to the `Resources` folder. These files and directories should be entirely included in the project's build output directory.

---

## 2. Using Global Shared Resources (Properties/Resources.resx)

### Create a `System.Resources.ResourceManager` Class
Create a `System.Resources.ResourceManager` class in your project, inheriting from `GTKSystem.Resources.ResourceManager` to override the native `System.Resources.ResourceManager` class.  
`GTKSystem.Resources.ResourceManager` provides functionality to read project resource files and image files.  
If your project does not use resource image files, you do not need to create this file.

---

### Create a `System.ComponentModel.ComponentResourceManager` Class
Create a `System.ComponentModel.ComponentResourceManager` class in your project, inheriting from `GTKSystem.ComponentResourceManager` to override the native `System.ComponentModel.ComponentResourceManager` class.  
`GTKSystem.ComponentModel.ComponentResourceManager` provides functionality to read project resource files and image files (calling `GTKSystem.Resources.ResourceManager`).  
If your project does not use resource image files, you do not need to create this file.

---

## 3. Using Form-Exclusive Resources (Form.resx)

### Create a `System.ComponentModel.ComponentResourceManager` Class
Create a `System.ComponentModel.ComponentResourceManager` class in your project, inheriting from `GTKSystem.ComponentResourceManager` to override the native `System.ComponentModel.ComponentResourceManager` class.  
`GTKSystem.ComponentResourceManager` provides functionality to read project resource files and image files (calling `GTKSystem.Resources.ResourceManager`).  
If your project does not use resource image files, you do not need to create this file.

---

### Using Image List Resources
Since `GTKSystem` cannot directly read image lists (`ImageList`), the images in an image list must be stored in the project's `Resources` folder. For example:

```csharp
// Configuration program for Form2.Designer.cs:
imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
imageList1.TransparentColor = System.Drawing.Color.Transparent;
imageList1.Images.SetKeyName(0, "010.jpg");
imageList1.Images.SetKeyName(1, "timg2.jpg");

// Therefore, you need to copy the images 010.jpg and timg2.jpg into the Resources folder or Resources/imageList1 folder.
