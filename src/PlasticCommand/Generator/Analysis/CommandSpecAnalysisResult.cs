using Microsoft.CodeAnalysis;

namespace PlasticCommand.Generator.Analysis;

internal class CommandSpecAnalysisResult
{
    public INamedTypeSymbol BaseInterface { get; }

    public INamedTypeSymbol DeclaredInterface { get; }

    public INamedTypeSymbol ImplementedClass { get; }

    public IMethodSymbol ExecuteMethod { get; }

    public ITypeSymbol Param { get; }

    public ITypeSymbol Result { get; }

    public AttributeData? PlasticCommandAttribute { get; }

    public string XmlCommentsId { get; }

    public CommandSpecAnalysisResult(
        INamedTypeSymbol baseInterface,
        INamedTypeSymbol declaredInterface,
        INamedTypeSymbol implementedClass,
        IMethodSymbol executeMethod,
        ITypeSymbol param,
        ITypeSymbol result,
        AttributeData? plasticCommandAttribute,
        string commentXml)
    {
        this.BaseInterface = baseInterface;
        this.DeclaredInterface = declaredInterface;
        this.ImplementedClass = implementedClass;
        this.ExecuteMethod = executeMethod;
        this.Param = param;
        this.Result = result;
        this.PlasticCommandAttribute = plasticCommandAttribute;
        this.XmlCommentsId = commentXml;
    }
}
