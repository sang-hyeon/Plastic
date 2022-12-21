using Microsoft.CodeAnalysis;

namespace PlasticCommand.Generator;

internal static class SymbolExtensions
{
    public static bool SymbolEquals(this ISymbol operand1, ISymbol operand2)
    {
        return SymbolEqualityComparer
                        .Default
                        .Equals(operand1, operand2);
    }
}
