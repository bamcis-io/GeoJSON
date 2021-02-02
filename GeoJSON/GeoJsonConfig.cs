using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Provides global configuration options for GeoJson serialization and 
    /// deserialization
    /// </summary>
    public static class GeoJsonConfig
    {
        #region Private Fields

        private static volatile object sync = new object();

        private static bool _ignoreLongitudeValidation;
        private static bool _ignoreLatitudeValidation;

        #endregion

        /// <summary>
        /// Set to true to ignore the validation applied to longitude
        /// coordinates in Position objects
        /// </summary>
        public static bool IgnoreLongitudeValidation { 
            get 
            { 
                return _ignoreLongitudeValidation; 
            } 
            set 
            {
                lock (sync)
                {
                    _ignoreLongitudeValidation = value;
                }
            } 
        }

        /// <summary>
        /// Set to true to ignore the validation applied to latitude
        /// coordinates in Position objects
        /// </summary>
        public static bool IgnoreLatitudeValidation {
            get
            {
                return _ignoreLatitudeValidation;
            }
            set
            {
                lock (sync)
                {
                    _ignoreLatitudeValidation = value;
                }
            }
        }

        static GeoJsonConfig()
        {
            IgnoreLongitudeValidation = false;
            IgnoreLatitudeValidation = false;
        }

        /// <summary>
        /// Convenience method for ignoring both latitude and longitude
        /// validation in Position objects, this operation is thread safe.
        /// </summary>
        public static void IgnorePositionValidation()
        {
            lock (sync)
            {
                IgnoreLatitudeValidation = true;
                IgnoreLongitudeValidation = true;
            }
        }

        /// <summary>
        /// Convenience method for enforcing both latitude and longitude
        /// validation in Position objects, this operation is thread safe.
        /// </summary>
        public static void EnforcePositionValidation()
        {
            lock (sync)
            {
                IgnoreLatitudeValidation = false;
                IgnoreLongitudeValidation = false;
            }
        }
    }
}
