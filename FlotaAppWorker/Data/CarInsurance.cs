using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
   public class CarInsurance
    {
        public int Id { get; set; }
        public string Insurer { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Package { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
