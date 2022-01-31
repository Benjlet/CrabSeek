namespace CrabSeek.Models
{
    /// <summary>
    /// The heuristic distance formula to use; some formulas are more effective, depending on the scenario. Manhattan is well suited to grids, for example.
    /// </summary>
    public enum DistanceFormula
    {
        Diagonal,
        Euclidean,
        Manhattan
    }
}
