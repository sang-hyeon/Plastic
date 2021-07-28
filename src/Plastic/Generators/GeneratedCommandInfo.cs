namespace Plastic.Generators
{
    internal record GeneratedCommandInfo
    {
        public readonly string CommandSpecFullName;
        public readonly string GeneratedCommandName;

        public GeneratedCommandInfo(string generatedCommandName, string commandSpecName)
        {
            this.GeneratedCommandName = generatedCommandName;
            this.CommandSpecFullName = commandSpecName;
        }
    }
}
