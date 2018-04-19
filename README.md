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


## Revision History

### 1.0.0.0
Initial release of the library.
