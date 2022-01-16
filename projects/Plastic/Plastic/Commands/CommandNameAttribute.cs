namespace Plastic
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CommandNameAttribute : Attribute
    {
        public string GeneratedCommandName { get; }

        public CommandNameAttribute(string generatedCommandName)
        {
            this.GeneratedCommandName = generatedCommandName;
        }
    }
}
