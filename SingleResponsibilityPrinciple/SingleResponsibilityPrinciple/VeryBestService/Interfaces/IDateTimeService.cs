using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.VeryBestService.Interfaces
{
    public interface IDateTimeService
    {
        DateTimeOffset Now();
    }
}
