using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class CustomerSummary
    {
        public string Date { get; set; }

        public string CustomerAccount { get; set;}

        public int ID { get; set; }

    }

    public class TransactionSummary
    {
        public int SaleID { get; set; }

        public string MemoID { get; set; }

        public string Date { get; set; }

        public int CustomerID { get; set; }

        public CustomerParticular Customer { get; set; }

        public List<RemittanceOrders> RemittanceOrders { get; set; }

        public List<SaleTransaction> SaleTransactions { get; set; }

        public string type { get; set; }
    }

    public class RemittanceOrdersList
    {
        public int ID { get; set; }

        public int RemittanceId { get; set; }

        public string TransactionID { get; set; }

        public int PayCurrency { get; set; }

        public int GetCurrency { get; set; }

        public decimal PayAmount { get; set; }

        public decimal GetAmount { get; set; }

        public string PayPaymentType { get; set; }

        public string GetPaymentType { get; set; }

        public int PayDepositAccount { get; set; }

        public decimal Fee { get; set; }

        public decimal Rate { get; set; }

        public int BeneficiaryId { get; set; }

        public string BeneficiaryFullName { get; set; }

        public string BeneficiaryBankAccountNo { get; set; }

        public string BeneficiaryBankCode { get; set; }

        public int BeneficiaryBankCountry { get; set; }

        public string BeneficiaryBankAddress { get; set; }

        public int BeneficiaryPurposeOfPayment { get; set; }

        public int BeneficiarySourceOfPayment { get; set; }

        public int BeneficiaryUploadSupportingType { get; set; }

        public string BeneficiaryUploadSupportingFile { get; set; }

        public string BeneficiaryPaymentDetails { get; set; }

        public string BeneficiaryType { get; set; }

        public int BeneficiaryNationality { get; set; }

        public string BeneficiaryCompanyRegistrationNo { get; set; }

        public int BeneficiaryCategoryOfBusiness { get; set; }

        public string BeneficiaryCompanyContactNo { get; set; }

        public string ChequeNo { get; set; }

        public string BankTransferNo { get; set; }

        public Remittances Remittances { get; set; }

        public RemittanceProducts GetCurrencyDecimal { get; set; }

        public RemittanceProducts PayCurrencyDecimal { get; set; }
    }

    public class SaleTransactionList
    {
        public int ID { get; set; }

        public int SaleId { get; set; }

        public string TransactionID { get; set; }

        public string TransactionType { get; set; }

        public int CurrencyId { get; set; }

        public decimal Rate { get; set; }

        //[Required(ErrorMessage = "Encashment Rate is required!")]
        public decimal? EncashmentRate { get; set; }

        //[Required(ErrorMessage = "Cross Rate is required!")]
        public decimal? CrossRate { get; set; }
        public int Unit { get; set; }

        public decimal AmountForeign { get; set; }
        public decimal AmountLocal { get; set; }

        //[Display(Name = "Classification")]
        //[Required(ErrorMessage = "Classification is required!")]
        //public string Classification { get; set; }
        //[Required(ErrorMessage = "Payment Mode is required!")]
        public string PaymentMode { get; set; }
        public string ChequeNo { get; set; }
        public string BankTransferNo { get; set; }

        public string VesselName { get; set; }

        public string YouGetPaymentType { get; set; }

        public string YouPayAccount { get; set; }

        public string BeneficiaryBankAccNo { get; set; }

        public string BeneficiaryBankAddress { get; set; }

        public Sale Sales { get; set; }

        public List<SaleTransactionDenomination> SaleTransactionDenominations { get; set; }

        public Product Products { get; set; }
    }

    public class MonthlySalesDate
    {
        public int CustomerParticularId { get; set; }

        public string LastApprovalOn { get; set; }
    }
}