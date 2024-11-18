//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System.Collections.Generic;

namespace MinimumSpanningTreeLibrary
{
    /// <summary>
    /// Represents a graph data structure.
    /// </summary>
    public class Graph
    {
        private readonly int _vertexCount;
        private readonly List<Edge> _edges;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="vertexCount">The number of vertices in the graph.</param>
        public Graph(int vertexCount)
        {
            _vertexCount = vertexCount;
            _edges = new List<Edge>();
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="source">The source vertex of the edge.</param>
        /// <param name="destination">The destination vertex of the edge.</param>
        /// <param name="weight">The weight of the edge.</param>
        public void AddEdge(int source, int destination, int weight)
        {
            _edges.Add(new Edge(source, destination, weight));
        }

        private int FindParent(int[] parents, int vertex)
        {
            if (parents[vertex] != vertex)
            {
                parents[vertex] = FindParent(parents, parents[vertex]); // Path compression
            }
            return parents[vertex];
        }

        private void Union(int[] parents, int[] ranks, int root1, int root2)
        {
            int root1Parent = FindParent(parents, root1);
            int root2Parent = FindParent(parents, root2);

            if (ranks[root1Parent] < ranks[root2Parent])
            {
                parents[root1Parent] = root2Parent;
            }
            else if (ranks[root1Parent] > ranks[root2Parent])
            {
                parents[root2Parent] = root1Parent;
            }
            else
            {
                parents[root2Parent] = root1Parent;
                ranks[root1Parent]++;
            }
        }

        /// <summary>
        /// Finds the minimum spanning tree of the graph using Kruskal's algorithm.
        /// </summary>
        /// <returns>A tuple containing the list of edges in the minimum spanning tree and the total weight of the tree.</returns>
        public (List<Edge> mstEdges, int totalWeight) KruskalMST()
        {
            List<Edge> mstEdges = new List<Edge>();
            int totalWeight = 0;

            // Sort edges by weight
            _edges.Sort();

            // Initialize parent and rank arrays
            int[] parents = new int[_vertexCount];
            int[] ranks = new int[_vertexCount];

            for (int i = 0; i < _vertexCount; i++)
            {
                parents[i] = i;
                ranks[i] = 0;
            }

            int edgesInMST = 0;
            int currentIndex = 0;

            while (edgesInMST < _vertexCount - 1 && currentIndex < _edges.Count)
            {
                Edge nextEdge = _edges[currentIndex++];

                int root1 = FindParent(parents, nextEdge.Source);
                int root2 = FindParent(parents, nextEdge.Destination);

                if (root1 != root2)
                {
                    mstEdges.Add(nextEdge);
                    totalWeight += nextEdge.Weight;
                    Union(parents, ranks, root1, root2);
                    edgesInMST++;
                }
            }

            return (mstEdges, totalWeight);
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
