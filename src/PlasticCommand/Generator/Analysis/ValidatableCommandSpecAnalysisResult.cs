using Microsoft.CodeAnalysis;

namespace PlasticCommand.Generator.Analysis;

internal class ValidatableCommandSpecAnalysisResult : CommandSpecAnalysisResult
{
    public ITypeSymbol ValidationResult { get; }

    public ValidatableCommandSpecAnalysisResult(
        INamedTypeSymbol baseInterface,
        INamedTypeSymbol declaredInterface,
        INamedTypeSymbol implementedClass,
        ITypeSymbol param,
        ITypeSymbol result,
        ITypeSymbol validationResult,
        AttributeData? plasticCommandAttribute)
        : base(baseInterface, declaredInterface,
                    implementedClass, param, result, plasticCommandAttribute)
    {
        this.ValidationResult = validationResult;
    }
}
