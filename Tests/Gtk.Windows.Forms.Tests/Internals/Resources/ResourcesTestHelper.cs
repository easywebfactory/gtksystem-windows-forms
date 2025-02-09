//
// ResourcesTestHelper.cs : Base class for new resource tests with methods 
// required across many.
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
using System.Collections;
using System.Drawing;
using GtkTests.Helpers;
using GtkTests.Resources;
using Newtonsoft.Json;

namespace GtkTests.Internals.Resources;

public class ResourcesTestHelper
{
    string tempFileWithSerializable = null;

    [SetUp]
    protected virtual void SetUp()
    {

    }

    protected ResXDataNode GetNodeFromResXReader(ResXDataNode node)
    {
        var sw = new StringWriter();
        using (var writer = new ResXResourceWriter(sw))
        {
            writer.AddResource(node);
        }

        var sr = new StringReader(sw.GetStringBuilder().ToString());

        using (var reader = new ResXResourceReader(sr))
        {
            reader.UseResXDataNodes = true;
            var enumerator = reader.GetEnumerator();
            enumerator.MoveNext();

            return ((DictionaryEntry)enumerator.Current).Value as ResXDataNode;
        }
    }

    protected ResXDataNode GetNodeFromResXReader(string contents)
    {
        var sr = new StringReader(contents);

        using var reader = new ResXResourceReader(sr);
        reader.UseResXDataNodes = true;
        var enumerator = reader.GetEnumerator();
        enumerator.MoveNext();

        return (ResXDataNode)((DictionaryEntry)enumerator.Current).Value;
    }

    public ResXDataNode GetNodeEmdeddedIcon()
    {
        var ico = new Icon(TestResourceHelper.GetStreamOfResource("Test/resources/32x32.ico"));
        var node = new ResXDataNode("test", ico);
        return node;
    }

    public ResXDataNode GetNodeFileRefToIcon()
    {
        var fileRef = new ResXFileRef(TestResourceHelper.GetFullPathOfResource("Test/resources/32x32.ico"), typeof(Icon).AssemblyQualifiedName);
        var node = new ResXDataNode("test", fileRef);

        return node;
    }

    public ResXDataNode GetNodeEmdeddedBytes1To10()
    {
        var someBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var node = new ResXDataNode("test", someBytes);
        return node;
    }

    public ResXDataNode GetNodeEmdeddedSerializable()
    {
        var ser = new serializable("testName", "testValue");
        var node = new ResXDataNode("test", ser);
        return node;
    }

    public ResXDataNode GetNodeFileRefToSerializable(string filename, bool assemblyQualifiedName)
    {
        tempFileWithSerializable = Path.GetTempFileName();  // remember to delete file in teardown
        var ser = new serializable("name", "value");

        SerializeToFile(tempFileWithSerializable, ser);

        string typeName;

        if (assemblyQualifiedName)
            typeName = typeof(serializable).AssemblyQualifiedName;
        else
            typeName = typeof(serializable).FullName;

        var fileRef = new ResXFileRef(tempFileWithSerializable, typeName);
        var node = new ResXDataNode("test", fileRef);

        return node;
    }

    static void SerializeToFile(string filepath, serializable ser)
    {
        Stream stream = File.Open(filepath, FileMode.Create);
        var streamReader = new StreamWriter(stream);
        JsonConvert.SerializeObject(ser);
        stream.Close();
    }

    [TearDown]
    protected virtual void TearDown()
    {
        if (tempFileWithSerializable != null)
        {
            File.Delete(tempFileWithSerializable);
            tempFileWithSerializable = null;
        }
    }
}