// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
//
// Author:
//	Rolf Bjarne Kvinge  (RKvinge@novell.com)
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

public static class DataGridViewCommon
{
    /// <summary>
    /// Creates a 2x2 grid.
    /// <para>     A   B </para>
    /// <para>1   A1  B1</para>
    /// <para>2   A2  B2</para>
    /// </summary>
    /// <returns></returns>
    public static DataGridView CreateAndFill ()
    {
        var dgv = new DataGridView ();
        dgv.Columns.Add ("A", "A");
        dgv.Columns.Add ("B", "B");
        dgv.Rows.Add ("Cell A1", "Cell B1");
        dgv.Rows.Add ("Cell A2", "Cell B2");
        return dgv;
    }

    /// <summary>
    /// Creates a 10x10 grid.
    /// <para>     A   B </para>
    /// <para>1   A1  B1</para>
    /// <para>2   A2  B2</para>
    /// </summary>
    /// <returns></returns>
    public static DataGridView CreateAndFillBig ()
    {
        var dgv = new DataGridView ();
        for (var c = 0; c < 10; c++) {
            var A = (((char) ((int) 'A') + c)).ToString ();
            dgv.Columns.Add (A, A);
        }
        for (var r = 0; r < 10; r++) {
            List<object> cells = new();
            for (var c = 0; c < 10; c++) {
                cells.Add (string.Format ("Cell {0}{1}", dgv.Columns [c].Name, r));
            }
            dgv.Rows.Add (cells);
        }
        return dgv;
    }
}