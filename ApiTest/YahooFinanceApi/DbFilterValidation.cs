using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.YahooFinanceApi
{
    public class DbFilterValidation
    {
        public async Task<bool> isFilterDataValidToSaveOnDb(
            string symboleName,
            string marketCap,
            string sharesOutstanding,
            string averageDailyVolume3Month,
            string trailingAnnualDividendRate,
            string movingAvg50,
            string movingAvg200,
            YahooRequestService yahooRequestService)
        {
         /*   if (!YahooRequestService.IsMarketCapHaveALargeThreshold(marketCap))
            {
                Console.WriteLine($"The market cap amount: {marketCap} is less than 2 bil");
                return false;
            }*/

            bool isSharesOutstandingDiff = yahooRequestService
                .isSharesOutstandingDiffFromaverageDailyVolume3Month(sharesOutstanding, averageDailyVolume3Month);

            if (isSharesOutstandingDiff)
            {
                Console.WriteLine($"shared outstanding of {symboleName} is significantly different from averageDailyVolume3Month");
                //it was true
                return true;
            }

            bool isTrailingAnnualDividendRateBigEnough = yahooRequestService
                .isTrailingAnnualDividendRateIsBig(movingAvg50, movingAvg200, trailingAnnualDividendRate);

            if (!isTrailingAnnualDividendRateBigEnough)
            {
                Console.WriteLine($"The trailingAnnualDividendRate for symbol {symboleName} amount: {trailingAnnualDividendRate} is less than 3");
                //it was true
                return true;
            }

            return true;
        }

    }
}
