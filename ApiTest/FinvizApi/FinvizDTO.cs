using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest
{
    public class FinvisOutputDto
    {
        public decimal lastOpen { get; set; }
        public decimal lastClose { get; set; }
        public decimal lastVolume { get; set; }
    }

  

    public class GetResponceOutputDTO : HttpResponseMessage
    {
        public int page { get; set; }
    }
}
