using System;

namespace PlasticCommand;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PlasticCommandAttribute : Attribute
{
    public string? GeneratedCommandName { get; set; }

    public string? GroupName { get; set; }
}
