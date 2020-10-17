using System.Collections.Generic;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    public static class Matrix
    {
        public static (int[,] qMatrix, int[,] rMatrix) GetQAndRMatrix(Component[] components, Node[] nodes)
        {
            var qMatrix = GetQMatrix(components, nodes);
            var rMatrix = GetRMatrix(qMatrix);

            return (qMatrix, rMatrix);
        }

        private static int[,] GetQMatrix(IReadOnlyList<Component> components, IReadOnlyList<Node> nodes)
        {
            var qMatrix = new int[components.Count, nodes.Count];

            for (var i = 0; i < components.Count; i++)
            {
                for (var j = 0; j < nodes.Count; j++)
                {
                    qMatrix[i, j] = components[i].Find(nodes[j]) ? 1 : 0;
                }
            }

            return qMatrix;
        }

        private static int[,] GetRMatrix(int[,] qMatrix)
        {
            var transposeQMatrix = Transpose(qMatrix);

            var length = qMatrix.GetLength(1);
            var rMatrix = new int[length, length];

            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    var temp = 0;

                    for (var k = 0; k < length; k++)
                    {
                        temp += qMatrix[i, k] * transposeQMatrix[k, j];
                    }

                    rMatrix[i, j] = temp;
                }
            }

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