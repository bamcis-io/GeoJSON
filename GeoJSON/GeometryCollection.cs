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

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { get; }

        #endregion Public Properties

        #region Constructors 

        /// <summary>
        /// Creates a new GeometryCollection
        /// </summary>
        /// <param name="geometries">The geometries that are part of the collection</param>
        [JsonConstructor]
        public GeometryCollection(IEnumerable<Geometry> geometries) : base(GeoJsonType.GeometryCollection, geometries.Any(x => x.IsThreeDimensional()))
        {
            this.Geometries = geometries.ToList() ?? throw new ArgumentNullException("geometries");

            this.BoundingBox = FetchBoundingBox();
        }

        #endregion

        #region Public Methods

        #region Converters

        public new static GeometryCollection FromJson(string json)
        {
            return JsonConvert.DeserializeObject<GeometryCollection>(json);
        }


        #endregion Converters

        #region Equality Evaluators

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

            GeometryCollection other = (GeometryCollection)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            bool geometriesEqual = true;

            if (this.Geometries != null && other.Geometries != null)
            {
                geometriesEqual = this.Geometries.SequenceEqual(other.Geometries);
            }
            else
            {
                geometriesEqual = (this.Geometries == null && other.Geometries == null);
            }

            return this.Type == other.Type &&
                geometriesEqual &&
                bBoxEqual;
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

        #endregion Equality Evaluators

        #region Topological Operations

        public Rectangle FetchBoundingBox()
        {
            double MaxLatitude = double.MinValue;
            double MaxLongitude = double.MinValue;
            double MinLatitude = double.MaxValue;
            double MinLongitude = double.MaxValue;
            bool allGeomsHaveEmptyBoundingBox = true;
            foreach (var geometry in this.Geometries)
            {
                var bbox = geometry?.BoundingBox ?? null;

                if (bbox != null)
                {
                    allGeomsHaveEmptyBoundingBox = false;
                    if (MaxLatitude < bbox.MaxLatitude)
                    {
                        MaxLatitude = bbox.MaxLatitude;
                    }

                    if (MaxLongitude < bbox.MaxLongitude)
                    {
                        MaxLongitude = bbox.MaxLongitude;
                    }

                    if (MinLatitude > bbox.MinLatitude)
                    {
                        MinLatitude = bbox.MinLatitude;
                    }

                    if (MinLongitude > bbox.MinLongitude)
                    {
                        MinLongitude = bbox.MinLongitude;
                    }
                }
                
            }

            if (allGeomsHaveEmptyBoundingBox)
            {
                return null;
            }
            else
            {
                Point LL = new Point(new Coordinate(MinLongitude, MinLatitude));
                Point LR = new Point(new Coordinate(MaxLongitude, MinLatitude));
                Point UL = new Point(new Coordinate(MinLongitude, MaxLatitude));
                Point UR = new Point(new Coordinate(MaxLongitude, MaxLatitude));

                return new Rectangle(LL, LR, UL, UR);

            }
            
        }


        #endregion Topological Operations


        #endregion
    }
}
