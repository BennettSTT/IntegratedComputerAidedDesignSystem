using IntegratedComputerAidedDesignSystem.Infrastructure;
using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers;
using System.IO;
using System.Threading.Tasks;

namespace IntegratedComputerAidedDesignSystem.NETCoreConsole
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            string text;

            using (StreamReader reader = new StreamReader("allegro_1.NET"))
            {
                text = await reader.ReadToEndAsync();
            }

            var parser = new Parser(text);
            var (components, nodes) = parser.Parse();

            var (qMatrix, rMatrix) = Matrix.GetQAndRMatrix(components, nodes);
        }
    }
}