// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace System.Drawing.Design
{
    /// <summary>
    ///  Provides a base implementation of a toolbox item.
    /// </summary>
    public class ToolboxItem : ISerializable
    {

        private static bool s_isScalingInitialized;
        private const int ICON_DIMENSION = 16;
        private static int s_iconWidth = ICON_DIMENSION;
        private static int s_iconHeight = ICON_DIMENSION;


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

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
