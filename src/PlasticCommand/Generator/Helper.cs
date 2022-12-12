using System.IO;
using System.Reflection;
using System.Text;

namespace PlasticCommand.Generator;

internal static class Helper
{
    public static string ReadEmbeddedResourceAsString(string resourceName)
    {
        using Stream resourceStream = Assembly.GetExecutingAssembly()
                                                            .GetManifestResourceStream(resourceName);

        using var reader = new StreamReader(resourceStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}