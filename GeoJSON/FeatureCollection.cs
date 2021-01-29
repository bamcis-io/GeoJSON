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
        [JsonProperty(PropertyName = "features")]
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

            FeatureCollection other = (FeatureCollection)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            bool featuresEqual = true;

            if (this.Features != null && other.Features != null)
            {
                featuresEqual = this.Features.SequenceEqual(other.Features);
            }
            else
            {
                featuresEqual = (this.Features == null && other.Features == null);
            }

            return this.Type == other.Type &&
                featuresEqual &&
                bBoxEqual;
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
