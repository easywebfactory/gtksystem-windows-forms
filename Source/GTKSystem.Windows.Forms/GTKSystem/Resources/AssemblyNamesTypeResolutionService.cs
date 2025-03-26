﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;

namespace System.Resources;

internal class AssemblyNamesTypeResolutionService : ITypeResolutionService
{
    private AssemblyName[]? _names;
    private Hashtable? _cachedAssemblies;
    private Hashtable? _cachedTypes;

    private static readonly string dotNetPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles")??string.Empty, "dotnet\\shared");
    private static readonly string dotNetPathX86 = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")??string.Empty, "dotnet\\shared");

    internal AssemblyNamesTypeResolutionService(AssemblyName[]? names)
    {
        _names = names;
    }

    public Assembly? GetAssembly(AssemblyName name)
    {
        return GetAssembly(name, true);
    }

    // [UnconditionalSuppressMessage("SingleFile", "IL3002", Justification = "Handles single file case")]
    public Assembly? GetAssembly(AssemblyName name, bool throwOnError)
    {
        Assembly? result = null;

        if (_cachedAssemblies is null)
        {
            _cachedAssemblies = Hashtable.Synchronized(new Hashtable());
        }

        if (_cachedAssemblies.Contains(name))
        {
            result = _cachedAssemblies[name] as Assembly;
        }

        if (result is null)
        {
            result = Assembly.Load(name.FullName);
            if (result != null)
            {
                _cachedAssemblies[name] = result;
            }
            else if (_names != null)
            {
                foreach (var asmName in _names.Where(an => an.Equals(name)))
                {
                    try
                    {
                        result = Assembly.LoadFrom(GetPathOfAssembly(asmName));
                        if (result != null)
                        {
                            _cachedAssemblies[asmName] = result;
                        }
                    }
                    catch
                    {
                        if (throwOnError)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        return result;
    }

    //[UnconditionalSuppressMessage("SingleFile", "IL3002", Justification = "Returns null if in a single file")]
    public string GetPathOfAssembly(AssemblyName name)
    {
#pragma warning disable SYSLIB0044 // Type or member is obsolete. Ref https://github.com/dotnet/winforms/issues/7308
        return name.CodeBase;
#pragma warning restore SYSLIB0044 // Type or member is obsolete
    }

    public Type? GetType(string name)
    {
        return GetType(name, true);
    }

    public Type? GetType(string name, bool throwOnError)
    {
        return GetType(name, throwOnError, false);
    }

    public Type? GetType(string name, bool throwOnError, bool ignoreCase)
    {
        Type? result = null;

        // Check type cache first
        if (_cachedTypes is null)
        {
            _cachedTypes = Hashtable.Synchronized(new Hashtable(StringComparer.Ordinal));
        }

        if (_cachedTypes.Contains(name))
        {
            result = _cachedTypes[name] as Type;
            return result;
        }

        // Missed in cache, try to resolve the type from the reference assemblies.
        if (name.IndexOf(',') != -1)
        {
            result = Type.GetType(name, false, ignoreCase);
        }

        if (result is null && _names != null)
        {
            // If the type is assembly qualified name, we sort the assembly names
            // to put assemblies with same name in the front so that they can
            // be searched first.
            var pos = name.IndexOf(',');
            if (pos > 0 && pos < name.Length - 1)
            {
                var fullName = name.Substring(pos + 1).Trim();
                AssemblyName? assemblyName = null;
                try
                {
                    assemblyName = new AssemblyName(fullName);
                }
                catch(Exception ex)
                {
                    Trace.Write(ex);
                }

                if (assemblyName != null)
                {
                    List<AssemblyName> assemblyList = new(_names.Length);
                    foreach (var asmName in _names)
                    {
                        if (string.Equals(assemblyName.Name, asmName.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            assemblyList.Insert(0, asmName);
                        }
                        else
                        {
                            assemblyList.Add(asmName);
                        }
                    }

                    _names = assemblyList.ToArray();
                }
            }

            // Search each reference assembly
            foreach (var asmName in _names)
            {
                var asm = GetAssembly(asmName, false);
                if (asm != null)
                {
                    result = asm.GetType(name, false, ignoreCase);
                    if (result is null)
                    {
                        var indexOfComma = name.IndexOf(',');
                        if (indexOfComma != -1)
                        {
                            var shortName = name.Substring(0, indexOfComma);
                            result = asm.GetType(shortName, false, ignoreCase);
                        }
                    }
                }

                if (result != null)
                {
                    break;
                }
            }
        }

        if (result is null && throwOnError)
        {
            throw new ArgumentException(string.Format("SR.InvalidResXNoType,{0}", name));
        }

        if (result != null)
        {
            // Only cache types from the shared framework  because they don't need to update.
            // For simplicity, don't cache custom types
            if (IsDotNetAssembly(result.Assembly.Location))
            {
                _cachedTypes[name] = result;
            }
        }

        return result;
    }

    /// <summary>
    ///  This is matching %windir%\Microsoft.NET\Framework*, so both 32bit and 64bit framework will be covered.
    /// </summary>
    private static bool IsDotNetAssembly(string assemblyPath)
    {
        return assemblyPath != null && (assemblyPath.StartsWith(dotNetPath, StringComparison.OrdinalIgnoreCase) || assemblyPath.StartsWith(dotNetPathX86, StringComparison.OrdinalIgnoreCase));
    }

    public void ReferenceAssembly(AssemblyName name)
    {
        throw new NotSupportedException();
    }
}