using FlotaAppWorker.Data;
using FlotaAppWorker.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly DataContext _context;

        public CarRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Car>> GetCarsAsync()
        {
            var cars = await _context.Cars
            .Where(x => x.isArchival == false)   
            .Include(a => a.User)
            .Include(c => c.CarInsurance)
            .Include(s => s.Service)
            .Include(t => t.TechnicalExamination)
            .ToListAsync();

            return cars;
        }
    }
}
