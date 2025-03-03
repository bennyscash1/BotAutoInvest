using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.Infra.BaseTest
{
    public static class EnumServiceList
    {
       
        public enum ErrorCode
        {
            //Transaction error list
            TimeOutTransaction = 10003,
            AmountTransactionMissing = 20001,
            AmountCreditTooHigh = 20002,
            AmountNegative = 20004,
            AmountIncorectFormat = 20005,
            BusinessIdEmpty = 20006,
            BusinessIdIncorrect = 20007,
            ExternalIdIncorect = 20009,
            PosIdIncorrect = 20010,
            GenericParam =20011,
            ExternalIdNotFound=20013,
            TransactionIdNotFound = 20014,
            RefundAmountHigherOriginalTransaction = 40001,
            Refund_BaseRefundTransactionNotAllowed = 40012,

            //Authontication
            UserOrPasswordIncorrect = 105
        }
        public enum TransactionType
        {
            DebitTransaction = 101,
            RefundTransaction = 103,
            RemoteTransfare =2
        }
        public enum BuyerMethod
        {
            BluetoothAndRefundTransaction =0,
            QrCode =1,
            PayByCode =2
        }

        public enum BuyerDataEnum
        {
            CurrentBalance,
            BuyerRegistrationStatus,
            UserId,
            PhoneNumber,
            BankAccountId
        }
        public enum StatusCode
        {
            Pending = 0,
            Success = 1,
            Canceled = 2,
            Rejected = 3,
            Requested = 7,
            Limited = 1
        }
        public enum TabName
        {
            Name1 = 1,
            [Description("Transactions")]
            Transactions,
            [Description("Ext. Transactions")]
            ExTransaction,
            [Description("Monitors & Alerts")]
            MonitoAndAlert,
            [Description("Monitors - Transactions")]
            MonitorsTransactions,
            [Description("Monitors - Users")]
            MonitorUsers,
            [Description("Monitors - Balances")]
            MonitorBalance
        }
        public static class Categories
        {
            //Test categoty
            public const string ApiStockDataGit = nameof(ApiStockDataGit);
            public const string AiIntegration = nameof(AiIntegration);
            public const string GetFinvizData = nameof(GetFinvizData);
            public const string UiWeb = nameof(UiWeb);
            public const string MobileAndroid = nameof(MobileAndroid);
            public const string LoadTest = nameof(LoadTest);

        }
        public static class TestLevel 
        {
            //Test level
            public const string Level_1 = nameof(Level_1);
            public const string Level_2 = nameof(Level_2);
          

        }
        public const string Category = nameof(Category);
    
    }
}
