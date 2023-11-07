using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAMCIS.GeoJSON.Wkb
{
    public static class WkbConverter
    {
        #region Public Methods

        /// <summary>
        /// Returns the geometry of the specified type from WKB.
        /// </summary>
        /// <typeparam name="T">The geometry type, i.e. Point, LineString, etc</typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T FromBinary<T>(byte[] bytes) where T : Geometry
        {
            return (T)FromBinary(bytes);
        }

        /// <summary>
        /// Returns the generic Geometry object from the WKB, but can be
        /// casted to the specific underlying geometry object like Point, LineString,
        /// Polygon, etc.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Geometry FromBinary(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (EndianAwareBinaryReader reader = new EndianAwareBinaryReader(stream))
                {
                    return FromBinary(reader);
                }
            }
        }

        /// <summary>
        /// Converts a geometry object to WKB based on what it's real underlying
        /// type is, like Point or LineString.
        /// </summary>
        /// <param name="geo">The geometry to serialize</param>
        /// <param name="endianness">The endianness to use during serialization, defaults to LITTLE endian.</param>
        /// <returns></returns>
        public static byte[] ToBinary(Geometry geo, Endianness endianness = Endianness.LITTLE)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, endianness))
                {
                    ToBinary(writer, geo);

                    return stream.ToArray();
                }
            }
        }

        #endregion

        #region Private Methods

        #region From Binary Methods

        private static Geometry FromBinary(EndianAwareBinaryReader reader)
        {
            Endianness endianness = (Endianness)reader.ReadByte();
            reader.SetEndianness(endianness);
            
            WkbType type = (WkbType)reader.ReadUInt32();

            switch (type)
            {
                case WkbType.Geometry:
                    {
                        return FromBinary(reader);
                    }
                case WkbType.Point:
                    {
                        return PointFrom(reader);
                    }
                case WkbType.LineString:
                    {
                        return LineStringFrom(reader);
                    }
                case WkbType.Polygon:
                    {
                        return PolygonFrom(reader);
                    }
                case WkbType.MultiPoint:
                    {
                        return MultiPointFrom(reader);
                    }
                case WkbType.MultiLineString:
                    {
                        return MultiLineStringFrom(reader);
                    }
                case WkbType.MultiPolygon:
                    {
                        return MultiPolygonFrom(reader);
                    }
                case WkbType.GeometryCollection:
                    {
                        return GeometryCollectionFrom(reader);
                    }
                default:
                    {
                        throw new NotSupportedException($"Unsupported WKB type {type}.");
                    }
            }
        }

        private static Point PointFrom(EndianAwareBinaryReader reader)
        {
            return new Point(new Coordinate(reader.ReadDouble(), reader.ReadDouble()));
        }

        private static LineString LineStringFrom(EndianAwareBinaryReader reader)
        {
            UInt32 amount = reader.ReadUInt32();
            List<Point> coordinates = new List<Point>();

            for (int i = 0; i < amount; i++)
            {
                var pos = new Coordinate(reader.ReadDouble(),
                                             reader.ReadDouble());

                coordinates.Add(new Point(pos));
            }

            return new LineString(coordinates);
        }

        private static Polygon PolygonFrom(EndianAwareBinaryReader reader)
        {
            int ringQuantity = reader.ReadInt32();
            List<Coordinate> positions = new List<Coordinate>(ringQuantity);
            List<LinearRing> rings = new List<LinearRing>(ringQuantity);
            for (int i = 0; i < ringQuantity; i++)
            {
                int numberOfPositions = reader.ReadInt32();
                List<Coordinate> coordinates = new List<Coordinate>(numberOfPositions);
                for (int j = 0; j < numberOfPositions; j++)
                {
                    coordinates.Add(new Coordinate(reader.ReadDouble(), reader.ReadDouble()));
                }

                var ring = new LinearRing(coordinates);

                rings.Add(ring);
            }

            return new Polygon(rings);
        }

        private static MultiPoint MultiPointFrom(EndianAwareBinaryReader reader)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            int numberOfGroups = reader.ReadInt32();

            for (int i = 0; i < numberOfGroups; i++)
            {
                Endianness endianness = (Endianness)reader.ReadByte();

                UInt32 numberOfPositions = reader.ReadUInt32((Endianness)endianness);

                for (int j = 0; j < numberOfPositions; j++)
                {
                    coordinates.Add(new Coordinate(reader.ReadDouble(), reader.ReadDouble()));
                }
            }

            return new MultiPoint(coordinates);
        }

        private static MultiLineString MultiLineStringFrom(EndianAwareBinaryReader reader)
        {
            List<LineString> lineStrings = new List<LineString>();
            int quantityOfLineStrings = reader.ReadInt32();

            for (int i = 0; i < quantityOfLineStrings; i++)
            {
                /*
                Endianness endianness = (Endianness)reader.ReadByte();
                WkbType type = (WkbType)reader.ReadUInt32(endianness);
                lineStrings.Add(LineStringFrom(reader));
                */

                lineStrings.Add((LineString)FromBinary(reader));
            }

            return new MultiLineString(lineStrings);
        }

        private static MultiPolygon MultiPolygonFrom(EndianAwareBinaryReader reader)
        {
            List<Polygon> polygons = new List<Polygon>();
            int quantityOfPolygons = reader.ReadInt32();

            for (int i = 0; i < quantityOfPolygons; i++)
            {
                /*
                Endianness endianness = (Endianness)reader.ReadByte();
                WkbType type = (WkbType)reader.ReadUInt32(endianness);
                polygons.Add(PolygonFrom(reader));
                */

                polygons.Add((Polygon)FromBinary(reader));
            }

            return new MultiPolygon(polygons);
        }

        private static GeometryCollection GeometryCollectionFrom(EndianAwareBinaryReader reader)
        {
            List<Geometry> geometries = new List<Geometry>();
            int quantity = reader.ReadInt32();

            for (int i = 0; i < quantity; i++)
            {
                geometries.Add(FromBinary(reader));
            }

            return new GeometryCollection(geometries);
        }

        #endregion

        #region To Binary Methods

        private static void ToBinary(EndianAwareBinaryWriter writer, Point point)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.Point);
            WritePosition(writer, point.Coordinates);
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, LineString lineString)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.LineString);
            var points = lineString.Points;
            writer.Write(points.Count());

            foreach (var point in points)
            {
                WritePosition(writer, point.Coordinates);
            }
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, Polygon polygon)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.Polygon);
            
            writer.Write((UInt32) polygon.LinearRings.Count());

            foreach (LinearRing linearRing in polygon.LinearRings)
            {
                writer.Write((UInt32) linearRing.Points.Count());

                foreach (Point pos in linearRing.Points)
                {
                    WritePosition(writer, pos.Coordinates);
                }
            }
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, MultiPoint multiPoint)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.MultiPoint);
            writer.Write((UInt32)multiPoint.Points.Count());

            foreach (Point point in multiPoint.Points)
            {
                ToBinary(writer, point);
            }
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, MultiLineString multiLineString)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.MultiLineString);
            writer.Write((UInt32)multiLineString.LineStrings.Count());

            foreach (LineString lineString in multiLineString.LineStrings)
            {
                ToBinary(writer, lineString);
            }
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, MultiPolygon multiPolygon)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.MultiPolygon);
            writer.Write((UInt32)multiPolygon.Polygons.Count());

            foreach (Polygon polygon in multiPolygon.Polygons)
            {
                ToBinary(writer, polygon);
            }
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, GeometryCollection geometryCollection)
        {
            writer.WriteEndianness();
            writer.Write((UInt32)WkbType.GeometryCollection);
            writer.Write((UInt32)geometryCollection.Geometries.Count());

            foreach (Geometry geometry in geometryCollection.Geometries)
            {
                ToBinary(writer, geometry);
            }
        }

        private static void ToBinary(EndianAwareBinaryWriter writer, Geometry geometry)
        {
            switch (geometry.Type)
            {
                case GeoJsonType.Point:
                    {
                        ToBinary(writer, (Point)geometry);
                        break;
                    }
                case GeoJsonType.LineString:
                    {
                        ToBinary(writer, (LineString)geometry);
                        break;
                    }
                case GeoJsonType.Polygon:
                    {
                        ToBinary(writer, (Polygon)geometry);
                        break;
                    }
                case GeoJsonType.MultiPoint:
                    {
                        ToBinary(writer, (MultiPoint)geometry);
                        break;
                    }
                case GeoJsonType.MultiLineString:
                    {
                        ToBinary(writer, (MultiLineString)geometry);
                        break;
                    }
                case GeoJsonType.MultiPolygon:
                    {
                        ToBinary(writer, (MultiPolygon)geometry);
                        break;
                    }
                case GeoJsonType.GeometryCollection:
                    {
                        ToBinary(writer, (GeometryCollection)geometry);
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException($"The GeoJson type {geometry.Type} is not supported for conversion to WKB.");
                    }
            }
        }

        private static EndianAwareBinaryWriter WritePosition(EndianAwareBinaryWriter writer, Coordinate position)
        {
            writer.Write(position.Longitude);
            writer.Write(position.Latitude);

            return writer;
        }

        #endregion

        #endregion
    }
}
