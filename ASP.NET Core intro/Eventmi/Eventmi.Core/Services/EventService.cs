using Eventmi.Core.Contracts;
using Eventmi.Core.Models;
using Eventmi.Infrastructure.Data.Common;
using Eventmi.Infrastructure.Data.Models;

namespace Eventmi.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository repo;
        public EventService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task AddAsync(EventDetailsModel model)
        {
            Event entity = new Event()
            {
                Name = model.Name,
                End = model.End,
                Start = model.Start,
                Place = model.Place
            };
            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EventModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<EventDetailsModel> GetEventAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(EventModel model)
        {
            throw new NotImplementedException();
        }
    }
}
