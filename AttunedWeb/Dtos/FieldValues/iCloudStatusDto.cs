using AttunedWebApi.CodeGen;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.FieldValues;

[TypescriptEnum]
public enum iCloudStatusDto
{
    Purchased,
    Matched,
    Uploaded,
    Ineligible,
    [TsValue(Initializer = "Local Only")] LocalOnly,
    Duplicate,
}