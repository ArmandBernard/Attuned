using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using ConfigurationBuilder = Reinforced.Typings.Fluent.ConfigurationBuilder;

namespace AttunedWebApi.CodeGen;

// This is used by Reinforced.Typings.settings.xml
// ReSharper disable once UnusedType.Global
public static class ReinforcedTypingsConfiguration
{
    private  record Import(string Imports, string From);

    private static readonly Import[] _customImports = {
        new("{ UTCDateTime }", "./UTCDateTime.ts"),
        new("{ TimeSpan }", "./TimeSpan.ts"),
        new("{ Rating }", "./Rating.ts"),
    };

    public static void Configure(ConfigurationBuilder builder)
    {
        builder.Global(config => config.UseModules());
        foreach (var import in _customImports)
        {
            builder.AddImport(import.Imports, import.From);
        }
        builder.Substitute(typeof(DateTime), new RtSimpleTypeName("UTCDateTime"));
        builder.Substitute(typeof(TimeSpan), new RtSimpleTypeName("TimeSpan"));
        // if a type is nullable, use "type | undefined"
        builder.SubstituteGeneric(typeof(Nullable<>),
            (type, resolver) =>
                new RtSimpleTypeName($"{resolver.ResolveTypeName(type.GenericTypeArguments.Single())} | undefined"));
    }
}