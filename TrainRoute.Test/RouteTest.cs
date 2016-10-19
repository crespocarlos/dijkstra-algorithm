using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TrainRoute.Business.Tests
{
    [TestClass()]
    public class RouteTests
    {
        private Route route;
        private List<Vertice> nodes;
        private List<Edge> edges;

        [TestInitialize]
        public void TestInitialize()
        {
            nodes = new List<Vertice>()
            {
                new Vertice("A"),
                new Vertice("B"),
                new Vertice("C"),
                new Vertice("D"),
                new Vertice("E")
            };

            edges = new List<Edge>();

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
            route = new Route(graph);
        }

        [TestMethod]
        public void CalculateShortestPathTest_ValidRoutes_ABC()
        {
            Assert.AreEqual(route.CalculateShortestPath(nodes[0], nodes[1], nodes[2]), 9);
        }

        [TestMethod]
        public void CalculateShortestPathTest_ValidRoutes_AD()
        {
            Assert.AreEqual(route.CalculateShortestPath(nodes[0], nodes[3]), 5);
        }

        [TestMethod]
        public void CalculateShortestPathTest_ValidRoutes_ADC()
        {
            Assert.AreEqual(route.CalculateShortestPath(nodes[0], nodes[3], nodes[2]), 13);
        }

        [TestMethod]
        public void CalculateShortestPathTest_ValidRoutes_AEBCD()
        {
            Assert.AreEqual(route.CalculateShortestPath(nodes[0], nodes[4], nodes[1], nodes[2], nodes[3]), 22);
        }

        [TestMethod]
        public void CalculateShortestPathTest_InvalidRoutes_AED()
        {
            Assert.AreEqual(route.CalculateShortestPath(nodes[0], nodes[4], nodes[3]), -1);
        }

        [TestMethod]
        public void GetNumberOfRoundTripsByMaxStops_Node_C()
        {
            Assert.AreEqual(route.GetNumberOfRoundTripsByMaxStops(nodes[2], 3), 2);
        }

        [TestMethod]
        public void GetNumberOfNodesTo_Node_A_To_C()
        {
            Assert.AreEqual(route.GetNumberOfNodesTo(nodes[0], nodes[2], 4), 3);
        }

        [TestMethod]
        public void GetLengthOfMiniumDistanceTo_A_To_C()
        {
            Assert.AreEqual(route.GetLengthOfMinimumDistanceTo(nodes[0], nodes[2]), 9);
        }

        [TestMethod]
        public void GetLengthOfMiniumDistanceTo_B_To_B()
        {
            Assert.AreEqual(route.GetLengthOfMinimumDistanceTo(nodes[1], nodes[1]), 9);
        }

        [TestMethod]

        public void GetNumberOfRoundTripsByMaxLength_30()
        {
            Assert.AreEqual(route.GetNumberOfRoundTripsByMaxLength(nodes[2], 30), 7);
        }
    }
}