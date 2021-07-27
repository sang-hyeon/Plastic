namespace Plastic
{
    using System;

    public delegate object? GetService(Type service);

    public delegate void AddService(Type type);
}
