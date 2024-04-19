using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class RemittanceOrders
    {
        [Key]
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

        public string BeneficiaryContactNoMain { get; set; }

        public string BeneficiaryAddressMain { get; set; }

        public string BeneficiaryBankName { get; set; }

        public string IBANEuropeBSBAustralia { get; set; }

        public string BankCountryIfOthers { get; set; }

        public string PurposeOfPaymentIfOthers { get; set; }

        public string SourceOfPaymentIfOthers { get; set; }

        public string UploadSupportingTypeIfOthers { get; set; }

        public string BeneficiaryNationalityIfOthers { get; set; }

        public string BeneficiaryBusinessCategoryIfOthers { get; set; }

        public string BeneficiaryUploadIDCopy { get; set; }

        public decimal currentPayRate { get; set; }

        [ForeignKey("RemittanceId")]
        public virtual Remittances Remittances { get; set; }

        [ForeignKey("GetCurrency")]
        public virtual RemittanceProducts GetCurrencyDecimal { get; set; }

        [ForeignKey("PayCurrency")]
        public virtual RemittanceProducts PayCurrencyDecimal { get; set; }
    }

}
