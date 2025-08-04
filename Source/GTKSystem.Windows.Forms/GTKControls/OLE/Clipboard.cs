using Gtk;
namespace System.Windows.Forms
{

    public static class Clipboard
    {
        public static void SetDataObject(object data) => SetDataObject(data, copy: false);

        public static void SetDataObject(object data, bool copy) => SetDataObject(data, copy, retryTimes: 10, retryDelay: 100);

        public static void SetDataObject(object data, bool copy, int retryTimes, int retryDelay)
        {
            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Serializable, true, data);
        }

        public static IDataObject? GetDataObject()
        {
            return new DataObject();
        }

        public static void Clear() => SetDataObject(new DataObject());

        public static object? GetData(string format) => string.IsNullOrWhiteSpace(format) ? null : GetData(format, autoConvert: false);

        private static object? GetData(string format, bool autoConvert)
            => GetDataObject() is { } dataObject ? dataObject.GetData(format, autoConvert) : null;

        public static Drawing.Image? GetImage() => GetData(DataFormats.Bitmap, autoConvert: true) as Drawing.Image;

        public static string GetText() => GetText(TextDataFormat.UnicodeText);

        public static string GetText(TextDataFormat format)
        {
            return GetData(format.ToString()) as string ?? string.Empty;
        }
        public static void SetData(string format, object data)
        {
            SetDataObject(new DataObject(format, data), copy: true);
        }

        public static void SetImage(Drawing.Image image) => SetDataObject(new DataObject(image), copy: true);

        public static void SetText(string text) => SetText(text, TextDataFormat.UnicodeText);

        public static void SetText(string text, TextDataFormat format)
        {
            var clipboard = Gtk.Clipboard.Get(Gdk.Selection.Clipboard);
            clipboard.Text = text;
            clipboard.Store();
        }
        public class ClipDataObject
        {
            public ClipDataObject(object obj)
            {
                Data = obj;
            }
            public object Data { get; set; }
        }
        public partial class DataObject : IDataObject
        {
            Gtk.Clipboard clipboard = Gtk.Clipboard.Get(Gdk.Selection.Clipboard);
            public DataObject()
            {

            }
            public DataObject(object data) : this(DataFormats.Serializable, data)
            {

            }
            public DataObject(string format, object data) : this() => SetData(format, data);

            internal DataObject(string format, bool autoConvert, object data) : this() => SetData(format, autoConvert, data);


            public object GetData(string format, bool autoConvert)
            {

                if (format == DataFormats.Text || format == DataFormats.UnicodeText)
                {
                    string value = string.Empty;
                    clipboard.RequestText((clip, text) =>
                    {
                        value = text;
                    });
                    return value;
                }
                else if (format == DataFormats.Bitmap)
                {
                    Drawing.Bitmap bitmap = null;
                    clipboard.RequestImage((clip, image) =>
                    {
                        bitmap = new Drawing.Bitmap(image.Width, image.Height);
                        bitmap.Pixbuf = image;
                    });
                    return bitmap;
                }
                else
                {
                    object result = null;
                    clipboard.RequestContents(Gdk.Selection.Clipboard, new ClipboardReceivedFunc((clip, data) =>
                    {
                        using (MemoryStream ms = new MemoryStream(data.Data))
                        {
                            System.Runtime.Serialization.DataContractSerializer dataContract = new Runtime.Serialization.DataContractSerializer(typeof(ClipDataObject));
                            ClipDataObject dataobj = dataContract.ReadObject(ms) as ClipDataObject;
                            result = dataobj.Data;
                        }
                    }));
                    return result;
                }
            }

            public object GetData(string format)
            {
                return GetData(format, true);
            }

            public object GetData(Type format)
            {
                return GetData(format.Name, true);
            }

            public bool GetDataPresent(string format, bool autoConvert)
            {
                throw new NotImplementedException();
            }

            public bool GetDataPresent(string format)
            {
                throw new NotImplementedException();
            }

            public bool GetDataPresent(Type format)
            {
                throw new NotImplementedException();
            }

            public string[] GetFormats(bool autoConvert)
            {
                throw new NotImplementedException();
            }

            public string[] GetFormats()
            {
                throw new NotImplementedException();
            }

            public void SetData(string format, bool autoConvert, object data)
            {
                if (format == DataFormats.Text || format == DataFormats.UnicodeText)
                {
                    clipboard.Text = data?.ToString() ?? string.Empty;
                }
                else if (format == DataFormats.Bitmap)
                {
                    clipboard.Image = (Gdk.Pixbuf)((Drawing.Image)data).Pixbuf.Clone();
                }
                else
                {
                    clipboard.SetWithData([new Gtk.TargetEntry(format, Gtk.TargetFlags.Widget, 0)], (clip, selectiondata, inf) =>
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            System.Runtime.Serialization.DataContractSerializer dataContract = new Runtime.Serialization.DataContractSerializer(typeof(ClipDataObject));
                            dataContract.WriteObject(ms, new ClipDataObject(data));
                            selectiondata.Set(Gdk.Selection.Clipboard, 0, ms.ToArray());
                        }
                    }, new Gtk.ClipboardClearFunc(clip => { }));
                    clipboard.Store();
                }
            }

            public void SetData(string format, object data)
            {
                SetData(format, true, data);
            }

            public void SetData(Type format, object data)
            {
                SetData(format.Name, true, data);
            }

            public void SetData(object data)
            {
                SetData(DataFormats.Serializable, true, data);
            }
        }
    }
}