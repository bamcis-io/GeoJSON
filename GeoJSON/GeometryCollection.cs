using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A GeoJSON object with type "GeometryCollection" is a Geometry object.
    /// A GeometryCollection has a member with the name "geometries".  The
    /// value of "geometries" is an array.
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class GeometryCollection : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The value of "geometries" is an array.Each element of this array is a
        /// GeoJSON Geometry object.  It is possible for this array to be empty.
        /// </summary>
        [JsonProperty(PropertyName = "geometries")]
        public IEnumerable<Geometry> Geometries { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new GeometryCollection
        /// </summary>
        /// <param name="geometries">The geometries that are part of the collection</param>
        [JsonConstructor]
        public GeometryCollection(IEnumerable<Geometry> geometries, IEnumerable<double> boundingBox = null) : base(GeoJsonType.GeometryCollection, geometries.Any(x => x.IsThreeDimensional()), boundingBox)
        {
            this.Geometries = geometries ?? throw new ArgumentNullException("geometries");
        }

        #endregion

        #region Public Methods

        public new static GeometryCollection FromJson(string json)
        {
            return JsonConvert.DeserializeObject<GeometryCollection>(json);
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

            GeometryCollection Other = (GeometryCollection)obj;

            bool BBoxEqual = true;

            if (this.BoundingBox != null && Other.BoundingBox != null)
            {
                BBoxEqual = this.BoundingBox.SequenceEqual(Other.BoundingBox);
            }
            else
            {
                BBoxEqual = (this.BoundingBox == null && Other.BoundingBox == null);
            }

            bool GeometriesEqual = true;

            if (this.Geometries != null && Other.Geometries != null)
            {
                GeometriesEqual = this.Geometries.SequenceEqual(Other.Geometries);
            }
            else
            {
                GeometriesEqual = (this.Geometries == null && Other.Geometries == null);
            }

            return this.Type == Other.Type &&
                GeometriesEqual &&
                BBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Geometries, this.BoundingBox);
        }

        public static bool operator ==(GeometryCollection left, GeometryCollection right)
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

        public static bool operator !=(GeometryCollection left, GeometryCollection right)
        {
            return !(left == right);
        }

        #endregion
    }
}
