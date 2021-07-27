namespace Plastic.Generators
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1024:기호를 올바르게 비교", Justification = "분석기 버그입니다.")]
    [Generator]
    internal class PlasticGenerator : ISourceGenerator
    {
        public const string ICOMMAND_SPEC_NAME = "Plastic.ICommandSpecification`2";

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (SyntaxReceiver)context.SyntaxContextReceiver!;

            if (receiver.Targets.Count <= 0)
                return;

            string commandTemplate = GetCommandTemplate();
            var commandBuilder = new StringBuilder(commandTemplate);
            var objectNameToAddAsService = new HashSet<string>();
            
            foreach (TypeDeclarationSyntax commandSpec in receiver.Targets)
            {
                SemanticModel model = context.Compilation.GetSemanticModel(commandSpec.SyntaxTree);

                INamedTypeSymbol icommandSpecSymbol =
                                            model.Compilation.GetTypeByMetadataName(ICOMMAND_SPEC_NAME)!;

                var commandSpecSymbol = (INamedTypeSymbol)model.GetDeclaredSymbol(commandSpec) !;
                string commandName = commandSpecSymbol.Name.Replace("CommandSpec", string.Empty) + "Command";

                INamedTypeSymbol commandSpecInterface = commandSpecSymbol.AllInterfaces
                                                                            .First(q => q.ConstructedFrom == icommandSpecSymbol);

                ITypeSymbol paramSymbol = commandSpecInterface.TypeArguments[0];
                ITypeSymbol responseSymbol = commandSpecInterface.TypeArguments[1];

                commandBuilder.Replace("Plastic.TTFFCommandSpec", commandSpecSymbol.ToString());
                commandBuilder.Replace("TTFFCommand", commandName);
                commandBuilder.Replace("Plastic.TTFFParameter", paramSymbol.ToString());
                commandBuilder.Replace("Plastic.TTFFResponse", responseSymbol.ToString());

                context.AddSource($"{commandName}.cs", commandBuilder.ToString());
                objectNameToAddAsService.Add(commandName);
                objectNameToAddAsService.Add(commandSpecSymbol.ToString());
            }

            string generatedInitializer = GeneratePlasticInitializer(objectNameToAddAsService);
            context.AddSource("PlasticInitializer.cs", generatedInitializer);
        }

        private static string GetCommandTemplate()
        {
            using Stream templateStream =
                Assembly.GetExecutingAssembly()
                             .GetManifestResourceStream("Plastic.Generators.Templates.CommandTemplate.CommandTemplate.txt");

            using var reader = new StreamReader(templateStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private static string GeneratePlasticInitializer(ICollection<string> objectNameToAdd)
        {
            using Stream templateStream =
                Assembly.GetExecutingAssembly()
                             .GetManifestResourceStream("Plastic.Generators.Templates.InitializerTemplate.PlasticInitializerTemplate.txt");

            using var reader = new StreamReader(templateStream, Encoding.UTF8);
            string template = reader.ReadToEnd();

            var builder = new StringBuilder(objectNameToAdd.Count);
            foreach (string commandName in objectNameToAdd)
            {
                builder.AppendLine($"\t\t\tadding.Invoke(typeof({commandName}));");
            }

            return template.Replace("{{AddServices}}", builder.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<TypeDeclarationSyntax> Targets = new List<TypeDeclarationSyntax>();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is TypeDeclarationSyntax typeNode)
                {
                    ISymbol? symbole = context.SemanticModel.GetDeclaredSymbol(typeNode);
                    if (symbole is INamedTypeSymbol namedSymbol)
                    {
                        INamedTypeSymbol commandSpecSymbol =
                            context.SemanticModel.Compilation.GetTypeByMetadataName(ICOMMAND_SPEC_NAME)!;
                        
                        if (namedSymbol.AllInterfaces.Any(q => q.ConstructedFrom == commandSpecSymbol))
                        {
                            this.Targets.Add(typeNode);
                        }
                    }
                }
            }
        }
    }
}
