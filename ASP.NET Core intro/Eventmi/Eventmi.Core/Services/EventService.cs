using Eventmi.Core.Contracts;
using Eventmi.Core.Models;
using Eventmi.Infrastructure.Data.Models;

namespace Eventmi.Core.Services
{
    public class EventService : IEventService
    {

        public async Task AddAsync(EventDetailsModel model)
        {
           Event entity = new Event()
        {
               Name = model.Name,
               End = model.End,
               Start = model.Start,
               Place = model.Place
        };
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
