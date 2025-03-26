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
// ResXFileRefTest.cs: Unit Tests for ResXFileRef.
//
// Authors:
//		Andreia Gaita	(avidigal@novell.com)
//  		Gary Barnett	(gary.barnett.mono@gmail.com)

using System.Reflection;
using System.Drawing;
using System.Resources;
using System.Collections;
using System.Resources;
using GtkTests.TypeResolutionService_;
using GtkTests.Resources;

namespace GtkTests.System.Resources;

[TestFixture]
public class ResXDataNodeTest : ResourcesTestHelper
{
    string _tempDirectory;
    string _otherTempDirectory;

    [Test]
    public void ConstructorEx1()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var d = new ResXDataNode(null, (object?)null);
        });
    }

    [Test]
    public void ConstructorEx2A()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var d = new ResXDataNode(null, new ResXFileRef("filename", "typename"));
        });
    }

    [Test]
    public void ConstructorEx2B()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var d = new ResXDataNode("aname", (ResXFileRef)null);
        });
    }

    [Test]
    public void ConstructorEx3()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var d = new ResXDataNode("", (object?)null);
        });
    }

    [Test]
    public void ConstructorEx4()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var d = new ResXDataNode("", (ResXFileRef)null);
        });
    }

    [Test]
    public void ConstructorEx5()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var d = new ResXDataNode("", new ResXFileRef("filename", "typename"));
        });
    }

    [Test]
    public void ConstructorEx6()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var d = new ResXDataNode("name", new notserializable());
        });
    }

    [Test]
    public void Name()
    {
        var node = new ResXDataNode("startname", (object?)null);
        Assert.AreEqual("startname", node.Name, "#A1");
        node.Name = "newname";
        Assert.AreEqual("newname", node.Name, "#A2");
    }

    [Test]
    public void NameCantBeNull()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var node = new ResXDataNode("startname", (object?)null);
            node.Name = null;
        });
    }

    [Test]
    public void NameCantBeEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var node = new ResXDataNode("name", (object?)null);
            node.Name = "";
        });
    }

    [Test]
    public void FileRef()
    {
        var fileRef = new ResXFileRef("fileName", "Type.Name");
        var node = new ResXDataNode("name", fileRef);
        Assert.AreEqual(fileRef, node.FileRef, "#A1");
    }

    [Test]
    public void Comment()
    {
        var node = new ResXDataNode("name", (object?)null);
        node.Comment = "acomment";
        Assert.AreEqual("acomment", node.Comment, "#A1");
    }

    [Test]
    public void CommentNullToStringEmpty()
    {
        var node = new ResXDataNode("name", (object?)null);
        node.Comment = null;
        Assert.AreEqual(String.Empty, node.Comment, "#A1");
    }

    [Test]
    public void ConstructorResXFileRef()
    {
        var node = GetNodeFileRefToIcon();
        Assert.IsNotNull(node.FileRef, "#A1");
        Assert.AreEqual(typeof(Icon).AssemblyQualifiedName, node.FileRef.TypeName, "#A2");
        Assert.AreEqual("test", node.Name, "#A3");
    }

    [Test]
    public void NullObjectGetValueTypeNameIsNull()
    {
        var node = new ResXDataNode("aname", (object?)null);
        Assert.IsNull(node.GetValueTypeName((AssemblyName[])null), "#A1");
    }

    [Test]
    public void NullObjectWrittenToResXOK()
    {
        var node = new ResXDataNode("aname", (object?)null);
        var returnedNode = GetNodeFromResXReader(node);
        Assert.IsNotNull(returnedNode, "#A1");
        Assert.IsNull(returnedNode.GetValue((AssemblyName[])null), "#A2");
    }

    [Test]
    public void NullObjectReturnedFromResXGetValueTypeNameReturnsObject()
    {
        var node = new ResXDataNode("aname", (object?)null);
        var returnedNode = GetNodeFromResXReader(node);
        Assert.IsNotNull(returnedNode, "#A1");
        Assert.IsNull(returnedNode.GetValue((AssemblyName[])null), "#A2");
        var type = returnedNode.GetValueTypeName((AssemblyName[])null);
        Assert.AreEqual(typeof(object).AssemblyQualifiedName, type, "#A3");
    }

    [Test]
    public void DoesNotRequireResXFileToBeOpen_TypeConverter()
    {
        var dn = new ResXDataNode("test", 34L);
        var resXFile = GetResXFileWithNode(dn, "resx.resx");

        var rr = new ResXResourceReader(resXFile);
        rr.UseResXDataNodes = true;
        var en = rr.GetEnumerator();
        en.MoveNext();

        var node = ((DictionaryEntry)en.Current).Value as ResXDataNode;
        rr.Close();

        Assert.IsNotNull(node, "#A1");

        var o = node.GetValue((AssemblyName[])null);
        Assert.True(typeof(long)== o.GetType(), "#A2");
        Assert.AreEqual(34L, o, "#A3");
    }

    [Test]
    public void ITRSPassedToResourceReaderDoesNotAffectResXDataNode_TypeConverter()
    {
        var dn = new ResXDataNode("test", 34L);

        var resXFile = GetResXFileWithNode(dn, "resx.resx");

        var rr = new ResXResourceReader(resXFile, new ReturnIntITRS());
        rr.UseResXDataNodes = true;
        var en = rr.GetEnumerator();
        en.MoveNext();

        var node = ((DictionaryEntry)en.Current).Value as ResXDataNode;

        Assert.IsNotNull(node, "#A1");

        var o = node.GetValue((AssemblyName[])null);

        Assert.True(typeof(long) == o.GetType(), "#A2");
        Assert.AreEqual(34L, o, "#A3");

        rr.Close();
    }

    [Test]
    public void BasePathSetOnResXResourceReaderDoesAffectResXDataNode()
    {
        var fileRef = new ResXFileRef("file.name", "type.name");
        var node = new ResXDataNode("anode", fileRef);
        var resXFile = GetResXFileWithNode(node, "afilename.xxx");

        using var rr = new ResXResourceReader(resXFile);
        rr.BasePath = "basePath";
        rr.UseResXDataNodes = true;
        var en = rr.GetEnumerator();
        en.MoveNext();

        var returnedNode = ((DictionaryEntry)en.Current).Value as ResXDataNode;

        Assert.IsNotNull(node, "#A1");
        Assert.AreEqual(Path.Combine("basePath", "file.name"), returnedNode.FileRef.FileName, "#A2");
    }

    [TearDown]
    protected override void TearDown()
    {
        //teardown
        if (Directory.Exists(_tempDirectory))
            Directory.Delete(_tempDirectory, true);

        base.TearDown();
    }

    string GetResXFileWithNode(ResXDataNode node, string filename)
    {
        string fullfileName;

        _tempDirectory = Path.Combine(Path.GetTempPath(), "ResXDataNodeTest");
        _otherTempDirectory = Path.Combine(_tempDirectory, "in");
        if (!Directory.Exists(_otherTempDirectory))
        {
            Directory.CreateDirectory(_otherTempDirectory);
        }

        fullfileName = Path.Combine(_tempDirectory, filename);

        using var writer = new ResXResourceWriter(fullfileName);
        writer.AddResource(node);

        return fullfileName;
    }

}