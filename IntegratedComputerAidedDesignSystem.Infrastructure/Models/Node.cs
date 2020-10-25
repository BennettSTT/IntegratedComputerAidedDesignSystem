namespace IntegratedComputerAidedDesignSystem.Infrastructure.Models
{
    /// <summary>
    /// V - узел
    /// </summary>
    public class Node
    {
        public Node(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}