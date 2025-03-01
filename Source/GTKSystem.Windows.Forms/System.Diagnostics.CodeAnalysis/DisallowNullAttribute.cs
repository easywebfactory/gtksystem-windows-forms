#if NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
internal sealed class DisallowNullAttribute : Attribute
{
}
#endif