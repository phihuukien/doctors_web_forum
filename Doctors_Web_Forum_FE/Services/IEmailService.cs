using Doctors_Web_Forum_FE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(String ToEmail,String Token);
    }
}
