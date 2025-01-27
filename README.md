# GTKSystem.Windows.Forms

### Introduction
**Native development in Visual Studio, no learning curve, compile once, and run cross-platform.**  
A C# desktop application cross-platform (Windows, Linux, macOS) UI development framework based on GTK components. The core advantage of this framework is the use of native WinForms controls and form designers in C#, with the same properties and methods. Native C# development is possible without requiring additional learning. Compile once, and run across platforms.  
This framework is designed to facilitate the development of cross-platform WinForms applications and enables C# to be upgraded to cross-platform software.

Project homepage: [https://www.gtkapp.com](https://www.gtkapp.com/)  

Currently, the functionality is being continuously updated, prioritizing commonly used features.

### Software Architecture
This framework uses GTK 3.24.24.95 to rewrite the UI of C#'s `System.Windows.Forms` component. It is compatible with native C# application components when applied.

### Installation Guide
1. Set the project framework to "Windows Application", modify the configuration `UseWindowsForms` to `false`, or choose "Console Application" with frameworks such as .NET Core 3.1 or .NET 6+.
2. Install the following NuGet packages: `GtkSharp(3.24.24.95)`, `GTKSystem.Windows.Forms`, and `GTKSystem.Windows.FormsDesigner`.
3. Check whether the form uses image resources. If used, create a `System.Resources.ResourceManager` and `System.ComponentModel.ComponentResourceManager` class. See details below.
4. Compile and test using the default configuration.
5. On Linux and macOS, run the command: `dotnet demo_app.dll`.
6. Compile the project and run the development plugin menu "Fix Form Designer" or manually create the `.designer.runtimeconfig.json` file in the `obj` directory (see point 5 below).

**Note:** After installing `GtkSharp`, when you compile your project, it will automatically download the GTK environment configuration `$(LOCALAPPDATA)\Gtk\3.24.24\gtk.zip`. Due to network restrictions in certain regions, this may fail to download.  
If automatic download fails, download it manually here: [/Dependencies/gtk-3.24.24.zip](/Dependencies/gtk-3.24.24.zip).  
Alternatively, download from [https://github.com/GtkSharp/Dependencies](https://github.com/GtkSharp/Dependencies), extract the files, and place them in the `$(LOCALAPPDATA)\Gtk\3.24.24\gtk.zip` directory.  
*(Note: $(LOCALAPPDATA) is the AppData\Local folder of your computer, e.g., C:\Users\chj\AppData\Local\Gtk\3.24.24)*

**Most desktop Linux operating systems already have GTK pre-installed. You only need to install the .NET SDK to run this framework.**

For Linux systems without a GTK environment, use the following commands to install:
```bash
# Debian/Ubuntu
sudo apt install libgtk-3-0      # Binary package
sudo apt install libgtk-3-dev    # Development package

# Arch
sudo apt install gtk3

# Fedora
sudo apt install gtk3            # Binary package
sudo apt install gtk3-devel      # Development package

# From MSYS2:
pacman -S mingw-w64-ucrt-x86_64-gtk3

# Check GTK environment (requires pkg-config):
pkg-config --cflags --libs gtk+-3.0
# Find GTK installation package directory:
ldconfig -p | grep gtk

## Install .NET Environment on Linux
To install .NET on Linux, refer to the official Microsoft guide:  
[Microsoft .NET Install Guide for Linux](https://learn.microsoft.com/dotnet/core/install/linux-scripted-manual)

---

## Visual Studio Plugin Installation

### Tool 1: NuGet Installation
Install the `GTKSystem.Windows.FormsDesigner` library from NuGet.  
This library helps fix form designers during project compilation.

### Tool 2: VSIX Plugin Installation
Download the plugin tool, close Visual Studio, and double-click the `GTKWinformVSIXProject.vsix` file to install it.  
*(Required if no Form template is available for projects in this framework.)*

The plugin installs two functionalities:
1. **Form Templates**: Adds templates for Forms and User Controls to "New Item" options.
2. **Right-Click Menu**: Adds a context menu to projects for specific GTKWinForms tasks.

![Visual Studio Plugin Screenshot](pic/vs_vsix.jpeg)

## Development Instructions

### Configuration Steps

1. **Create `System.Resources.ResourceManager` Class**
   - Create a `System.Resources.ResourceManager` class in your project.
   - Inherit from `GTKSystem.Resources.ResourceManager` to override the native `System.Resources.ResourceManager`.
   - This is used for reading project resource files and images.  
   *(If your project does not use resource files or images, this step is unnecessary.)*

2. **Create `System.ComponentModel.ComponentResourceManager` Class**
   - Create a `System.ComponentModel.ComponentResourceManager` class in your project.
   - Inherit from `GTKSystem.ComponentResourceManager` to override the native `System.ComponentModel.ComponentResourceManager`.
   - This class enables reading project resources and images (calls `GTKSystem.Resources.ResourceManager`).  
   *(If your project does not use resource files or images, this step is unnecessary.)*

3. **Modify `GTKWinFormsApp.csproj`**
   - Configure the project file by setting `UseWindowsForms` to `false`.  
   - Example configuration:
     ```xml
     <Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
       <PropertyGroup>
         <OutputType>WinExe</OutputType>
         <TargetFramework>net8.0</TargetFramework>
         <UseWindowsForms>false</UseWindowsForms>
     ```

4. **Add References**
   - Add the following references to your project:
     - `GTKSystem.Windows.Forms` (mandatory)
     - `System.Resources.Extensions` (optional, used only for specific design-time exceptions in Visual Studio).

5. **Configure Runtime Files**
   - Ensure the runtime configuration files include the following changes for Visual Studio form designers:  
     Example `GTKWinFormsApp.designer.runtimeconfig.json`:
     ```json
     {
       "runtimeOptions": {
         "tfm": "net8.0",
         "framework": {
           "name": "Microsoft.WindowsDesktop.App",
           "version": "8.0.0"
         }
       }
     }
     ```

---

## Support and Resources

**Enterprise Services**: [https://www.gtkapp.com/vipservice](https://www.gtkapp.com/vipservice)  

![Support GTKSystem](pic/love_reward_qrcode_.png)  
![Contact GTKSystem](pic/contact_weixin.png)

---

## Common Issues

**Why can't I open the Form Designer?**  
Follow these steps to resolve the issue:
1. Compile the project.
2. Open the Form Designer.
3. If it still doesn't open:
   - Close the Form Designer.
   - Recompile the project.
   - Restart Visual Studio.
   - Open the Form Designer again.

---

## Contribution
You can contribute to the development of this framework:

- [Gitee Repository](https://gitee.com/easywebfactory)  
- [GitHub Repository](https://github.com/easywebfactory)  
- [CSDN Blog](https://blog.csdn.net/auto_toyota)  

---

## Update History
For detailed updates, check: [Update History >>](UpdateHistory.md)

