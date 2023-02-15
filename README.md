## Kiwiland Trains 

### Build and Run

- Requires .net6.0

Run `dotnet build`

Run `dotnet run --project src\Kiwiland.Cli --routes AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7`

There are two projects:

- Kiwiland.RouteComputation: Library for computing route information such as shortest distance.
- Kiwiland.Cli: A CLI that will run route computation on the routes given.