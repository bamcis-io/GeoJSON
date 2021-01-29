namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Provides a method for implementing custom GetHashCode() overrides.
    /// </summary>
    internal class Hashing
    {
        /// <summary>
        /// Computes a hash for a set of objects
        /// </summary>
        /// <param name="args">The arguments to hash</param>
        /// <returns>The hash code of the objects</returns>
        internal static int Hash(params object[] args)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;

                foreach (object item in args)
                {
                    if (item != null)
                    {
                        hash = (hash * 23) + item.GetHashCode();
                    }
                }

                return hash;
            }
        }
    }
}
