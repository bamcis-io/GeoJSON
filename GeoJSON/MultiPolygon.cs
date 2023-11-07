using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiPolygon", the "coordinates" member is an array of
    /// Polygon coordinate arrays.
    /// </summary>
    [JsonConverter(typeof(MultiPolygonConverter))]
    public class MultiPolygon : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates are an array of polygons.
        /// </summary>
        [JsonProperty(PropertyName = "Polygons")]
        public IEnumerable<Polygon> Polygons { get; }

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { get { return FetchBoundingBox(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MultiPolygon
        /// </summary>
        /// <param name="Polygons">The coordinates that make up the multi polygon</param>
        [JsonConstructor]
        public MultiPolygon(IEnumerable<Polygon> Polygons) : base(GeoJsonType.MultiPolygon, Polygons.Any(x => x.IsThreeDimensional()))
        {
            this.Polygons = Polygons ?? throw new ArgumentNullException(nameof(Polygons));

            if (!this.Polygons.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(Polygons), "A MultiPolygon must have at least 1 polygon.");
            }

        }

        #endregion

        #region Public Methods

        public Rectangle FetchBoundingBox()
        {
            
            double MaxLatitude = double.MinValue ;
            double MaxLongitude = double.MinValue;
            double MinLatitude = double.MaxValue;
            double MinLongitude = double.MaxValue;
            
            foreach (var polygon in this.Polygons)
            {
                if (MaxLatitude < polygon.BoundingBox.MaxLatitude)
                {
                    MaxLatitude = polygon.BoundingBox.MaxLatitude;
                }

                if (MaxLongitude < polygon.BoundingBox.MaxLongitude)
                {
                    MaxLongitude = polygon.BoundingBox.MaxLongitude;
                }

                if (MinLatitude > polygon.BoundingBox.MinLatitude)
                {
                    MinLatitude = polygon.BoundingBox.MinLatitude;
                }

                if (MinLongitude > polygon.BoundingBox.MinLongitude)
                {
                    MinLongitude = polygon.BoundingBox.MinLongitude;
                }
            }

            Point LL = new Point(new Coordinate(MinLongitude, MinLatitude));
            Point LR = new Point(new Coordinate(MaxLongitude, MinLatitude));
            Point UL = new Point(new Coordinate(MinLongitude, MaxLatitude));
            Point UR = new Point(new Coordinate(MaxLongitude, MaxLatitude));

            return new Rectangle(LL, LR, UL, UR);

        }

        /// <summary>
        /// Deserializes the json into a MultiPolygon
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>A Point object</returns>
        public static new MultiPolygon FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MultiPolygon>(json);
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

            MultiPolygon other = (MultiPolygon)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            bool coordinatesEqual = true;

            if (this.Polygons != null && other.Polygons != null)
            {
                coordinatesEqual = this.Polygons.SequenceEqual(other.Polygons);
            }
            else
            {
                coordinatesEqual = (this.Polygons == null && other.Polygons == null);
            }

            return this.Type == other.Type &&
                coordinatesEqual &&
                bBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Polygons, this.BoundingBox);
        }


        public static bool operator ==(MultiPolygon left, MultiPolygon right)
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

        public static bool operator !=(MultiPolygon left, MultiPolygon right)
        {
            return !(left == right);
        }

        #endregion
    }
}
