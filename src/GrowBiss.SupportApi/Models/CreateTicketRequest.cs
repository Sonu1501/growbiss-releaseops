using System.ComponentModel.DataAnnotations;

namespace GrowBiss.SupportApi.Models;

public sealed class CreateTicketRequest
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string CustomerEmail { get; init; } = string.Empty;

    public TicketPriority Priority { get; init; } = TicketPriority.Normal;
}