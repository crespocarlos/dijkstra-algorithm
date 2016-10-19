using TrainRoute.Business;

namespace TrainRoute.Contracts
{
    /// <summary>
    /// Class specialized in building routes
    /// </summary>
    /// <seealso cref="TrainRoute.Contracts.IRoute" />
    public interface IRoute
    {
        int CalculateShortestPath(params Vertice[] route);
        int GetNumberOfRoundTripsByMaxStops(Vertice startNode, int maxStops);
        int GetNumberOfNodesTo(Vertice startNode, Vertice lastNode, int maxStops);
        int GetLengthOfMinimumDistanceTo(Vertice startNode, Vertice lastNode);
        int GetNumberOfRoundTripsByMaxLength(Vertice startNode, int maxLenghts);
    }
}
