using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMCIS.GeoJSON.Externals
{
    public static class WkbConverter
    {
        public enum WkbType : UInt32
        {
            // 2D
            Geometry = 0000,
            Point = 0001,
            LineString = 0002,
            Polygon = 0003,
            MultiPoint = 0004,
            MultiLineString = 0005,
            MultiPolygon = 0006,
            GeometryCollection = 0007,
            CircularString = 0008,
            CompoundCurve = 0009,
            CurvePolygon = 0010,
            MultiCurve = 0011,
            MultiSurface = 0012,
            Curve = 0013,
            Surface = 0014,
            PolyhedralSurface = 0015,
            TIN = 0016,
            Triangle = 0017,
            Circle = 0018,
            GeodesicString = 0019,
            EllipticalCurve = 0020,
            NurbsCurve = 0021,
            Clothoid = 0022,
            SpiralCurve = 0023,
            CompoundSurface = 0024,
            BrepSolid = 0025,
            AffinePlacement = 0102,

            // Z
            Z_Geometry = 1000,
            Z_Point = 1001,
            Z_LineString = 1002,
            Z_Polygon = 1003,
            Z_MultiPoint = 1004,
            Z_MultiLineString = 1005,
            Z_MultiPolygon = 1006,
            Z_GeometryCollection = 1007,
            Z_CircularString = 1008,
            Z_CompoundCurve = 1009,
            Z_CurvePolygon = 1010,
            Z_MultiCurve = 1011,
            Z_MultiSurface = 1012,
            Z_Curve = 1013,
            Z_Surface = 1014,
            Z_PolyhedralSurface = 1015,
            Z_TIN = 1016,
            Z_Triangle = 1017,
            Z_Circle = 1018,
            Z_GeodesicString = 1019,
            Z_EllipticalCurve = 1020,
            Z_NurbsCurve = 1021,
            Z_Clothoid = 1022,
            Z_SpiralCurve = 1023,
            Z_CompoundSurface = 1024,
            Z_BrepSolid = 1025,
            Z_AffinePlacement = 1102,


            // M
            M_Geometry = 2000,
            M_Point = 2001,
            M_LineString = 2002,
            M_Polygon = 2003,
            M_MultiPoint = 2004,
            M_MultiLineString = 2005,
            M_MultiPolygon = 2006,
            M_GeometryCollection = 2007,
            M_CircularString = 2008,
            M_CompoundCurve = 2009,
            M_CurvePolygon = 2010,
            M_MultiCurve = 2011,
            M_MultiSurface = 2012,
            M_Curve = 2013,
            M_Surface = 2014,
            M_PolyhedralSurface = 2015,
            M_TIN = 2016,
            M_Triangle = 2017,
            M_Circle = 2018,
            M_GeodesicString = 2019,
            M_EllipticalCurve = 2020,
            M_NurbsCurve = 2021,
            M_Clothoid = 2022,
            M_SpiralCurve = 2023,
            M_CompoundSurface = 2024,
            M_BrepSolid = 2025,
            M_AffinePlacement = 2102,


            // ZM
            ZM_Geometry = 3000,
            ZM_Point = 3001,
            ZM_LineString = 3002,
            ZM_Polygon = 3003,
            ZM_MultiPoint = 3004,
            ZM_MultiLineString = 3005,
            ZM_MultiPolygon = 3006,
            ZM_GeometryCollection = 3007,
            ZM_CircularString = 3008,
            ZM_CompoundCurve = 3009,
            ZM_CurvePolygon = 3010,
            ZM_MultiCurve = 3011,
            ZM_MultiSurface = 3012,
            ZM_Curve = 3013,
            ZM_Surface = 3014,
            ZM_PolyhedralSurface = 3015,
            ZM_TIN = 3016,
            ZM_Triangle = 3017,
            ZM_Circle = 3018,
            ZM_GeodesicString = 3019,
            ZM_EllipticalCurve = 3020,
            ZM_NurbsCurve = 3021,
            ZM_Clothoid = 3022,
            ZM_SpiralCurve = 3023,
            ZM_CompoundSurface = 3024,
            ZM_BrepSolid = 3025,
            ZM_AffinePlacement = 3102
        }

               
        #region BinaryEdian Reader & Writer
        class BinaryEndianReader
        {
            private bool doReverse;

            private byte[] bytes;

            private int index;

            public BinaryEndianReader(byte[] input, int indexStart = 0)
            {

                bytes = new byte[input.Length];
                input.CopyTo(bytes, 0);

                index = indexStart;
            }

            public BinaryEndianReader ReadEndianness()
            {
                doReverse = (BitConverter.IsLittleEndian != (ReadByte() == 1));

                return this;
            }


            public byte ReadByte()
            {
                return bytes[index++];
            }


            public int ReadInt32()
            {
                if (doReverse)
                {
                    Array.Reverse(bytes, index, 4);
                }

                int result = BitConverter.ToInt32(bytes, index);

                index += 4;

                return result;
            }

            public Int16 ReadInt16()
            {
                if (doReverse)
                {
                    Array.Reverse(bytes, index, 2);
                }

                Int16 result = BitConverter.ToInt16(bytes, index);

                index += 2;

                return result;
            }

            public Int64 ReadInt64()
            {
                if (doReverse)
                {
                    Array.Reverse(bytes, index, 8);
                }

                Int64 result = BitConverter.ToInt64(bytes, index);

                index += 8;

                return result;
            }

            public UInt32 ReadUInt32()
            {
                if (doReverse)
                {
                    Array.Reverse(bytes, index, 4);
                }

                UInt32 result = BitConverter.ToUInt32(bytes, index);

                index += 4;

                return result;
            }


            public double ReadDouble()
            {
                if (doReverse)
                {
                    Array.Reverse(bytes, index, 8);
                }

                double result = BitConverter.ToDouble(bytes, index);

                index += 8;

                return result;
            }

        }

        class BinaryEndianWriter
        {
            private readonly bool requestedLittleEndian;
            private bool doReverse;

            private readonly List<byte> bytes;

            private int index;

            public BinaryEndianWriter(bool requestedLittleEndian = true)
            {
                this.requestedLittleEndian = requestedLittleEndian;

                bytes = new List<byte>();

                index = 0;
            }

            public BinaryEndianWriter WriteEndianness()
            {
                doReverse = (BitConverter.IsLittleEndian != requestedLittleEndian);

                Write(requestedLittleEndian ? (byte)1 : (byte)0);

                return this;
            }


            public BinaryEndianWriter Write(byte output)
            {
                bytes.Add(output);

                index++;

                return this;
            }


            public BinaryEndianWriter Write(Int16 output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 2);
                }

                index += 2;

                return this;
            }

            public BinaryEndianWriter Write(Int32 output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 4);
                }

                index += 4;

                return this;
            }



            public BinaryEndianWriter Write(Int64 output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 8);
                }

                index += 8;

                return this;
            }



            public BinaryEndianWriter Write(UInt16 output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 2);
                }

                index += 2;

                return this;
            }

            public BinaryEndianWriter Write(UInt32 output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 4);
                }

                index += 4;

                return this;
            }



            public BinaryEndianWriter Write(UInt64 output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 8);
                }

                index += 8;

                return this;
            }




            public BinaryEndianWriter Write(double output)
            {
                bytes.AddRange(BitConverter.GetBytes(output));
                if (doReverse)
                {
                    bytes.Reverse(index, 8);
                }

                index += 8;

                return this;
            }


            public byte[] ToByteArray()
            {
                return bytes.ToArray();
            }
        }

        #endregion BinaryEdian Reader & Writer


        #region From WKB to GeoJson Part

        public static Geometry From(byte[] bytes)
        {
            BinaryEndianReader bytesReader = new BinaryEndianReader(bytes);

            return From(bytesReader);
        }

        public static MT From<MT>(byte[] bytes)
            where MT : Geometry
        {
            BinaryEndianReader bytesReader = new BinaryEndianReader(bytes);

            return (MT)From(bytesReader);
        }


        private static Geometry From(BinaryEndianReader bytesReader)
        {
            Geometry geom;

            WkbType type = (WkbType)bytesReader.ReadEndianness().ReadUInt32();


            switch (type)
            {
                case WkbType.Geometry:
                    {
                        geom = From(bytesReader);
                        break;
                    }
                case WkbType.Point:
                    {
                        geom = PointFrom(bytesReader);
                        break;
                    }
                case WkbType.LineString:
                    {
                        geom = LineStringFrom(bytesReader);
                        break;
                    }
                case WkbType.Polygon:
                    {
                        geom = PolygonFrom(bytesReader);
                        break;
                    }
                case WkbType.MultiPoint:
                    {
                        geom = MultiPointFrom(bytesReader);
                        break;
                    }
                case WkbType.MultiLineString:
                    {
                        geom = MultiLineStringFrom(bytesReader);
                        break;
                    }
                case WkbType.MultiPolygon:
                    {
                        geom = MultiPolygonFrom(bytesReader);
                        break;
                    }
                case WkbType.GeometryCollection:
                    {
                        geom = GeometryCollectionFrom(bytesReader);
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException("Byte array does not contain supported WKB format!");
                    }
            }

            return geom;
        }




        private static Point PointFrom(BinaryEndianReader bytesReader)
        {
            return new Point(coordinates: new Position(bytesReader.ReadDouble(), bytesReader.ReadDouble()));
        }

        private static LineString LineStringFrom(BinaryEndianReader bytesReader)
        {
            UInt32 amount = bytesReader.ReadUInt32();
            List<Position> coordinates = new List<Position>();

            for (int i = 0; i < amount; i++)
            {
                coordinates.Add(new Position(bytesReader.ReadDouble(), bytesReader.ReadDouble()));
            }

            return new LineString(coordinates: coordinates);
        }

        private static Polygon PolygonFrom(BinaryEndianReader bytesReader)
        {
            int amountRings = (int)bytesReader.ReadUInt32();
            List<LinearRing> rings = new List<LinearRing>(amountRings);

            for (int i = 0; i < amountRings; i++)
            {
                int amountPos = (int)bytesReader.ReadUInt32();
                List<Position> coordinates = new List<Position>(amountPos);
                for (int j = 0; j < amountPos; j++)
                {
                    coordinates.Add(new Position(bytesReader.ReadDouble(), bytesReader.ReadDouble()));
                }

                rings.Add(new LinearRing(coordinates));
            }

            return new Polygon(coordinates: rings);
        }


        private static MultiPoint MultiPointFrom(BinaryEndianReader bytesReader)
        {
            List<Position> coordinates = new List<Position>();
            int amountGroups = (int)bytesReader.ReadUInt32();

            for (int i = 0; i < amountGroups; i++)
            {
                bytesReader.ReadEndianness();

                UInt32 amountPos = bytesReader.ReadUInt32();

                for (int j = 0; j < amountPos; j++)
                {
                    coordinates.Add(new Position(bytesReader.ReadDouble(), bytesReader.ReadDouble()));
                }
            }

            return new MultiPoint(coordinates: coordinates);
        }

        private static MultiLineString MultiLineStringFrom(BinaryEndianReader bytesReader)
        {
            List<LineString> lineStrings = new List<LineString>();
            int amountLineStrings = (int)bytesReader.ReadUInt32();

            for (int i = 0; i < amountLineStrings; i++)
            {

                lineStrings.Add((LineString)From(bytesReader));
            }

            return new MultiLineString(coordinates: lineStrings);
        }


        private static MultiPolygon MultiPolygonFrom(BinaryEndianReader bytesReader)
        {
            List<Polygon> polygons = new List<Polygon>();
            int amount = (int)bytesReader.ReadUInt32();

            for (int i = 0; i < amount; i++)
            {
                polygons.Add((Polygon)From(bytesReader));
            }

            return new MultiPolygon(coordinates: polygons);
        }


        private static GeometryCollection GeometryCollectionFrom(BinaryEndianReader bytesReader)
        {
            List<Geometry> geometries = new List<Geometry>();
            int amount = (int)bytesReader.ReadUInt32();

            for (int i = 0; i < amount; i++)
            {
                geometries.Add(From(bytesReader));
            }

            return new GeometryCollection(geometries: geometries);
        }

        #endregion From WKB to GeoJson Part


        #region To WKB from GeoJson Part

        public static byte[] To(GeoJson geometry, bool useLittleEndian = false)
        {
            BinaryEndianWriter writer = new BinaryEndianWriter(useLittleEndian);

            if (geometry is Point point)
            {
                To(writer, point);
            }
            else if (geometry is LineString lineString)
            {
                To(writer, lineString);
            }
            else if (geometry is Polygon polygon)
            {
                To(writer, polygon);
            }
            else if (geometry is MultiPoint points)
            {
                To(writer, points);
            }
            else if (geometry is MultiLineString lines)
            {
                To(writer, lines);
            }
            else if (geometry is MultiPolygon polygons)
            {
                To(writer, polygons);
            }
            else if (geometry is GeometryCollection geometries)
            {
                To(writer, geometries);
            }

            return writer.ToByteArray();
        }

        private static void To(BinaryEndianWriter writer, Point point)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.Point)
                    .Write(point.Coordinates.Longitude)
                    .Write(point.Coordinates.Latitude);
        }

        private static void To(BinaryEndianWriter writer, LineString lineString)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.LineString)
                    .Write((UInt32)lineString.Coordinates.Count());

            foreach (Position pos in lineString.Coordinates)
            {
                writer.Write(pos.Longitude)
                    .Write(pos.Latitude);
            }
        }

        private static void To(BinaryEndianWriter writer, Polygon polygon)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.Polygon)
                    .Write((UInt32)polygon.Coordinates.Count());


            foreach (var linearRing in polygon.Coordinates)
            {
                writer.Write((UInt32)linearRing.Coordinates.Count());

                foreach (var pos in linearRing.Coordinates)
                {
                    writer.Write(pos.Longitude)
                        .Write(pos.Latitude);
                }
            }
        }

        private static void To(BinaryEndianWriter writer, MultiPoint points)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.MultiPoint)
                    .Write((UInt32)points.Coordinates.Count());


            foreach (var pos in points.Coordinates)
            {
                writer
                    .WriteEndianness()
                    .Write((UInt32)1)
                    .Write(pos.Longitude)
                    .Write(pos.Latitude);
            }
        }


        private static void To(BinaryEndianWriter writer, MultiLineString lineStrings)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.MultiLineString)
                    .Write((UInt32)lineStrings.Coordinates.Count());


            foreach (var lineString in lineStrings.Coordinates)
            {
                To(writer, lineString);
            }
        }


        private static void To(BinaryEndianWriter writer, MultiPolygon polygons)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.MultiPolygon)
                    .Write((UInt32)polygons.Coordinates.Count());


            foreach (var polygon in polygons.Coordinates)
            {
                To(writer, polygon);
            }
        }

        private static void To(BinaryEndianWriter writer, GeometryCollection geometries)
        {
            writer.WriteEndianness()
                    .Write((UInt32)WkbType.GeometryCollection)
                    .Write((UInt32)geometries.Geometries.Count());


            foreach (var geometry in geometries.Geometries)
            {
                To(writer, geometry);
            }
        }

        private static void To(BinaryEndianWriter writer, Geometry geometry)
        {
            if ( geometry.Type == GeoJsonType.Point )
            {
                To(writer, (Point)geometry);
            }
            else if (geometry.Type == GeoJsonType.LineString)
            {
                To(writer, (LineString)geometry);
            }
            else if (geometry.Type == GeoJsonType.Polygon)
            {
                To(writer, (Polygon)geometry);
            }
            else if (geometry.Type == GeoJsonType.MultiPoint)
            {
                To(writer, (MultiPoint)geometry);
            }
            else if (geometry.Type == GeoJsonType.MultiLineString)
            {
                To(writer, (MultiLineString)geometry);
            }
            else if (geometry.Type == GeoJsonType.MultiPolygon)
            {
                To(writer, (MultiPolygon)geometry);
            }
            else if (geometry.Type == GeoJsonType.GeometryCollection)
            {
                To(writer, (GeometryCollection)geometry);
            }
            else
            {
                throw new NotSupportedException("This GeoJson type is not supported for convertion to WKB!");
            }
        }


        #endregion To WKB from GeoJson Part

    }
}
