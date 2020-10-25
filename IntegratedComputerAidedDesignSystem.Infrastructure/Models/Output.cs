namespace IntegratedComputerAidedDesignSystem.Infrastructure.Models
{
    /// <summary>
    /// C - Вывод из компонента
    /// </summary>
    public class Output
    {
        public Output(string name, Node node)
        {
            Name = name;
            Node = node;
        }

        public string Name { get; }

        public Node Node { get; }
    }
}