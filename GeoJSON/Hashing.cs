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
        internal static int Hash(params object[] args)
        {
            unchecked // Overflow is fine, just wrap
            {
                int Hash = 17;

                foreach (object Item in args)
                {
                    if (Item != null)
                    {
                        Hash = (Hash * 23) + Item.GetHashCode();
                    }
                }

                return Hash;
            }
        }
    }
}
