using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using ConfigurationBuilder = Reinforced.Typings.Fluent.ConfigurationBuilder;

namespace AttunedWebApi.CodeGen;

// This is used by Reinforced.Typings.settings.xml
// ReSharper disable once UnusedType.Global
public static class ReinforcedTypingsConfiguration
{
    public static void Configure(ConfigurationBuilder builder)
    {
        builder.Global(config => config.UseModules());
        builder.Substitute(typeof(DateTime), new RtSimpleTypeName("UTCDateTime"));
        // if a type is nullable, use "type | undefined"
        builder.SubstituteGeneric(typeof(Nullable<>),
            (type, resolver) =>
                new RtSimpleTypeName($"{resolver.ResolveTypeName(type.GenericTypeArguments.Single())} | undefined"));
    }
}