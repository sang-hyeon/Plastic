namespace Plastic
{
    using Plastic.Commands;

    internal static class PlasticInitializer
    {
        public static void AddGeneratedCommands(AddService adding)
        {
            adding.Invoke(typeof(TTFFCommand));
        }
    }
}
