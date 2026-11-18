using GrowBiss.SupportApi.Models;
using GrowBiss.SupportApi.Services;
using Xunit;

namespace GrowBiss.SupportApi.Tests;

public sealed class TicketStoreTests
{
    private readonly InMemoryTicketStore _store = new();

    [Fact]
    public void GetAll_ReturnsSeededTickets()
    {
        var tickets = _store.GetAll();

        Assert.Equal(2, tickets.Count);
    }

    [Fact]
    public void GetById_ReturnsTicket_WhenTicketExists()
    {
        var created = _store.Create(new CreateTicketRequest
        {
            Title = "Test support ticket",
            Description = "This is a test support request.",
            CustomerEmail = "test@example.com",
            Priority = TicketPriority.Normal
        });

        var result = _store.GetById(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
    }

    [Fact]
    public void Create_AddsNewTicket()
    {
        var request = new CreateTicketRequest
        {
            Title = "Payment issue",
            Description = "Customer cannot complete payment.",
            CustomerEmail = "customer@example.com",
            Priority = TicketPriority.High
        };

        var result = _store.Create(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Payment issue", result.Title);
        Assert.Equal(TicketPriority.High, result.Priority);
        Assert.Equal(TicketStatus.Open, result.Status);
        Assert.Equal(result.Id, _store.GetById(result.Id)?.Id);
    }

    [Fact]
    public void Update_ModifiesExistingTicket()
    {
        var created = _store.Create(new CreateTicketRequest
        {
            Title = "Original ticket",
            Description = "Original ticket description.",
            CustomerEmail = "customer@example.com",
            Priority = TicketPriority.Normal
        });

        var request = new UpdateTicketRequest
        {
            Title = "Updated ticket",
            Description = "Updated ticket description.",
            CustomerEmail = "customer@example.com",
            Priority = TicketPriority.Urgent,
            Status = TicketStatus.InProgress
        };

        var updated = _store.Update(created.Id, request);

        Assert.NotNull(updated);
        Assert.Equal("Updated ticket", updated.Title);
        Assert.Equal(TicketPriority.Urgent, updated.Priority);
        Assert.Equal(TicketStatus.InProgress, updated.Status);
    }

    [Fact]
    public void Delete_RemovesExistingTicket()
    {
        var created = _store.Create(new CreateTicketRequest
        {
            Title = "Ticket to delete",
            Description = "This ticket will be deleted.",
            CustomerEmail = "customer@example.com",
            Priority = TicketPriority.Low
        });

        var deleted = _store.Delete(created.Id);

        Assert.True(deleted);
        Assert.Null(_store.GetById(created.Id));
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenTicketDoesNotExist()
    {
        var result = _store.Delete(Guid.NewGuid());

        Assert.False(result);
    }
}