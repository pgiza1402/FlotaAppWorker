using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int MeterStatus { get; set; }
        public string VAT { get; set; }
        public User User { get; set; }
        public CarInsurance CarInsurance { get; set; }
        public TechnicalExamination TechnicalExamination { get; set; }
        public Service Service { get; set; }
        public int Year { get; set; }
        public bool isArchival { get; set; }
    }
}
