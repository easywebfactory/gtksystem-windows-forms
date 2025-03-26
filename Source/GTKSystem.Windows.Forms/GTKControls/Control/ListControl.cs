using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms;

[LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
public abstract class ListControl : ScrollableControl
{
    protected EventHandlerList events = new();
    [DefaultValue(null)]
    [RefreshProperties(RefreshProperties.Repaint)]
    [AttributeProvider(typeof(IListSource))]
    public virtual object? DataSource
    {
        get;set;
    }

    [DefaultValue("")]
    public virtual string DisplayMember
    {
        get;
        set;
    } = string.Empty;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DefaultValue(null)]
    public virtual IFormatProvider FormatInfo
    {
        get;
        set;
    } = CultureInfo.CurrentCulture;

    [DefaultValue("")]
    [MergableProperty(false)]
    public virtual string FormatString
    {
        get;
        set;
    } = string.Empty;

    [DefaultValue(false)]
    public virtual bool FormattingEnabled
    {
        get; set;
    }

    [DefaultValue("")]
    public virtual string ValueMember
    {
        get; set;
    } = string.Empty;

    public abstract int SelectedIndex { get; set; }

    [DefaultValue(null)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Bindable(true)]
    public virtual object? SelectedValue
    {
        get; set;
    }

    public event EventHandler? DataSourceChanged
    {
        add => events.AddHandler("DataSourceChanged", value);
        remove => events.RemoveHandler("DataSourceChanged", value);
    }

    public event EventHandler? DisplayMemberChanged;

    public event ListControlConvertEventHandler? Format;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public event EventHandler? FormatInfoChanged;

    public event EventHandler? FormatStringChanged;

    public event EventHandler? FormattingEnabledChanged;

    public event EventHandler? ValueMemberChanged;
    public event EventHandler? SelectedItemChanged
    {
        add => events.AddHandler("SelectedItemChanged", value);
        remove => events.RemoveHandler("SelectedItemChanged", value);
    }
    public event EventHandler? SelectedValueChanged
    {
        add => events.AddHandler("SelectedValueChanged", value);
        remove => events.RemoveHandler("SelectedValueChanged", value);
    }
    public event EventHandler? SelectedIndexChanged
    {
        add => events.AddHandler("SelectedIndexChanged", value);
        remove => events.RemoveHandler("SelectedIndexChanged", value);
    }
    public virtual string? GetItemText(object? item)
    {
        return item?.ToString();
    }

    protected virtual void OnDisplayMemberChanged(EventArgs e)
    {
        DisplayMemberChanged?.Invoke(this, e);
    }

    protected virtual void OnFormat(ListControlConvertEventArgs e)
    {
        Format?.Invoke(this, e);
    }

    protected virtual void OnFormatInfoChanged(EventArgs e)
    {
        FormatInfoChanged?.Invoke(this, e);
    }

    protected virtual void OnFormatStringChanged(EventArgs e)
    {
        FormatStringChanged?.Invoke(this, e);
    }

    protected virtual void OnFormattingEnabledChanged(EventArgs e)
    {
        FormattingEnabledChanged?.Invoke(this, e);
    }

    protected virtual void OnValueMemberChanged(EventArgs e)
    {
        ValueMemberChanged?.Invoke(this, e);
    }
}