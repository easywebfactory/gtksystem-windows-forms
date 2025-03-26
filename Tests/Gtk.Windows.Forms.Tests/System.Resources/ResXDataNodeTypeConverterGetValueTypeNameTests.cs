//
// ResXDataNodeTypeConverterGetValueTypeNameTests.cs
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
using System.Drawing;
using System.Reflection;
using System.Resources;
using GtkTests.Resources;
using GtkTests.TypeResolutionService_;

namespace GtkTests.System.Resources;

[TestFixture]
public class ResXDataNodeTypeConverterGetValueTypeNameTests : ResourcesTestHelper {
    [Test]
    public void ITRSUsedWithNodeFromReader ()
    {
        ResXDataNode returnedNode, originalNode;
        originalNode = new ResXDataNode ("aNumber", 23L);
        returnedNode = GetNodeFromResXReader (originalNode);

        Assert.IsNotNull (returnedNode, "#A1");
        var returnedType = returnedNode.GetValueTypeName (new ReturnIntITRS ());
        Assert.AreEqual ((typeof (Int32)).AssemblyQualifiedName, returnedType, "#A2");
    }

    [Test]
    public void ITRSUsedEachTimeWhenNodeFromReader ()
    {
        ResXDataNode returnedNode, originalNode;
        originalNode = new ResXDataNode ("aNumber", 23L);
        returnedNode = GetNodeFromResXReader (originalNode);

        Assert.IsNotNull (returnedNode, "#A1");
        var newType = returnedNode.GetValueTypeName (new ReturnIntITRS ());
        Assert.AreEqual (typeof (int).AssemblyQualifiedName, newType, "#A2");
        var origType = returnedNode.GetValueTypeName ((ITypeResolutionService) null);
        Assert.AreEqual (typeof (long).AssemblyQualifiedName, origType, "#A3");				
    }

    [Test]
    public void ITRSNotUsedWhenNodeCreatedNew ()
    {
        ResXDataNode node;
        node = new ResXDataNode ("along", 34L);

        var returnedType = node.GetValueTypeName (new ReturnIntITRS ());
        Assert.AreEqual ((typeof (long)).AssemblyQualifiedName, returnedType, "#A1");
    }

}