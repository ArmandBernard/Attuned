using Reinforced.Typings.Attributes;

namespace AttunedWebApi.CodeGen;

public sealed class TsDtoAttribute: TsInterfaceAttribute
{
    public TsDtoAttribute()
    {
        AutoI = false;
        AutoExportMethods = false;
    }
}