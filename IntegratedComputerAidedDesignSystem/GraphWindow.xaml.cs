using IntegratedComputerAidedDesignSystem.Infrastructure;
using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using System.Windows;
using Node = Microsoft.Msagl.Drawing.Node;

namespace IntegratedComputerAidedDesignSystem
{
    public partial class AlgGraph : Window
    {
        public AlgGraph(string text)
        {
            InitializeComponent();

            Graph graph = new Graph("graph");

            var componentManager = new ComponentManager();
            var components = componentManager.GetComponents(text);

            RenderGraph(graph, components);

            gViewer.Graph = graph;
        }

        private static void RenderGraph(Graph graph, Component[] components)
        {
            foreach (var component in components)
            {
                foreach (var output in component.Outputs)
                {
                    var fullNameOutput = $"{component.Name}:{output.Name}";
                    var nodeName = output.Node.Name;
                    
                    graph.AddEdge(component.Name, fullNameOutput);
                    graph.AddEdge(fullNameOutput, nodeName);

                    var nodeNode = graph.FindNode(nodeName);
                    nodeNode.Attr.FillColor = Color.YellowGreen;
                    
                    var outputNode = graph.FindNode(fullNameOutput);
                    outputNode.Attr.FillColor = Color.Beige;
                }

                Node? componentNode = graph.FindNode(component.Name);
                componentNode.Attr.FillColor = Color.LightBlue;
                componentNode.Attr.Padding = 100;
                componentNode.Attr.Shape = Shape.Circle;
            }
        }
    }
}