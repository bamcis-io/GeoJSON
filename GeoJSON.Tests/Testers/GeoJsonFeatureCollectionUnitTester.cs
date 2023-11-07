using BAMCIS.GeoJSON;
using GeoJSON.Tests.Tests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class GeoJsonFeatureCollectionUnitTester : GeoJsonBaseUnitTester
    {

        [Fact]
        public void FeatureCollectionTest()
        {
            // ARRANGE
            var featCollection = FetchDefaultFeaturecollection();
            string content = JsonConvert.SerializeObject(featCollection);

            
            // ACT
            FeatureCollection geo = JsonConvert.DeserializeObject<FeatureCollection>(content);
            string contentDeserialized = JsonConvert.SerializeObject(geo);
            FeatureCollection geoAfterDesrialization = JsonConvert.DeserializeObject<FeatureCollection>(contentDeserialized);

            // ASSERT
            Assert.True(geo.Equals(geoAfterDesrialization));
        }

        private static FeatureCollection FetchDefaultFeaturecollection()
        {
            var linearRing = new LinearRing(new List<LineSegment>{
                                                                    new LineSegment(new Point(100,0),
                                                                                    new Point(101,0)
                                                                             ),

                                                                    new LineSegment(new Point(101,0),
                                                                                    new Point(101,1)
                                                                             ),

                                                                     new LineSegment(new Point(101,1),
                                                                                     new Point(100,1)
                                                                             ),

                                                                     new LineSegment(new Point(100,1),
                                                                                     new Point(100,0)
                                                                             )
            });

            var polygon = new Polygon(linearRing);


            var LineString = new LineString(new List<Point> { new Point(102.0, 0.0), new Point(103.0, 1.0) });

            var point = new Point(102, 0.5);


            var featCol = new FeatureCollection(new List<Feature> { new Feature(point, new Dictionary<string, dynamic>{ {"PropertyToTest", "Test" } } ), 
                                                                    new Feature(LineString, new Dictionary<string, dynamic>{ {"PropertyToTest", "Test" } }),
                                                                    new Feature(polygon, new Dictionary<string, dynamic>{ {"PropertyToTest", "Test" } })
                                                                   });

            return featCol;
            
        }
    }
}
