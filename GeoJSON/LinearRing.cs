using BAMCIS.GeoJSON.Serde;
using Extensions.ArrayExtensions;
using Extensions.ListExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A linear ring is a closed LineString with four or more positions.
    /// 
    /// A linear ring is the boundary of a surface or the boundary of a
    /// hole in a surface.
    /// 
    /// A linear ring MUST follow the right-hand rule with respect to the
    /// area it bounds, i.e., exterior rings are counterclockwise, and
    /// holes are clockwise.
    /// </summary>
    [JsonConverter(typeof(LinearRingConverter))]
    public class LinearRing : LineString
    {
        


        #region Constructors

        
        /// <summary>
        /// Creates a new LinearRing
        /// </summary>
        /// <param name="coordinates">The coordinates that make up the linear ring</param>
        [JsonConstructor]
        public LinearRing(IEnumerable<LineSegment> lineSegments) : base(lineSegments)
        {
            LineSegment[] lines = lineSegments.ToArray();

            if (lines.Length < 3)
            {
                throw new ArgumentOutOfRangeException("A linear ring requires at least 3 lineSegments (or 4 points).");
            }

            if (!lines.First().P1.Equals(lines.Last().P2))
            {
                throw new ArgumentException("The first and last Points of the LinearRing must be equivalent.");
            }
        }

        /// <summary>
        /// Creates a new LinearRing
        /// </summary>
        /// <param name="coordinates">The coordinates that make up the linear ring</param>
        public LinearRing(IEnumerable<Coordinate> coordinates) : this(LinearRing.PositionsToLineSegments(coordinates))
        {
            this.Points = coordinates.Select(x => x.ToPoint()).ToList() ;

            if (Points.Count() < 4)
            {
                throw new ArgumentOutOfRangeException("A linear ring requires at least 4 coordinates.");
            }

            var c1 = this.Points.First();

            var c2 = this.Points.Last();

            if (!c1.Equals(c2))
            {
                throw new ArgumentException("The first and last Points of the LinearRing must be equivalent.");
            }

        }

        #endregion Constructors

        #region Topographic Operations

        internal new bool Touches(LineString lineString, double eps)
        {
            foreach(LineSegment lineSeg in lineString.LineSegments)
            {
                if (this.Touches(lineSeg, eps))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(Point point)
        {
            if (!this.BoundingBox.Contains(point))
            {
                return false;
            }

            var points = this.Points.ToList();

            int signIsAlwaysEquals = 0;

            for (int i = 1; i < points.Count; i++)
            {
                var Vii = points[i];

                var Vi = points[i - 1];

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


        public bool Contains(LineSegment lineSegment)
        {
            return lineSegment.All(p => this.Contains(p));
        }

        public bool Contains(LineString lineString)
        {
            return lineString.Points.All(p => this.Contains(p));
        }

        public static bool Within(LineString _)
        {
            return false;
        }

        public bool Within(LinearRing lineRing)
        {
            return this.Points.All(p => lineRing.Contains(p));
        }

        public new bool Within(Polygon polygon)
        {
            return polygon.Contains(this);
        }

        public bool Contains(Polygon polygon)
        {
            return this.Contains(polygon.LinearRings.First());
        }

        #endregion Topographic Operations



        #region Public Methods

        public int CountPointsthatComposeThisLineRing()
        {
            int points = 0;

            foreach(LineSegment line in LineSegments) {

                // Each linesegment is composed of solely 2 points:

                points += 2;
            }

            return points;
        }

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

            Coordinate prior_position = position_init.Copy();

            Coordinate after_position;

            var NPositions = positions.Count;

            for (var i = 0; i < NPositions; i++)
            {

                after_position = positions.Pop();

                var lineSegment = new LineSegment(new Point(prior_position),
                                                  new Point(after_position)
                                                  );

                lineSegments.Add(lineSegment);

                prior_position = after_position;

            }

            var closingLineSegment = new LineSegment(new Point(prior_position),
                                              new Point(position_init)
                                             );

            lineSegments.Add(closingLineSegment);

            return lineSegments;
        }

        #endregion Public Methods


    }
}
