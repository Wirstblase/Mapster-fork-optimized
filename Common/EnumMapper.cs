namespace Mapster.Common;

public class EnumMapper
{
    public static int test()
    {
        return 1;
    }
    
    public enum Keys {
        Place,
        Water,
        Building,
        Highway_2020,
        Admin_Level,
        Leisure,
        Building_2020,
        Boundary,
        Building_Architecture,
        WaterWay,
        Highway_Lanes_Forward,
        Placement_Forward,
        Natural,
        Building_Levels,
        Highway,
        Railway,
        Landuse,
        Name,
        Highway_Lanes_Backward,
        Amenity,
        Amenity_2020,
        Water_Point,
        Placement,
        NULL
    }

    public enum Values {
        
        Highway_Unclassified,
        Landuse_Basin,
        Natural_Beach,
        Natural_Heath,
        Landuse_Orchard,
        Highway_Motorway,
        Landuse_Allotments,
        Highway_Secondary,
        Landuse_Meadow,
        Highway_Tertiary,
        Landuse_Grass,
        Landuse_Square,
        Landuse_Quarry,
        Landuse_Winter_Sports,
        Natural_Sand,
        Natural_Moor,
        Boundary_Administrative,
        Natural_Rock,
        Place_Locality,
        Natural_Fell,
        Landuse_Construction,
        Place_Hamlet,
        Natural_Tree_Row,
        Boundary_Forest,
        Natural_Scree,
        Landuse_Commercial,
        Landuse_Greenfield,
        Highway_Residential,
        Landuse_Farm,
        Highway_Primary,
        Natural_Wetland,
        Natural_Bare_Rock,
        Natural_Wood,
        Landuse_Residential,
        Landuse_Recreation_Ground,
        Place_Town,
        Landuse_Cemetery,
        Highway_Trunk,
        Landuse_Brownfield,
        Highway_Road,
        Landuse_Military,
        Place_City,
        Landuse_Industrial,
        Landuse_Reservoir,
        Landuse_Forest,
        Name_ID,
        Natural_Water,
        Natural_Grassland,
        Admin_Level_Two,
        Natural_Scrub,
        NULL
    }
    
    public static Keys ConvertStringToUniqueKey(string str) {
        var map = new Dictionary<string, Keys> {
            {"landuse", Keys.Landuse},
            {"water", Keys.Water},
            {"highway", Keys.Highway},
            {"amenity:2020", Keys.Amenity_2020},
            {"admin_level", Keys.Admin_Level},
            {"name", Keys.Name},
            {"building:levels", Keys.Building_Levels},
            {"highway:lanes:forward", Keys.Highway_Lanes_Forward},
            {"highway:2020", Keys.Highway_2020},
            {"building:architecture", Keys.Building_Architecture},
            {"boundary", Keys.Boundary},
            {"placement:forward", Keys.Placement_Forward},
            {"building:2020", Keys.Building_2020},
            {"building", Keys.Building},
            {"waterway", Keys.WaterWay},
            {"amenity", Keys.Amenity},
            {"water_point", Keys.Water_Point},
            {"highway:lanes:backward", Keys.Highway_Lanes_Backward},
            {"railway", Keys.Railway},
            {"natural", Keys.Natural},
            {"placement", Keys.Placement},
            {"leisure", Keys.Leisure},
            {"place", Keys.Place}
        };

        if (map.TryGetValue(str, out Keys value)) {
            return value;
        }

        //Console.WriteLine(str);

        return Keys.NULL;
    }

    public static Values ConvertStringToUniqueValue(string key, string str, Keys keyEnum) {
    var map = new Dictionary<string, Values> {
        {"motorway", Values.Highway_Motorway},
        {"trunk", Values.Highway_Trunk},
        {"primary", Values.Highway_Primary},
        {"secondary", Values.Highway_Secondary},
        {"tertiary", Values.Highway_Tertiary},
        {"unclassified", Values.Highway_Unclassified},
        {"road", Values.Highway_Road},
        {"administrative", Values.Boundary_Administrative},
        {"2", Values.Admin_Level_Two},
        {"city", Values.Place_City},
        {"town", Values.Place_Town},
        {"locality", Values.Place_Locality},
        {"hamlet", Values.Place_Hamlet},
        {"fell", Values.Natural_Fell},
        {"grassland", Values.Natural_Grassland},
        {"heath", Values.Natural_Heath},
        {"moor", Values.Natural_Moor},
        {"scrub", Values.Natural_Scrub},
        {"wetland", Values.Natural_Wetland},
        {"wood", Values.Natural_Wood},
        {"tree_row", Values.Natural_Tree_Row},
        {"bare_rock", Values.Natural_Bare_Rock},
        {"rock", Values.Natural_Rock},
        {"scree", Values.Natural_Scree},
        {"beach", Values.Natural_Beach},
        {"sand", Values.Natural_Sand},
        {"water", Values.Natural_Water},
        {"orchard", Values.Landuse_Orchard},
        {"cemetery", Values.Landuse_Cemetery},
        {"industrial", Values.Landuse_Industrial},
        {"commercial", Values.Landuse_Commercial},
        {"square", Values.Landuse_Square},
        {"construction", Values.Landuse_Construction},
        {"military", Values.Landuse_Military},
        {"quarry", Values.Landuse_Quarry},
        {"brownfield", Values.Landuse_Brownfield},
        {"farm", Values.Landuse_Farm},
        {"meadow", Values.Landuse_Meadow},
        {"grass", Values.Landuse_Grass},
        {"greenfield", Values.Landuse_Greenfield},
        {"recreation_ground", Values.Landuse_Recreation_Ground},
        {"winter_sports", Values.Landuse_Winter_Sports},
        {"allotments", Values.Landuse_Allotments},
        {"reservoir", Values.Landuse_Reservoir},
        {"basin", Values.Landuse_Basin},
    };

    var specialMap = new List<Tuple<string, string, Values>> {
        new Tuple<string, string, Values>("residential", "highway", Values.Highway_Residential),
        new Tuple<string, string, Values>("forest", "boundary", Values.Boundary_Forest),
        new Tuple<string, string, Values>("forest", "landuse", Values.Landuse_Forest),
        new Tuple<string, string, Values>("residential", "landuse", Values.Landuse_Residential),
    };

    if (key == "name") {
        return Values.Name_ID;
    }

    foreach (var special in specialMap) {
        if (str.StartsWith(special.Item1) && key.StartsWith(special.Item2)) {
            return special.Item3;
        }
    }

    foreach (var kvp in map) {
        if (str.StartsWith(kvp.Key)) {
            return kvp.Value;
        }
    }

    return Values.NULL;
}
}