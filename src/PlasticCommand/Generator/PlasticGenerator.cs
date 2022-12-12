
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticCommand.Generator;

[Generator]
internal class PlasticGenerator : ISourceGenerator
{
    public const string INITIALIZER_TEMPLATE =
        "PlasticCommand.Generator.Templates.InitializerTemplate.ServiceCollectionExtensionsTemplate.txt";

    public static readonly string ICOMMAND_SPEC_NAME
        = typeof(ICommandSpecification<,>).FullName!;

    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (SyntaxReceiver)context.SyntaxContextReceiver!;
        if (receiver.Targets.Count <= 0)
            return;

        var generatedCommands = new HashSet<GeneratedCommandInfo>();
        var generators = new TemplateBasedCommandGenerator[]
        {
            new ValidatableCommandGenerator(),
            new CommandGenerator()
        };

        foreach (TypeDeclarationSyntax userCommandSpec in receiver.Targets)
        {
            foreach (TemplateBasedCommandGenerator generator in generators)
            {
                GeneratedCommandInfo? generatedCommandInfo =
                    generator.GenerateCommand(context, userCommandSpec);

                if (generatedCommandInfo != null)
                {
                    generatedCommands.Add(generatedCommandInfo);
                    break;
                }
            }
        }

        GeneratePlasticInitializer(context, generatedCommands);
    }

    private static void GeneratePlasticInitializer(
        GeneratorExecutionContext contextToAdd, ICollection<GeneratedCommandInfo> generatedCommands)
    {
        string template = Helper.ReadEmbeddedResourceAsString(INITIALIZER_TEMPLATE);

        var builder = new StringBuilder();
        foreach (GeneratedCommandInfo commandName in generatedCommands)
        {
            builder.AppendLine($"\t\t\tservices.AddTransient(typeof({commandName.CommandSpecFullName}));");
            builder.AppendLine($"\t\t\tservices.AddTransient(typeof({commandName.GeneratedCommandName}));");
        }

        string generatedCode = template.Replace("{{ ServicesToBeAdded }}", builder.ToString());
        contextToAdd.AddSource("PlasticInitializer.cs", generatedCode);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    protected static bool SymbolEquals(ISymbol operand1, ISymbol operand2)
    {
        return SymbolEqualityComparer
                        .Default
                        .Equals(operand1, operand2);
    }

    private sealed class SyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly List<TypeDeclarationSyntax> _targets = new();

        public IReadOnlyList<TypeDeclarationSyntax> Targets => this._targets;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax typeNode)
            {
                OnVisitTypeDeclarationSyntax(context, typeNode);
            }
        }

        private void OnVisitTypeDeclarationSyntax(GeneratorSyntaxContext context, TypeDeclarationSyntax typeSyntax)
        {
            ISymbol? symbole = context.SemanticModel.GetDeclaredSymbol(typeSyntax);
            if (symbole is INamedTypeSymbol namedSymbol)
            {
                if (IsValid(namedSymbol, context))
                {
                    this._targets.Add(typeSyntax);
                }
            }
        }

        private static bool IsValid(INamedTypeSymbol target, GeneratorSyntaxContext context)
        {
            INamedTypeSymbol commandSpecSymbol =
                context.SemanticModel.Compilation.GetTypeByMetadataName(ICOMMAND_SPEC_NAME)!;

            return target.AllInterfaces.Any(q => SymbolEquals(q.ConstructedFrom, commandSpecSymbol))
                        && target.ContainingNamespace.GetTypeMembers().Contains(target);
        }
    }
}
