using System.Threading;
using System.Threading.Tasks;

namespace EJokeBot.Services;

public interface IJokeGenerator
{
    ValueTask<string> GenerateJokeAsync(string topic, CancellationToken cancellationToken);
}
