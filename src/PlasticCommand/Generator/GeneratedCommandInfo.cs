using PlasticCommand.Generator.Analysis;

namespace PlasticCommand.Generator;

internal record GeneratedCommandInfo
{
    public string CommandSpecFullName { get; }
    public string GeneratedCommandFullName { get; }
    public string GeneratedCommandInterfaceFullName { get; }

    public CommandSpecAnalysisResult AnalysisResult { get; }

    public GeneratedCommandInfo(
        string generatedCommandName, string generatedCommandInterface,
        string commandSpecName, CommandSpecAnalysisResult analysisResult)
    {
        this.GeneratedCommandFullName = generatedCommandName;
        this.GeneratedCommandInterfaceFullName = generatedCommandInterface;
        this.CommandSpecFullName = commandSpecName;
        this.AnalysisResult = analysisResult;
    }
}
