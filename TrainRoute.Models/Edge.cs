using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainRoute.Business
{
    public class Edge
    {
        public Vertice StartNode { get; set; }
        public Vertice DestinationNode { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }

        public Edge(string name, Vertice source, Vertice destination, int weight)
        {
            this.StartNode = source;
            this.DestinationNode = destination;
            this.Weight = weight;
            this.Name = name;
        }
    }
}
