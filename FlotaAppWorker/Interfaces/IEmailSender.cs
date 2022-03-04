using FlotaAppWorker.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Interfaces
{
    public interface IEmailSender
    {
        void Send(Car car, string message);
    }
}
