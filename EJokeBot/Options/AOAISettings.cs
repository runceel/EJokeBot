using System.ComponentModel.DataAnnotations;

namespace EJokeBot.Options;

public class AOAISettings
{
    [Required]
    public string DeploymentName { get; set; } = "";

    [Required]
    public string Endpoint { get; set; } = "";

    [Required]
    public string ApiKey { get; set; } = "";
}
