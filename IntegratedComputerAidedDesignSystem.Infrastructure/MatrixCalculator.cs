using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using System;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    internal class MatrixCalculator
    {
        private readonly Component[] _components;
        private readonly Node[] _nodes;

        public MatrixCalculator(Component[] components, Node[] nodes)
        {
            _components = components;
            _nodes = nodes;
        }

        public int[,] GetMatrix(MatrixType matrixType)
        {
            return matrixType switch
            {
                MatrixType.Q => GetQMatrix(),
                MatrixType.R => GetRMatrix(GetQMatrix()),

                _ => throw new ArgumentException(nameof(matrixType))
            };
        }

        private int[,] GetQMatrix()
        {
            var qMatrix = new int[_components.Length, _nodes.Length];

            for (var i = 0; i < _components.Length; i++)
            {
                for (var j = 0; j < _nodes.Length; j++)
                {
                    qMatrix[i, j] = _components[i].Find(_nodes[j]) ? 1 : 0;
                }
            }

            return qMatrix;
        }

        private static int[,] GetRMatrix(int[,] qMatrix)
        {
            var transposeQMatrix = Transpose(qMatrix);

            var lineCount = qMatrix.GetLength(1);
            var columnCount = qMatrix.GetLength(0);
            var rMatrix = new int[columnCount, columnCount];

            for (var i = 0; i < columnCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    if (i == j)
                    {
                        rMatrix[i, j] = 0;
                        continue;
                    }

                    var temp = 0;

                    for (var k = 0; k < lineCount; k++)
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