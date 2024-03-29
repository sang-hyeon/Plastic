﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlasticCommand.Generator.Analysis;
using System.Linq;
using System.Text;

namespace PlasticCommand.Generator.CommandGenerators;

internal class CommandGenerator
{
    public const string PROJECT_NAME = "PlasticCommand";
    protected const string TEMPLATE_NAME =
        PROJECT_NAME + ".Generator.Templates.CommandTemplate.CommandTemplate.txt";

    protected GeneratorExecutionContext Context { get; }
    protected CommandSpecAnalyzer Analyzer { get; }

    public CommandGenerator(
        GeneratorExecutionContext context)
    {
        this.Context = context;
        this.Analyzer = GetAnalyzer();
    }

    public virtual GeneratedCommandInfo? Generate(
        TypeDeclarationSyntax implementedCommandSpec)
    {
        CommandSpecAnalysisResult? analysis =
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
        commandBuilder.Replace("PlasticCommand.Generator.TTFFCommandSpec", analysis.ImplementedClass.ToString());
        commandBuilder.Replace("TTFFCommand", commandName);
        commandBuilder.Replace("{{ ServicesToBeProvided }}", codeForServicesToBeProvided);
        commandBuilder.Replace("{{XmlComment}}", analysis.XmlCommentsId);
        commandBuilder.Replace("TTFFGeneratedCommandInterface", commandInterfaceName);
        commandBuilder.Replace("TTFFParamName", analysis.ExecuteMethod.Parameters[0].Name);

        this.Context.AddSource($"{@namespace}.{commandName}.cs", commandBuilder.ToString());

        return new GeneratedCommandInfo(
            @namespace + "." + commandName,
            @namespace + "." + commandInterfaceName,
            analysis.ImplementedClass.ToString(),
            analysis);
    }


    protected virtual CommandSpecAnalyzer GetAnalyzer()
    {
        return new CommandSpecAnalyzer();
    }

    protected virtual string GetTemplate()
    {
        return Helper.ReadEmbeddedResourceAsString(TEMPLATE_NAME);
    }

    protected virtual string BuildServiceInjectionCodeForPipelineContext(
        CommandSpecAnalysisResult analysis)
    {
        IParameterSymbol[] parameters =
                analysis.ImplementedClass.Constructors
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
                builder.Append($"\t\t\t\tprovider.GetService<{item.Type}>(),\n");
                builder.Replace("?", string.Empty); // to not null
            }

            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }
        else
            return string.Empty;
    }

    protected string GenerateCommandName(CommandSpecAnalysisResult analysis)
    {
        (string? commandName, string? _)
            = PlasticCommandAttributeAnalyzer.Analyze(analysis.ImplementedClass);

        if (commandName is not null)
        {
            return commandName;
        }
        else
            return analysis.ImplementedClass
                                .Name.Replace("CommandSpec", string.Empty) + "Command";
    }
}