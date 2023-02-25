using Kiwiland.RouteComputation.Core;

namespace Kiwiland.Cli.Builder;

public abstract class Helper
{
    public static TerminalGateway TerminalGateway(string input)
    {
        var list = input.Split(new[] { ' ', ',', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.ToCharArray())
            .ToList();

        var gateway = new TerminalGateway();
        foreach (var charArray in list)
        {
            var source = char.ToString(charArray[0]);
            var destination = char.ToString(charArray[1]);
            var distance = int.Parse(char.ToString(charArray[2]));
            gateway.AddRoute(source, destination, distance);
        }

        return gateway;
    }
}