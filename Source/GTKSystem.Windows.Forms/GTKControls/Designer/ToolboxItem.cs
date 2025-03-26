// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace System.Drawing.Design;

/// <summary>
///  Provides a base implementation of a toolbox item.
/// </summary>
public class ToolboxItem : ISerializable
{
    public Type? ToolType { get; }

    private const int iconDimension = 16;


    /// <summary>
    ///  Initializes a new instance of the ToolboxItem class.
    /// </summary>
    public ToolboxItem()
    {

    }

    /// <summary>
    ///  Initializes a new instance of the ToolboxItem class using the specified type.
    /// </summary>
    public ToolboxItem(Type? toolType) : this()
    {
        ToolType = toolType;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {

    }
}