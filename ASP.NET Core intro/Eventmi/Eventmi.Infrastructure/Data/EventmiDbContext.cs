﻿namespace Eventmi.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

public class EventmiDbContext : DbContext
{
    public EventmiDbContext()
    {

    }
    public EventmiDbContext(DbContextOptions<EventmiDbContext> options)
        : base(options)
    {

    }
}
