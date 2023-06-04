using Mapster.Common.MemoryMappedTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using Mapster.Common;

namespace Mapster.Rendering;

public static class TileRenderer
{
    public static BaseShape Tessellate(this MapFeatureData feature, ref BoundingBox boundingBox, ref PriorityQueue<BaseShape, int> shapes)
    {
        BaseShape? baseShape = null;

        var featureType = feature.Type;
        if (feature.Properties.Any(p => (p.Key == EnumMapper.Keys.Highway) && 
                                    MapFeature.HighwayTypes.Any(v => p.Value == v)))
        {
            var coordinates = feature.Coordinates;
            var road = new Road(coordinates);
            baseShape = road;
            shapes.Enqueue(road, road.ZIndex);
        }
        else if (feature.Properties.Any(p => (p.Key == EnumMapper.Keys.Water || p.Key == EnumMapper.Keys.WaterWay || p.Key == EnumMapper.Keys.Water_Point)) 
                                        && feature.Type != GeometryType.Point)
        {
            var coordinates = feature.Coordinates;

            var waterway = new Waterway(coordinates, feature.Type == GeometryType.Polygon);
            baseShape = waterway;
            shapes.Enqueue(waterway, waterway.ZIndex);
        }
        else if (Border.ShouldBeBorder(feature))
        {
            var coordinates = feature.Coordinates;
            var border = new Border(coordinates);
            baseShape = border;
            shapes.Enqueue(border, border.ZIndex);
        }
        else if (PopulatedPlace.ShouldBePopulatedPlace(feature))
        {
            var coordinates = feature.Coordinates;
            var popPlace = new PopulatedPlace(coordinates, feature);
            baseShape = popPlace;
            shapes.Enqueue(popPlace, popPlace.ZIndex);
        }
        else if (feature.Properties.Any(p => p.Key == EnumMapper.Keys.Railway))
        {
            var coordinates = feature.Coordinates;
            var railway = new Railway(coordinates);
            baseShape = railway;
            shapes.Enqueue(railway, railway.ZIndex);
        }
        else if (feature.Properties.Any(p => p.Key == EnumMapper.Keys.Natural && featureType == GeometryType.Polygon))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, feature);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Properties.Any(p => p.Key == EnumMapper.Keys.Boundary && p.Value == EnumMapper.Values.Boundary_Forest))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Forest);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Properties.Any(p => p.Key == EnumMapper.Keys.Landuse && (p.Value == EnumMapper.Values.Landuse_Forest || p.Value == EnumMapper.Values.Landuse_Orchard)))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Forest);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p
                     => p.Key == EnumMapper.Keys.Landuse && (p.Value == EnumMapper.Values.Landuse_Residential || p.Value == EnumMapper.Values.Landuse_Cemetery || p.Value == EnumMapper.Values.Landuse_Industrial || p.Value == EnumMapper.Values.Landuse_Commercial ||
                                                        p.Value == EnumMapper.Values.Landuse_Square || p.Value == EnumMapper.Values.Landuse_Construction || p.Value == EnumMapper.Values.Landuse_Military || p.Value == EnumMapper.Values.Landuse_Quarry ||
                                                        p.Value == EnumMapper.Values.Landuse_Brownfield)))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p
                     => p.Key == EnumMapper.Keys.Landuse && (p.Value == EnumMapper.Values.Landuse_Farm || p.Value == EnumMapper.Values.Landuse_Meadow || p.Value == EnumMapper.Values.Landuse_Grass || p.Value == EnumMapper.Values.Landuse_Greenfield ||
                                                        p.Value == EnumMapper.Values.Landuse_Recreation_Ground || p.Value == EnumMapper.Values.Landuse_Winter_Sports || p.Value == EnumMapper.Values.Landuse_Allotments)))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Plain);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Type == GeometryType.Polygon &&
                 feature.Properties.Any(p => p.Key == EnumMapper.Keys.Landuse && (p.Value == EnumMapper.Values.Landuse_Reservoir || p.Value == EnumMapper.Values.Landuse_Basin)))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Water);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => (p.Key == EnumMapper.Keys.Building ||
                                                                                        p.Key == EnumMapper.Keys.Building_2020 ||
                                                                                        p.Key == EnumMapper.Keys.Building_Architecture ||
                                                                                        p.Key == EnumMapper.Keys.Building_Levels)))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p.Key == EnumMapper.Keys.Leisure))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }
        else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => (p.Key == EnumMapper.Keys.Amenity ||
                                                                                        p.Key == EnumMapper.Keys.Amenity_2020)))
        {
            var coordinates = feature.Coordinates;
            var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
            baseShape = geoFeature;
            shapes.Enqueue(geoFeature, geoFeature.ZIndex);
        }

        if (baseShape != null)
        {
            for (var j = 0; j < baseShape.ScreenCoordinates.Length; ++j)
            {
                boundingBox.MinX = Math.Min(boundingBox.MinX, baseShape.ScreenCoordinates[j].X);
                boundingBox.MaxX = Math.Max(boundingBox.MaxX, baseShape.ScreenCoordinates[j].X);
                boundingBox.MinY = Math.Min(boundingBox.MinY, baseShape.ScreenCoordinates[j].Y);
                boundingBox.MaxY = Math.Max(boundingBox.MaxY, baseShape.ScreenCoordinates[j].Y);
            }
        }

        return baseShape;
    }

    public static Image<Rgba32> Render(this PriorityQueue<BaseShape, int> shapes, BoundingBox boundingBox, int width, int height)
    {
        var canvas = new Image<Rgba32>(width, height);

        // Calculate the scale for each pixel, essentially applying a normalization
        var scaleX = canvas.Width / (boundingBox.MaxX - boundingBox.MinX);
        var scaleY = canvas.Height / (boundingBox.MaxY - boundingBox.MinY);
        var scale = Math.Min(scaleX, scaleY);

        // Background Fill
        canvas.Mutate(x => x.Fill(Color.White));
        while (shapes.Count > 0)
        {
            var entry = shapes.Dequeue();
            // FIXME: Hack
            if (entry.ScreenCoordinates.Length < 2)
            {
                continue;
            }
            entry.TranslateAndScale(boundingBox.MinX, boundingBox.MinY, scale, canvas.Height);
            canvas.Mutate(x => entry.Render(x));
        }

        return canvas;
    }

    public struct BoundingBox
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
    }

    
}
