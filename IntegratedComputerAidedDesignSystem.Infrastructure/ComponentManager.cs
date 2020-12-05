using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    public class ComponentManager
    {
        public Component[] GetComponents(string text)
        {
            var parser = new Parser(text);
            var (components, _) = parser.Parse();

            return components;
        }
    }
}