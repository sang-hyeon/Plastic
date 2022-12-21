using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace PlasticCommand.Generator.Analysis;

internal class ValidatableCommandSpecAnalyzer : CommandSpecAnalyzer
{
    protected override Type GetRootCommandSpecType()
    {
        return typeof(ICommandSpecificationWithValidation<,,>);
    }

    protected override CommandSpecAnalysisResult? OnAnalyze(
        TypeDeclarationSyntax implementedCommandSpecSyntax,
        INamedTypeSymbol rootCommandSpecInterface,
        SemanticModel model)
    {
        CommandSpecAnalysisResult? result =
            base.OnAnalyze(implementedCommandSpecSyntax, rootCommandSpecInterface, model);

        if (result != null)
        {
            return new ValidatableCommandSpecAnalysisResult(
                result.BaseInterface,
                result.DeclaredInterface,
                result.ImplementedClass,
                result.Param,
                result.Result,
                result.DeclaredInterface.TypeArguments[2],
                result.PlasticCommandAttribute
                );
        }
        else
            return default;
    }
}