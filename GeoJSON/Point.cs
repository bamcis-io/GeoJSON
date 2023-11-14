using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "Point", the "coordinates" member is a single position.
    /// </summary>
    [JsonConverter(typeof(PointConverter))]
    public class Point : Geometry, 
                         IAdimTopology<Point>,
                         IAdimTopology<Polygon>
    {
        #region Public Properties

        /// <summary>
        /// For type "Point", the "coordinates" member is a single position.
        /// </summary>
        [JsonProperty(PropertyName = "Coordinates")]
        public Coordinate Coordinates { get; }

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { get { return null; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new point with the provided coordinates
        /// </summary>
        /// <param name="coordinates">The position of this point</param>
        [JsonConstructor]
        public Point(Coordinate coordinates) : base(GeoJsonType.Point, coordinates.HasElevation())
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));

        }

        public Point(double longitude, double latitude) : base(GeoJsonType.Point, false)
        {
            this.Coordinates = new Coordinate(longitude, latitude) ?? throw new ArgumentNullException("Longitude and Latitude are invalid");
            
        }

        public Point(double longitude, double latitude, double altitude) : base(GeoJsonType.Point, true)
        {
            this.Coordinates = new Coordinate(longitude, latitude, altitude) ?? throw new ArgumentNullException("Longitude and Latitude and Altitude are invalid");

        }

        #endregion

        #region Public Methods

        #region Converters

        /// <summary>
        /// Deserializes the json into a Point
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>A Point object</returns>
        public static new Point FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Point>(json);
        }

        /// <summary>
        /// Converts this Point into a 1D Array of length 2 (longitude and latitude)
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            var vector = new double[] { this.Coordinates.Longitude, this.Coordinates.Latitude };

            return vector;

        }

        #endregion Converters

        #region Coordinates

        /// <summary>
        /// Gets the longitude or easting of the point
        /// </summary>
        /// <returns>The longitude</returns>
        public double GetLongitude()
        {
            return this.Coordinates.Longitude;
        }

        /// <summary>
        /// Gets the latitude or northing of the point
        /// </summary>
        /// <returns>The latitude</returns>
        public double GetLatitude()
        {
            return this.Coordinates.Latitude;
        }

        public bool TryGetElevation(out double elevation)
        {
            if (this.Coordinates.HasElevation())
            {
                elevation = this.Coordinates.Elevation;
                return true;
            }

            elevation = double.NaN;
            return false;
        }


        #endregion Coordinates

        /// <summary>
        /// Gets the elevation of the point if it exists
        /// in the coordinates.
        /// </summary>
        /// <returns>The elevation or if it wasn't set, the returns NaN</returns>

        #region Equality Evaluators

        public bool Equals(Point obj)
        {
            return obj.Coordinates.Equals(this.Coordinates);
        }

        public bool Equals(Polygon other)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;

            if (point == null )
            {
                return false;                
            }

            return Equals(point);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Coordinates);
        }


        public static bool operator ==(Point left, Point right)
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

        public static Point operator -(Point left, Point right)
        {
            
            Coordinate newPosition = left.Coordinates - right.Coordinates;

            return new Point(newPosition);
        }

        public static Point operator +(Point left, Point right)
        {
            Coordinate newPosition = left.Coordinates + right.Coordinates;

            return new Point(newPosition);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }


        #endregion Equality Evaluators

        #region Topographic Operations

        #region Touching Rules

        public bool Touches(Point otherPoint, double eps = double.MinValue * 100)
        {
            return this.Equals(otherPoint);
        }
        public bool Touches(LineSegment lineSegment)
        {
            return lineSegment.Touches(this);
        }


        public bool Touches(LineString lineString, double eps = double.MinValue * 100)
        {
            foreach (LineSegment lineSegment in lineString)
            {
                if (lineSegment.Touches(this, eps))
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }

        public bool Touches(Polygon polygon, double eps = double.MinValue * 100)
        {
            foreach (var lineRing in polygon)
            {
                if (this.Touches(lineRing, eps))
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }

            return false;
        }
        #endregion Touching Rules

        #region Within Rules

        

        public bool Within(Point point, double eps = double.MinValue * 100)
        {
            return this.Equals(point);
        }


        public bool Within(Polygon polygon, double eps = double.MinValue * 100)
        {
            return polygon.Contains(this, eps);
        }

        /// <summary>
        /// A point always has an empty Rectangle as its bounding box.
        /// </summary>
        /// <returns></returns>
        public Rectangle FetchBoundingBox()
        {
            return null;
        }

        public Point Copy()
        {
            return new Point(this.Coordinates.Copy());
        }

        public bool HasElevation()
        {
            return this.Coordinates.HasElevation();
        }



        #endregion Within Rules

        #region Intersection Rules

        public static bool Intersects(Geometry _)
        {
            return false;
        }

        #endregion Intersection Rules


        #region Contain Rules

        public static bool Contains(Geometry _)
        {
            return false;
        }

        #endregion Contain Rules


        #endregion Topographic Operations

        #endregion Public Methods
    }
}