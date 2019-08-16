namespace Dependous
{
    using System.Reflection;

    /// <summary>
    /// Matches the entry assembly
    /// </summary>
    /// <seealso cref="Dependous.StartsWith" />
    public class Self : StartsWith
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Self" /> class.
        /// </summary>
        internal Self() : base(null)
        {
            this.PatternToMatch = Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}