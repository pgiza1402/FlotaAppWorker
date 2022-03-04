using FlotaAppWorker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Interfaces
{
    public interface ICarRepository
    {

        Task<IReadOnlyList<Car>> GetCarsAsync();
    }
}
