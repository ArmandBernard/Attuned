using Reinforced.Typings.Attributes;

namespace AttunedWebApi.CodeGen;

public sealed class TypescriptNullableAttribute : TsPropertyAttribute
{
    private string? _type;

    public override string? Type
    {
        get => _type + " | undefined";
        set => _type = value;
    }
}