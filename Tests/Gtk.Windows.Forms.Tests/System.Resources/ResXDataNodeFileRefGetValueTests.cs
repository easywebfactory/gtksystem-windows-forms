//
// ResXDataNodeFileRefGetValueTests.cs
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

using System.Reflection;
using System.Drawing;
using System.Resources;
using System.ComponentModel.Design;
using System.Resources;
using GtkTests.TypeResolutionService_;
using GtkTests.Resources;

namespace GtkTests.System.Resources;

[TestFixture]
public class ResXDataNodeFileRefGetValueTests : ResourcesTestHelper
{
    [Test]
    public void CantGetValueWithOnlyFullNameAsType()
    {
        Assert.Throws<TypeLoadException>(() =>
        {
            ResXDataNode originalNode, returnedNode;
            originalNode = GetNodeFileRefToSerializable("ser.bbb", false);
            returnedNode = GetNodeFromResXReader(originalNode);

            Assert.IsNotNull(returnedNode, "#A1");
            var obj = returnedNode.GetValue((AssemblyName[])null);
        });
    }

    [Test]
    public void LoadingFileFails()
    {
        Assert.Throws<TargetInvocationException>(() =>
        {
            var corruptFile = Path.GetTempFileName();
            var fileRef = new ResXFileRef(corruptFile, typeof(serializable).AssemblyQualifiedName);

            File.AppendAllText(corruptFile, "corrupt");
            var node = new ResXDataNode("aname", fileRef);
            node.GetValue((AssemblyName[])null);
        });
    }

}