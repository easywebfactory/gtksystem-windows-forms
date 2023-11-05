// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GLib;
//using Gtk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{

    public class ToolStripSeparator : ToolStripItem
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr d_gtk_separator_menu_item_new();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr d_gtk_separator_menu_item_get_type();

        private static d_gtk_separator_menu_item_new gtk_separator_menu_item_new = FuncLoader.LoadFunction<d_gtk_separator_menu_item_new>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_separator_menu_item_new"));

        private static AbiStruct _class_abi = null;

        private static d_gtk_separator_menu_item_get_type gtk_separator_menu_item_get_type = FuncLoader.LoadFunction<d_gtk_separator_menu_item_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_separator_menu_item_get_type"));

        private static AbiStruct _abi_info = null;

        public new static AbiStruct class_abi
        {
            get
            {
                if (_class_abi == null)
                {
                    _class_abi = new AbiStruct(new List<AbiField>
                    {
                        new AbiField("_gtk_reserved1", MenuItem.class_abi.Fields, (uint)Marshal.SizeOf(typeof(IntPtr)), null, "_gtk_reserved2", (uint)Marshal.SizeOf(typeof(IntPtr)), 0u),
                        new AbiField("_gtk_reserved2", -1L, (uint)Marshal.SizeOf(typeof(IntPtr)), "_gtk_reserved1", "_gtk_reserved3", (uint)Marshal.SizeOf(typeof(IntPtr)), 0u),
                        new AbiField("_gtk_reserved3", -1L, (uint)Marshal.SizeOf(typeof(IntPtr)), "_gtk_reserved2", "_gtk_reserved4", (uint)Marshal.SizeOf(typeof(IntPtr)), 0u),
                        new AbiField("_gtk_reserved4", -1L, (uint)Marshal.SizeOf(typeof(IntPtr)), "_gtk_reserved3", null, (uint)Marshal.SizeOf(typeof(IntPtr)), 0u)
                    });
                }

                return _class_abi;
            }
        }

        public new static GType GType
        {
            get
            {
                IntPtr val = gtk_separator_menu_item_get_type();
                return new GType(val);
            }
        }

        public new static AbiStruct abi_info
        {
            get
            {
                if (_abi_info == null)
                {
                    _abi_info = new AbiStruct(MenuItem.abi_info.Fields);
                }

                return _abi_info;
            }
        }

        public ToolStripSeparator(IntPtr raw)
            : base(raw)
        {
        }

        public ToolStripSeparator()
            : base(IntPtr.Zero)
        {
            if (GetType() != typeof(ToolStripSeparator))
            {
                CreateNativeObject(new string[0], new Value[0]);
            }
            else
            {
                Raw = gtk_separator_menu_item_new();
            }
        }

        public override string Text { get; set; }
    }
}
