# BAMCIS GeoJSON

## Usage

### Basic Example 1

Given some GeoJSON data, such as:

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

You can receive the text and deserialize it to a GeoJSON object

    GeoJson data = JsonConvert.Deserialize<GeoJson>(jsonString);

Once the data is deserialized, you can cast it to its actual type

	switch (data.Type)
	{
		case GeoJsonType.Feature:
		{
            Feature feature = (Feature)data;
		    break;
		}
		...
	}

### Basic Example 2

Or you can create a GeoJSON object and serialize it:

    Position Pos1 = new Position(100.0, 0.5);
	Position Pos2 = new Position(101.0, 1.0);
	MultiPoint Mp = new MultiPoint(new Position[] {Pos1, Pos2});
	string Json = JsonConvert.Serialize(Mp);


## Revision History

### 1.0.0.0
Initial release of the library.
