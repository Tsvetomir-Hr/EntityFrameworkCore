using Eventmi.Core.Contracts;
using Eventmi.Core.Models;
using Eventmi.Infrastructure.Data.Common;
using Eventmi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventmi.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository repo;
        public EventService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task AddAsync(EventModel model)
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
            await repo.DeleteAsync<Event>(id);

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventModel>> GetAllAsync()
        {
            return await repo.AllReadonly<Event>()
                .Select(e => new EventModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    End = e.End,
                    Start = e.Start,
                    Place = e.Place
                })
                .OrderBy(e => e.Start)
                .ToListAsync();
        }

        public async Task<EventModel> GetEventAsync(int id)
        {
            Event entity = await repo.GetByIdAsync<Event>(id);
            if (entity == null)
            {
                throw new ArgumentException("Not valid id !", nameof(id));
            }

            return new EventModel()
            {
                Name = entity.Name,
                End = entity.End,
                Start = entity.Start,
                Place = entity.Place,
                Id = entity.Id
            };
        }

        public async Task UpdateAsync(EventModel model)
        {

            var entity = await repo.GetByIdAsync<Event>(model.Id);
            if (entity == null)
            {
                throw new ArgumentException("Not valid id !", nameof(model.Id));

            }

            entity.Name = model.Name;
            entity.Start = model.Start;
            entity.End = model.End;
            entity.Place = model.Place;

            await repo.SaveChangesAsync();
        }
    }
}
