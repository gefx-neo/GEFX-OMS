using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class GreatEastForex : DbContext
    {
        public GreatEastForex()
            : base("GreatEastForex")
        {
            this.Database.CommandTimeout = 5000;
            this.Database.Connection.StateChange += Connection_StateChange;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerCustomRate>().Property(x => x.BuyRate).HasPrecision(18, 8);
            modelBuilder.Entity<CustomerCustomRate>().Property(x => x.SellRate).HasPrecision(18, 8);
            modelBuilder.Entity<CustomerCustomRate>().Property(x => x.EncashmentRate).HasPrecision(18, 8);
			modelBuilder.Entity<Temp_CustomerCustomRates>().Property(x => x.BuyRate).HasPrecision(18, 8);
			modelBuilder.Entity<Temp_CustomerCustomRates>().Property(x => x.SellRate).HasPrecision(18, 8);
			modelBuilder.Entity<Temp_CustomerCustomRates>().Property(x => x.EncashmentRate).HasPrecision(18, 8);
			modelBuilder.Entity<EndDayTrade>().Property(x => x.OpeningBankAmount).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.OpeningCashAmount).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.OpeningForeignCurrencyBalance).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.OpeningAveragePurchaseCost).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.OpeningBalanceAtAveragePurchase).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.OpeningProfitAmount).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.ClosingBankAmount).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.ClosingCashAmount).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.ClosingForeignCurrencyBalance).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.ClosingAveragePurchaseCost).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.ClosingBalanceAtAveragePurchase).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.ClosingProfitAmount).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTrade>().Property(x => x.CurrentSGDBuyRate).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTradeTransaction>().Property(x => x.AmountForeign).HasPrecision(18, 8);
            modelBuilder.Entity<EndDayTradeTransaction>().Property(x => x.AmountLocal).HasPrecision(18, 8);
            modelBuilder.Entity<Inventory>().Property(x => x.Amount).HasPrecision(18, 8);
            modelBuilder.Entity<ProductInventory>().Property(x => x.TotalInAccount).HasPrecision(18, 8);
            modelBuilder.Entity<Product>().Property(x => x.BuyRate).HasPrecision(18, 8);
            modelBuilder.Entity<Product>().Property(x => x.SellRate).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.BuyRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.SellRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.AutomatedBuyRate).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.AutomatedSellRate).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.AcceptableRange).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.EncashmentRate).HasPrecision(18, 8);
			modelBuilder.Entity<Product>().Property(x => x.MaxAmount).HasPrecision(18, 8);
			modelBuilder.Entity<Sale>().Property(x => x.CashAmount).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.Cheque1Amount).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.Cheque2Amount).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.Cheque3Amount).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.BankTransferAmount).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.MemoBalance).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.TotalAmountLocal).HasPrecision(18, 8);
            modelBuilder.Entity<Sale>().Property(x => x.TotalAmountForeign).HasPrecision(18, 8);
            modelBuilder.Entity<SaleTransactionDenomination>().Property(x => x.AmountForeign).HasPrecision(18, 8);
            modelBuilder.Entity<SaleTransaction>().Property(x => x.Rate).HasPrecision(18, 8);
            modelBuilder.Entity<SaleTransaction>().Property(x => x.EncashmentRate).HasPrecision(18, 8);
            modelBuilder.Entity<SaleTransaction>().Property(x => x.CrossRate).HasPrecision(18, 8);
            modelBuilder.Entity<SaleTransaction>().Property(x => x.AmountLocal).HasPrecision(18, 8);
            modelBuilder.Entity<SaleTransaction>().Property(x => x.AmountForeign).HasPrecision(18, 8);
            modelBuilder.Entity<RemittanceOrders>().Property(x => x.PayAmount).HasPrecision(28, 8);
            modelBuilder.Entity<RemittanceOrders>().Property(x => x.GetAmount).HasPrecision(28, 8);
            modelBuilder.Entity<RemittanceOrders>().Property(x => x.Rate).HasPrecision(28, 12);
			modelBuilder.Entity<RemittanceOrders>().Property(x => x.currentPayRate).HasPrecision(28, 12);
			modelBuilder.Entity<RemittanceOrders>().Property(x => x.Fee).HasPrecision(28, 8);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.PayRate).HasPrecision(28, 12);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.GetRate).HasPrecision(28, 12);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.BuyRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.SellRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.AutomatedGetRate).HasPrecision(28, 12);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.AutomatedPayRate).HasPrecision(28, 12);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.AcceptableRange).HasPrecision(18, 8);
			modelBuilder.Entity<RemittanceProducts>().Property(x => x.MaxAmount).HasPrecision(28, 8);
			modelBuilder.Entity<CustomerRemittanceProductCustomRate>().Property(x => x.PayRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<CustomerRemittanceProductCustomRate>().Property(x => x.GetRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<CustomerRemittanceProductCustomRate>().Property(x => x.Fee).HasPrecision(18, 8);
			modelBuilder.Entity<KYC_CustomerRemittanceProductCustomRates>().Property(x => x.PayRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<KYC_CustomerRemittanceProductCustomRates>().Property(x => x.GetRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<KYC_CustomerRemittanceProductCustomRates>().Property(x => x.Fee).HasPrecision(18, 8);
			modelBuilder.Entity<Temp_CustomerRemittanceProductCustomRates>().Property(x => x.PayRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<Temp_CustomerRemittanceProductCustomRates>().Property(x => x.GetRateAdjustment).HasPrecision(18, 8);
			modelBuilder.Entity<Temp_CustomerRemittanceProductCustomRates>().Property(x => x.Fee).HasPrecision(18, 8);
			modelBuilder.Entity<Remittances>().Property(x => x.TotalPayAmount).HasPrecision(28, 8);
			modelBuilder.Entity<Remittances>().Property(x => x.TotalGetAmount).HasPrecision(28, 8);
			modelBuilder.Entity<Remittances>().Property(x => x.AgentRate).HasPrecision(28, 12);
			modelBuilder.Entity<Remittances>().Property(x => x.AgentFee).HasPrecision(28, 2);
			modelBuilder.Entity<Sale>().Property(d => d.Version).IsRowVersion();
		}

        void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            if (e.CurrentState == System.Data.ConnectionState.Open)
            {
                var connection = (System.Data.Common.DbConnection)sender;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SET ANSI_NULLS OFF";
                    //cmd.CommandText = @"
                    //                    SET ARITHABORT OFF
                    //                    SET NUMERIC_ROUNDABORT OFF
                    //                    SET ANSI_WARNINGS ON
                    //                    SET ANSI_PADDING ON
                    //                    SET ANSI_NULLS ON
                    //                    SET CONCAT_NULL_YIELDS_NULL ON
                    //                    SET CURSOR_CLOSE_ON_COMMIT OFF
                    //                    SET IMPLICIT_TRANSACTIONS OFF
                    //                    SET LANGUAGE US_ENGLISH
                    //                    SET DATEFORMAT MDY
                    //                    SET DATEFIRST 7
                    //                    SET TRANSACTION ISOLATION LEVEL READ COMMITTED
                    //                    ";

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Beneficiaries> Beneficiaries { get; set; }

        public DbSet<BeneficiaryList> BeneficiaryLists { get; set; }

        public DbSet<CustomerActingAgent> CustomerActingAgents { get; set; }

        public DbSet<CustomerAppointmentOfStaff> CustomerAppointmentOfStaffs { get; set; }

        public DbSet<CustomerCustomRate> CustomerCustomRates { get; set; }

        public DbSet<CustomerDocumentCheckList> CustomerDocumentCheckLists { get; set; }

        public DbSet<CustomerScreeningReport> CustomerScreeningReports { get; set; }

        public DbSet<CustomerOther> CustomerOthers { get; set; }

        public DbSet<CustomerParticular> CustomerParticulars { get; set; }

        public DbSet<CustomerSourceOfFund> CustomerSourceOfFunds { get; set; }

		public DbSet<CustomerActivityLog> CustomerActivityLogs { get; set; }
        public DbSet<KYC> eKYC { get; set; }

        public DbSet<EmailLog> EmailLogs { get; set; }

        public DbSet<EndDayTrade> EndDayTrades { get; set; }

        public DbSet<EndDayTradeTransaction> EndDayTradeTransactions { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductDenomination> ProductDenominations { get; set; }

        public DbSet<ProductInventory> ProductInventories { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleTransaction> SaleTransactions { get; set; }

        public DbSet<SaleTransactionDenomination> SaleTransactionDenominations { get; set; }

        public DbSet<ScanIncoming> ScanIncomings { get; set; }

        public DbSet<ScanOutgoing> ScanOutgoings { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<User> Users { get; set; }

		public DbSet<SearchTags> SearchTags { get; set; }

		public DbSet<ApprovalHistorys> ApprovalHistorys { get; set; }

		public DbSet<CustomerPortal_SupportContactSetting> SupportContactSettings { get; set; }

		public DbSet<GetAllCustomerActiveList> GetAllCustomerActiveLists { get; set; }

		public DbSet<GetAllCustomerActiveListInSalesModule> GetAllCustomerActiveListInSalesModules { get; set; }

		public DbSet<Temp_CustomerParticulars> Temp_CustomerParticulars { get; set; }

		public DbSet<Temp_CustomerOthers> Temp_CustomerOthers { get; set; }

		public DbSet<Temp_CustomerSourceOfFunds> Temp_CustomerSourceOfFunds { get; set; }

		public DbSet<Temp_CustomerActingAgents> Temp_CustomerActingAgents { get; set; }

		public DbSet<Temp_CustomerAppointmentOfStaffs> Temp_CustomerAppointmentOfStaffs { get; set; }

		public DbSet<Temp_CustomerDocumentCheckLists> Temp_CustomerDocumentsCheckList { get; set; }

		public DbSet<Temp_CustomerScreeningReports> Temp_CustomerScreeningReports { get; set; }

		public DbSet<Temp_CustomerActivityLogs> Temp_CustomerActivityLogs { get; set; }

		public DbSet<Temp_CustomerCustomRates> Temp_CustomerCustomRates { get; set; }

		public DbSet<KYC_CustomerParticulars> KYC_CustomerParticulars { get; set; }

		public DbSet<KYC_CustomerOthers> KYC_CustomerOthers { get; set; }

		public DbSet<KYC_CustomerSourceOfFunds> KYC_CustomerSourceOfFunds { get; set; }

		public DbSet<KYC_CustomerDocumentCheckLists> KYC_CustomerDocumentCheckLists { get; set; }

		public DbSet<KYC_CustomerActingAgents> KYC_CustomerActingAgents { get; set; }

		public DbSet<Countries> Countries { get; set; }

		public DbSet<CountryCodeLists> CountryCodeLists { get; set; }

		public DbSet<Agents> Agents { get; set; }

		public DbSet<RemittanceProducts> RemittanceProducts { get; set; }

        public DbSet<Remittances> Remittances { get; set; }

        public DbSet<RemittanceOrders> RemittanceOrders { get; set; }

        public DbSet<Nationalities> Nationalities { get; set; }

        public DbSet<BusinessCategoriesLists> BusinessCategoriesLists { get; set; }

        public DbSet<PaymentLists> PaymentLists { get; set; }
        public DbSet<PayBankLists> PayBankLists { get; set; }

        public DbSet<FundLists> FundLists { get; set; }
        public DbSet<SupportingDocumentTypeLists> SupportingDocumentTypeLists { get; set; }
		public DbSet<PaymentModeLists> PaymentModeLists { get; set; }

		public DbSet<CustomerRemittanceProductCustomRate> CustomerRemittanceProductCustomRates { get; set; }

		public DbSet<Temp_CustomerRemittanceProductCustomRates> Temp_CustomerRemittanceProductCustomRates { get; set; }

		public DbSet<KYC_CustomerCustomRates> KYC_CustomerCustomRates { get; set; }

		public DbSet<KYC_CustomerRemittanceProductCustomRates> KYC_CustomerRemittanceProductCustomRates { get; set; }

        public DbSet<CheckSalesCurrencyIDExists> CheckSalesCurrencyIDExists { get; set; }

    }
}