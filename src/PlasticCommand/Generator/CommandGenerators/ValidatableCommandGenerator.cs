using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlasticCommand.Generator.Analysis;
using System.Text;

namespace PlasticCommand.Generator.CommandGenerators;

internal class ValidatableCommandGenerator
    : CommandGenerator
{
    protected new const string TEMPLATE_NAME =
        PROJECT_NAME + ".Generator.Templates.CommandTemplate.ValidatableCommandTemplate.txt";

    protected new ValidatableCommandSpecAnalyzer Analyzer
        => (ValidatableCommandSpecAnalyzer)base.Analyzer;

    public ValidatableCommandGenerator(
        GeneratorExecutionContext context)
        : base(context)
    {
    }

    public override GeneratedCommandInfo? Generate(
        TypeDeclarationSyntax implementedCommandSpec)
    {
        var analysis =
            (ValidatableCommandSpecAnalysisResult?)
                this.Analyzer.Analyze(this.Context, implementedCommandSpec);

        if (analysis == null)
            return default;

        var template = GetTemplate();
        string codeForServicesToBeProvided =
            BuildServiceInjectionCodeForPipelineContext(analysis);

        string commandName = GenerateCommandName(analysis);
        string commandInterfaceName = "I" + commandName;
        string @namespace = analysis.ImplementedClass.ContainingNamespace.ToString();

        var commandBuilder = new StringBuilder(template);
        commandBuilder.Replace("{{ Namespace }}", @namespace);
        commandBuilder.Replace("Generator.TTFFResult", analysis.Result.ToString());
        commandBuilder.Replace("Generator.TTFFParameter", analysis.Param.ToString());
        commandBuilder.Replace("Generator.TTFFValidationResult", analysis.ValidationResult.ToString());
        commandBuilder.Replace("PlasticCommand.Generator.TTFFValidatableCommandSpec", analysis.ImplementedClass.ToString());
        commandBuilder.Replace("TTFFValidatableCommand", commandName);
        commandBuilder.Replace("{{ ServicesToBeProvided }}", codeForServicesToBeProvided);
        commandBuilder.Replace("TTFFGeneratedValidatableCommandInterface", commandInterfaceName);
        commandBuilder.Replace("TTFFParamName", analysis.ExecuteMethod.Parameters[0].Name);
        commandBuilder.Replace("TTFFCanExecuteParamName", analysis.CanExecuteMethod.Parameters[0].Name);

        this.Context.AddSource($"{commandName}.cs", commandBuilder.ToString());

        return new GeneratedCommandInfo(
            @namespace + "." + commandName,
            @namespace + "." + commandInterfaceName,
            analysis.ImplementedClass.ToString(),
            analysis);
    }

    protected override string GetTemplate()
    {
        return Helper.ReadEmbeddedResourceAsString(TEMPLATE_NAME);
    }

    protected override CommandSpecAnalyzer GetAnalyzer()
    {
        return new ValidatableCommandSpecAnalyzer();
    }
}
