using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using ConfigurationBuilder = Reinforced.Typings.Fluent.ConfigurationBuilder;

namespace AttunedWebApi.CodeGen;

public static class ReinforcedTypingsConfiguration
{
    public static void Configure(ConfigurationBuilder builder)
    {
        builder.Global(config => config.UseModules());
        builder.Substitute(typeof(DateTime), new RtSimpleTypeName("UTCDateTime"));
    }
}