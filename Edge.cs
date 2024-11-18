//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
/// <summary>
/// Represents an edge in a graph.
/// </summary>
public class Edge : IComparable<Edge>
{
    /// <summary>
    /// Gets or sets the source vertex of the edge.
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// Gets or sets the destination vertex of the edge.
    /// </summary>
    public int Destination { get; set; }

    /// <summary>
    /// Gets or sets the weight of the edge.
    /// </summary>
    public int Weight { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Edge"/> class.
    /// </summary>
    /// <param name="source">The source vertex of the edge.</param>
    /// <param name="destination">The destination vertex of the edge.</param>
    /// <param name="weight">The weight of the edge.</param>
    public Edge(int source, int destination, int weight)
    {
        Source = source;
        Destination = destination;
        Weight = weight;
    }

    /// <summary>
    /// Compares the current edge with another edge based on their weights.
    /// </summary>
    /// <param name="other">The other edge to compare.</param>
    /// <returns>
    /// A value indicating whether the current edge is less than, equal to, or greater than the other edge.
    /// </returns>
    public int CompareTo(Edge other)
    {
        return Weight.CompareTo(other.Weight);
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
