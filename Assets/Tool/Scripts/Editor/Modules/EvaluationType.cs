
namespace GameLoop.Editor.Modules
{
    public enum EvaluationType
    {
        /// <summary>
        /// A singular evaluation evaluates out of itself without nested evaluations in use.
        /// </summary>
        Singular,

        /// <summary>
        /// Unary evaluations use another evaluation module and work with it.
        /// </summary>
        Unary,

        /// <summary>
        /// Binary evaluations use two other evaluations to form a new one.
        /// </summary>
        Binary
    }
}