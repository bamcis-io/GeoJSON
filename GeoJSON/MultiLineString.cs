using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiLineString", the "coordinates" member is an array of
    /// LineString coordinate arrays.
    /// </summary>
    [JsonConverter(typeof(MultiLineStringConverter))]
    public class MultiLineString : Geometry, IEnumerable<LineString>
    {
        #region Public Properties

        /// <summary>
        /// For type "MultiLineString", the "coordinates" member is an array of
        /// LineString coordinate arrays.
        /// </summary>
        [JsonProperty(PropertyName = "Points")]
        public IEnumerable<LineString> LineStrings { get; }

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { get { return FetchBoundingBox(); } }

        #endregion Public Properties

        #region Constructors 

        /// <summary>
        /// Creates a new MultiLineString
        /// </summary>
        /// <param name="coordinates">The line strings that make up the object</param>
        [JsonConstructor]
        public MultiLineString(IEnumerable<LineString> coordinates) : base(GeoJsonType.MultiLineString, coordinates.Any(x => x.IsThreeDimensional()))
        {
            this.LineStrings = coordinates ?? throw new ArgumentNullException("Points");
        }

        #endregion

        #region Public Methods

        #region Converters

        public static new MultiLineString FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MultiLineString>(json);
        }

        #endregion Converters

        #region Enumerable


        public IEnumerable<Geometry> ToList()
        {
            foreach (var geometry in this.LineStrings)
            {

                yield return geometry;
            }
        }

        public IEnumerator<LineString> GetEnumerator()
        {
            foreach (var line in LineStrings)
            {
                yield return line;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #endregion Enumerable

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

            MultiLineString other = (MultiLineString)obj;

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

            if (this.LineStrings != null && other.LineStrings != null)
            {
                coordinatesEqual = this.LineStrings.SequenceEqual(other.LineStrings);
            }
            else
            {
                coordinatesEqual = (this.LineStrings == null && other.LineStrings == null);
            }

            return this.Type == other.Type &&
                coordinatesEqual &&
                bBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.LineStrings, this.BoundingBox);
        }

        
        public static bool operator ==(MultiLineString left, MultiLineString right)
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

        public static bool operator !=(MultiLineString left, MultiLineString right)
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

            foreach (var geometry in this.LineStrings)
            {
                if (MaxLatitude < geometry.BoundingBox.MaxLatitude)
                {
                    MaxLatitude = geometry.BoundingBox.MaxLatitude;
                }

                if (MaxLongitude < geometry.BoundingBox.MaxLongitude)
                {
                    MaxLongitude = geometry.BoundingBox.MaxLongitude;
                }

                if (MinLatitude > geometry.BoundingBox.MinLatitude)
                {
                    MinLatitude = geometry.BoundingBox.MinLatitude;
                }

                if (MinLongitude > geometry.BoundingBox.MinLongitude)
                {
                    MinLongitude = geometry.BoundingBox.MinLongitude;
                }
            }

            Point LL = new Point(new Coordinate(MinLongitude, MinLatitude));
            Point LR = new Point(new Coordinate(MaxLongitude, MinLatitude));
            Point UL = new Point(new Coordinate(MinLongitude, MaxLatitude));
            Point UR = new Point(new Coordinate(MaxLongitude, MaxLatitude));

            return new Rectangle(LL, LR, UL, UR);

        }



        #endregion Topological Operations

        #endregion
    }
}
