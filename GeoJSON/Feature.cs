using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Feature objects in GeoJSON contain a Geometry object and additional members.
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class Feature : GeoJson
    {
        #region Public Properties

        /// <summary>
        /// The geometry of this feature
        /// </summary>
        public Geometry Geometry { get; }

        /// <summary>
        /// Additional properties for the feature, the value
        /// is dynamic as it could be a string, bool, null, number,
        /// array, or object
        /// </summary>
        public IDictionary<string, dynamic> Properties { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new Feature with the given geometry and optional properties
        /// </summary>
        /// <param name="geometry">The geometry to create the feature from</param>
        /// <param name="properties">The feature properties</param>
        [JsonConstructor]
        public Feature(Geometry geometry, IDictionary<string, dynamic> properties = null) : base(GeoJsonType.Feature)
        {
            this.Geometry = geometry ?? throw new ArgumentNullException("geometry");
            this.Properties = properties ?? new Dictionary<string, dynamic>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a Feature from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static new Feature FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Feature>(json);
        }

        /// <summary>
        /// Compares equality of this Feature to another. It is important to note
        /// that only the Keys at the top level of the properties dictionary are
        /// checked to determine equality of that property.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

            Feature Other = (Feature)obj;

            return this.Type == Other.Type &&
                this.Geometry == Other.Geometry &&
                this.BoundingBox == Other.BoundingBox &&
                this.Properties.Keys.SequenceEqual(Other.Properties.Keys);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Geometry, this.BoundingBox, this.Properties);
        }

        public static bool operator ==(Feature left, Feature right)
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

        public static bool operator !=(Feature left, Feature right)
        {
            return !(left == right);
        }

        #endregion
    }
}
