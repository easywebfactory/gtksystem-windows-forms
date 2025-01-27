// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
    /// <summary>
    ///  Provides a designer that can design components that extend Control.
    /// </summary>
    public partial class ControlDesigner : ComponentDesigner
    {
#pragma warning disable IDE1006 // Naming Styles - Public API
        protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);
#pragma warning restore IDE1006

        private static uint s_currentProcessId;
        private IDesignerHost _host;                        // the host for our designer

        private bool _liveRegion;                           // is the mouse is over a live region of the control?
        private bool _inHitTest;                            // A popular way to implement GetHitTest is by WM_NCHITTEST
                                                            //  ...which would cause a cycle.
        private bool _hasLocation;                          // Do we have a location property?
        private bool _locationChecked;                      // And did we check it
        private bool _locked;                               // Signifies if this control is locked or not
        private bool _enabledchangerecursionguard;


        // Transient values that are used during mouse drags
        private Point _mouseDragLast = InvalidPoint;        // the last position of the mouse during a drag.
        private bool _mouseDragMoved;                       // has the mouse been moved during this drag?
        private int _lastMoveScreenX;
        private int _lastMoveScreenY;

        // Values used to simulate double clicks for controls that don't support them.
        private uint _lastClickMessageTime;
        private int _lastClickMessagePositionX;
        private int _lastClickMessagePositionY;

        private Point _downPos = Point.Empty;               // point used to track first down of a double click
        private event EventHandler DisposingHandler;
        private CollectionChangeEventHandler _dataBindingsCollectionChanged;
        private Exception _thrownException;

        private bool _ctrlSelect;                           // if the CTRL key was down at the mouse down
        private bool _toolPassThrough;                      // a tool is selected, allow the parent to draw a rect for it.
        private bool _removalNotificationHooked;
        private bool _revokeDragDrop = true;
        private bool _hadDragDrop;

        private static bool s_inContextMenu;

        internal bool ForceVisible { get; set; } = true;

    }
}