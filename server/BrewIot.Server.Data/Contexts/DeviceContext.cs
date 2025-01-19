using Microsoft.EntityFrameworkCore;
using BrewIoT.Server.Data.Models;

namespace BrewIoT.Server.Data.Contexts;

public class DeviceContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Device> Devices => Set<Device>();
}
