using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Represents a dynamic Feature Id that can be provided as a string or
    /// integer value
    /// </summary>
    [JsonConverter(typeof(FeatureIdConverter))]
    public class FeatureId
    {
        #region Private Properties

        private string stringValue = null;
        private Int64? intValue = null;
        private Type originalType = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// The string representation of the value
        /// </summary>
        public string Value { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create the id with a string value
        /// </summary>
        /// <param name="id"></param>
        public FeatureId(string id)
        {
            this.stringValue = id;
            this.originalType = typeof(string);
            this.Value = id;
        }

        /// <summary>
        /// Create the id with an integer value
        /// </summary>
        /// <param name="id"></param>
        public FeatureId(Int64 id)
        {
            this.intValue = id;
            this.originalType = typeof(int);
            this.Value = id.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides the original type of the id, either string or integer
        /// </summary>
        /// <returns></returns>
        public Type GetOriginalType()
        {
            return this.originalType;
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

            FeatureId other = (FeatureId)obj;

            if (other.originalType != this.originalType)
            {
                return false;
            }

            if (other.originalType == typeof(string))
            {
                return other.stringValue == this.stringValue;
            }
            else
            {
                return other.intValue == this.intValue;
            }
        }

        public override int GetHashCode()
        {
            if (this.originalType == typeof(string))
            {
                return Hashing.Hash(this.stringValue);
            }
            else
            {
                return Hashing.Hash(this.intValue);
            }
        }

        public static bool operator ==(FeatureId left, FeatureId right)
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

        public static bool operator !=(FeatureId left, FeatureId right)
        {
            return !(left == right);
        }

        #endregion
    }
}
