namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    public static class Matrix
    {
        public static int[,] GetQMatrix(Component[] components, Node[] nodes)
        {
            var qMatrix = new int[components.Length, nodes.Length];

            for (var i = 0; i < components.Length; i++)
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

            var rMatrix = new int[qMatrix.GetLength(0), qMatrix.GetLength(0)];

            return rMatrix;
        }

        private static int[,] Transpose(int[,] matrix)
        {
            var w = matrix.GetLength(0);
            var h = matrix.GetLength(1);

            var transposeMatrix = new int[h, w];

            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    transposeMatrix[j, i] = matrix[i, j];
                }
            }

            return transposeMatrix;
        }
    }
}