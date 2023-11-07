using BAMCIS.GeoJSON;
using GeoJSON.Tests.Tests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class GeoJsonPolygonUnitTester : GeoJsonBaseUnitTester
    {

        private Polygon FetchDefaultPolygon()
        {

            var linearRing = new LinearRing(new List<List<Coordinate>>{ new List<Coordinate> { new Coordinate(40, 40) ,
                                                                                           new Coordinate(20,45) ,
                                                                                           new Coordinate(45, 30) ,
                                                                                           new Coordinate(40,40)
                                                                                         }

                                                                    }
                                           );
            var polygon = new Polygon(linearRing);

            return polygon;
        }

        [Fact]
        public void SerializePolygon()
        {
            // ARRANGE

            var polygon = FetchDefaultPolygon();
            string polygonContent = JsonConvert.SerializeObject(polygon);
            WriteJsonFile("ReferenceFiles/Polygon.json", polygonContent);

            string content = ReadJsonFile("ReferenceFiles/Polygon.json");

            // ACT
            var PolygonReconstituted = JsonConvert.DeserializeObject<Polygon>(content);
            

            // ASSERT
            Assert.True(polygon.Equals(PolygonReconstituted));
        }

    }
}
