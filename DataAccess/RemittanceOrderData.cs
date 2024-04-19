using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataAccess
{
    public class RemittanceOrderData
    {
        public string RowId { get; set; }

        public int ID { get; set; }

        //public int RemittanceId { get; set; }

        public string PayCurrency { get; set; }

        public string GetCurrency { get; set; }

        public string PayAmount { get; set; }

        public string GetAmount { get; set; }

        public string PayPaymentType { get; set; }

        public string GetPaymentType { get; set; }

        public string PayDepositAccount { get; set; }

        public string Fee { get; set; }

        public string Rate { get; set; }

        public string PayAmountPayRate { get; set; }

        public string BeneficiaryFullName { get; set; }

        public string BeneficiaryBankAccountNo { get; set; }

        public string BeneficiaryBankCode { get; set; }

        public string BeneficiaryBankCountry { get; set; }

        public string BeneficiaryBankAddress { get; set; }

        public string BeneficiaryPurposeOfPayment { get; set; }

        public string BeneficiarySourceOfPayment { get; set; }

        public string BeneficiaryUploadSupportingType { get; set; }

        public string BeneficiaryUploadSupportingFile { get; set; }

        public string BeneficiaryPaymentDetails { get; set; }

        public string BeneficiaryType { get; set; }

        public string BeneficiaryNationality { get; set; }

        public string BeneficiaryCompanyRegistrationNo { get; set; }

        public string BeneficiaryCategoryOfBusiness { get; set; }

        public string BeneficiaryCompanyContactNo { get; set; }

        public string ChequeNo { get; set; }

        public string BankTransferNo { get; set; }

        public string DisabledChequeNo { get; set; }

        public string DisabledBankTransferNo { get; set; }

        public string DisabledDepositAccount { get; set; }

        public SelectList PayCurrencyDDL { get; set; }

        public SelectList GetCurrencyDDL { get; set; }

        public SelectList PayPaymentModeDDL { get; set; }

        public SelectList GetPaymentModeDDL { get; set; }

        public SelectList DepositAccountDDL { get; set; }
        public SelectList countriesDDL { get; set; }
        public SelectList paymentListDDL { get; set; }
        public SelectList fundListDDL { get; set; }
        public SelectList nationalitiesDDL { get; set; }
        public SelectList suppDocDDL { get; set; }
        public SelectList categoryBusinessDDL { get; set; }

        public string transactionID { get; set; }

        public string transactionPayrate { get; set; }

        public string transactionGetrate { get; set; }

        public string gettransactionFees { get; set; }
        public string transactionFees { get; set; }
        public string payDecimalFormat { get; set; }
        public string getDecimalFormat { get; set; }

        public string BeneficiaryContactNoMain { get; set; }

        public string BeneficiaryAddressMain { get; set; }

        public string BeneficiaryBankName { get; set; }

        public string IBANEuropeBSBAustralia { get; set; }

        public string BankCountryIfOthers { get; set; }
        public string disabledBankCountryIfOthers { get; set; }

        public string PurposeOfPaymentIfOthers { get; set; }
        public string disabledPurposeOfPaymentIfOthers { get; set; }

        public string SourceOfPaymentIfOthers { get; set; }
        public string disabledSourceOfPaymentIfOthers { get; set; }

        public string UploadSupportingTypeIfOthers { get; set; }
        public string disabledUploadSupportingTypeIfOthers { get; set; }

        public string BeneficiaryNationalityIfOthers { get; set; }
        public string disabledBeneficiaryNationalityIfOthers { get; set; }

        public string BeneficiaryBusinessCategoryIfOthers { get; set; }
        public string disabledBeneficiaryBusinessCategoryIfOthers { get; set; }
        public string currentPayRate { get; set; }
        public string BeneficiaryUploadIDCopy { get; set; }
    }

    public class ChangePayCurrency
    {
        public string RowID { get; set; }
        public int PayCurrencyValue { get; set; }
        public int GetCurrencyValue { get; set; }
        public SelectList PayPaymentModeDDL { get; set; }
        public SelectList PayCurrencyDDL { get; set; }
        public string PaySymbol { get; set; }
        public string PayRate { get; set; }
        public string PayDecimalFormat { get; set; }
        public string PaytransactionFees { get; set; }
        public string DisabledPayType { get; set; }

    }
    public class PurposeSelectList
    {
        public string Result { get; set; }
        public SelectList PurposeofPaymentDDL { get; set; }
    }

    public class PayCurrencyApproval
    {
        public string RowID { get; set; }
        public string GetRate { get; set; }
        public string PayRate { get; set; }
        public string PaytransactionFees { get; set; }
        public string Rate { get; set; }
        public string PayAmount { get; set; }
        public string GetAmount { get; set; }
        public string GettransactionFees { get; set; }
        public string GetDecimalformat { get; set; }
        public string PayDecimalformat { get; set; }
        public string Fee { get; set; }
        public string PayProductID { get; set; }
        public string GetProductID { get; set; }
        public int Type { get; set; }
      
    }

    public class RemittanceBulkUploadClassRoot
    {
        public List<RemittanceBulkUploadClass> RemittanceClass { get; set; }
        public bool success { get; set; }
        public string error { get; set; }

        public int NoOfErrorRow { get; set; }
    }

    public class RemittanceBulkUploadClass
    {
        public decimal PayAmount { get; set; }

        public int PayDecimal { get; set; }

        public decimal Payrate { get; set; }

        public decimal PayTransactionFee { get; set; }

        public int PayCurrencyID { get; set; }

        public string PayCurrencyName { get; set; }

        public string PayCurrencyCode { get; set; }
        public string payDecimalFormat { get; set; }

        public decimal GetAmount { get; set; }

        public int GetDecimal { get; set; }

        public int GetCurrencyID { get; set; }

        public string GetCurrencyName { get; set; }

        public string getDecimalFormat { get; set; }
        public string GetCurrencyCode { get; set; }

        public decimal GetRate { get; set; }
        public string Rate { get; set; }

        public string GetBeneficiaryFriendlyName { get; set; }

        public int BusinessCategoryID { get; set; }

        public int NationalityID { get; set; }

        public int SourceOfPaymentID { get; set; }

        public int PurposeOfPaymentID { get; set; }

        public int CountryID { get; set; }

        public int DepositAccountID { get; set; }

        public int PaymentModeID { get; set; }

        public string PaymentModeText { get; set; }

        public string PaymentInstructionText { get; set; }

        public string DepositAccountText { get; set; }

        public string BankAccountNoText { get; set; }

        //public string BankType { get; set; }

        //public string BankTypeName { get; set; }

        public string BankCodeText { get; set; }

        public string CountryText { get; set; }

        public string BankAddressText { get; set; }

        public string PurposeOfPaymentText { get; set; }

        public string SourceOfPaymentText { get; set; }

        public string NationalityText { get; set; }

        public string PaymentDetailsText { get; set; }

        public string BeneficiaryTypeText { get; set; }

        public string BeneficiaryTypeValue { get; set; }

        public string BeneficiaryCompanyRegistrationNoText { get; set; }

        public string BusinessCategoryText { get; set; }

        public string BeneficiaryContactNoText { get; set; }

        public string NationalityIfOthersText { get; set; }

        public string BankCountryIfOthersText { get; set; }

        public string PurposeOfPaymentIfOthersText { get; set; }

        public string SourceOfPaymentIfOthersText { get; set; }

        public string UploadSupportingTypeIfOthersText { get; set; }

        public int UploadSupportingTypeID { get; set; }

        public string UploadSupportingTypeText { get; set; }

        public string BusinessCategoryIfOthersText { get; set; }

        public string BeneficiaryContactNoMainText { get; set; }

        public string BeneficiaryAddressMainText { get; set; }

        public string BeneficiaryBankNameText { get; set; }

        public string IBANEuropeBSBAustraliaText { get; set; }

        public SelectList PayCurrencyDDL { get; set; }

        public SelectList GetCurrencyDDL { get; set; }

        public SelectList PayPaymentModeDDL { get; set; }

        public SelectList GetPaymentModeDDL { get; set; }

        public SelectList DepositAccountDDL { get; set; }
        public SelectList countriesDDL { get; set; }
        public SelectList paymentListDDL { get; set; }
        public SelectList fundListDDL { get; set; }
        public SelectList nationalitiesDDL { get; set; }
        public SelectList suppDocDDL { get; set; }
        public SelectList categoryBusinessDDL { get; set; }
    }
}