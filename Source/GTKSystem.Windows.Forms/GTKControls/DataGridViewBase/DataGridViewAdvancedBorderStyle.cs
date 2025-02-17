// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

public sealed class DataGridViewAdvancedBorderStyle : ICloneable
{
    private readonly DataGridView? _owner;
    private bool _all = true;
    private readonly DataGridViewAdvancedCellBorderStyle _banned1;
    private readonly DataGridViewAdvancedCellBorderStyle _banned2;
    private readonly DataGridViewAdvancedCellBorderStyle _banned3;
    private DataGridViewAdvancedCellBorderStyle _top = DataGridViewAdvancedCellBorderStyle.None;
    private DataGridViewAdvancedCellBorderStyle _left = DataGridViewAdvancedCellBorderStyle.None;
    private DataGridViewAdvancedCellBorderStyle _right = DataGridViewAdvancedCellBorderStyle.None;
    private DataGridViewAdvancedCellBorderStyle _bottom = DataGridViewAdvancedCellBorderStyle.None;

    public DataGridViewAdvancedBorderStyle()
        : this(
            null,
            DataGridViewAdvancedCellBorderStyle.NotSet,
            DataGridViewAdvancedCellBorderStyle.NotSet,
            DataGridViewAdvancedCellBorderStyle.NotSet)
    {
    }

    internal DataGridViewAdvancedBorderStyle(DataGridView owner)
        : this(
            owner,
            DataGridViewAdvancedCellBorderStyle.NotSet,
            DataGridViewAdvancedCellBorderStyle.NotSet,
            DataGridViewAdvancedCellBorderStyle.NotSet)
    {
    }

    /// <summary>
    ///  Creates a new DataGridViewAdvancedBorderStyle. The specified owner will
    ///  be notified when the values are changed.
    /// </summary>
    internal DataGridViewAdvancedBorderStyle(
        DataGridView? owner,
        DataGridViewAdvancedCellBorderStyle banned1,
        DataGridViewAdvancedCellBorderStyle banned2,
        DataGridViewAdvancedCellBorderStyle banned3)
    {
        _owner = owner;
        _banned1 = banned1;
        _banned2 = banned2;
        _banned3 = banned3;
    }

    public DataGridViewAdvancedCellBorderStyle All
    {
        get
        {
            return _all ? _top : DataGridViewAdvancedCellBorderStyle.NotSet;
        }
        set
        {

        }
    }

    public DataGridViewAdvancedCellBorderStyle Bottom
    {
        get
        {
            if (_all)
            {
                return _top;
            }

            return _bottom;
        }
        set
        {

        }
    }

    internal DataGridViewAdvancedCellBorderStyle BottomInternal
    {
        set
        {

        }
    }

    public DataGridViewAdvancedCellBorderStyle Left
    {
        get
        {
            if (_all)
            {
                return _top;
            }

            return _left;
        }
        set
        {

        }
    }

    internal DataGridViewAdvancedCellBorderStyle LeftInternal
    {
        set
        {

        }
    }

    public DataGridViewAdvancedCellBorderStyle Right
    {
        get
        {
            if (_all)
            {
                return _top;
            }

            return _right;
        }
        set
        {

        }
    }

    internal DataGridViewAdvancedCellBorderStyle RightInternal
    {
        set
        {
 
        }
    }

    public DataGridViewAdvancedCellBorderStyle Top
    {
        get
        {
            return _top;
        }
        set
        {

        }
    }

    internal DataGridViewAdvancedCellBorderStyle TopInternal
    {
        set
        {

        }
    }

    public override bool Equals(object? other)
    {
        if (other is not DataGridViewAdvancedBorderStyle dgvabsOther)
        {
            return false;
        }

        return dgvabsOther._all == _all &&
            dgvabsOther._top == _top &&
            dgvabsOther._left == _left &&
            dgvabsOther._bottom == _bottom &&
            dgvabsOther._right == _right;
    }

    public override int GetHashCode() => HashCode.Combine(_top, _left, _bottom, _right);

    public override string ToString()
    {
        return $"DataGridViewAdvancedBorderStyle {{ All={All}, Left={Left}, Right={Right}, Top={Top}, Bottom={Bottom} }}";
    }

    object ICloneable.Clone()
    {
        DataGridViewAdvancedBorderStyle dgvabs = new(_owner, _banned1, _banned2, _banned3)
        {
            _all = _all,
            _top = _top,
            _right = _right,
            _bottom = _bottom,
            _left = _left
        };
        return dgvabs;
    }
}
