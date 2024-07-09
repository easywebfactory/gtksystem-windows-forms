// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Drawing.Printing
{
    public partial class PaperSource
    {
        private string _name;
        private PaperSourceKind _kind;

        public PaperSource()
        {
            _kind = PaperSourceKind.Custom;
            _name = string.Empty;
        }

        internal PaperSource(PaperSourceKind kind, string name)
        {
            _kind = kind;
            _name = name;
        }

        public PaperSourceKind Kind => _kind;

        public int RawKind
        {
            get => (int)_kind;
            set => _kind = (PaperSourceKind)value;
        }

        public string SourceName
        {
            get => _name;
            set => _name = value;
        }

        public override string ToString() => $"[PaperSource {SourceName} Kind={Kind}]";
    }
}