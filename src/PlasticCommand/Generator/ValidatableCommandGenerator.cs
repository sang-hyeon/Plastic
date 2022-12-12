using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;

namespace PlasticCommand.Generator;

internal class ValidatableCommandGenerator : TemplateBasedCommandGenerator
{
    public const string VALIDATABLE_COMMAND_TEMPLATE =
        PROJECT_NAME + ".Generator.Templates.CommandTemplate.ValidatableCommandTemplate.txt";

    public ValidatableCommandGenerator()
        : base(typeof(ICommandSpecificationWithValidation<,,>)
                    , VALIDATABLE_COMMAND_TEMPLATE)
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
            INamedTypeSymbol? commandSpecInterface =
                userCommandSpecSymbol.AllInterfaces.FirstOrDefault(q => SymbolEquals(q.ConstructedFrom, originalInterface));

            if (commandSpecInterface is not null)
            {
                string commandName = GenerateCommandName(userCommandSpecSymbol);
                return GenerateCommandFromTemplate(
                    context, userCommandSpecSymbol, commandName, commandSpecInterface);
            }
            else return default;
        }
        else return default;
    }

    private GeneratedCommandInfo GenerateCommandFromTemplate(
        GeneratorExecutionContext context, INamedTypeSymbol userCommandSpec,
        string commandName, INamedTypeSymbol originalCommandSpec)
    {
        ITypeSymbol paramSymbol = originalCommandSpec.TypeArguments[0];
        ITypeSymbol resultSymbol = originalCommandSpec.TypeArguments[1];
        ITypeSymbol validationResultSymbol = originalCommandSpec.TypeArguments[2];

        string codeForServicesToBeProvided = BuildServiceInjectionCodeForPipelineContext(userCommandSpec);
        string @namespace = userCommandSpec.ContainingNamespace.ToString();

        var commandBuilder = new StringBuilder(this.Template);
        commandBuilder.Replace("{{ Namespace }}", @namespace);
        commandBuilder.Replace("Generator.TTFFResult", resultSymbol.ToString());
        commandBuilder.Replace("Generator.TTFFParameter", paramSymbol.ToString());
        commandBuilder.Replace("Generator.TTFFValidationResult", validationResultSymbol.ToString());
        commandBuilder.Replace("PlasticCommand.Generator.TTFFValidatableCommandSpec", userCommandSpec.ToString());
        commandBuilder.Replace("TTFFValidatableCommand", commandName);
        commandBuilder.Replace("{{ ServicesToBeProvided }}", codeForServicesToBeProvided);

        context.AddSource($"{commandName}.cs", commandBuilder.ToString());

        return new GeneratedCommandInfo(@namespace + "." + commandName, userCommandSpec.ToString());
    }
}
