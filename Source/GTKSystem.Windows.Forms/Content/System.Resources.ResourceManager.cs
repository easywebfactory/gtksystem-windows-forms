// This file is mainly to overwrite the native System.Resources.ResourceManager class.
// The native class does not support reading project resource image files.
// GTKSystem.Resources.ResourceManager implements reading of project resource files and image files.
// If the resource image file is not used in the project, there is no need to create this file.

using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace System.Resources;

public class ResourceManager : System.Resources.GtkResourceManager
{
    public ResourceManager(System.Type resourceSource) : base(null, null, resourceSource)
    {

    }

    public ResourceManager(string? baseName, Assembly? assemblyWithResources) : base(baseName, assemblyWithResources, null)
    {

    }

    public ResourceManager(string? baseName, Assembly? assemblyWithResources, System.Type resourceSource) : base(baseName,
        assemblyWithResources, resourceSource)
    {

    }

    protected ResourceManager()
    {

    }
}