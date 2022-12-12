using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Text;

namespace PlasticCommand.Generator;

internal abstract class TemplateBasedCommandGenerator
{
    public const string PROJECT_NAME = "PlasticCommand";
    public readonly string TargetCommandSpecFullName;
    public readonly string TemplateSourceName;
    public readonly string Template;

    protected TemplateBasedCommandGenerator(
        Type userCommandSpec, string templateSourceName)
    {
        this.TargetCommandSpecFullName = userCommandSpec.FullName;
        this.TemplateSourceName = templateSourceName;
        this.Template = Helper.ReadEmbeddedResourceAsString(this.TemplateSourceName);
    }

    public abstract GeneratedCommandInfo? GenerateCommand(
        GeneratorExecutionContext context, TypeDeclarationSyntax userCommandSpec);

    protected INamedTypeSymbol CreateTargetCommandSpec(SemanticModel model)
    {
        string fullName = this.TargetCommandSpecFullName;
        return model.Compilation.GetTypeByMetadataName(fullName)!;
    }

    protected static string BuildServiceInjectionCodeForPipelineContext(
        INamedTypeSymbol userCommandSpecSymbol)
    {
        IParameterSymbol[] parameters =
            userCommandSpecSymbol.Constructors
                                                .SelectMany(q => q.Parameters)
                                                .Select(q => (ISymbol)q)
                                                .Distinct(SymbolEqualityComparer.Default)
                                                .Select(q => (IParameterSymbol)q)
                                                .ToArray();
        if (0 < parameters.Length)
        {
            var builder = new StringBuilder();
            foreach (IParameterSymbol item in parameters)
            {
                builder.Append($"\t\t\t\tprovider.GetService<{item}>(),\n");
                builder.Replace("?", string.Empty); // to not null
            }

            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }
        else
            return string.Empty;
    }

    protected static bool SymbolEquals(ISymbol operand1, ISymbol operand2)
    {
        return SymbolEqualityComparer
                        .Default
                        .Equals(operand1, operand2);
    }

    protected static string GenerateCommandName(
        INamedTypeSymbol userCommandSpecSymbol)
    {
        string attributeName = typeof(CommandNameAttribute).FullName;

        AttributeData? commandNameAtt =
            userCommandSpecSymbol
                .GetAttributes()
                .FirstOrDefault(att => att.AttributeClass?.ToString() == attributeName);

        if (commandNameAtt?.ConstructorArguments.FirstOrDefault().Value
                is string commandName)
        {
            return commandName;
        }
        else
            return userCommandSpecSymbol
                        .Name.Replace("CommandSpec", string.Empty) + "Command";
    }
}