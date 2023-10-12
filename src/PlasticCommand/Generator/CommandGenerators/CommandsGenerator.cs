using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticCommand.Generator.CommandGenerators;

internal class CommandsGenerator
{
    public const string PROJECT_NAME = "PlasticCommand";
    protected const string TEMPLATE_NAME =
        PROJECT_NAME + ".Generator.Templates.CommandsTemplate.CommandsTemplate.txt";

    private readonly GeneratorExecutionContext _context;

    public CommandsGenerator(GeneratorExecutionContext context)
    {
        this._context = context;
    }

    public HashSet<string> Generate(IEnumerable<GeneratedCommandInfo> generatedCommands)
    {
        HashSet<string> generatedCommandGroups = new();
        IEnumerable<IGrouping<INamespaceSymbol, GeneratedCommandInfo>> commandGroups;
        commandGroups = GroupCommandByNamespace(generatedCommands);

        foreach (IGrouping<INamespaceSymbol, GeneratedCommandInfo> group in commandGroups)
        {
            string template = Helper.ReadEmbeddedResourceAsString(TEMPLATE_NAME);
            var builder = new StringBuilder(template);

            builder.Replace("TTFFNamespace", group.Key.ToDisplayString());
            builder.Replace("TTFFCommands", group.Key.Name + "Commands");

            string memberCode = MakeMemberPropertyCode(group);
            builder.Replace("{{Members}}", memberCode);

            (string argCode, string initCode) = MakeConstructorCode(group);
            builder.Replace("{{Arguments}}", argCode);
            builder.Replace("{{Init}}", initCode);

            string functionCode = MakeFunctionCode(group);
            builder.Replace("{{Methods}}", functionCode);

            this._context.AddSource($"{group.Key.ToDisplayString()}.cs", builder.ToString());
            generatedCommandGroups.Add($"{group.Key.ToDisplayString()}.{group.Key.Name}Commands");
        }

        return generatedCommandGroups;
    }

    private string MakeFunctionCode(IGrouping<INamespaceSymbol, GeneratedCommandInfo> group)
    {
        var builder = new StringBuilder();
        foreach (GeneratedCommandInfo command in group)
        {
            string name = MakeMemberPropertyName(command);
            string methodName = name.Replace("Command", "Async");
            string returnTypeName = command.AnalysisResult.ExecuteMethod.ReturnType.ToDisplayString();
            string paramTypeName = command.AnalysisResult.ExecuteMethod.Parameters[0].ToDisplayString();
            string paramArgName = command.AnalysisResult.ExecuteMethod.Parameters[0].Name;
            string cancelTypeName = command.AnalysisResult.ExecuteMethod.Parameters[1].ToDisplayString();
            builder.AppendLine($"\t\tpublic {returnTypeName} {methodName}({paramTypeName}, {cancelTypeName} = default)");
            builder.AppendLine("\t\t{");
            builder.AppendLine($"\t\t\treturn this.{name}.ExecuteAsync({paramArgName}, token);");
            builder.AppendLine("\t\t}");
            builder.AppendLine();
        }

        return builder.ToString();
    }

    private (string args, string init) MakeConstructorCode(
        IGrouping<INamespaceSymbol, GeneratedCommandInfo> group)
    {
        List<(string argName, GeneratedCommandInfo info)> argsInfo = new();
        var builder = new StringBuilder();
        foreach (GeneratedCommandInfo command in group)
        {
            if (builder.Length == 0)
                builder.Append("\t\t\t");
            else if (builder.Length != 0)
                builder.Append(", ");

            var name = command.GeneratedCommandInterfaceFullName.Split('.').Last();
            name = name.TrimStart('I').ToLower();
            builder.Append($"{command.GeneratedCommandInterfaceFullName} {name}");
            argsInfo.Add((name, command));
        }

        var initBuilder = new StringBuilder();
        foreach ((string argName, GeneratedCommandInfo info) in argsInfo)
        {
            string name = MakeMemberPropertyName(info);
            initBuilder.AppendLine($"\t\t\tthis.{name} = {argName};");
        }

        return (builder.ToString(), initBuilder.ToString());
    }

    private string MakeMemberPropertyCode(
        IGrouping<INamespaceSymbol, GeneratedCommandInfo> group)
    {
        var builder = new StringBuilder();
        foreach (GeneratedCommandInfo command in group)
        {
            var name = MakeMemberPropertyName(command);
            builder.AppendLine($"\t\tprivate readonly {command.GeneratedCommandInterfaceFullName} {name};");
        }

        return builder.ToString();
    }

    private static string MakeMemberPropertyName(GeneratedCommandInfo command)
    {
        var name = command.GeneratedCommandInterfaceFullName.Split('.').Last();
        name = name.TrimStart('I');
        return name;
    }

    private IEnumerable<IGrouping<INamespaceSymbol, GeneratedCommandInfo>> GroupCommandByNamespace(
        IEnumerable<GeneratedCommandInfo> generatedCommands)
    {
        IEnumerable<IGrouping<INamespaceSymbol, GeneratedCommandInfo>> grouped;

        grouped = generatedCommands
                            .GroupBy(q => q.AnalysisResult.ImplementedClass.ContainingNamespace);

        return grouped;
    }
}
