namespace PlasticCommand.Generator;

internal record GeneratedCommandInfo
{
    public string CommandSpecFullName { get; }
    public string GeneratedCommandFullName { get; }
    public string GeneratedCommandInterfaceFullName { get; }

    public GeneratedCommandInfo(
        string generatedCommandName, string generatedCommandInterface,
        string commandSpecName)
    {
        this.GeneratedCommandFullName = generatedCommandName;
        this.GeneratedCommandInterfaceFullName = generatedCommandInterface;
        this.CommandSpecFullName = commandSpecName;
    }
}
