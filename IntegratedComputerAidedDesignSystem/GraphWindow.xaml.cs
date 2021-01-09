using IntegratedComputerAidedDesignSystem.Infrastructure;
using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Incremental;
using System.Collections.Generic;
using System.Windows;
using Node = Microsoft.Msagl.Drawing.Node;

namespace IntegratedComputerAidedDesignSystem
{
    public partial class AlgGraph : Window
    {
        public AlgGraph(string text, int batchCount, int vertexCount)
        {
            InitializeComponent();

            Graph graph = new Graph("graph");

            var parser = new Parser(text);
            var (components, nodes) = parser.Parse();

            var calculator = new MatrixCalculator(components, nodes);

            var matrix = calculator.GetMatrix(MatrixType.R);

            var result = Algorithm.Run(components, matrix, batchCount, vertexCount);

            RenderGraph(graph, result);

            gViewer.Graph = graph;
        }

        private static void RenderGraph(Graph graph, List<Component[]> list)
        {
            graph.LayoutAlgorithmSettings = new FastIncrementalLayoutSettings();
            Color[] colors =
            {
                Color.YellowGreen,
                Color.LightBlue,
                Color.Aqua,
                Color.SeaShell,
                Color.MediumVioletRed,
                Color.Firebrick,
                Color.Magenta,
                Color.Orange,
                Color.Silver,
                Color.DarkKhaki,
            };

            int index = 0;
            foreach (Component[] components in list)
            {
                Color color = colors[index];

                foreach (var component in components)
                {
                    foreach (var output in component.Outputs)
                    {
                        var fullNameOutput = $"{component.Name}:{output.Name}";
                        var nodeName = output.Node.Name;

                        graph.AddEdge(component.Name, fullNameOutput);
                        graph.AddEdge(fullNameOutput, nodeName);

                        var nodeNode = graph.FindNode(nodeName);
                        nodeNode.Attr.FillColor = color;

                        var outputNode = graph.FindNode(fullNameOutput);
                        outputNode.Attr.FillColor = color;
                    }

                    Node? componentNode = graph.FindNode(component.Name);
                    componentNode.Attr.FillColor = color;
                    componentNode.Attr.Padding = 100;
                }

                index++;
            }
        }
    }
}