using System;

namespace PlasticCommand;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class GeneratedByPlasticCommandAttribute : Attribute
{
    public GeneratedByPlasticCommandAttribute()
    {
    }
}
