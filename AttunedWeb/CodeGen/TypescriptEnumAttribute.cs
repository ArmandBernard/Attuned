using Reinforced.Typings.Attributes;

namespace AttunedWebApi.CodeGen;

public class TypescriptEnumAttribute: TsEnumAttribute
{
    public TypescriptEnumAttribute()
    {
        UseString = true;
    }
}