using System.Reflection;

namespace GTKSystem.Windows.Forms.Resources
{
    public class AssemblyResources
    {
        public static Assembly CurrentAssembly=> Assembly.GetExecutingAssembly();
        public static string ToSystemUri(string filename) => $"GTKSystem.Windows.Forms.Resources.System.{filename}";
        public static string ToCursorsUri(string filename) => $"GTKSystem.Windows.Forms.Resources.Cursors.{filename}";
        public static string ToResourcesUri(string subdirctory, string filename) => $"GTKSystem.Windows.Forms.Resources.{subdirctory}.{filename}";
    }
}
