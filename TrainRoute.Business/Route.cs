using System.Collections.Generic;
using System.Linq;
using TrainRoute.Contracts;
using TrainRoute.Business;
using System;

namespace TrainRoute.Business
{
    /// <summary>
    /// Class specialized in building routes
    /// </summary>
    /// <seealso cref="TrainRoute.Contracts.IRoute" />
    public class Route : IRoute
    {
        public Graph DirectedGraph { get; set; }

        public Route(Graph graph)
        {
            this.DirectedGraph = graph;
        }

        /// <summary>
        /// Calculates the shortest path of the graph.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>total weight of the path</returns>
        public int CalculateShortestPath(params Vertice[] route)
        {
            int pathWeight = 0;
            for (var i = 0; i < route.Length; i++)
            {
                if (i >= route.Length - 1)
                    return pathWeight;

                var current = route[i];
                var nextNode = route[i + 1];

                var path = this.FindShortestPath(current);

                if (!this.DirectedGraph.RouteExists(current, nextNode))
                    return -1;

                pathWeight += this.GetShortestDistance(path, nextNode);
            }

            return pathWeight;
        }

        /// <summary>
        /// Gets the number of round trips by maximum stops.
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="maxStops">The maximum stops.</param>
        /// <returns>number of possible round trips</returns>
        public int GetNumberOfRoundTripsByMaxStops(Vertice startNode, int maxStops)
        {
            return this.GetNumberOfNodesTo(startNode, startNode, maxStops);
        }


        /// <summary>
        /// Gets the maximum of possible round trips that can be made till achieve the maximum weight
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="maxWeight">The maximum wight.</param>
        /// <returns>number of possible round trips</returns>
        public int GetNumberOfRoundTripsByMaxLength(Vertice startNode, int maxWeight)
        {
            int validTrips = 0;

            var path = this.FindAllPossiblePath(startNode);

            List<Vertice> possibleRoutes = this.DirectedGraph.GetNodeNeighbors(startNode);

            foreach (var route in possibleRoutes)
            {
                int weight = 0;
                var pathTo = new List<Vertice>();
                var nodes = new List<Vertice>() { startNode };
                path = this.FindAllPossiblePath(route);

                while (weight >= 0 && weight < maxWeight)
                {
                    var previousNode = this.GetStartNode(path, route);

                    while (previousNode != null)
                    {
                        previousNode = this.GetStartNode(path, previousNode);
                        nodes.Add(previousNode);
                        if (previousNode == startNode)
                        {
                            break;
                        }
                    }

                    nodes.Reverse();

                    pathTo.InsertRange(0, nodes);

                    weight = this.CalculateShortestPath(pathTo.ToArray());
                    if (weight > 0 && weight < maxWeight)
                    {
                        validTrips++;
                        nodes = new List<Vertice>();
                    }
                }

            }

            return validTrips;
        }

        /// <summary>
        /// Gets the number of nodes to from one to anoter.
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="lastNode">The last node.</param>
        /// <param name="maxStops">The maximum stops.</param>
        /// <returns>number of nodes</returns>
        public int GetNumberOfNodesTo(Vertice startNode, Vertice lastNode, int maxStops)
        {
            int validTrip = 0;

            List<Vertice> possibleRoutes = this.DirectedGraph.GetNodeNeighbors(startNode);

            foreach (var route in possibleRoutes)
            {
                int nodesPassed = this.GetNumberOfNodesBetween(route, lastNode);
                if (nodesPassed > 0 && nodesPassed <= maxStops)
                    validTrip++;
            }

            return validTrip;
        }

        /// <summary>
        /// Gets the weight of the minimum distance from one node to anoter
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="lastNode">The last node.</param>
        /// <returns>minimum weight</returns>
        public int GetLengthOfMinimumDistanceTo(Vertice startNode, Vertice lastNode)
        {
            List<Vertice> possibleRoutes = this.DirectedGraph.GetNodeNeighbors(startNode);
            List<Vertice> pathTo = null;

            foreach (var route in possibleRoutes)
            {
                pathTo = new List<Vertice>()
                {
                    startNode
                };

                var path = this.FindShortestPath(route);
                pathTo.AddRange(this.GetPath(path, lastNode));
                if (pathTo.Last() == lastNode)
                    break;
            }

            return this.CalculateShortestPath(pathTo.ToArray());
        }

        /// <summary>
        /// Finds the shortest path the train can go.
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>the path</returns>
        private List<Edge> FindShortestPath(Vertice from)
        {
            var visitedNodes = new HashSet<Vertice>();
            var notVisitedNodes = new HashSet<Vertice>();
            var path = new List<Edge>();

            notVisitedNodes.Add(from);

            while (notVisitedNodes.Any())
            {
                Vertice node = this.GetMinimumDistance(path, notVisitedNodes);
                visitedNodes.Add(node);
                notVisitedNodes.Remove(node);
                path.AddRange(this.FindMinimalDistances(node, path, visitedNodes, notVisitedNodes));
            }

            return path;
        }


        /// <summary>
        /// Finds all possible path the train can go.
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>the path</returns>
        private List<Edge> FindAllPossiblePath(Vertice from)
        {
            var visitedNodes = new HashSet<Vertice>();
            var notVisitedNodes = new HashSet<Vertice>();

            var path = new List<Edge>();

            notVisitedNodes.Add(from);

            while (notVisitedNodes.Any())
            {
                Vertice node = notVisitedNodes.First();
                if (from != node)
                    visitedNodes.Add(node);

                notVisitedNodes.Remove(node);

                var neighbors = this.GetUnvisitedNodeNeighbors(node, visitedNodes);
                foreach (var target in neighbors)
                {
                    notVisitedNodes.Add(target);
                    path.Add(this.DirectedGraph.GetEdge(node, target));
                }
            }

            return path;
        }

        private List<Edge> FindMinimalDistances(Vertice node, List<Edge> path, HashSet<Vertice> visitedNodes, HashSet<Vertice> notVisitedNodes)
        {
            var neighbors = this.GetUnvisitedNodeNeighbors(node, visitedNodes);
            var edges = new List<Edge>();

            foreach (var target in neighbors)
            {
                var distanceBetween = this.DirectedGraph.GetDistanceBetween(node, target);
                var shortestDistance = this.GetShortestDistance(path, target);

                if (shortestDistance > shortestDistance + distanceBetween)
                {
                    notVisitedNodes.Add(target);
                    edges.Add(this.DirectedGraph.GetEdge(node, target));
                }
            }

            return edges;
        }

        private List<Vertice> GetPath(List<Edge> path, Vertice destinationNode)
        {
            var nodes = new List<Vertice>()
            {
                destinationNode
            };

            var previousNode = this.GetStartNode(path, destinationNode);

            while (previousNode != null)
            {
                nodes.Add(previousNode);
                previousNode = this.GetStartNode(path, previousNode);
            }

            nodes.Reverse();

            return nodes;
        }

        private int GetNumberOfNodesBetween(Vertice startNode, Vertice destinationNode)
        {
            int number = 1;
            var path = this.FindShortestPath(startNode);

            var previousNode = this.GetStartNode(path, destinationNode);

            while (previousNode != null)
            {
                previousNode = this.GetStartNode(path, previousNode);
                number++;
            }

            return number;
        }

        private List<Vertice> GetUnvisitedNodeNeighbors(Vertice node, HashSet<Vertice> visitedNodes)
        {
            var neighbors = new List<Vertice>();
            foreach (var edge in this.DirectedGraph.Edges)
            {
                if (edge.StartNode.Equals(node) && !visitedNodes.Contains(edge.DestinationNode))
                    neighbors.Add(edge.DestinationNode);
            }

            return neighbors;
        }

        private Vertice GetMinimumDistance(List<Edge> path, HashSet<Vertice> vertexes)
        {
            Vertice minimum = null;
            foreach (var vertex in vertexes)
            {
                if (minimum == null)
                {
                    minimum = vertex;
                }
                else
                {
                    if (this.GetShortestDistance(path, vertex) < this.GetShortestDistance(path, minimum))
                    {
                        minimum = vertex;
                    }
                }
            }

            return minimum;
        }

        private int GetShortestDistance(List<Edge> path, Vertice destination)
        {
            var edge = path.FirstOrDefault(p => p.DestinationNode == destination);
            return edge != null ? edge.Weight : int.MaxValue;

        }

        private Vertice GetStartNode(List<Edge> path, Vertice destination)
        {
            var destinationNode = path.FirstOrDefault(p => p.DestinationNode == destination);
            return destinationNode != null ? destinationNode.StartNode : null;
        }
    }
}
