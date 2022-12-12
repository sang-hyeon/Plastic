using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;

namespace PlasticCommand.Generator;

internal class CommandGenerator : TemplateBasedCommandGenerator
{
    public const string TEMPLATE =
        PROJECT_NAME + ".Generator.Templates.CommandTemplate.CommandTemplate.txt";

    public CommandGenerator()
        : base(typeof(ICommandSpecification<,>), TEMPLATE)
    {
    }

    public override GeneratedCommandInfo? GenerateCommand(
        GeneratorExecutionContext context, TypeDeclarationSyntax userCommandSpec)
    {
        SemanticModel model = context.Compilation.GetSemanticModel(userCommandSpec.SyntaxTree);
        INamedTypeSymbol? originalInterface =
            model.Compilation.GetTypeByMetadataName(this.TargetCommandSpecFullName);

        if (originalInterface is INamedTypeSymbol targetCommandSpecSymbol
             && model.GetDeclaredSymbol(userCommandSpec) is INamedTypeSymbol userCommandSpecSymbol)
        {
            string commandName = GenerateCommandName(userCommandSpecSymbol);

            INamedTypeSymbol commandSpecInterface =
                userCommandSpecSymbol.AllInterfaces.First(q => SymbolEquals(q.ConstructedFrom, originalInterface));

            ITypeSymbol paramSymbol = commandSpecInterface.TypeArguments[0];
            ITypeSymbol resultSymbol = commandSpecInterface.TypeArguments[1];

            string codeForServicesToBeProvided = BuildServiceInjectionCodeForPipelineContext(userCommandSpecSymbol);
            string @namespace = userCommandSpecSymbol.ContainingNamespace.ToString();

            var commandBuilder = new StringBuilder(this.Template);
            commandBuilder.Replace("{{ Namespace }}", @namespace);
            commandBuilder.Replace("Generator.TTFFResult", resultSymbol.ToString());
            commandBuilder.Replace("Generator.TTFFParameter", paramSymbol.ToString());
            commandBuilder.Replace("PlasticCommand.Generator.TTFFCommandSpec", userCommandSpecSymbol.ToString());
            commandBuilder.Replace("TTFFCommand", commandName);
            commandBuilder.Replace("{{ ServicesToBeProvided }}", codeForServicesToBeProvided);

            context.AddSource($"{commandName}.cs", commandBuilder.ToString());

            return new GeneratedCommandInfo(@namespace + "." + commandName, userCommandSpecSymbol.ToString());
        }
        else return default;
    }
}
