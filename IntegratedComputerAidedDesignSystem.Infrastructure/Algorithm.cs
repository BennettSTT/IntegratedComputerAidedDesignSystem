using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    public static class Algorithm
    {
        public static List<Component[]> Run(Component[] components, int[,] rMatrix, int batchCount, int vertexCount)
        {
            List<Component[]> batchs = new List<Component[]>();

            for (var xIndex = 0; xIndex < batchCount; xIndex++)
            {
                var (minDegree, minDegreeIndex, allDegree) = GetMinDegree(rMatrix);

                var batch = GetBatch(rMatrix, allDegree, minDegreeIndex, vertexCount);
                var batchComponents = new Component[batch.Length];

                var batchIndex = 0;
                foreach (var element in batch)
                {
                    batchComponents[batchIndex] = components[element];
                    batchIndex++;
                }

                components = components.Where(x => batchComponents.All(b => b.Name != x.Name)).ToArray();

                batchs.Add(batchComponents);

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

            return batchs;
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

        private static (int Vertex, int MaxDeltaValue) GetMaxDelta(int[,] rMatrix, int[] allDegree,
            int[] adjacentVertices, List<int> xArray)
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

                if (index == 0 || degree.MinDegree > currentDegree ||
                    degree.MinDegree == currentDegree && degree.CountVertex > countVertex)
                {
                    degree = (currentDegree, countVertex, index);
                }

                allDegree[index] = currentDegree;
            }

            return (degree.MinDegree, degree.MinDegreeIndex, allDegree);
        }
    }
}