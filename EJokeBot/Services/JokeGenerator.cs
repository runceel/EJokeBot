using Microsoft.Bot.Builder;
using Microsoft.SemanticKernel;
using System.Threading;
using System.Threading.Tasks;

namespace EJokeBot.Services;

public class JokeGenerator : IJokeGenerator
{
    private const string _prompt = """
            貴方は漫才師です。以下の文章・単語を使ってエンジニアが楽しめるジョークを１００文字位で作ってください。

            {{$input}}
            """;

    private readonly IKernel _kernel;

    public JokeGenerator(IKernel kernel)
    {
        _kernel = kernel;
    }

    public async ValueTask<string> GenerateJokeAsync(string topic, CancellationToken cancellationToken)
    {
        var joke = _kernel.CreateSemanticFunction(_prompt);
        var reply = await joke.InvokeAsync(topic, _kernel, cancellationToken: cancellationToken);
        return reply.GetValue<string>();
    }
}
