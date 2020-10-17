using IntegratedComputerAidedDesignSystem.Infrastructure;
using System.IO;
using System.Threading.Tasks;

namespace IntegratedComputerAidedDesignSystem.NETCoreConsole
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            string text;

            using (StreamReader reader = new StreamReader("shema1_cl90.net"))
            {
                text = await reader.ReadToEndAsync();
            }

            var parser = new CalayParser();
            var (components, nodes) = parser.Parse(text);

            var (qMatrix, rMatrix) = Matrix.GetQAndRMatrix(components, nodes);
            
        }
    }
}