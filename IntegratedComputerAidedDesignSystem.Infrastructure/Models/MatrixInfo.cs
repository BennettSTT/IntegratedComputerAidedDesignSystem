namespace IntegratedComputerAidedDesignSystem.Infrastructure.Models
{
    public class MatrixInfo
    {
        public MatrixInfo(string[] rows, string[] columns, int[,] matrix)
        {
            Rows = rows;
            Columns = columns;
            Matrix = matrix;
        }

        public string[] Rows { get; }

        public string[] Columns { get; }

        public int[,] Matrix { get; }
    }
}