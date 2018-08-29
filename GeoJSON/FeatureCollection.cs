using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Represents a collection of feature objects
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
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
        public FeatureCollection(IEnumerable<Feature> features, IEnumerable<double> boundingBox = null) : base(GeoJsonType.FeatureCollection, features.Any(x => x.IsThreeDimensional()), boundingBox)
        {
            this.Features = features ?? throw new ArgumentNullException("features");
        }

        #endregion

        #region Public Methods

        public new static FeatureCollection FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FeatureCollection>(json);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            FeatureCollection Other = (FeatureCollection)obj;

            bool BBoxEqual = true;

            if (this.BoundingBox != null && Other.BoundingBox != null)
            {
                BBoxEqual = this.BoundingBox.SequenceEqual(Other.BoundingBox);
            }
            else
            {
                BBoxEqual = (this.BoundingBox == null && Other.BoundingBox == null);
            }

            bool FeaturesEqual = true;

            if (this.Features != null && Other.Features != null)
            {
                FeaturesEqual = this.Features.SequenceEqual(Other.Features);
            }
            else
            {
                FeaturesEqual = (this.Features == null && Other.Features == null);
            }

            return this.Type == Other.Type &&
                FeaturesEqual &&
                BBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Features, this.BoundingBox);
        }

        public static bool operator ==(FeatureCollection left, FeatureCollection right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (right is null || left is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(FeatureCollection left, FeatureCollection right)
        {
            return !(left == right);
        }

        #endregion
    }
}
