namespace IntegratedComputerAidedDesignSystem
{
    public class Matrix
    {
        public static int[,] GetQMatrix(Component[] components, Node[] nodes)
        {
            int[,] qMatrix = new int[components.Length, nodes.Length];

            for (int i = 0; i < components.Length; i++)
            {
                for (var j = 0; j < nodes.Length; j++)
                {
                    qMatrix[i, j] = components[i].Find(nodes[j]) ? 1 : 0;
                }
            }

            return qMatrix;
        }

        public static int[,] GetRMatrix(int[,] qMatrix)
        {
            var transposeQMatrix = Transpose(qMatrix);
            
            // Тут нужно умножить Q и Q'
            
            int[,] rMatrix = new int[qMatrix.GetLength(0), qMatrix.GetLength(0)];
            
            return rMatrix;
        }
        
        private static int[,] Transpose(int[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            int[,] result = new int[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }
    }
}