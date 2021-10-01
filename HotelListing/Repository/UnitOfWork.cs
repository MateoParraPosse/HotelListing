using HotelListing.Data;
using HotelListing.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;
        private iGebericRepository<Country> _countries;
        private iGebericRepository<Hotel> _hotels;
        public UnitOfWork(DataBaseContext context)
        {
            _context = context;
        }
        public iGebericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);

        public iGebericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        //private void Dispose(bool v)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
