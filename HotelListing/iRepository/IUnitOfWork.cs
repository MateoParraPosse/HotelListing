using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.iRepository
{
    public interface IUnitOfWork : IDisposable
    {
        iGebericRepository<Country> Countries { get; }
        iGebericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}
