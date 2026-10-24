using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Common.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
}