using Reinforced.Typings.Attributes;

namespace AttunedWebApi.CodeGen;

public sealed class TypescriptDtoAttribute: TsInterfaceAttribute
{
    public TypescriptDtoAttribute()
    {
        AutoI = false;
        AutoExportMethods = false;
    }
}