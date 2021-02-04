# BAMCIS GeoJSON
An implementation of GeoJSON written in .NET Core 2.0. The library complies with the requirements of RFC 7946 (https://tools.ietf.org/html/rfc7946). Can be found on nuget https://www.nuget.org/packages/geojson.

## Table of Contents
- [Limitations](#limitations)
- [Usage](#usage)
  * [Example 1](#example-1)
  * [Example 2](#example-2)
  * [Example 3](#example-3)
  * [Example 4](#example-4)
  * [Usage Notes](#usage-notes)
  * [Global Configuration](#global-configuration)
- [Revision History](#revision-history)


## Limitations

The library does not support section 6 of RFC 7946, extending GeoJSON with Foreign Members. You would need to extend the existing Geometries, Feature, and FeatureCollection classes with the additional properties included in the serialized JSON.

## Usage

### Example 1

Given some GeoJSON data, such as:

```json
{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [ 102.0, 0.5 ]
  },
  "properties": {
    "prop0": "value0"
  }
}
```

You can receive the text and deserialize it to a GeoJSON object

```csharp
GeoJson data = JsonConvert.DeserializeObject<GeoJson>(jsonString);
```

Once the data is deserialized, you can cast it to its actual type

```csharp
switch (data.Type)
{
    case GeoJsonType.Feature:
    {
        Feature feature = (Feature)data;
        break;
    }
    ...
}
```

### Example 2
If you know what kind of GeoJSON you're receiving, you can deserialize to it directly:

```csharp
FeatureCollection col = FeatureCollection.FromJson(data);
```

And also go back to JSON data:

```csharp
string json = col.ToJson(Formatting.Idented);
```

### Example 3

Or you can create a GeoJSON object and serialize it:
```csharp
Position pos1 = new Position(100.0, 0.5);
Position pos2 = new Position(101.0, 1.0);
MultiPoint mp = new MultiPoint(new Position[] {pos1, pos2});
string json = JsonConvert.Serialize(mp);
```

### Example 4
The library also supports conversion of `Geometry` objects to and from Well-Known Binary (WKB). For example:

```json
{
  "type": "Point",
  "coordinates": [ 2.0, 4.0 ]
}
```

```csharp
Point point = new Point(102.0, 0.5);
byte[] wkb = point.ToWkb();
point = Geometry.FromWkb<Point>(wkb);
```

The binary produced is `0x000000000140000000000000004010000000000000`. You can also convert this way.

```csharp
Point point = new Point(102.0, 0.5);
byte[] wkb = point.ToWkb();
Geometry geo = Point.FromWkb(wkb)
point = (Point)geo;
```

You can also specify the endianness of the binary encoding (the default is LITTLE).

```csharp
Point point = new Point(102.0, 0.5);
byte[] wkb = point.ToWkb(Endianness.BIG);
Geometry geo = Point.FromWkb(wkb)
point = (Point)geo;
```

Finally, you can use the `WkbConverter` class directly.

```csharp
Point point = new Point(new Position(2.0, 4.0));
byte[] bytes = WkbConverter.ToBinary(point, Endianness.BIG);
```

```csharp
byte[] bytes = HexStringToByteArray("000000000140000000000000004010000000000000");
Point point = WkbConverter.FromBinary<Point>(bytes);
```

### Usage Notes

Each of the 9 GeoJSON types: **Feature**, **FeatureCollection**, **GeometryCollection**, **LineString**, **MultiLineString**, **MultiPoint**, **MultiPolygon**, **Point**, and **Polygon** all have convenience methods ToJson() and FromJson() to make serialization and deserialization easy.

There are two additional types that can be used. A **LinearRing** is a LineString that is connected as the start and end and forms the basis of a polygon. You can also use the abstract **Geometry** class that encompasses LineString, MultiLineString, MultiPoint, MultiPolygon, Point, and Polygon.

The Feature **'Properties'** property implements an `IDictionary<string, dynamic>` in order to accomodate any type of property structure that may be sent.

### Global Configuration

This library provides a global configuration class, `GeoJsonConfig`. Currently, the config offers a way to ignore the validation of latitude and longitude coordinates. For example, given this input:

```json
{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [ 200.0, 65000.5 ]
  },
  "properties": {
    "prop0": "value0"
  }
}
```

We would expect this operation to throw an `ArgumentOutOfRangeException` due to the coordinate values.

```csharp
Feature geo = JsonConvert.DeserializeObject<Feature>(content);
```

To ignore the validation, do this:

```csharp
GeoJsonConfig.IgnorePositionValidation();
Feature geo = JsonConvert.DeserializeObject<Feature>(content);
```

## Revision History

### 2.3.0
Added Well-Known Binary serialization and deserialization support for `Geometry` objects.

### 2.2.0
Added an Id property to in `Feature`. Also added a global config object that can be used to ignore validation of coordinate values.

### 2.1.1
Added validation for latitude and longitude values in `Position`.

### 2.1.0
Added a method to remove interior linear rings from a `Polygon` object.

### 2.0.2
Fixed bug for `MultiLineString` with less than 2 coordinates.

### 2.0.1
Bug fix for NULL geometry JSON token in a Feature, which is allowed by the RFC.

### 2.0.0
Changed JSON serialized property names to proper camel case by default to be RFC compliant. Added strong-named assembly signing.

### 1.2.1
Fixed sequence comparison null value handling.

### 1.2.0
Enabled the use of the bounding box property on all GeoJSON types.

### 1.1.2
Actually fixed targeting `netstandard1.6` instead of v1.6.1.

### 1.1.1
Fixed targetting of netstandard1.6, dropped JSON.NET to 9.0.1. Added `netstandard2.0` and `net45` as target frameworks.

### 1.1.0
Retargetted library to netstandard1.6.

### 1.0.0
Initial release of the library.
