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
            parser.Parse(text);
        }
    }
}