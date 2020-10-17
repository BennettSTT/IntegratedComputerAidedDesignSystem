using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    /// <summary>
    /// E - компонент
    /// </summary>
    public class Component
    {
        public string Name { get; set; }

        public List<Output> Outputs { get; } = new List<Output>();

        public int GetCount(Node node) => Outputs.Count(output => output.Node.Name == node.Name);
    }
}