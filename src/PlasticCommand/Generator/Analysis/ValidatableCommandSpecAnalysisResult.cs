using Microsoft.CodeAnalysis;

namespace PlasticCommand.Generator.Analysis;

internal class ValidatableCommandSpecAnalysisResult : CommandSpecAnalysisResult
{
    public ITypeSymbol ValidationResult { get; }

    public IMethodSymbol CanExecuteMethod { get; }

    public ValidatableCommandSpecAnalysisResult(
        INamedTypeSymbol baseInterface,
        INamedTypeSymbol declaredInterface,
        INamedTypeSymbol implementedClass,
        IMethodSymbol executeMethod,
        IMethodSymbol canExecuteMethod,
        ITypeSymbol param,
        ITypeSymbol result,
        ITypeSymbol validationResult,
        SyntaxTrivia xmlComments,
        AttributeData? plasticCommandAttribute)
        : base(baseInterface, declaredInterface, implementedClass,
            executeMethod, param, result, plasticCommandAttribute, xmlComments)
    {
        this.CanExecuteMethod = canExecuteMethod;
        this.ValidationResult = validationResult;
    }
}
