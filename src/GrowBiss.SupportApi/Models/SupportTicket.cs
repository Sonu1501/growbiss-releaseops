namespace GrowBiss.SupportApi.Models;

public sealed record SupportTicket
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string CustomerEmail { get; init; } = string.Empty;

    public TicketPriority Priority { get; init; }

    public TicketStatus Status { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public DateTimeOffset UpdatedAtUtc { get; init; }
}