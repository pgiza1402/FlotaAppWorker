using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
   public class Service
    {
        public int Id { get; set; }

        public DateTime ServiceExpirationDate { get; set; }

        public int NextServiceMeterStatus { get; set; }


        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
