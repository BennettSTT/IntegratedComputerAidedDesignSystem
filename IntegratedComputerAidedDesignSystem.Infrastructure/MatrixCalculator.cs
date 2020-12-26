using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    internal class MatrixCalculator
    {
        private  Component[] _components;
        private  Node[] _nodes;

        public MatrixCalculator(Component[] components, Node[] nodes)
        {
            _components = components;
            _nodes = nodes;
        }

        public int[,] GetMatrix(MatrixType matrixType)
        {
            _components = new[]
            {
                new Component("1"),
                new Component("2"),
                new Component("3"),
                new Component("4"),
                new Component("5"),
                new Component("6"),
                new Component("7"),
                new Component("8"),
            };
            
            var localComponents = _components;
            
            const int batchCount = 2;
            const int vertexCount = 4;
            
            int[,] rMatrix = 
            {
                /* 1 */ { 0, 1, 0, 0, 2, 3, 0, 0 }, // 6
                /* 2 */ { 1, 0, 1, 0, 0, 1, 0, 0 }, // 3
                /* 3 */ { 0, 1, 0, 2, 0, 0, 1, 0 }, // 4
                /* 4 */ { 0, 0, 2, 0, 0, 0, 3, 1 }, // 6
                /* 5 */ { 2, 0, 0, 0, 0, 1, 0, 0 }, // 3
                /* 6 */ { 3, 1, 0, 0, 1, 0, 0, 1 }, // 6
                /* 7 */ { 0, 0, 1, 3, 0, 0, 0, 2 }, // 6
                /* 8 */ { 0, 0, 0, 1, 0, 1, 2, 0 }  // 4
            };
            
            for (var xIndex = 0; xIndex < batchCount; xIndex++)
            {
                var (minDegree, minDegreeIndex, allDegree) = GetMinDegree(rMatrix);
                
                var batch = GetBatch(rMatrix, allDegree, minDegreeIndex, vertexCount);
                var batchComponents = new Component[batch.Length];

                var batchIndex = 0;
                foreach (var element in batch)
                {
                    batchComponents[batchIndex] = _components[element];
                    batchIndex++;
                }

                _components = _components.Where(x => batchComponents.All(b => b.Name != x.Name)).ToArray();
                
                
                var temp = new int[rMatrix.GetLength(0)];
                for (var index = 0; index < temp.Length; index++)
                {
                    temp[index] = index;
                }

                var elements = temp.Where(x => !batch.Contains(x)).ToArray();
                var elementsLength = elements.Length;

                var newRMatrix = new int[elementsLength, elementsLength];
                var i = 0;
                foreach (var elementI in elements)
                {
                    var j = 0;
                    foreach (var elementJ in elements)
                    {
                        newRMatrix[i, j] = rMatrix[elementI, elementJ];
                        j++;
                    }
                    i++;
                }
                rMatrix = newRMatrix;
            }

            return matrixType switch
            {
                MatrixType.Q => GetQMatrix(),
                MatrixType.R => GetRMatrix(GetQMatrix()),

                _ => throw new ArgumentException(nameof(matrixType))
            };
        }

        private static int[] GetBatch(int[,] rMatrix, int[] allDegree, int firstVertex, int vertexCount)
        {
            var currentVertex = firstVertex;
            var vertices = new List<int> { firstVertex };
            
            for (var indexVertex = 1; indexVertex < vertexCount; indexVertex++)
            {
                var adjacentVertices = GetAdjacentVertices(rMatrix, vertices, currentVertex);

                var (vertex, maxDeltaValue) = GetMaxDelta(rMatrix, allDegree, adjacentVertices, vertices);

                currentVertex = vertex;

                if (!vertices.Contains(currentVertex))
                {
                    vertices.Add(currentVertex);
                }
            }

            return vertices.ToArray();
        }
        
        private static (int Vertex, int MaxDeltaValue) GetMaxDelta(int[,] rMatrix, int[] allDegree, int[] adjacentVertices, List<int> xArray)
        {
            var list = new List<(int Vertex, int DeltaValue)>();
            
            foreach (var vertex in adjacentVertices)
            {
                var delta = xArray.Sum(x => rMatrix[vertex, x]) * 2 - allDegree[vertex];
                list.Add((vertex, delta));
            }

            var maxDelta = list.Max(tuple => tuple.DeltaValue);

            return list.Find(tuple => tuple.DeltaValue == maxDelta);
        }
        
        private static int[] GetAdjacentVertices(int[,] rMatrix, List<int> xArray, int indexElement)
        {
            List<int> adjacentVertices = new List<int>();
            
            for (var i = 0; i < rMatrix.GetLength(0); i++)
            {
                var value = rMatrix[i, indexElement];
                
                if (value > 0 && !xArray.Contains(i))
                {
                    adjacentVertices.Add(i);
                }
            }

            return adjacentVertices.ToArray();
        }
        
        private static (int MinDegree, int MinDegreeIndex, int[] AllDegree) GetMinDegree(int[,] rMatrix)
        {
            var rMatrixLength = rMatrix.GetLength(0);

            int[] allDegree = new int[rMatrixLength];
            (int MinDegree, int CountVertex, int MinDegreeIndex) degree = (0, 0, 0);

            for (var index = 0; index < rMatrixLength; index++)
            {
                var currentDegree = 0;
                var countVertex = 0;
                for (var j = 0; j < rMatrixLength; j++)
                {
                    var value = rMatrix[index, j];
                    if (value <= 0) continue;

                    currentDegree += value;
                    countVertex++;
                }

                if (index == 0 || degree.MinDegree > currentDegree || degree.MinDegree == currentDegree && degree.CountVertex > countVertex)
                {
                    degree = (currentDegree, countVertex, index);
                }

                allDegree[index] = currentDegree;
            }

            return (degree.MinDegree, degree.MinDegreeIndex, allDegree);
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