using System.ComponentModel.DataAnnotations;

namespace SupportDesk.Configuration
{
    public sealed class FoundryOptions
    {
        public const string SectionName = "Foundry";

        [Required, Url]
        public string ProjectEndpoint { get; init; } = string.Empty;

        [Required]
        public string ModelDeployment { get; init; } = string.Empty;
    }
}
