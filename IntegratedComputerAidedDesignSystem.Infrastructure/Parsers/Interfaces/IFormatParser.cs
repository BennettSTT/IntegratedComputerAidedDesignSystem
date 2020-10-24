using IntegratedComputerAidedDesignSystem.Infrastructure.Models;

namespace IntegratedComputerAidedDesignSystem.Infrastructure.Parsers.Interfaces
{
    internal interface IFormatParser
    {
        (Component[] components, Node[] nodes) Parse(string text);
    }
}