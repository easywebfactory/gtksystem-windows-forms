using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace System.Windows.Forms;

public class DataGridViewCellStyle : ICloneable
{
    private readonly DataGridViewCellStyle? dataGridViewCellStyle;
    private DataGridViewCellStyleScopes scope;
    public DataGridViewCellStyle() { }
    public DataGridViewCellStyle(DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.dataGridViewCellStyle = dataGridViewCellStyle;
    }

    public Color SelectionForeColor { get; set; }
    public Color SelectionBackColor { get; set; }
    public Padding Padding { get; set; }
    [DefaultValue("")]
    [TypeConverter(typeof(StringConverter))]
    public object? NullValue { get; set; }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public bool IsNullValueDefault => default;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public bool IsFormatProviderDefault => default;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public bool IsDataSourceNullValueDefault => default;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public IFormatProvider? FormatProvider { get; set; }
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public string? Format { get; set; }
    public Font? Font { get; set; }
    public Color ForeColor { get; set; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public object? DataSourceNullValue { get; set; }
    public Color BackColor { get; set; }

    public DataGridViewContentAlignment Alignment { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object? Tag { get; set; }
    [DefaultValue(DataGridViewTriState.NotSet)]
    public DataGridViewTriState WrapMode { get; set; }

        public virtual void ApplyStyle(DataGridViewCellStyle dataGridViewCellStyle) { }
        public virtual DataGridViewCellStyle Clone() {
            var celltype = GetType();
            var newobj = new DataGridViewCellStyle();
            var propertys = celltype.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
            foreach (var property in propertys)
                if (property.CanRead && property.CanWrite)
                    property.SetValue(newobj, property.GetValue(this));
            return newobj;
        }
        public override bool Equals(object? o) { return false; }
        public override int GetHashCode()
        {
            return 0;
        }
        public override string ToString()
        {
            return "DataGridViewCellStyle";
        }

    object ICloneable.Clone()
    {
        throw new NotImplementedException();
    }

    internal DataGridViewCellStyleScopes Scope
    {
        get => scope;
        set => scope = value;
    }
}