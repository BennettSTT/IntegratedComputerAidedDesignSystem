using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntegratedComputerAidedDesignSystem
{
    public enum FileFormat
    {
        Allegro = 1,
        Calay
    }

    /// <summary>
    /// E - компонент
    /// </summary>
    public class Component
    {
        public string Name { get; set; }

        public List<Output> Outputs { get; } = new List<Output>();

        public bool Find(Node node) => Outputs.Any(output => output.Node.Name == node.Name);
    }

    /// <summary>
    /// C - Вывод из компонента
    /// </summary>
    public class Output
    {
        public string Name { get; set; }

        public Node Node { get; set; }
    }

    /// <summary>
    /// V - узел
    /// </summary>
    public class Node
    {
        public string Name { get; set; }
    }
    
    public class CalayParser
    {
        public (Component[] components, Node[] nodes) Parse(string text)
        {
            Dictionary<string, Component> components = new Dictionary<string, Component>();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            string[] rows = text.Replace(Environment.NewLine, string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                string[] elements = row.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (elements.Length <= 1)
                {
                    throw new Exception();
                }

                var node = new Node { Name = elements[0] };
                
                nodes.Add(node.Name, node);

                for (int i = 1; i < elements.Length; i++)
                {
                    string element = elements[i];

                    var res = element.Split(new[] { '(', '\'', ')' }, StringSplitOptions.RemoveEmptyEntries);

                    if (res.Length != 2)
                    {
                        throw new Exception();
                    }

                    var componentName = res[0];
                    if (!components.TryGetValue(componentName, out var component))
                    {
                        component = new Component { Name = componentName };
                        components.Add(component.Name, component);
                    }

                    string outputName = res[1];

                    var output = new Output { Name = outputName, Node = node };

                    component.Outputs.Add(output);
                }
            }

            return (components.Values.ToArray(), nodes.Values.ToArray());
        }
    }
}