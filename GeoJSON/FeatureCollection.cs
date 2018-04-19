using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Represents a collection of feature objects
    /// </summary>
    public class FeatureCollection : GeoJson
    {
        #region Public Properties

        /// <summary>
        /// The collection of features
        /// </summary>
        public IEnumerable<Feature> Features { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// The default constructor building the feature collection
        /// </summary>
        /// <param name="features">The features that are part of the feature collection</param>
        [JsonConstructor]
        public FeatureCollection(IEnumerable<Feature> features) : base(GeoJsonType.FEATURECOLLECTION)
        {
            this.Features = features ?? throw new ArgumentNullException("features");
        }

        #endregion
    }
}
