using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
   public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public int? CarId { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
