using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class Rectangle : Polygon,
                             IEquatable<Rectangle>
    {
        #region Properties
        public double MinLongitude { get; set; } = double.MaxValue;

        public double MaxLongitude { get; set; } = double.MinValue;
        

        public double MinLatitude { get; set; } = double.MaxValue;


        public double MaxLatitude { get; set; } = double.MinValue;

        /// <summary>
        /// Lower Left corner of the Bounding Box
        /// </summary>
        public Point LL { get; set; }

        /// <summary>
        /// Lower Right corner of the Bounding Box
        /// </summary>
        public Point LR { get; set; }

        /// <summary>
        /// Upper Left corner of the Bounding Box
        /// </summary>
        public Point UL { get; set; }

        /// <summary>
        /// Upper Right corner of the Bounding Box
        /// </summary>
        public Point UR { get; set; }


        /// <summary>
        /// Returns the LL, LR, UL, UR points of this bounding box
        /// </summary>
        [JsonProperty(PropertyName = "Points")]
        [JsonIgnore]
        public new IEnumerable<Point> Points
        {
            get
            {
                return new List<Point> { LL, LR, UL, UR };
            }
        }


        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
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

        #endregion Properties


        #region Constructors

        public Rectangle(LinearRing lineRing) : base(new LinearRing(lineRing))
        {

            
            foreach (var point in lineRing.Points)
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

            this.LL = LL;

            this.LR = LR;

            this.UL = UL;

            this.UR = UR;

        }

        
        public Rectangle(Point LL, Point LR, Point UL, Point UR): this(new LinearRing(new List<Coordinate> { UL.Coordinates, 
                                                                                                             UR.Coordinates, 
                                                                                                             LR.Coordinates, 
                                                                                                             LL.Coordinates, 
                                                                                                             UL.Coordinates }))
        {
           
        }


        public Rectangle(Coordinate LL, Coordinate LR, Coordinate UL, Coordinate UR) : this(LL.ToPoint(), LR.ToPoint(), UL.ToPoint(), UR.ToPoint())
        {

        }


        #endregion Constructors


        #region Topographic Operations

        public new Rectangle FetchBoundingBox()
        {
            return this;
        }

        public IEnumerable<Point> FetchCoordinates()
        {

            var list = new List<Point> { this.UL, this.UR, this.LR, this.LL};

            foreach (var p in list)
            { 
                yield return p;
            }
        }

        /// <summary>
        /// Verifies whether the other Rectangle boundaries are within this one.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public bool WithinBoundaries(Rectangle other)
        {
            if (this.MinLatitude <= other.MinLatitude &&
                this.MaxLatitude >= other.MaxLatitude &&
                this.MinLongitude <= other.MinLongitude &&
                this.MaxLongitude >= other.MaxLongitude
                )
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        public bool Contains(Point point)
        {
            if (this.MaxLatitude >= point.GetLatitude() &&
            
                this.MinLatitude <= point.GetLatitude() &&
                
                this.MaxLongitude >= point.GetLongitude() && 
                
                this.MinLongitude <= point.GetLongitude() ) { return true; }

            return false;
        }

        #endregion Topographic Operations

        #region Equality Evaluators

        public bool Equals(Rectangle other)
        {
            return (this.MinLatitude == other.MinLatitude &&
                    this.MaxLatitude == other.MaxLatitude &&
                    this.MinLongitude == other.MinLongitude &&
                    this.MaxLongitude == other.MaxLongitude
                    );
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rectangle);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.Points.ToList()).GetHashCode();
        }

        #endregion Equality Evaluators

        
    }


    internal class BBoxEnumerator : IEnumerator, IDisposable
    {

        #region Proprieties
        public int Position { get; private set; } = 0;

        public List<Point> Coordinates { get; private set; }

        #endregion Proprieties

        public BBoxEnumerator(Rectangle rectangle)
        {
            Coordinates = new List<Point> { rectangle.UL, rectangle.UR, rectangle.LR, rectangle.LL };
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
