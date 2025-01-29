﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Drawing.Printing
{
    /// <summary>
    ///  Specifies the margins of a printed page.
    /// </summary>
    [TypeConverter(typeof(MarginsConverter))]
    public partial class Margins : ICloneable
    {
        private int _left;
        private int _right;
        private int _bottom;
        private int _top;

        [OptionalField] private double _doubleLeft;

        [OptionalField] private double _doubleRight;

        [OptionalField] private double _doubleTop;

        [OptionalField] private double _doubleBottom;

        /// <summary>
        ///  Initializes a new instance of a the <see cref='Margins'/> class with one-inch margins.
        /// </summary>
        public Margins() : this(100, 100, 100, 100)
        {
        }

        /// <summary>
        ///  Initializes a new instance of a the <see cref='Margins'/> class with the specified left, right, top, and bottom margins.
        /// </summary>
        public Margins(int left, int right, int top, int bottom)
        {
            CheckMargin(left, nameof(left));
            CheckMargin(right, nameof(right));
            CheckMargin(top, nameof(top));
            CheckMargin(bottom, nameof(bottom));

            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;

            _doubleLeft = left;
            _doubleRight = right;
            _doubleTop = top;
            _doubleBottom = bottom;
        }

        /// <summary>
        ///  Gets or sets the left margin, in hundredths of an inch.
        /// </summary>
        public int Left
        {
            get => _left;
            set
            {
                CheckMargin(value, nameof(value));
                _left = value;
                _doubleLeft = value;
            }
        }

        /// <summary>
        ///  Gets or sets the right margin, in hundredths of an inch.
        /// </summary>
        public int Right
        {
            get => _right;
            set
            {
                CheckMargin(value, nameof(value));
                _right = value;
                _doubleRight = value;
            }
        }

        /// <summary>
        ///  Gets or sets the top margin, in hundredths of an inch.
        /// </summary>
        public int Top
        {
            get => _top;
            set
            {
                CheckMargin(value, nameof(value));
                _top = value;
                _doubleTop = value;
            }
        }

        /// <summary>
        ///  Gets or sets the bottom margin, in hundredths of an inch.
        /// </summary>
        public int Bottom
        {
            get => _bottom;
            set
            {
                CheckMargin(value, nameof(value));
                _bottom = value;
                _doubleBottom = value;
            }
        }

        /// <summary>
        ///  Gets or sets the left margin with a double value, in hundredths of an inch.
        /// </summary>
        internal double DoubleLeft
        {
            get => _doubleLeft;
            set
            {
                Left = (int)Math.Round(value);
                _doubleLeft = value;
            }
        }

        /// <summary>
        ///  Gets or sets the right margin with a double value, in hundredths of an inch.
        /// </summary>
        internal double DoubleRight
        {
            get => _doubleRight;
            set
            {
                Right = (int)Math.Round(value);
                _doubleRight = value;
            }
        }

        /// <summary>
        ///  Gets or sets the top margin with a double value, in hundredths of an inch.
        /// </summary>
        internal double DoubleTop
        {
            get => _doubleTop;
            set
            {
                Top = (int)Math.Round(value);
                _doubleTop = value;
            }
        }

        /// <summary>
        ///  Gets or sets the bottom margin with double value, in hundredths of an inch.
        /// </summary>
        internal double DoubleBottom
        {
            get => _doubleBottom;
            set
            {
                Bottom = (int)Math.Round(value);
                _doubleBottom = value;
            }
        }

        private static void CheckMargin(int margin, string name)
        {
            if (margin < 0)
            {
                throw new ArgumentOutOfRangeException(name, margin, "InvalidLowBoundArgumentEx");
            }
        }

        public object Clone() => MemberwiseClone();

        public override bool Equals(object obj) =>
            obj is Margins margins
            && margins.Left == Left
            && margins.Right == Right
            && margins.Top == Top
            && margins.Bottom == Bottom;

        /// <summary>
        ///  Calculates and retrieves a hash code based on the left, right, top, and bottom margins.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(Left, Right, Top, Bottom);

        /// <summary>
        ///  Tests whether two <see cref='Margins'/> objects are identical.
        /// </summary>
        public static bool operator ==(Margins m1, Margins m2)
        {
            if (m1 is null)
            {
                return m2 is null;
            }

            if (m2 is null)
            {
                return false;
            }

            return m1.Equals(m2);
        }

        /// <summary>
        ///  Tests whether two <see cref='Margins'/> objects are different.
        /// </summary>
        public static bool operator !=(Margins m1, Margins m2) => !(m1 == m2);

        /// <summary>
        ///  Provides some interesting information for the Margins in String form.
        /// </summary>
        public override string ToString() => $"[Margins Left={Left} Right={Right} Top={Top} Bottom={Bottom}]";
    }
}