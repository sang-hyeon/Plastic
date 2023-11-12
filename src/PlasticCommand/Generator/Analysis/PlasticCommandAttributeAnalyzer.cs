using Microsoft.CodeAnalysis;
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

        return (
            (string?)commandNameAtt?.NamedArguments.Single(q => q.Key == commandNameArg).Value.Value,
            (string?)commandNameAtt?.NamedArguments.Single(q => q.Key == groupNameArg).Value.Value
            );
    }
}
