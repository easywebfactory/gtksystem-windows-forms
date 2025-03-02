using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

public class DataGridViewHeaderCell : DataGridViewCell
{
    protected ButtonState ButtonState => default;

    [Browsable(false)]
    public override bool Displayed => default;

    internal Bitmap? FlipXpThemesBitmap
    {
        get;
        set;
    } = default;

    public override Type? FormattedValueType => default;

    [Browsable(false)]
    public override bool Frozen => default;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool ReadOnly
    {
        get;
        set;
    }

    [Browsable(false)]
    public override bool Resizable => default;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool Selected
    {
        get;
        set;
    }

    public override Type? ValueType
    {
        get;
        set;
    }

    [Browsable(false)]
    public override bool Visible => true;
}