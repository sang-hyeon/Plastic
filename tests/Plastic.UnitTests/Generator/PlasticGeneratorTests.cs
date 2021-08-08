namespace Plastic.UnitTests.Generator
{
    using System.Collections.Immutable;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Plastic.Generators;
    using Xunit;

    public class PlasticGeneratorTests
    {
        private const string DATA_PREFIX = "Plastic.UnitTests.Generator.TestSet.";

        [Fact]
        public void Generator_does_not_generate_a_command_if_CommandSpec_is_abstract_class()
        {
            // Arrange
            string inputCode = ReadEmbeddedResourceAsString(DATA_PREFIX + "CommandSpec_Abstract.cs");
            var sut = new PlasticGenerator();

            // Act
            (Compilation _,
             ImmutableArray<Diagnostic> _,
             GeneratorDriverRunResult runResult) = TryGenerate(sut, inputCode);

            // Assert
            runResult.GeneratedTrees.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void Generator_does_not_generate_a_command_if_CommandSpec_is_inner_class()
        {
            // Arrange
            string inputCode = ReadEmbeddedResourceAsString(DATA_PREFIX + "CommandSpec_InnerClass.cs");
            var sut = new PlasticGenerator();

            // Act
            (Compilation _,
             ImmutableArray<Diagnostic> _,
             GeneratorDriverRunResult runResult) = TryGenerate(sut, inputCode);

            // Assert
            runResult.GeneratedTrees.IsEmpty.Should().BeTrue();
        }

        #region Utils

        private static (Compilation, ImmutableArray<Diagnostic>, GeneratorDriverRunResult) TryGenerate(
            ISourceGenerator generator, string inputCode)
        {
            Compilation inputCompilation = CreateCompilation(inputCode);
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(
                inputCompilation, out Compilation outputCompilation, out ImmutableArray<Diagnostic> diagnostics);

            return (outputCompilation, diagnostics, driver.GetRunResult());
        }

        private static Compilation CreateCompilation(string source)
                => CSharpCompilation.Create("compilation",
                    new[] { CSharpSyntaxTree.ParseText(source) },
                    new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                    new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        private static string ReadEmbeddedResourceAsString(string resourceName)
        {
            using Stream resourceStream = Assembly.GetExecutingAssembly()
                                                                .GetManifestResourceStream(resourceName)!;

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        #endregion
    }
}
