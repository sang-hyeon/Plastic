﻿using Microsoft.CodeAnalysis;

namespace PlasticCommand.Generator.Analysis;

internal class CommandSpecAnalysisResult
{
    public INamedTypeSymbol BaseInterface { get; }

    public INamedTypeSymbol DeclaredInterface { get; }

    public INamedTypeSymbol ImplementedClass { get; }

    public ITypeSymbol Param { get; }

    public ITypeSymbol Result { get; }

    public AttributeData? PlasticCommandAttribute { get; }

    public CommandSpecAnalysisResult(
        INamedTypeSymbol baseInterface,
        INamedTypeSymbol declaredInterface,
        INamedTypeSymbol implementedClass,
        ITypeSymbol param,
        ITypeSymbol result,
        AttributeData? plasticCommandAttribute)
    {
        this.BaseInterface = baseInterface;
        this.DeclaredInterface = declaredInterface;
        this.ImplementedClass = implementedClass;
        this.Param = param;
        this.Result = result;
        this.PlasticCommandAttribute = plasticCommandAttribute;
    }
}
