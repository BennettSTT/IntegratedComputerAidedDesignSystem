using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    public class ComponentManager
    {
        public (Component[] components, Node[] nodes) GetComponents(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}