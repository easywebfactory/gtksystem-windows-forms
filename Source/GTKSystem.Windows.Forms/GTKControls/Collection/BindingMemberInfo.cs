namespace System.Windows.Forms;

public readonly struct BindingMemberInfo : IEquatable<BindingMemberInfo>
{
    private readonly string? _dataList;

    private readonly string? _dataField;

    public string BindingPath => _dataList ?? string.Empty;

    public string BindingField => _dataField ?? string.Empty;

    public string BindingMember => (BindingPath?.Length??0) > 0
        ? $"{BindingPath}.{BindingField}"
        : BindingField;

    public BindingMemberInfo(string? dataMember)
    {
        dataMember ??= string.Empty;
        var lastDot = dataMember.LastIndexOf('.');
        if (lastDot != -1)
        {
            _dataList = dataMember.Substring(0, lastDot);
            _dataField = dataMember.Substring(lastDot + 1);
        }
        else
        {
            _dataList = string.Empty;
            _dataField = dataMember;
        }
    }

    public override bool Equals(object? otherObject)
    {
        if (otherObject is BindingMemberInfo otherMember)
        {
            return Equals(otherMember);
                
        }
        return false;
    }

    public bool Equals(BindingMemberInfo other)
        => string.Equals(BindingMember, other.BindingMember, StringComparison.OrdinalIgnoreCase);

    public static bool operator ==(BindingMemberInfo a, BindingMemberInfo b) => a.Equals(b);

    public static bool operator !=(BindingMemberInfo a, BindingMemberInfo b) => !a.Equals(b);

    public override int GetHashCode() => base.GetHashCode();
}