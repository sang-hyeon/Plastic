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
        private const string COMMAND_TEMPLATE =
            "Plastic.Generators.Templates.CommandTemplate.CommandTemplate.txt";

        private const string INITIALIZER_TEMPLATE =
            "Plastic.Generators.Templates.InitializerTemplate.PlasticInitializerTemplate.txt";

        private static readonly string ICOMMAND_SPEC_FULL_NAME
            = typeof(ICommandSpecification<,>).FullName;

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (SyntaxReceiver)context.SyntaxContextReceiver!;
            if (receiver.Targets.Count <= 0)
                return;

            string commandTemplate = ReadEmbeddedResourceAsString(COMMAND_TEMPLATE);
            var generatedCommands = new HashSet<GeneratedCommandInfo>();
            
            foreach (TypeDeclarationSyntax userCommandSpec in receiver.Targets)
            {
                GeneratedCommandInfo? generatedCommandInfo =
                                GenerateCommands(context, userCommandSpec, commandTemplate);

                if (generatedCommandInfo != null)
                    generatedCommands.Add(generatedCommandInfo);
            }

            GeneratePlasticInitializer(context, generatedCommands);
        }

        private static GeneratedCommandInfo? GenerateCommands(
            GeneratorExecutionContext contextToAdd, TypeDeclarationSyntax userCommandSpec, string commandTemplate)
        {
            SemanticModel model = contextToAdd.Compilation.GetSemanticModel(userCommandSpec.SyntaxTree);
            INamedTypeSymbol originalInterface =
                                        model.Compilation.GetTypeByMetadataName(ICOMMAND_SPEC_FULL_NAME)!;

            if (model.GetDeclaredSymbol(userCommandSpec) is INamedTypeSymbol userCommandSpecSymbol)
            {
                string commandNameGenerated = GenerateCommandName(userCommandSpecSymbol);

                INamedTypeSymbol commandSpecInterface =
                    userCommandSpecSymbol.AllInterfaces.First(q => q.ConstructedFrom == originalInterface);

                ITypeSymbol paramSymbol = commandSpecInterface.TypeArguments[0];
                ITypeSymbol responseSymbol = commandSpecInterface.TypeArguments[1];

                var commandBuilder = new StringBuilder(commandTemplate);
                commandBuilder.Replace("Plastic.TTFFCommandSpec", userCommandSpecSymbol.ToString());
                commandBuilder.Replace("TTFFCommand", commandNameGenerated);
                commandBuilder.Replace("Plastic.TTFFParameter", paramSymbol.ToString());
                commandBuilder.Replace("Plastic.TTFFResponse", responseSymbol.ToString());

                contextToAdd.AddSource($"{commandNameGenerated}.cs", commandBuilder.ToString());

                return new GeneratedCommandInfo(commandNameGenerated, userCommandSpecSymbol.ToString());
            }
            else
                return default;
        }

        private static string GenerateCommandName(INamedTypeSymbol userCommandSpecSymbol)
        {
            string attributeName = typeof(CommandNameAttribute).FullName;
            AttributeData? commandNameAtt = userCommandSpecSymbol
                                                                    .GetAttributes()
                                                                    .FirstOrDefault(att => att.AttributeClass?.ToString() == attributeName);

            if (commandNameAtt?.ConstructorArguments.FirstOrDefault().Value is string commandName)
            {
                return commandName;
            }
            else
                return userCommandSpecSymbol.Name.Replace("CommandSpec", string.Empty) + "Command";
        }

        private static void GeneratePlasticInitializer(
            GeneratorExecutionContext contextToAdd, ICollection<GeneratedCommandInfo> generatedCommands)
        {
            string template = ReadEmbeddedResourceAsString(INITIALIZER_TEMPLATE);

            var builder = new StringBuilder();
            foreach (GeneratedCommandInfo commandName in generatedCommands)
            {
                builder.AppendLine($"\t\t\tadding.Invoke(typeof({commandName.CommandSpecFullName}));");
                builder.AppendLine($"\t\t\tadding.Invoke(typeof({commandName.GeneratedCommandName}));");
            }

            string generatedCode = template.Replace("{{AddServices}}", builder.ToString());
            contextToAdd.AddSource("PlasticInitializer.cs", generatedCode);
        }

        private static string ReadEmbeddedResourceAsString(string resourceName)
        {
            using Stream resourceStream = Assembly.GetExecutingAssembly()
                                                                .GetManifestResourceStream(resourceName);

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            return reader.ReadToEnd();
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
                            context.SemanticModel.Compilation.GetTypeByMetadataName(ICOMMAND_SPEC_FULL_NAME)!;
                        
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
