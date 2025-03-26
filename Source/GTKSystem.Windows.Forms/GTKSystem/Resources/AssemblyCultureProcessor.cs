using System.Reflection;

namespace System.Resources;

internal static class AssemblyCultureProcessor
{
    public static (string Culture, Assembly Assembly)[] GetAssembliesWithResources(this Assembly assemblyWithResources,
        string cultureName)
    {
        var assemblyWithResourcesLocation = Path.GetFullPath(assemblyWithResources.Location);
        var assemblyWithResourcesDirectory = Path.GetDirectoryName(assemblyWithResourcesLocation)!;
        var assemblies = AddCultureNeutralAssembly(assemblyWithResources);
        var insertPoint = AddSpecificCultureAssembly(cultureName, assemblyWithResourcesLocation, assemblyWithResourcesDirectory, assemblies);
        AddGenericCultureAssembly(cultureName, assemblyWithResourcesDirectory, assemblyWithResourcesLocation, assemblies, insertPoint);
        var valueTuples = assemblies.ToArray();
        return valueTuples;
    }

    private static List<(string Culture, Assembly Assembly)> AddCultureNeutralAssembly(Assembly assemblyWithResources)
    {
        return new List<(string Culture, Assembly Assembly)> { (string.Empty, assemblyWithResources) };
    }

    private static void AddGenericCultureAssembly(string cultureName, string assemblyWithResourcesDirectory,
        string assemblyWithResourcesLocation, List<(string Culture, Assembly Assembly)> assemblies, int insertPoint)
    {
        var name = cultureName.Split('-').FirstOrDefault();
        if (!string.IsNullOrEmpty(name))
        {
            var combine = Path.Combine(assemblyWithResourcesDirectory, name, $"{Path.GetFileNameWithoutExtension(assemblyWithResourcesLocation)}{GtkResourceManager.resFileExtension}{Path.GetExtension(assemblyWithResourcesLocation)}");
            if (File.Exists(combine))
            {
                assemblies.Insert(insertPoint, (name, Assembly.LoadFile(combine)));
            }
        }
    }

    private static int AddSpecificCultureAssembly(string cultureName, string assemblyWithResourcesLocation,
        string assemblyWithResourcesDirectory, List<(string Culture, Assembly Assembly)> assemblies)
    {
        var resourceAssemblyName = $"{Path.GetFileNameWithoutExtension(assemblyWithResourcesLocation)}{GtkResourceManager.resFileExtension}{Path.GetExtension(assemblyWithResourcesLocation)}";
        var combine = Path.Combine(assemblyWithResourcesDirectory, cultureName, resourceAssemblyName);
        var insertPoint = 0;
        if (File.Exists(combine))
        {
            assemblies.Insert(insertPoint, (cultureName, Assembly.LoadFile(combine)));
            insertPoint++;
        }

        return insertPoint;
    }
}