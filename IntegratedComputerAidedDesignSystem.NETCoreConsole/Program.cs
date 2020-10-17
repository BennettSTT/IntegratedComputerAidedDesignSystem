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

            using (StreamReader reader = new StreamReader("allegro_1.NET"))
            {
                text = await reader.ReadToEndAsync();
            }

            var manager = new MatrixManager(text);

            var q = manager.GetMatrixInfo(MatrixType.Q);
            var r = manager.GetMatrixInfo(MatrixType.R);
        }
    }
}