using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Interfaces
{
    public interface IErrorEmailSender
    {
        void Send(string statusCode, string message);
    }
}
