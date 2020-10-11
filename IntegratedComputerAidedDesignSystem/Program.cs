using System.IO;
using System.Threading.Tasks;

namespace IntegratedComputerAidedDesignSystem
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string text;
            
            using (StreamReader reader = new StreamReader("shema1_cl90.net"))
            {
                text = await reader.ReadToEndAsync();
            }
            
            var parser = new CalayParser(); 
            (Component[] components, Node[] nodes) = parser.Parse(text);
            
            int[,] Q = new int[components.Length, nodes.Length];

            for (int i = 0; i < components.Length; i++)
            {
                for (var j = 0; j < nodes.Length; j++)
                {
                    Q[i, j] = components[i].Find(nodes[j]) ? 1 : 0;
                }
            }
        }
    }
}