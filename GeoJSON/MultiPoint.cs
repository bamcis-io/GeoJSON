using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiPoint", the "coordinates" member is an array of positions.
    /// </summary>
    [JsonConverter(typeof(MultiPointConverter))]
    public class MultiPoint : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates of a multipoint are an array of positions
        /// </summary>
        [JsonProperty(PropertyName = "Points")]
        [Description("Points")]
        public IEnumerable<Point> Points { get; }


        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { get { return FetchBoundingBox(); } }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Creates a new multipoint object
        /// </summary>
        /// <param name="coordinates"></param>
        [JsonConstructor]
        public MultiPoint([NotNull] IEnumerable<Coordinate> coordinates) : base(GeoJsonType.MultiPoint, coordinates.Any(x => x.HasElevation()))
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(Coordinate));

            this.Points = coordinates.Select(c => new Point(c));
        }

        /// <summary>
        /// Creates a new multipoint object
        /// </summary>
        /// <param name="coordinates"></param>
        
        public MultiPoint([NotNull] IEnumerable<IEnumerable<Coordinate>> coordinates) : base(GeoJsonType.MultiPoint, coordinates.Any(x => x.Any(y => y.HasElevation())))
        {
            var points = new List<Point>(); 
        
            foreach(var pointCoordinates in coordinates)
            {
                points.AddRange(pointCoordinates.Select(p => p.ToPoint()).ToList());
            }

            this.Points = points;
        }

        
        
        public MultiPoint([NotNull] IEnumerable<Point> points) : base(GeoJsonType.MultiPoint, points.Any(x => x.HasElevation()))
        {
            if (points == null)
                throw new ArgumentNullException(nameof(Coordinate));

            this.Points = points;
        }

        #endregion Constructors

        #region Public Methods

        #region Comparers

        public static new Point FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Point>(json);
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

            MultiPoint other = (MultiPoint)obj;

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

            if (this.Points != null && other.Points != null)
            {
                coordinatesEqual = this.Points.SequenceEqual(other.Points);
            }
            else
            {
                coordinatesEqual = (this.Points == null && other.Points == null);
            }

            return this.Type == other.Type &&
                coordinatesEqual &&
                bBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Points, this.BoundingBox);
        }

        public static bool operator ==(MultiPoint left, MultiPoint right)
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

        public static bool operator !=(MultiPoint left, MultiPoint right)
        {
            return !(left == right);
        }

        #endregion Comparers


        #region Topological Operations
        public Rectangle FetchBoundingBox()
        {

            double MaxLatitude = double.MinValue;
            double MaxLongitude = double.MinValue;
            double MinLatitude = double.MaxValue;
            double MinLongitude = double.MaxValue;

            foreach (Point geometry in this.Points)
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
