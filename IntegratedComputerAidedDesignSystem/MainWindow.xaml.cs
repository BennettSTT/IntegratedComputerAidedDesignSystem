using IntegratedComputerAidedDesignSystem.Infrastructure;
using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers;
using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;

namespace IntegratedComputerAidedDesignSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Net Files (NET)|*.NET"
            };

            if (openFileDialog.ShowDialog() != true) return;

            var text = await ReadFileAsync(openFileDialog.FileName);

            var manager = new MatrixManager(text);

            var qMatrixInfo = manager.GetMatrixInfo(MatrixType.Q);
            var rMatrixInfo = manager.GetMatrixInfo(MatrixType.R);
        }

        private static async Task<string> ReadFileAsync(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}