//
// ResXDataNodeWriteBehavior.cs : Tests how ResXDataNode's are written to
// resx files.
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
using System.Resources;
using System.Collections;
using System.Text;
using System.ComponentModel.Design;
using GtkTests.TypeResolutionService_;
using GtkTests.Internals.Resources;
using GtkTests.Resources;

namespace GtkTests.System.Resources;

[TestFixture]
public class ResXDataNodeWriteBehavior : ResourcesTestHelper {
		
    [Test]
    public void SerializedObjectNotLoaded ()
    {
        var node = GetNodeFromResXReader (serializedResXCorruped);
        Assert.IsNotNull (node, "#A1");
        // would cause error if object loaded
        GetNodeFromResXReader (node);
    }

    [Test]
    public void FileRefIsLoaded ()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            // .NET doesnt instantiate the encoding until the write
            var node = GetNodeFromResXReader(fileRefResXCorrupted);
            Assert.IsNotNull(node, "#A1");
            // would cause error if object loaded
            GetNodeFromResXReader(node);
        });
    }

    [Test]
    public void ResXNullRef_WriteBack ()
    {
        var node = new ResXDataNode ("NullRef", (object) null);
        node.Comment = "acomment";
        var returnedNode = GetNodeFromResXReader (node);
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.IsNull (returnedNode.GetValue ((AssemblyName []) null), "#A2");
        Assert.AreEqual ("acomment", returnedNode.Comment,"#A3");
        var finalNode = GetNodeFromResXReader (returnedNode);
        Assert.IsNotNull (finalNode, "#A4");
        Assert.IsNull (finalNode.GetValue ((AssemblyName []) null), "#A5");
        Assert.AreEqual ("acomment", finalNode.Comment,"#A6");
    }

    [Test]
    public void InvalidMimeType_WriteBack ()
    {
        //FIXME: should check the ResX output to ensure mime type / value info still there
        var node = GetNodeFromResXReader (serializedResXInvalidMimeType);
        Assert.IsNotNull (node, "#A1");
        var returnedNode = GetNodeFromResXReader (node);
        Assert.IsNotNull (returnedNode, "#A2");
        var obj = returnedNode.GetValue ((AssemblyName []) null);
        Assert.IsNull (obj, "#A3");
    }

    [Test]
    public void BinTypeConverter_WriteBack ()
    {
        var mb = new MyBinType ("contents");
        var node = new ResXDataNode ("aname", mb);
        node.Comment = "acomment";
        var returnedNode = GetNodeFromResXReader (node);
        Assert.IsNotNull (returnedNode, "#A1");
        var returnedMB = (MyBinType) returnedNode.GetValue ((AssemblyName []) null);
        Assert.AreEqual ("contents", returnedMB.Value, "#A2");
        Assert.AreEqual ("acomment", returnedNode.Comment, "#A3");
        var finalNode = GetNodeFromResXReader (returnedNode);
        Assert.IsNotNull (finalNode, "#A4");
        var finalMB = (MyBinType) finalNode.GetValue ((AssemblyName []) null);
        Assert.AreEqual ("contents", finalMB.Value, "#A5");
        Assert.AreEqual ("acomment", finalNode.Comment, "#A6");
    }

    [Test]
    public void ByteArray_WriteBack ()
    {
        var testBytes = new byte [] { 1,2,3,4,5,6,7,8,9,10 };
        var node = new ResXDataNode ("aname", testBytes);
        node.Comment = "acomment";
        var returnedNode = GetNodeFromResXReader (node);
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual (testBytes, returnedNode.GetValue ((AssemblyName []) null), "#A2");
        Assert.AreEqual ("acomment", returnedNode.Comment, "#A3");
        var finalNode = GetNodeFromResXReader (returnedNode);
        Assert.IsNotNull (finalNode,"#A4");
        Assert.AreEqual (testBytes, finalNode.GetValue ((AssemblyName []) null), "#A5");
        Assert.AreEqual ("acomment", finalNode.Comment, "#A6");
    }

    [Test]
    public void ChangesToReturnedByteArrayNotLaterWrittenBack ()
    {
        ResXDataNode originalNode, returnedNode, finalNode;
        originalNode = GetNodeEmdeddedBytes1To10 ();
        returnedNode = GetNodeFromResXReader (originalNode);

        Assert.IsNotNull (returnedNode, "#A1");

        var val = returnedNode.GetValue ((ITypeResolutionService) null);
        Assert.True (typeof (byte [])== val.GetType(), "#A2");

        var newBytes = (byte[]) val;
        Assert.AreEqual (1, newBytes [0], "A3");
        newBytes [0] = 99;

        finalNode = GetNodeFromResXReader (returnedNode);
			
        Assert.IsNotNull (finalNode, "#A4");

        var finalVal = finalNode.GetValue ((ITypeResolutionService) null);
        Assert.True(typeof(byte[]) == finalVal.GetType(), "#A5");
        var finalBytes = (byte []) finalVal;
        // would be 99 if written back
        Assert.AreEqual (1,finalBytes [0],"A6");
    }

    static readonly string fileRefResXCorrupted = 
        @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  
  <resheader name=""resmimetype"">
	<value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
	<value>2.0</value>
  </resheader>
  <resheader name=""reader"">
	<value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
	<value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <assembly alias=""System.Windows.Forms"" name=""System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" />
  <data name=""test"" type=""System.Resources.ResXFileRef, System.Windows.Forms"">
	<value>.\somethingthatdoesntexist.txt;System.String, System.Windows.Forms_test_net_2_0, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;AValidCultureStringThisIsNot</value>
  </data>
</root>";

    static readonly string serializedResXCorruped =
        @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  
  <resheader name=""resmimetype"">
	<value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
	<value>2.0</value>
  </resheader>
  <resheader name=""reader"">
	<value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
	<value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""test"" mimetype=""application/x-microsoft.net.object.binary.base64"">
	<value>
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
</value>
  </data>
</root>";

    static readonly string serializedResXInvalidMimeType =
        @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  
  <resheader name=""resmimetype"">
	<value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
	<value>2.0</value>
  </resheader>
  <resheader name=""reader"">
	<value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
	<value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""test"" mimetype=""application/xxxx"">
	<value>
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
</value>
  </data>
</root>";
}