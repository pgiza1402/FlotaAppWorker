using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
   public class TechnicalExamination
    {
        public int Id { get; set; }

        public DateTime TechnicalExaminationExpirationDate { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
