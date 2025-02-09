//
// TestResourceHelper.cs
//
// Author:
//	Alexander Köplinger (alkpli@microsoft.com)
//
// Copyright (C) Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Diagnostics;
using System.Reflection;

namespace GtkTests.Helpers;

public static class TestResourceHelper
{
    static readonly string tempFolder;
    static readonly Assembly currentAssembly;

    static TestResourceHelper()
    {
        // create temp directory for extracting all the test resources to disk
        tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempFolder);

        currentAssembly = Assembly.GetExecutingAssembly();
        foreach (var resourceName in currentAssembly.GetManifestResourceNames())
        {
            // skip non-test assets
            if (!resourceName.StartsWith("GtkTests."))
                continue;

            // persist the resource to disk
            var stream = currentAssembly.GetManifestResourceStream(resourceName)!;
            var resourcePath = Path.Combine(tempFolder, Path.ChangeExtension(resourceName, ".resx"));
            Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            File.WriteAllText(resourcePath, content);
        }

        // delete the temp directory at the end of the test process
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            try
            {
                Directory.Delete(tempFolder, true);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
            }
        };
    }

    public static string GetFullPathOfResource(string resourceName)
    {
        return Path.Combine(tempFolder, resourceName);
    }

    public static Stream GetStreamOfResource(string resourceName)
    {
        return currentAssembly.GetManifestResourceStream(resourceName)!;
    }
}