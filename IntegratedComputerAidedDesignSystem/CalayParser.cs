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

    public class Microcircuit
    {
        public string Name { get; set; }

        public List<Output> Outputs { get; } = new List<Output>();
    }

    public class Output
    {
        public string Name { get; set; }

        public Node Node { get; set; }
    }

    public class Node
    {
        public string Name { get; set; }
    }
    
    
    public class CalayParser
    {
        public Microcircuit[] Parse(string text)
        {
            Dictionary<string, Microcircuit> microcircuitCache = new Dictionary<string, Microcircuit>();

            string[] rows = text.Replace(Environment.NewLine, string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                string[] elements = row.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (elements.Length <= 1)
                {
                    throw new Exception();
                }

                var node = new Node { Name = elements[0] };
                for (int i = 1; i < elements.Length; i++)
                {
                    string element = elements[i];

                    var res = element.Split(new[] { '(', '\'', ')' }, StringSplitOptions.RemoveEmptyEntries);

                    if (res.Length != 2)
                    {
                        throw new Exception();
                    }

                    Microcircuit microcircuit;
                    var microcircuitName = res[0];
                    if (!microcircuitCache.TryGetValue(microcircuitName, out microcircuit))
                    {
                        microcircuit = new Microcircuit { Name = microcircuitName };
                        microcircuitCache.Add(microcircuitName, microcircuit);
                    }

                    string outputName = res[1];

                    var output = new Output { Name = outputName, Node = node };

                    microcircuit.Outputs.Add(output);
                }
            }

            return microcircuitCache.Values.ToArray();
        }
    }
}