using Reinforced.Typings.Ast.TypeNames;

namespace AttunedWebApi;

using Reinforced.Typings.Fluent;

public static class ReinforcedTypingsConfiguration
{
    public static void Configure(ConfigurationBuilder builder)
    {
        builder.Global(config => config.UseModules());
        builder.Substitute(typeof(DateTime), new RtSimpleTypeName("UTCDateTime"));
    }
}