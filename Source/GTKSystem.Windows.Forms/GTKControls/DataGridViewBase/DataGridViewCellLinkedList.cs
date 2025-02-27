// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;

namespace System.Windows.Forms;

/// <summary>
///  Represents a linked list of <see cref="DataGridViewCell"/> objects
/// </summary>
internal class DataGridViewCellLinkedList : IEnumerable
{
    public DataGridViewCell? this[int index] => null;

    public int Count => 0;

    public DataGridViewCell? HeadCell => null;

    public void Add(DataGridViewCell dataGridViewCell)
    {
            
    }

    public void Clear()
    {
 
    }

    public bool Contains(DataGridViewCell dataGridViewCell)
    {
 
        return false;
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool Remove(DataGridViewCell dataGridViewCell)
    {
 
        return false;
    }

    public int RemoveAllCellsAtBand(bool column, int bandIndex)
    {
        var removedCount = 0;
          
        return removedCount;
    }
}