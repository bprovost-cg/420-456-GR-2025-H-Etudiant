using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.VeryBestService.Interfaces;

namespace SingleResponsibilityPrinciple.VeryBestService.Mocks
{
    internal class MockDataTimeService : IDateTimeService
    {
        private DateTimeOffset _initialDateTime;
        private DateTimeOffset _currentDateTime;
        public MockDataTimeService(DateTimeOffset initialDateTime)
        {
            _initialDateTime = initialDateTime;
            _currentDateTime = initialDateTime;
        }

        public DateTimeOffset Now()
        {
            return _currentDateTime;
        }

        public void AddTime(TimeSpan timeSpan)
        {
            _currentDateTime.AddSeconds(timeSpan.TotalSeconds);
        }

        public void Reset()
        {
            _currentDateTime = _initialDateTime;
        }
    }
}
