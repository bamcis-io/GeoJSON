using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    public class GeometryEnumerator<T> : IEnumerator<T>  where T: Geometry
    {

        public int Position { get; private set; } = 0;

        public T[] Geometries { get; private set; }

        public GeometryEnumerator(IEnumerable<T> geometries)
        {
            Geometries = geometries.ToArray();
        }

        public T Current()
        {
            return this.Geometries[Position];
        }

        object IEnumerator.Current
        {
            get
            {
                return Current();
            }
        }

        T IEnumerator<T>.Current
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
            this.Position ++;

            if (this.Position < Geometries.Count())
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