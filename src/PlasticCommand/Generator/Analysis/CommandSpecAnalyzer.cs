using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace PlasticCommand.Generator.Analysis;

internal class CommandSpecAnalyzer
{
    protected virtual Type GetRootCommandSpecType()
    {
        return typeof(ICommandSpecification<,>);
    }

    public CommandSpecAnalysisResult? Analyze(
        GeneratorExecutionContext context,
        TypeDeclarationSyntax implementedCommandSpecSyntax)
    {
        INamedTypeSymbol? rootCommandSpecInterface = GetRootCommandSpecSymbol(context);
        if (rootCommandSpecInterface == null)
            return default;

        SemanticModel model = GetModel(context, implementedCommandSpecSyntax);
        return OnAnalyze(implementedCommandSpecSyntax, rootCommandSpecInterface, model);
    }

    protected virtual CommandSpecAnalysisResult? OnAnalyze(
        TypeDeclarationSyntax implementedCommandSpecSyntax,
        INamedTypeSymbol rootCommandSpecInterface,
        SemanticModel model)
    {
        INamedTypeSymbol? implementedCommandSpec =
            model.GetDeclaredSymbol(implementedCommandSpecSyntax);

        if (implementedCommandSpec == null)
            return default;

        if (IsSubclass(rootCommandSpecInterface, implementedCommandSpec) == false)
            return default;

        INamedTypeSymbol declaredInterface =
            GetDeclaredInterface(implementedCommandSpec, rootCommandSpecInterface)!;

        IMethodSymbol executeMethod
            = GetImplementedExecuteMethod(declaredInterface, implementedCommandSpec);

        return new CommandSpecAnalysisResult(
                rootCommandSpecInterface,
                declaredInterface,
                implementedCommandSpec,
                executeMethod,
                declaredInterface.TypeArguments[0],
                declaredInterface.TypeArguments[1],
                FindAttribute(implementedCommandSpec),
                GetXmlCommentsIdOfExecuteMethod(executeMethod)
            );
    }

    private static bool IsSubclass(
        INamedTypeSymbol rootCommandSpecInterface,
        INamedTypeSymbol implementedCommandSpec)
    {
        return implementedCommandSpec
            .AllInterfaces
            .Any(x => x.ConstructedFrom.SymbolEquals(rootCommandSpecInterface));
    }

    private INamedTypeSymbol? GetRootCommandSpecSymbol(GeneratorExecutionContext context)
    {
        Type rootCommandSpecInterfaceType = GetRootCommandSpecType();
        return context.Compilation.GetTypeByMetadataName(rootCommandSpecInterfaceType.FullName);
    }

    private AttributeData? FindAttribute(INamedTypeSymbol implementedCommand)
    {
        var attributeName = typeof(PlasticCommandAttribute).FullName;
        return implementedCommand
                    .GetAttributes()
                    .FirstOrDefault(att => att.AttributeClass?.ToString() == attributeName);
    }

    private IMethodSymbol GetImplementedExecuteMethod(
        INamedTypeSymbol declaredInterface, INamedTypeSymbol implementedCommandSpec)
    {
        ISymbol executeMethodContract =
            declaredInterface.GetMembers("ExecuteAsync").Any()
            ? declaredInterface.GetMembers("ExecuteAsync")[0]
            : declaredInterface.AllInterfaces.SelectMany(q => q.GetMembers("ExecuteAsync")).First();

        ISymbol executeMethodImpl =
            implementedCommandSpec.FindImplementationForInterfaceMember(executeMethodContract)!;

        return (IMethodSymbol)executeMethodImpl;
    }

    private string GetXmlCommentsIdOfExecuteMethod(IMethodSymbol implementedMethod)
    {
        return implementedMethod.GetDocumentationCommentId()!.Replace("M:", "");
    }

    private static INamedTypeSymbol? GetDeclaredInterface(
        INamedTypeSymbol implementedCommandSpec,
        INamedTypeSymbol rootCommandSpecInterface)
    {
        return implementedCommandSpec
                    .AllInterfaces
                    .FirstOrDefault(x => x.ConstructedFrom.SymbolEquals(rootCommandSpecInterface));
    }

    public static SemanticModel GetModel(
        GeneratorExecutionContext context, TypeDeclarationSyntax syntax)
    {
        return context.Compilation.GetSemanticModel(syntax.SyntaxTree);
    }
}
