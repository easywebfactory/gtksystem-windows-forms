// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

namespace System.Resources;

internal class DataNodeInfo
{
    public string? name;
    public string? comment;
    public string? typeName;
    public string? mimeType;
    public string? valueData;
    public Point readerPosition; //only used to track position in the reader

    internal DataNodeInfo Clone()
    {
        return new DataNodeInfo
        {
            name = name,
            comment = comment,
            typeName = typeName,
            mimeType = mimeType,
            valueData = valueData,
            readerPosition = new Point(readerPosition.X, readerPosition.Y)
        };
    }
}