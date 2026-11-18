using GrowBiss.SupportApi.Models;

namespace GrowBiss.SupportApi.Services;

public interface ITicketStore
{
    IReadOnlyCollection<SupportTicket> GetAll();

    SupportTicket? GetById(Guid id);

    SupportTicket Create(CreateTicketRequest request);

    SupportTicket? Update(Guid id, UpdateTicketRequest request);

    bool Delete(Guid id);
}