using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem
{
    /// <summary>
    /// E - компонент
    /// </summary>
    public class Component
    {
        public string Name { get; set; }

        public List<Output> Outputs { get; } = new List<Output>();

        public bool Find(Node node) => Outputs.Any(output => output.Node.Name == node.Name);
    }
}