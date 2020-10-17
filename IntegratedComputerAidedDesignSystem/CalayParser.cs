using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem
{
    public class CalayParser
    {
        public (Component[] components, Node[] nodes) Parse(string text)
        {
            Dictionary<string, Component> components = new Dictionary<string, Component>();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            string[] rows = text.Replace(Environment.NewLine, string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                string[] rowEntries = row.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (rowEntries.Length <= 1)
                {
                    throw new Exception();
                }

                var node = new Node { Name = rowEntries[0] };
                
                nodes.Add(node.Name, node);

                for (int i = 1; i < rowEntries.Length; i++)
                {
                    string rowEntry = rowEntries[i];

                    var rowEntryEntries = rowEntry.Split(new[] { '(', '\'', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rowEntryEntries.Length != 2)
                    {
                        throw new Exception();
                    }

                    var componentName = rowEntryEntries[0];
                    if (!components.TryGetValue(componentName, out var component))
                    {
                        component = new Component { Name = componentName };
                        components.Add(component.Name, component);
                    }

                    string outputName = rowEntryEntries[1];
                    var output = new Output { Name = outputName, Node = node };

                    component.Outputs.Add(output);
                }
            }

            return (components.Values.ToArray(), nodes.Values.ToArray());
        }
    }
}