//
// HelperClasses_Resources.cs : Various types to use as resources during 
// testing. Note some more are present in DummyAssembly project and 
// WriterTest.cs
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

using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GtkTests.Resources;

class notserializable
{
    public object test;
    public notserializable()
    {

    }
}

[Serializable]
public class serializable : ISerializable
{
    public string name;
    public string value;

    public serializable()
    {
    }

    public serializable(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    public serializable(SerializationInfo info, StreamingContext ctxt)
    {
        name = (string)info.GetValue("sername", typeof(string));
        value = (string)info.GetValue("servalue", typeof(string));
    }

    public serializable(Stream stream)
    {
        var streamReader = new StreamReader(stream);
        var deser = JsonConvert.DeserializeObject<serializable>(streamReader.ReadToEnd());
        stream.Close();

        name = deser.name;
        value = deser.value;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("sername", name);
        info.AddValue("servalue", value);
    }

    public override string ToString()
    {
        return string.Format("name={0};value={1}", name, value);
    }

    public override bool Equals(object obj)
    {
        var o = obj as serializable;
        if (o == null)
            return false;
        return name.Equals(o.name) && value.Equals(o.value);
    }
}

[Serializable]
public class serializableSubClass : serializable
{
    public serializableSubClass()
    {
    }

    public serializableSubClass(SerializationInfo info, StreamingContext ctxt)
        : base(info, ctxt)
    {
    }

    public serializableSubClass(Stream stream)
    {
        var streamReader = new StreamReader(stream);
        var deser = JsonConvert.DeserializeObject<serializable>(streamReader.ReadToEnd());
        stream.Close();

        name = deser.name;
        value = deser.value;
    }
}

[Serializable]
[TypeConverter(typeof(ThisAssemblyConvertableConverter))]
public class ThisAssemblyConvertable
{
    protected string name;
    protected string value;

    public ThisAssemblyConvertable()
    {
    }

    public ThisAssemblyConvertable(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("sername", name);
        info.AddValue("servalue", value);
    }

    public override string ToString()
    {
        return string.Format("{0}\t{1}", name, value);
    }

    public override bool Equals(object obj)
    {
        var o = obj as ThisAssemblyConvertable;
        if (o == null)
            return false;
        return name.Equals(o.name) && value.Equals(o.value);
    }
}

class ThisAssemblyConvertableConverter : TypeConverter
{
    public ThisAssemblyConvertableConverter()
    {
    }

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value.GetType() != typeof(string))
            throw new Exception("value not string");

        var serialised = (string)value;

        string[] parts = serialised.Split('\t');

        if (parts.Length != 2)
            throw new Exception("string in incorrect format");

        var convertable = new ThisAssemblyConvertable(parts[0], parts[1]);
        return convertable;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType != typeof(string))
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        return ((ThisAssemblyConvertable)value).ToString();
    }
}