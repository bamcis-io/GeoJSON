using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Represents a collection of feature objects
    /// </summary>
    [JsonConverter(typeof(FeatureCollectionConverter))]
    public class FeatureCollection : GeoJson, IEnumerable<Feature>
    {
        #region Public Properties

        /// <summary>
        /// The collection of features
        /// </summary>
        [JsonProperty(PropertyName = "features")]
        public IEnumerable<Feature> Features { get; }

        
        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public Rectangle BoundingBox { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// The default constructor building the feature collection
        /// </summary>
        /// <param name="features">The features that are part of the feature collection</param>
        [JsonConstructor]
        public FeatureCollection(IEnumerable<Feature> features) : base(GeoJsonType.FeatureCollection, features.Any(x => x.IsThreeDimensional()))
        {
            this.Features = features ?? throw new ArgumentNullException(nameof(features));

            this.BoundingBox = FetchBoundingBox();

        }

        public Rectangle FetchBoundingBox()
        {
            double MaxLatitude = double.MinValue;
            double MaxLongitude = double.MinValue;
            double MinLatitude = double.MaxValue;
            double MinLongitude = double.MaxValue;

            foreach (Feature feature in this.Features)
            {

                if (feature.Geometry?.BoundingBox != null)
                {
                    if (MaxLatitude < (feature?.Geometry?.BoundingBox?.MaxLatitude ?? 0.0))
                    {
                        MaxLatitude = (double) ( feature?.Geometry?.BoundingBox.MaxLatitude );
                    }

                    if (MaxLongitude < (feature?.Geometry?.BoundingBox?.MaxLongitude ?? 0.0 ))
                    {
                        MaxLongitude = (double)feature.Geometry?.BoundingBox.MaxLongitude;
                    }

                    if (MinLatitude > (feature?.Geometry?.BoundingBox?.MinLatitude ?? 0.0 ))
                    {
                        MinLatitude = (double) feature?.Geometry.BoundingBox.MinLatitude;
                    }

                    if (MinLongitude > (feature?.Geometry?.BoundingBox?.MinLongitude ?? 0.0 ))
                    {
                        MinLongitude = (double) feature?.Geometry?.BoundingBox.MinLongitude;
                    }
                }
                else
                {
                    MaxLatitude = 0;
                    MaxLongitude = 0;
                    MinLatitude = 0;
                    MinLongitude = 0;
                }
            }

            var LL = new Point(new Coordinate(MinLongitude, MinLatitude));
            var LR = new Point(new Coordinate(MaxLongitude, MinLatitude));
            var UL = new Point(new Coordinate(MinLongitude, MaxLatitude));
            var UR = new Point(new Coordinate(MaxLongitude, MaxLatitude));

            return new Rectangle(LL, LR, UL, UR);
        }

        #endregion

        #region Public Methods

        #region Conversion Methods
        public new static FeatureCollection FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FeatureCollection>(json);
        }

        #endregion Conversion Methods

        #region Equality Operations

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

            FeatureCollection other = (FeatureCollection)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            bool featuresEqual = true;

            if (this.Features != null && other.Features != null)
            {
                featuresEqual = this.Features.SequenceEqual(other.Features);
            }
            else
            {
                featuresEqual = (this.Features == null && other.Features == null);
            }

            return this.Type == other.Type &&
                featuresEqual &&
                bBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Features, this.BoundingBox);
        }

        public static bool operator ==(FeatureCollection left, FeatureCollection right)
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

        public static bool operator !=(FeatureCollection left, FeatureCollection right)
        {
            return !(left == right);
        }

        #endregion Equality Operations

        #endregion Public Methods

        #region Enumerable
        public IEnumerator<Feature> GetEnumerator()
        {
            foreach (Feature feat in this.Features)
            {
                yield return feat;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Enumerable
    }
}
