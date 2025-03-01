// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;


namespace System.Windows.Forms.Design
{
    /// <summary>
    ///  Provides a designer that extends the ScrollableControlDesigner and implements
    ///  IRootDesigner.
    /// </summary>
    [ToolboxItemFilter("System.Windows.Forms")]
    public partial class DocumentDesigner : ScrollableControlDesigner, IRootDesigner, IToolboxUser, IOleDragClient
    {

        private bool initializing;   // is the designer initializing?

        // used to keep the state of the tab order view
        //
        private bool queriedTabOrder;
        private MenuCommand tabOrderCommand;

        internal static IDesignerSerializationManager manager;


        private int trayHeight = 80;
        private bool trayLargeIcon;
        private bool trayAutoArrange;
        private bool trayLayoutSuspended;

        // ActiveX support
        //
        private static readonly Guid htmlDesignTime = new Guid("73CEF3DD-AE85-11CF-A406-00AA00C00940");

        private const string AxClipFormat = "CLSID";

        public ViewTechnology[] SupportedTechnologies => throw new NotImplementedException();

        public object GetView(ViewTechnology technology)
        {
            throw new NotImplementedException();
        }

        public bool GetToolSupported(ToolboxItem tool)
        {
            throw new NotImplementedException();
        }

        public void ToolPicked(ToolboxItem tool)
        {
            throw new NotImplementedException();
        }
    }
}