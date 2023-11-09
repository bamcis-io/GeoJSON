using BAMCIS.GeoJSON.Serde;
using Extensions.ListExtensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "LineString", the "coordinates" member is an array of two or
    /// more positions.
    /// </summary>
    [JsonConverter(typeof(LineStringConverter))]
    public class LineString : Geometry, 
                              IEnumerable<LineSegment>,
                              ILine<Point>,
                              ILine<LineSegment>,
                              ILine<LinearRing>,
                              ILine<Polygon>,
                              IContains<Point>,
                              IContains<LineSegment>,
                              IContains<LinearRing>,
                              IContains<Polygon>,
                              IEquatable<LineSegment>,
                              IComparable<LineSegment>,
                              IComparer<LineSegment>

    {
        #region Public Properties

        /// <summary>
        /// The coordinates of a linestring are an array of positions
        /// </summary>
        [JsonProperty(PropertyName = "LineSegments")]
        public IEnumerable<LineSegment> LineSegments { get; }

        [JsonProperty(PropertyName = "coordinates")]
        public IEnumerable<Coordinate> Coordinates { get { return Points.Select(p => p.Coordinates).ToList(); } }

        [JsonIgnore]
        public IEnumerable<Point> Points { get; init; }

        public double Length 
        {
            get {
                return LineSegments.Select(x => x.Length).Sum();
            }
        }

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { 
            get {

                    if (this._BoundingBox == null)
                    {
                        this._BoundingBox = FetchBoundingBox(this.Points);
                    }
                    return this._BoundingBox; 
                }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new LineString
        /// </summary>
        /// <param name="coordinates">The coordinates in the line string</param>
        [JsonConstructor]
        public LineString(IEnumerable<LineSegment> lineSegments) : base(GeoJsonType.LineString, lineSegments.Any(x => x.HasElevation()))
        {
            this.Points = lineSegments.SelectMany(lineSeg => lineSeg.Points.ToList()).ToList();

            this.LineSegments = lineSegments ?? throw new ArgumentNullException(nameof(lineSegments));

        }

        
        public LineString(IEnumerable<Point> points) : base(GeoJsonType.LineString, points.Any(x => x.HasElevation()))
        {
            this.Points = points;

            this.LineSegments = ConvertPointsToLineSegments(points.ToList()) ?? throw new ArgumentNullException("lineSegments");
        }

        public LineString(IEnumerable<Coordinate> coordinates) : base(GeoJsonType.LineString, CoordinatesToPoints(coordinates).Any(x => x.HasElevation()))
        {
            this.Points = CoordinatesToPoints(coordinates);

            this.LineSegments = ConvertPointsToLineSegments(this.Points.ToList()) ?? throw new ArgumentNullException("lineSegments");
        }

        public static List<Point> CoordinatesToPoints(IEnumerable<Coordinate> positions)
        {
            List<Point> points = positions.Select(x => new Point(x)).ToList();

            return points;

        }

        private static IEnumerable<LineSegment> ConvertPointsToLineSegments(List<Point> points)
        {
            var lineSegments = new List<LineSegment>();
            if (points.Count < 2)
            {
                throw new ArgumentException("lineString must have at least two points to define itself");
            }

            var previousPoint = points.Pop();

            int NSize = points.Count;

            for (int i=0; i < NSize; i++)
            {
                var nextPoint = points.Pop();

                var lineSeg = new LineSegment(previousPoint, nextPoint);

                lineSegments.Add(lineSeg);

                previousPoint = nextPoint;

            }

            return lineSegments;

        }

        #endregion

        #region Public Methods


        #region Converters

        public LineSegment[] ToVector()
        {

            return this.LineSegments.ToArray();
        }

        public static new LineString FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LineString>(json);
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

            LineString other = (LineString)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            bool coordinatesEqual;

            if (this.LineSegments != null && other.LineSegments != null)
            {
                coordinatesEqual = this.LineSegments.SequenceEqual(other.LineSegments);
            }
            else
            {
                coordinatesEqual = (this.LineSegments == null && other.LineSegments == null);
            }

            return this.Type == other.Type &&
                coordinatesEqual &&
                bBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.LineSegments, this.BoundingBox);
        }

        public static bool operator ==(LineString left, LineString right)
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

        public static bool operator !=(LineString left, LineString right)
        {
            return !(left == right);
        }

        public int CompareTo(LineSegment other)
        {
            return this.Length.CompareTo(other.Length);
        }

        public int Compare(LineSegment x, LineSegment y)
        {
            return x.Length.CompareTo(y.Length);
        }


        #endregion Equality Evaluators


        #region Enumerable
        public IEnumerator<LineSegment> GetEnumerator()
        {
            foreach (LineSegment lineSegment in LineSegments)
            {
                yield return lineSegment;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LineStringEnumerator<LineSegment>(this);
        }

        #endregion Enumerable


        #region Topological Operations

        public static Rectangle FetchBoundingBox(IEnumerable<Point> points)
        {
            double MinLongitude = double.MaxValue;

            double MaxLongitude = double.MinValue;


            double MinLatitude = double.MaxValue;


            double MaxLatitude = double.MinValue;


            foreach (var point in points)
            {
                if (MinLongitude > point.GetLongitude())
                {
                    MinLongitude = point.GetLongitude();
                }

                if (MaxLongitude < point.GetLongitude())
                {
                    MaxLongitude = point.GetLongitude();
                }

                if (MinLatitude > point.GetLatitude())
                {
                    MinLatitude = point.GetLatitude();
                }

                if (MaxLatitude < point.GetLatitude())
                {
                    MaxLatitude = point.GetLatitude();
                }
            }

            var LL = new Point(new Coordinate(MinLongitude, MinLatitude));

            var LR = new Point(new Coordinate(MaxLongitude, MinLatitude));

            var UL = new Point(new Coordinate(MinLongitude, MaxLatitude));

            var UR = new Point(new Coordinate(MaxLongitude, MaxLatitude));

            return new Rectangle(LL, LR, UL, UR);
        }


        public static bool Intersects(LineString left, LineString right, double eps = double.MinValue * 100)
        {
            foreach(var left_LineStringSegment in left.LineSegments)
            {
                foreach(var right_LineStringSegment in right.LineSegments)
                {

                    if (left_LineStringSegment.Intersects(right_LineStringSegment, eps))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }

            }

            return false;
            
        }


        public bool Intersects(LineString other, double eps = double.MinValue * 100)
        {
            return Intersects(this, other, eps);
        }

        /// <summary>
        /// Given a LineString and a point, this method evaluates if this point intersects any of the LineSegments that compose the provided LineString.
        /// 
        /// If so, then this point is said to intersect this LineString.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="point"></param>
        /// <param name="EPS"></param>
        /// <returns></returns>
        public static bool Intersects(LineString left, Point point, double eps = double.MinValue * 100)
        {

            LineSegment[] lineSegments = left.LineSegments.ToArray();

            foreach(var linesegment in left.LineSegments)
            {
                if (linesegment.Intersects(point, eps))
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

        public bool Within(Polygon otherGeometry)
        {
            return otherGeometry.Contains(this);
        }

        public bool Intersects(LineString lineString)
        {
            foreach (LineSegment lineSegment in lineString)
            {
                if (this.Intersects(lineSegment))
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


        public bool Intersects(LineSegment other)
        {
            foreach (LineSegment lineSegment in this)
            {
                if (lineSegment.Intersects(other))
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

        public bool Touches(LineString other, double eps = double.MinValue * 100)
        {

            bool condition = false;
            foreach(LineSegment lineSegmentFromOther in other)
            {
                foreach(var point in lineSegmentFromOther)
                {
                    if (this.LineSegments.Any(line => line.Touches(point)))
                    {
                        condition = true;
                    }
                }
            }

            if (Intersects(other, eps))
            {
                return false;
            }
            else
            {
                return condition;
            }
        }


        public bool Equals(Point other)
        {
            return false;
        }

        public bool Intersects(Point other, double eps = double.MinValue * 100)
        {
            return false;
        }

        public bool Touches(Point other, double eps = double.MinValue * 100)
        {
            return other.Touches(this, eps);
        }

        

        public bool Within(Point other, double eps = double.MinValue * 100)
        {
            return false;
        }

        public bool Equals(LineSegment other)
        {
            if (this.LineSegments.Count() == 1)
            {
                return other.Equals(this);
            }
            else
            {
                return false;
            }
        }

        public bool Intersects(LineSegment other, double eps = double.MinValue * 100)
        {
            if (this.LineSegments.Count() == 1)
            {
                return this.LineSegments.First().Intersects(other, eps);
            }
            else
            {
                return false;
            }
        }

        public bool Touches(LineSegment other, double eps = double.MinValue * 100)
        {
            return this.LineSegments.Any(line => line.Touches(other, eps));
        }

        public bool Within(LineSegment other, double eps = double.MinValue * 100)
        {
            return false;
        }

        public bool Equals(LinearRing other)
        {
            
            var ls1 = this.LineSegments.ToList();
            var ls2 = this.LineSegments.ToList();

            for (int i = 0; i < this.LineSegments.Count();i ++)
            {
                if (!ls1[i].Equals(ls2[i]))
                {
                    return false;    
                }
            }

            return true;
        }

        public bool Intersects(LinearRing other, double eps = double.MinValue * 100)
        {
            return this.Intersects(other as LineString, eps);
        }

        public bool Touches(LinearRing other, double eps = double.MinValue * 100)
        {
            return this.Touches(other as LineString, eps);
        }

        public bool Within(LinearRing other, double eps = double.MinValue * 100)
        {
            return false;
        }

        public bool Equals(Polygon other)
        {
            return false;
        }

        public bool Intersects(Polygon other, double eps = double.MinValue * 100)
        {
            return other.LinearRings.Any(l => l.Intersects(this));
        }

        public bool Touches(Polygon other, double eps = double.MinValue * 100)
        {
            return other.LinearRings.Any(l => l.Touches(this));
        }

        public bool Within(Polygon other, double eps = double.MinValue * 100)
        {
            return other.Contains(this);
        }

        public bool Contains(Point other, double eps = double.NegativeInfinity)
        {
            return false;
        }

        public bool Contains(LineSegment other, double eps = double.NegativeInfinity)
        {
            return false;
        }

        public bool Contains(LinearRing other, double eps = double.NegativeInfinity)
        {
            return false;
        }

        public bool Contains(Polygon other, double eps = double.NegativeInfinity)
        {
            return false;
        }


        #endregion Topological Operations

        #endregion Public Methods

    }

    internal class LineStringEnumerator<T> : IEnumerator<T> where T: LineSegment
    {

        #region Proprieties
        public int Position { get; private set; } = 0;

        public List<T> LineSegments { get; private set; }

        T IEnumerator<T>.Current => Current();

        object IEnumerator.Current => Current();


        #endregion Proprieties

        public LineStringEnumerator(LineString lineString)
        {
            LineSegments = lineString.LineSegments.Select(x => (T) x).ToList();
        }

        public T Current()
        {
            return this.LineSegments[Position];
        }

        public void Dispose()
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);

        }

        public bool MoveNext()
        {
            this.Position++;

            if (this.Position < LineSegments.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            this.Position = 0;
        }
    }


}
