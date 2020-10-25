using IntegratedComputerAidedDesignSystem.Infrastructure.Models;
using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers;
using System;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem.Infrastructure
{
    public class MatrixManager
    {
        private readonly string _text;

        public MatrixManager(string text)
        {
            _text = text;
        }

        public MatrixInfo GetMatrixInfo(MatrixType matrixType)
        {
            var parser = new Parser(_text);
            var (components, nodes) = parser.Parse();

            var calculator = new MatrixCalculator(components, nodes);

            var matrix = calculator.GetMatrix(matrixType);

            var nodeNames = nodes.Select(x => x.Name).ToArray();
            var componentNames = components.Select(x => x.Name).ToArray();

            return matrixType switch
            {
                MatrixType.Q => new MatrixInfo(componentNames, nodeNames, matrix),
                MatrixType.R => new MatrixInfo(componentNames, componentNames, matrix),

                _ => throw new ArgumentException(nameof(matrixType))
            };
        }
    }
}