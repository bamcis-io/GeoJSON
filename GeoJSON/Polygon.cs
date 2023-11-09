using BAMCIS.GeoJSON.Serde;
using Extensions.ArrayExtensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{


    /// <summary>
    /// A polygon is formed with 1 or more linear rings, which are an enclosed LineString.
    /// 
    /// 
    /// For spatial operations, check:
    ///     https://www.topcoder.com/thrive/articles/Geometry%20Concepts%20part%203:%20Using%20Geometry%20in%20Topcoder%20Problems
    /// </summary>
    [JsonConverter(typeof(PolygonConverter))]
    public class Polygon : Geometry,
                           IEquatable<Polygon>,
                           IContains<Point>,
                           ITouch<Point>,
                           IPolygon<LineSegment>, 
                           IPolygon<LineString>,
                           IEnumerable<LinearRing>

    {
   

        #region Public Properties

        /// <summary>
        /// The coordinates are an array of linear ring coordinate arrays.
        /// For Polygons with more than one of these rings, the first MUST be
        /// the exterior ring, and any others MUST be interior rings.The
        /// exterior ring bounds the surface, and the interior rings(if
        /// present) bound holes within the surface.
        /// </summary>
        [JsonProperty(PropertyName = "LinearRings")]
        [JsonIgnore]
        public IEnumerable<LinearRing> LinearRings { get; set; } 
           

        /// <summary>
        /// The points that represent the linearRings of the polygon
        /// </summary>
        [JsonProperty(PropertyName = "Points")]
        [JsonIgnore]
        public IEnumerable<Point> Points
        {
            get
            {
                return this.LinearRings.SelectMany(lineRing => lineRing.Points).ToList();
            }
        }

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public override Rectangle BoundingBox { get { return FetchBoundingBox(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Polygon
        /// </summary>
        /// <param name="coordinates">The linear rings that make up the polygon</param>
        [JsonConstructor]
        public Polygon(IEnumerable<LinearRing> linearRings) : base(GeoJsonType.Polygon, linearRings.Any(x => x.IsThreeDimensional()))
        {
            this.LinearRings = linearRings ?? throw new ArgumentNullException("Points");

            if (!this.LinearRings.Any())
            {
                throw new ArgumentOutOfRangeException("Points", "A polygon must have at least 1 linear ring.");
            }
        }

        

        /// <summary>
        /// Creates a new Polygon
        /// </summary>
        /// <param name="coordinates">The linear rings that make up the polygon</param>
        
        public Polygon(LinearRing linearRing) : base(GeoJsonType.Polygon, linearRing.Any(x => x.HasElevation()))
        {
            this.LinearRings = new List<LinearRing> { linearRing } ?? throw new ArgumentNullException("Points");

            if (!this.LinearRings.Any())
            {
                throw new ArgumentOutOfRangeException("Points", "A polygon must have at least 1 linear ring.");
            }
        }

        
        public Polygon(IEnumerable<IEnumerable<IEnumerable<Coordinate>>> coordinates) : this(CoordinatesToLinearRings(coordinates))
        {

        }

        public static IEnumerable<LinearRing> CoordinatesToLinearRings(IEnumerable<IEnumerable<IEnumerable<Coordinate>>> coordinates)
        {
            var rings = new List<LinearRing>();

            foreach (var lineRing in coordinates)
            {
                var positionsOfASingleLineRing = new List<Coordinate>();

                foreach (var lineSegments in lineRing)
                {
                    List<Coordinate> posWithinALineSegment = lineSegments.ToList();
                    positionsOfASingleLineRing.AddRange(posWithinALineSegment);
                }

                var ring = new LinearRing(LineSegment.PositionsToLineSegments(positionsOfASingleLineRing));

                rings.Add(ring);
            }

            return rings;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Removes the interior linear rings that bound holes within the surface from the polygon's coordinates
        /// leaving just 1 linear ring in the coordinates.
        /// </summary>
        /// <returns>Returns true if the polygon had more than linear ring and false if there were no linear rings to remove</returns>
        
        #region Comparable Methods

        /// <summary>
        /// Deserializes the json into a Polygon
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>A Polygon object</returns>
        public static new Polygon FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Polygon>(json);
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

            var other = obj as Polygon;

            if (other == null) 
            { 
                return false; 
            }

            return this.Equals(other);
            
        }


        public bool Equals(Polygon other)
        {
            bool bBoxEqual;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.Equals(other.BoundingBox);
            }
            else
            {
                bBoxEqual = ( this.BoundingBox == null && other.BoundingBox == null );
            }

            if (bBoxEqual)
            {
                bool coordinatesEqual = true;

                if (this.LinearRings != null && other.LinearRings != null)
                {
                    coordinatesEqual = this.LinearRings.SequenceEqual(other.LinearRings);
                }
                else
                {
                    coordinatesEqual = ( this.LinearRings == null && other.LinearRings == null );
                }

                return coordinatesEqual && this.Type == other.Type;
            }

            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.LinearRings, this.BoundingBox);
        }


        public static bool operator ==(Polygon left, Polygon right)
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

        public static bool operator !=(Polygon left, Polygon right)
        {
            return !(left == right);
        }

        #endregion Comparable Methods


        #region Enumeration

        public IEnumerator<LinearRing> GetEnumerator()
        {
            foreach (var line in LinearRings)
            {
                yield return line;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Enumeration


        #region Topological Operations

        public Rectangle FetchBoundingBox()
        {
            double MinLongitude = double.MaxValue;

            double MaxLongitude = double.MinValue;

            double MinLatitude = double.MaxValue;

            double MaxLatitude = double.MinValue;


            foreach (LinearRing line in this.LinearRings)
            {
                if (MinLongitude > line.BoundingBox.MinLongitude)
                {
                    MinLongitude = line.BoundingBox.MinLongitude;
                }

                if (MaxLongitude < line.BoundingBox.MaxLongitude)
                {
                    MaxLongitude = line.BoundingBox.MaxLongitude;
                }

                if (MinLatitude > line.BoundingBox.MinLatitude)
                {
                    MinLatitude = line.BoundingBox.MinLatitude;
                }

                if (MaxLatitude < line.BoundingBox.MaxLatitude)
                {
                    MaxLatitude = line.BoundingBox.MaxLatitude;
                }
            }

            var LL = new Point(new Coordinate(MinLongitude, MinLatitude));

            var LR = new Point(new Coordinate(MinLongitude, MinLatitude));

            var UL = new Point(new Coordinate(MinLongitude, MinLatitude));

            var UR = new Point(new Coordinate(MinLongitude, MinLatitude));

            return new Rectangle(LL, LR, UL, UR);

        }

        public void RemoveInteriorRings()
        {
            // If there is more than element


            if (this.LinearRings != null && this.LinearRings.Count()>1)
            {
                this.LinearRings = new List<LinearRing> { this.LinearRings.First() };
            }
        }

        public LinearRing OuterRing()
        {
            return this.LinearRings.First();
        }

        public bool Contains(Point point, double eps = double.MinValue * 100)
        {

            int numberOfRingsThatContainPoint = 0;
            bool firstRingContainsPoint = false;
            bool otherRingsContainPoint = false;

            int ringCounter = 0;
            foreach (var ring in LinearRings)
            {
                if(ring.Contains(point))
                {
                    if (ringCounter == 0)
                    {
                        firstRingContainsPoint = true;
                    }
                    else{
                        otherRingsContainPoint = true;
                    }

                    numberOfRingsThatContainPoint++;
                }

                ringCounter++;
            }

            if (firstRingContainsPoint && !otherRingsContainPoint)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }


        public bool Intersects(Point point)
        {
            var PolygonPoints = FetchEdge().ToList();

            int signIsAlwaysEquals = 0;

            for (int i = 1; i < PolygonPoints.Count; i++)
            {
                var Vii = PolygonPoints[i];

                var Vi = PolygonPoints[i - 1];

                var Vii_Vi = Vii - Vi;

                var PVI = ( point - Vi );


                var CrossProd = Vii_Vi.ToArray().CrossProduct2D(PVI.ToArray());

                if (CrossProd > 0)
                {
                    if (signIsAlwaysEquals < 0)
                    {
                        return false;
                    }
                    else
                    {
                        signIsAlwaysEquals = -1;
                    }
                }
                else if (CrossProd == 0)
                {
                    continue;
                }
                else
                {
                    if (signIsAlwaysEquals > 0)
                    {
                        return false;
                    }
                    else
                    {
                        signIsAlwaysEquals = -1;
                    }
                }
            }
            return true;
        }

        public bool Touches(Point point, double eps = double.MinValue * 100)
        {
            foreach(var line in LinearRings)
            {
                if (line.Touches(point))
                {
                    return true;
                }
                else
                { }
            }
            return false;
        }

        public bool Touches(LineString lineString, double eps = double.MinValue * 100)
        {
            foreach (LinearRing lineRing in LinearRings)
            {
                if (lineRing.Touches(lineString, eps))
                {
                    return true;
                }
                else
                { }
            }
            return false;
        }


        public bool Contains(LineString lineString, double eps = double.MinValue * 100)
        {
            return lineString.Points.All(l => this.Contains(l));
        }

        /// <summary>
        /// In order to check if a lineSegment is within a polygon, one must do three steps:
        ///     1) sort the edges of the polygon in clock-wise (or counter clock-wise) order
        ///     2) evaluate the cross-product of (V[i+1] - V[i]) x (P - V[i]) for every i-esimal vertice within the polygon
        ///     3) compare the sign of all these cross-products. if they are all of same sign (e.g., all positive, or all negative),
        ///         then point P is within the polygon.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Contains(LineSegment lineSegment, double eps = double.MinValue * 100)
        {
            return lineSegment.Points.All(p => this.Contains(p, eps));
        }

        public bool Intersects(LineSegment lineSegment, double eps = double.MinValue * 100)
        {
            int pointWithin = 0;
            foreach (Point point in lineSegment.Points)
            {
                if (point.Touches(this, eps))
                {
                    pointWithin ++;
                }

            }

            if (pointWithin > 1 || pointWithin < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Intersects(LineString lineString, double eps = double.MinValue * 100)
        {
            
            foreach (LineSegment lineSeg in lineString.LineSegments)
            {
                if (this.Intersects(lineSeg, eps))
                {
                    return true;
                }

            }

            return true;
            
        }

        /// <summary>
        /// Return The Points that compose the outer-ring of the polygon.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> FetchEdge()
        {
            return LinearRings.First().Points;
        }

        internal bool Within(LinearRing linearRing)
        {
            return linearRing.Contains(this);
        }
        internal bool Within(Polygon polygon)
        {
            return polygon.Contains(this.LinearRings.First());
        }

        #endregion Topological Operations


        #endregion Public Methods
    }
}
