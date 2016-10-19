using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainRoute.Business
{
    public class Graph
    {
        public List<Vertice> Vertices { get; set; }
        public List<Edge> Edges { get; set; }

        public Graph(List<Vertice> vertices, List<Edge> edges)
        {
            this.Vertices = vertices;
            this.Edges = edges;
        }

        public Edge GetEdge(Vertice source, Vertice destination)
        {
            return this.Edges.FirstOrDefault(p => p.StartNode == source && p.DestinationNode == destination);
        }

        public int GetDistanceBetween(Vertice source, Vertice destination)
        {
            var edge = this.GetEdge(source, destination);
            if (edge == null)
                return int.MaxValue;
            return edge.Weight;
        }

        public bool RouteExists(Vertice source, Vertice destination)
        {
            return this.Edges.Exists(p => p.StartNode == source && p.DestinationNode == destination);
        }

        public List<Vertice> GetNodeNeighbors(Vertice node)
        {
            var neighbors = new List<Vertice>();
            foreach (var edge in this.Edges)
            {
                if (edge.StartNode.Equals(node))
                    neighbors.Add(edge.DestinationNode);
            }

            return neighbors;
        }

    }
}
