using BAMCIS.GeoJSON.Serde;
using Extensions.ArrayExtensions;
using Extensions.ListExtensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A LineSegment is composed solely of two points:
    ///     1) an origin Point (p1)
    ///     
    ///     2) a terminal Point (p2)
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class LineSegment: Geometry,
                              ILine<Point>,
                              ILine<LineSegment>,
                              ILine<LinearRing>,
                              ILine<Polygon>,
                              IEquatable<LineSegment>, 
                              IComparable<LineSegment>, 
                              IComparer<LineSegment>,
                              IEnumerable,
                              IEnumerable<Point>

    {
        #region Properties

        public IEnumerable<Point> Points { 
        
            get
                {
                    return new Point[2] { P1, P2 };
                }
        }
        
        [JsonProperty(PropertyName = "Initial Point")]
        public Point P1 { get; set; }
        
        [JsonProperty(PropertyName = "Final Point")]
        public Point P2 { get; set; }

        [JsonProperty(PropertyName = "Length")]
        public double Length { get; set; }

        public double MinLongitude {

            get {
                return Math.Min(P1.GetLongitude(), P2.GetLongitude()); 
            }
        }

        public double MaxLongitude
        {

            get
            {
                return Math.Max(P1.GetLongitude(), P2.GetLongitude());
            }
        }


        public double MinLatitude
        {

            get
            {
                return Math.Min(P1.GetLatitude(), P2.GetLatitude());
            }
        }

        public double MaxLatitude
        {

            get
            {
                return Math.Max(P1.GetLatitude(), P2.GetLatitude());
            }
        }

        [JsonProperty(PropertyName = "BoundingBox")]
        public override Rectangle BoundingBox
        {
            get
            {

                if (this._BoundingBox == null)
                {
                    this._BoundingBox = FetchBoundingBox();
                }
                return this._BoundingBox;
            }
        }

        /// <summary>
        /// Angle in radians of this lineSegment
        /// </summary>
        public double Angle { get; }


        #endregion Properties


        #region Constructors
        [JsonConstructor]
        public LineSegment(IEnumerable<Point> p1p2) : base(GeoJsonType.LineSegment, p1p2.First().Coordinates.HasElevation())
        {

            var P1P2List = p1p2.ToList();

            if (P1P2List.Count != 2)
            {
                throw new ArgumentException($"{nameof(LineSegment)} only accepts an IEnumerable<Point> of size two");
            }

            this.P1 = P1P2List[0];

            this.P2 = P1P2List[1];


            var dx = ( P1P2List[0].GetLongitude() - P1P2List[1].GetLongitude() );

            var dy = ( P1P2List[0].GetLatitude() - P1P2List[1].GetLatitude() );

            this.Length = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            this.Angle = Math.Atan2(dy, dx);

        }

        public LineSegment(IEnumerable<Coordinate> coordinates) : this(coordinates.Select(c => c.ToPoint()))
        {

        }


        public LineSegment(Point p1, Point p2) : base(GeoJsonType.LineSegment, p1.Coordinates.HasElevation())
        {

            this.P1 = p1;

            this.P2 = p2;


            var dx = ( p1.GetLongitude() - p2.GetLongitude() );

            var dy = ( p1.GetLatitude() - p2.GetLatitude() );

            this.Length = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            this.Angle = Math.Atan2(dy, dx);

        }


        public Rectangle FetchBoundingBox()
        {
            var LL = new Point(new Coordinate(MinLongitude, MinLatitude));

            var LR = new Point(new Coordinate(MaxLongitude, MinLatitude));

            var UL = new Point(new Coordinate(MinLongitude, MaxLatitude));

            var UR = new Point(new Coordinate(MaxLongitude, MaxLatitude));

            return new Rectangle(LL, LR, UL, UR);
        }


        #endregion Constructors

        #region Methods

        #region Private Methods




        #endregion Private Methods


        #region Public Methods

        #region Comparables

        public bool Equals(Point other)
        {
            return false;
        }


        public bool Equals(LineSegment other)
        {
            return (this.P1.Equals(other.P1) &&
                    this.P2.Equals(other.P2)
                    );
        }

        public bool Equals(LinearRing other)
        {
            if (other.LineSegments.Count() > 1)
            {
                return false;
            }
            else
            {
                return this.Equals(other.LineSegments.First());
            }
        }

        public bool Equals(LineString other)
        {
            if (other.LineSegments.Count() > 1)
            {
                return false;
            }
            else
            {
                return this.CompareTo(other.LineSegments.First()) == 0;
            }
        }

        public bool Equals(Polygon other)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            var objCasted = obj as LineSegment;


            if (ReferenceEquals(this, objCasted))
            {
                return true;
            }

            if (objCasted is null)
            {
                return false;
            }

            return this.Equals(objCasted);
        }

        public int CompareTo(LineSegment other)
        {
            if (this.Length < other.Length)
            {
                return -1;
            }
            else if (this.Length > other.Length)
            {
                return 1;
            }
            else{ return 0; }
        }

        public int Compare(LineSegment x, LineSegment y)
        {
            return x.CompareTo(y);
        }
        
        public override int GetHashCode()
        {
            return Tuple.Create(P1.GetHashCode(), P2.GetHashCode()).GetHashCode();
        }

        public static bool operator ==(LineSegment left, LineSegment right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.P1 == right.P1 && left.P2 == right.P2;
        }

        public static bool operator !=(LineSegment left, LineSegment right)
        {
            return !( left == right );
        }

        public static bool operator <(LineSegment left, LineSegment right)
        {
            return left.Length < right.Length;
        }

        public static bool operator <=(LineSegment left, LineSegment right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(LineSegment left, LineSegment right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(LineSegment left, LineSegment right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        #endregion Comparables

        #region Enumerators

        public IEnumerator<Point> GetEnumerator()
        {
            foreach (Point point in Points)
            {
                yield return point;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #endregion Enumerators

        #region Topological Operations

        internal bool HasElevation()
        {
            return this.Points.Any(p => p.Coordinates.HasElevation());
        }
        public bool Within(Polygon otherGeometry)
        {
            return this.Points.All(p => otherGeometry.Contains(p));
        }

        public bool Intersects(Polygon otherGeometry)
        {
            return this.Points.All(p => otherGeometry.Contains(p));
        }

        /// <summary>
        /// The four endpoints are:
        ///     P1) (x0, y0)
        ///     P2) (x1,y1) 
        ///     P3) (a0,b0) 
        ///     P4) (a1,b1)
        ///     
        /// The returned values xy and ab are the fractional distance along xy and ab
        /// and are only defined when the result is true
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="a0"></param>
        /// <param name="b0"></param>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="xy"></param>
        /// <param name="ab"></param>
        /// <returns></returns>
        private static bool FindIntersection(double x0, double y0,
                                             double x1, double y1,
                                             double a0, double b0,
                                             double a1, double b1)
        {

            double xy;
            double ab;

            bool partial = false;
            double denom = ( b0 - b1 ) * ( x0 - x1 ) - ( y0 - y1 ) * ( a0 - a1 );
            if (denom == 0)
            {
                xy = -1;
                ab = -1;

                return false;
            }
            else
            {
                xy = ( a0 * ( y1 - b1 ) + a1 * ( b0 - y1 ) + x1 * ( b1 - b0 ) ) / denom;

                partial = IsBetween(0, xy, 1);
                
                if (partial)
                {
                    // no point calculating this unless xy is between 0 & 1
                    ab = ( y1 * ( x0 - a1 ) + b1 * ( x1 - x0 ) + y0 * ( a1 - x1 ) ) / denom;

                    if (IsBetween(0, ab, 1))
                    {
                        ab = 1 - ab;
                        xy = 1 - xy;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            } 
        }

        
        private static bool IsBetween(double x0, double x, double x1)
        {
            return ( x >= x0 ) && ( x <= x1 );
        }

        public bool WithinBoundaries(LineSegment otherLineSegment)
        {
            return this.BoundingBox.WithinBoundaries(otherLineSegment.BoundingBox);
        }

        /// <summary>
        /// Verifies whether a point is aligned to this linesegment.
        /// 
        /// This approach is done by means of the cross-product between the initial point of the LineSegment (p1) and the 
        /// provided unknown point (p3) and the final point of the LineSegment (p2) and the
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsAligned([NotNull] Point p3, double eps = double.MinValue * 100)
        {
            if (eps == double.NegativeInfinity)
            {
                eps = double.MinValue * 100;
            }

            var p1p3LineSeg = new LineSegment(this.P1, p3);

            double k = Math.Abs(this.Angle - p1p3LineSeg.Angle);

            if (k == 0)
            {
                return true;
            }
            else if (k <= eps)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Intersects(Point other, double eps = double.MinValue * 100)
        {
            return false;
        }

        /// <summary>
        /// Verifies whether a point intersects to this linesegment.
        /// 
        /// A point (p) intersects a lineSegment if (and only if) two conditions are met:
        ///     1) the direction of the vector composed of the points p and any of the edges of the linesegment is equal to the direction of the lineSegment itself
        ///     2) the coordinates of the point must be within the boundaries of the linesegment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Touches(Point point, double eps = double.MinValue * 100)
        {
            if (IsAligned(point, eps))
            {
                if (point.GetLatitude() < this.MinLatitude ||
                    point.GetLatitude() > this.MaxLatitude ||
                    point.GetLongitude() < this.MinLongitude ||
                    point.GetLongitude() > this.MaxLongitude)
                    {
                        return false;
                    }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Within(Point other, double eps = double.MinValue * 100)
        {
            return false;
        }



        public bool Intersects(LineSegment otherGeometry, double eps = double.MinValue * 100)
        {
            var angleDif = Math.Abs(otherGeometry.Angle - this.Angle);

            if (angleDif < eps)
            {
                return FindIntersection(this.P1.Coordinates.Longitude, this.P1.Coordinates.Latitude,
                                        this.P2.Coordinates.Longitude, this.P2.Coordinates.Latitude,
                                        otherGeometry.P1.Coordinates.Longitude, otherGeometry.P1.Coordinates.Latitude,
                                        otherGeometry.P2.Coordinates.Longitude, otherGeometry.P2.Coordinates.Latitude
                                        );
            }
            else
            {
                return false;
            }
        }

        public bool Touches(LineSegment otherlineSegment, double eps = double.MinValue * 100)
        {
            
            // Checking first if any of the points that compose both lineSegments are equal, and also verifying if the lines do not intersect each other.
            // If so, then, the lineSegments do touch each other;
            if (otherlineSegment.Intersects(this))
            {
                return false;
            }
            else
            {
                if (otherlineSegment.Any(pOther => this.Equals(pOther)))
                {
                    return true;
                }
                // In this second, case, we must verify if the lines touch, and, furthermore, if they do not intersects.
                else
                {
                    if (otherlineSegment.Any(pOther => this.Touches(pOther))
                      )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool Within(LineSegment other, double eps = double.MinValue * 100)
        {
            return false;
        }

        

        public bool Intersects(LinearRing other, double eps = double.MinValue * 100)
        {
            foreach(LineSegment lineSegmentFromOther in other)
            {
                if (this.Intersects(lineSegmentFromOther))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Touches(LinearRing other, double eps = double.MinValue * 100)
        {
            foreach (LineSegment lineSegmentFromOther in other)
            {
                if (this.Touches(lineSegmentFromOther))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Within(LinearRing other, double eps = double.MinValue * 100)
        {
            return false;
        }



        public bool Intersects(Polygon other, double eps = double.MinValue * 100)
        {
            return other.Intersects(this, eps);
        }

        public bool Touches(Polygon other, double eps = double.MinValue * 100)
        {
            foreach(LinearRing linearRingFromOther in other)
            {
                if (this.Touches(linearRingFromOther))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Within(Polygon other, double eps = double.MinValue * 100)
        {
            return other.Contains(this);
        }




        #endregion Topological Operations


        #endregion Public Methods

        #region Converters

        public static LineString CoordinatesToLineString(IEnumerable<Coordinate> coordinates)
        {
            List<LineSegment> lineSegments = PositionsToLineSegments(coordinates);

            return new LineString(lineSegments);
        }

        public static List<LineSegment> PositionsToLineSegments(IEnumerable<Coordinate> _positions)
        {
            var positions = _positions.ToList();
            var lineSegments = new List<LineSegment> { };
            
            var position_init = positions.Pop();

            var NPositions = positions.Count;

            for (var i = 0; i < NPositions; i++)
            {
                
                var position_final = positions.Pop();

                var lineSegment = new LineSegment(new Point(position_init),
                                                  new Point(position_final)
                                                  );

                lineSegments.Add(lineSegment);

                position_init = position_final;

            }

            return lineSegments;
        }

        public static new LineSegment FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LineSegment>(json);
        }
        #endregion Converters


        #endregion Methods

    }



    internal class LineSegmentEnumerator : IEnumerator, IDisposable
    {

        #region Proprieties
        public int Position { get; private set; } = 0;

        public List<Point> Coordinates { get; private set; }

        #endregion Proprieties

        public LineSegmentEnumerator(LineSegment lineSegment)
        {
            Coordinates = new List<Point> { lineSegment.P1, lineSegment.P2 };
        }

        public Point Current()
        {
            return this.Coordinates[Position];
        }

        object IEnumerator.Current
        {
            get
            {
                return Current();
            }
        }


        public void Dispose()
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);

        }

        public bool MoveNext()
        {
            this.Position++;

            if (this.Position < Coordinates.Count)
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
