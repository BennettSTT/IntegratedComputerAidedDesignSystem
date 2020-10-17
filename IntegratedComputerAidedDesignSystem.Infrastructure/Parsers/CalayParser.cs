using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem.Infrastructure.Parsers
{
    internal class CalayParser : IFormatParser
    {
        public (Component[] components, Node[] nodes) Parse(string text)
        {
            var components = new Dictionary<string, Component>();
            var nodes = new Dictionary<string, Node>();

            var rows = text.Replace(Environment.NewLine, string.Empty)
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
            {
                var rowEntries = row.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (rowEntries.Length <= 1)
                {
                    throw new Exception();
                }

                var node = new Node { Name = rowEntries[0] };

                nodes.Add(node.Name, node);

                for (var i = 1; i < rowEntries.Length; i++)
                {
                    var rowEntry = rowEntries[i];

                    var rowEntryEntries =
                        rowEntry.Split(new[] { '(', '\'', ')' }, StringSplitOptions.RemoveEmptyEntries);
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

                    var outputName = rowEntryEntries[1];
                    var output = new Output { Name = outputName, Node = node };

                    component.Outputs.Add(output);
                }
            }

            return (components.Values.ToArray(), nodes.Values.ToArray());
        }
    }
}