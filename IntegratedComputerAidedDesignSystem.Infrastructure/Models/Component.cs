using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem.Infrastructure.Models
{
    /// <summary>
    /// E - компонент
    /// </summary>
    public class Component
    {
        public Component(string name)
        {
            Name = name;

            Outputs = new List<Output>();
        }

        public string Name { get; }

        public List<Output> Outputs { get; }

        public bool Find(Node node) => Outputs.Any(output => output.Node.Name == node.Name);
    }
}