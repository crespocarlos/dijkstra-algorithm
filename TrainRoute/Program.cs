using System;
using System.Collections.Generic;
using TrainRoute.Business;
using TrainRoute.Contracts;

namespace TrainRoute
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = new List<Vertice>()
            {
                new Vertice("A"),
                new Vertice("B"),
                new Vertice("C"),
                new Vertice("D"),
                new Vertice("E")
            };

            var edges = new List<Edge>();

            edges.Add(new Edge("AB", nodes[0], nodes[1], 5));
            edges.Add(new Edge("BC", nodes[1], nodes[2], 4));
            edges.Add(new Edge("CD", nodes[2], nodes[3], 8));
            edges.Add(new Edge("DC", nodes[3], nodes[2], 8));
            edges.Add(new Edge("DE", nodes[3], nodes[4], 6));
            edges.Add(new Edge("AD", nodes[0], nodes[3], 5));
            edges.Add(new Edge("CE", nodes[2], nodes[4], 2));
            edges.Add(new Edge("EB", nodes[4], nodes[1], 3));
            edges.Add(new Edge("AE", nodes[0], nodes[4], 7));

            Graph graph = new Graph(nodes, edges);
            IRoute route = new Business.Route(graph);

            // Test Case: 1
            Console.WriteLine(string.Concat("Test Case 1: ", TratarMensagem(route.CalculateShortestPath(nodes[0], nodes[1], nodes[2]))));

            // Test Case: 2
            Console.WriteLine(string.Concat("Test Case 2: ", TratarMensagem(route.CalculateShortestPath(nodes[0], nodes[3]))));

            // Test Case: 3
            Console.WriteLine(string.Concat("Test Case 3: ", TratarMensagem(route.CalculateShortestPath(nodes[0], nodes[3], nodes[2]))));

            // Test Case: 4
            Console.WriteLine(string.Concat("Test Case 4: ", TratarMensagem(route.CalculateShortestPath(nodes[0], nodes[4], nodes[1], nodes[2], nodes[3]))));

            // Test Case: 5
            Console.WriteLine(string.Concat("Test Case 5: ", TratarMensagem(route.CalculateShortestPath(nodes[0], nodes[4], nodes[3]))));

            // Test Case: 6
            Console.WriteLine(string.Concat("Test Case 6: ", route.GetNumberOfRoundTripsByMaxStops(nodes[2], 3)));

            // Test Case: 7
            Console.WriteLine(string.Concat("Test Case 7: ", route.GetNumberOfNodesTo(nodes[0], nodes[2], 4)));

            // Test Case: 8
            Console.WriteLine(string.Concat("Test Case 8: ", route.GetLengthOfMinimumDistanceTo(nodes[0], nodes[2])));

            // Test Case: 9
            Console.WriteLine(string.Concat("Test Case 9: ", route.GetLengthOfMinimumDistanceTo(nodes[1], nodes[1])));

            // Test Case: 10: NOT WORKING
            Console.WriteLine(string.Concat("Test Case 10: ", route.GetNumberOfRoundTripsByMaxLength(nodes[2], 30)));

            Console.ReadKey();
        }

        private static string TratarMensagem(int distancia)
        {
            return distancia >= 0 ? distancia.ToString() : "NO SUCH ROUTE";
        }
    }
}
