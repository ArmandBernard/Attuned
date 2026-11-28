using System.Reflection;
using iTunesSmartParser.Data;
using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Generators;

namespace AttunedWebApi.CodeGen;

/// <summary>
/// Generates a TypeScript interface for a DTO, handling special cases.
/// </summary>
public class DtoInterfaceGenerator : InterfaceCodeGenerator
{
    public override RtInterface GenerateNode(Type element, RtInterface result, TypeResolver resolver)
    {
        var node = base.GenerateNode(element, result, resolver);

        foreach (var field in node.Members.OfType<RtField>())
        {
            var property = element.GetProperty(field.Identifier.IdentifierName);

            if (property != null)
            {
                HandleCustomTypes(field, property);
            }
        }

        return node;
    }

    private static void HandleCustomTypes(RtField field, PropertyInfo property)
    {
        if (property is {PropertyType: {IsGenericType: true} type} &&
            type.GetGenericTypeDefinition() == typeof(Option<>))
        {
            // makes fields optional (as opposed to nullable)
            field.Identifier.IsNullable = true;
        }
    }
}