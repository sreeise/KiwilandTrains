using Newtonsoft.Json;

namespace Kiwiland.RouteComputation.Core;

public class TrainRoute
{
    public string Directions { get; set; }

    public int Distance { get; set; }

    public int Stops => Directions.Count(a => a == '=');

    public TrainRoute()
    {
        Directions = string.Empty;
        Distance = 0;
    }

    public TrainRoute NextStop(string nextStop, int distance)
    {
        return new TrainRoute()
        {
            Directions = Directions + nextStop,
            Distance = Distance + distance
        };
    }

    public RouteInformation BuildRouteInformation(string nextStop, int distance)
    {
        var directions = Directions + nextStop;
        var directionList = directions.Split("=>").Select(s => s.Trim()).ToList();
        return new RouteInformation(directionList, Distance + distance, directionList.Count - 1);
    }

    public static TrainRoute None => new();

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}