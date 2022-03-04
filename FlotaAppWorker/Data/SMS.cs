using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
   public class SMS
    {
        public SMS(string phoneNumber, string message)
        {
            this.phoneNumber = phoneNumber;
            this.message = message;
        }

        public string phoneNumber { get; set; }
        public string message { get; set; }

    }
}
