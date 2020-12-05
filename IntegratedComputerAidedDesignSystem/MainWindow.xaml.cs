using IntegratedComputerAidedDesignSystem.Infrastructure;
using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IntegratedComputerAidedDesignSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string? _text;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Net Files (NET)|*.NET"
            };

            if (openFileDialog.ShowDialog() != true) return;

            _text = await ReadFileAsync(openFileDialog.FileName);
        }

        private static void RenderGrid(Grid grid, MatrixInfo matrixInfo)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            grid.ShowGridLines = true;

            for (var i = 0; i < matrixInfo.Columns.Length + 1; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(60),
                });
            }

            for (var i = 0; i < matrixInfo.Rows.Length + 1; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(35)
                });
            }

            for (var i = 0; i < matrixInfo.Columns.Length; i++)
            {
                var column = matrixInfo.Columns[i];
                TextBlock txtBlock = new TextBlock
                {
                    Text = column,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.Black),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                Grid.SetRow(txtBlock, 0);
                Grid.SetColumn(txtBlock, i + 1);

                grid.Children.Add(txtBlock);
            }

            for (var i = 0; i < matrixInfo.Rows.Length; i++)
            {
                var row = matrixInfo.Rows[i];
                TextBlock txtBlock = new TextBlock
                {
                    Text = row,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.Black),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                Grid.SetRow(txtBlock, i + 1);
                Grid.SetColumn(txtBlock, 0);

                grid.Children.Add(txtBlock);
            }

            for (var i = 0; i < matrixInfo.Matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrixInfo.Matrix.GetLength(1); j++)
                {
                    TextBlock txtBlock = new TextBlock
                    {
                        Text = matrixInfo.Matrix[i, j].ToString(),
                        FontSize = 14,
                        Foreground = new SolidColorBrush(Colors.Black),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    Grid.SetRow(txtBlock, i + 1);
                    Grid.SetColumn(txtBlock, j + 1);

                    grid.Children.Add(txtBlock);
                }
            }
        }

        private static async Task<string> ReadFileAsync(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            return await reader.ReadToEndAsync();
        }

        private void RenderQMatrix(object sender, RoutedEventArgs e) => RenderMatrix(MatrixType.Q);
        private void RenderRMatrix(object sender, RoutedEventArgs e) => RenderMatrix(MatrixType.R);

        private void RenderGraph(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_text)) return;

            AlgGraph algGraph = new AlgGraph(_text);
            algGraph.Show();
        }

        private void RenderMatrix(MatrixType matrixType)
        {
            if (_text == null) return;

            var manager = new MatrixManager(_text);
            var matrixInfo = manager.GetMatrixInfo(matrixType);

            RenderGrid(Grid, matrixInfo);

            MatrixNameLabel.Content = matrixType + " = ";
        }
    }
}