using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace PlasticCommand.Generator.Analysis;

internal static class PlasticCommandAttributeAnalyzer
{
    public static (string? CommandName, string? GroupName) Analyze(
        INamedTypeSymbol implementedCommandSpecClass)
    {
        string attributeName = typeof(PlasticCommandAttribute).FullName;

        AttributeData? commandNameAtt;
        commandNameAtt = implementedCommandSpecClass
                                                .GetAttributes()
                                                .FirstOrDefault(att => att.AttributeClass?.ToString() == attributeName);

        string commandNameArg = nameof(PlasticCommandAttribute.GeneratedCommandName);
        string groupNameArg = nameof(PlasticCommandAttribute.GroupName);
        ImmutableArray<KeyValuePair<string, TypedConstant>>? attributeArgs = commandNameAtt?.NamedArguments;

        return (
            (string?)attributeArgs?.SingleOrDefault(q => q.Key == commandNameArg).Value.Value,
            (string?)attributeArgs?.SingleOrDefault(q => q.Key == groupNameArg).Value.Value
            );
    }
}
