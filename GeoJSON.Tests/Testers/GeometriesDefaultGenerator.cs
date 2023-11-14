using BAMCIS.GeoJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoJSON.Tests.Tests
{
    internal static class GeometriesDefaultGenerator
    {

        internal static LineString GenerateDefaultLineStringForUnitTests(int maxLineSegmentsPerLineString = 4, int x0 = 0, int y0 = 0)
        {
            var lineSegments = new List<LineSegment>();

            for (int lineSegmentCounter = 0; lineSegmentCounter < maxLineSegmentsPerLineString; lineSegmentCounter++)
            {
                for (int x = x0; x < x0 + 2; x++)
                {
                    int y = 0;
                    var p1 = new Point(new Coordinate(x, y));

                    for (y = y0; y < y0 + 2; y++)
                    {
                        var p2 = new Point(new Coordinate(x, y));

                        if (p1 == p2)
                        {

                        }
                        else
                        {
                            var lineSegment = new LineSegment(p1, p2);
                            lineSegments.Add(lineSegment);
                        }
                    }
                }
            }

            var geo = new LineString(lineSegments);
            return geo;
        }


    }
}
