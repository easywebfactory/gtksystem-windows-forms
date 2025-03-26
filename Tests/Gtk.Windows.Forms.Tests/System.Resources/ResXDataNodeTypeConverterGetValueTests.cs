//
// ResXDataNodeTypeConverterGetValueTests.cs
// 
// Author:
//	Gary Barnett (gary.barnett.mono@gmail.com)
// 
// Copyright (C) Gary Barnett (2012)
//
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

using System.Resources;
using System.ComponentModel.Design;
using System.Reflection;
using System.Drawing;
using System.Resources;
using GtkTests.TypeResolutionService_;
using GtkTests.Resources;

namespace GtkTests.System.Resources;

[TestFixture]
public class ResXDataNodeTypeConverterGetValueTests : ResourcesTestHelper {
    [Test]
    public void ITRSNotUsedWhenCreatedNew ()
    {
        ResXDataNode node;
        node = new ResXDataNode ("along", 34L);

        var obj = node.GetValue (new ReturnIntITRS ());
        Assert.True(typeof(long) == obj.GetType(), "#A1");
    }

    [Test]
    public void ITRSUsedEachTimeWithNodeFromReader ()
    {
        ResXDataNode returnedNode, originalNode;
        originalNode = new ResXDataNode ("aNumber", 23L);
        returnedNode = GetNodeFromResXReader (originalNode);

        Assert.IsNotNull (returnedNode, "#A1");

        var newVal = returnedNode.GetValue (new ReturnIntITRS ());
        Assert.AreEqual (typeof (int).AssemblyQualifiedName, newVal.GetType ().AssemblyQualifiedName, "#A2");

        var origVal = returnedNode.GetValue ((ITypeResolutionService) null);
        Assert.AreEqual (typeof (long).AssemblyQualifiedName, origVal.GetType ().AssemblyQualifiedName, "#A3");
    }

}