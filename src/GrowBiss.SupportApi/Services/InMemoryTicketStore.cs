using System.Collections.Concurrent;
using GrowBiss.SupportApi.Models;

namespace GrowBiss.SupportApi.Services;

public sealed class InMemoryTicketStore : ITicketStore
{
    private readonly ConcurrentDictionary<Guid, SupportTicket> _tickets = new();

    public InMemoryTicketStore()
    {
        var now = DateTimeOffset.UtcNow;

        var sampleTickets = new[]
        {
            new SupportTicket
            {
                Id = Guid.NewGuid(),
                Title = "Unable to access customer dashboard",
                Description = "Customer receives an error while accessing the dashboard.",
                CustomerEmail = "alex@example.com",
                Priority = TicketPriority.High,
                Status = TicketStatus.Open,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new SupportTicket
            {
                Id = Guid.NewGuid(),
                Title = "Request for account configuration",
                Description = "Customer needs assistance configuring their account.",
                CustomerEmail = "jane@example.com",
                Priority = TicketPriority.Normal,
                Status = TicketStatus.InProgress,
                CreatedAtUtc = now.AddHours(-2),
                UpdatedAtUtc = now.AddHours(-1)
            }
        };

        foreach (var ticket in sampleTickets)
        {
            _tickets.TryAdd(ticket.Id, ticket);
        }
    }

    public IReadOnlyCollection<SupportTicket> GetAll()
    {
        return _tickets.Values
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToArray();
    }

    public SupportTicket? GetById(Guid id)
    {
        return _tickets.TryGetValue(id, out var ticket)
            ? ticket
            : null;
    }

    public SupportTicket Create(CreateTicketRequest request)
    {
        var now = DateTimeOffset.UtcNow;

        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            Priority = request.Priority,
            Status = TicketStatus.Open,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _tickets.TryAdd(ticket.Id, ticket);

        return ticket;
    }

    public SupportTicket? Update(Guid id, UpdateTicketRequest request)
    {
        if (!_tickets.TryGetValue(id, out var existing))
        {
            return null;
        }

        var updated = existing with
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            Priority = request.Priority,
            Status = request.Status,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        return _tickets.TryUpdate(id, updated, existing)
            ? updated
            : null;
    }

    public bool Delete(Guid id)
    {
        return _tickets.TryRemove(id, out _);
    }
}