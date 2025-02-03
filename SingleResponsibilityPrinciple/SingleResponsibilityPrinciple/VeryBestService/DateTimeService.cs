using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.VeryBestService.Interfaces;

namespace SingleResponsibilityPrinciple.VeryBestService
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}
