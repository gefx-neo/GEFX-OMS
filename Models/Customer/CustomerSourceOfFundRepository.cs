using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerSourceOfFundRepository : ICustomerSourceOfFundRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerSourceOfFundRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerSourceOfFund> Select()
        {
            var result = from c in db.CustomerSourceOfFunds select c;

            return result;
        }

        public CustomerSourceOfFund GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerSourceOfFund> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerSourceOfFund GetSourceOfFund(int CustomerParticularId)
        {
            try
            {
                IQueryable<CustomerSourceOfFund> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(CustomerSourceOfFund addData)
        {
            try
            {
                db.CustomerSourceOfFunds.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool AddReject(Temp_CustomerSourceOfFunds addData)
		{
			try
			{
				CustomerSourceOfFund customerSourceOfFund = new CustomerSourceOfFund();

				customerSourceOfFund.Company_PoliticallyExposedIndividuals_1 = addData.Company_PoliticallyExposedIndividuals_1;
				customerSourceOfFund.Company_PoliticallyExposedIndividuals_2 = addData.Company_PoliticallyExposedIndividuals_2;
				customerSourceOfFund.Company_PoliticallyExposedIndividuals_3 = addData.Company_PoliticallyExposedIndividuals_3;
				customerSourceOfFund.Company_SourceOfFund = addData.Company_SourceOfFund;
				customerSourceOfFund.Company_SourceOfFundIfOthers = addData.Company_SourceOfFundIfOthers;
				customerSourceOfFund.Company_ServiceLikeToUse = addData.Company_ServiceLikeToUse;
				customerSourceOfFund.Company_HearAboutUs = addData.Company_HearAboutUs;
				customerSourceOfFund.Company_PurposeOfIntendedTransactions = addData.Company_PurposeOfIntendedTransactions;
				customerSourceOfFund.CustomerParticularId = addData.CustomerParticularId;
				customerSourceOfFund.Natural_AnnualIncome = addData.Natural_AnnualIncome;
				customerSourceOfFund.Natural_PoliticallyExposedIndividuals_1 = addData.Natural_PoliticallyExposedIndividuals_1;
				customerSourceOfFund.Natural_PoliticallyExposedIndividuals_2 = addData.Natural_PoliticallyExposedIndividuals_2;
				customerSourceOfFund.Natural_PoliticallyExposedIndividuals_3 = addData.Natural_PoliticallyExposedIndividuals_3;
				customerSourceOfFund.Natural_SourceOfFund = addData.Natural_SourceOfFund;
				customerSourceOfFund.Natural_SourceOfFundIfOthers = addData.Natural_SourceOfFundIfOthers;
				customerSourceOfFund.Natural_ServiceLikeToUse = addData.Natural_ServiceLikeToUse;
				customerSourceOfFund.Natural_HearAboutUs = addData.Natural_HearAboutUs;
				customerSourceOfFund.Natural_PurposeOfIntendedTransactions = addData.Natural_PurposeOfIntendedTransactions;

				db.CustomerSourceOfFunds.Add(customerSourceOfFund);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddApproveKYC(KYC_CustomerSourceOfFunds addData)
		{
			try
			{
				CustomerSourceOfFund customerSourceOfFund = new CustomerSourceOfFund();

				customerSourceOfFund.Company_PoliticallyExposedIndividuals_1 = addData.Company_PoliticallyExposedIndividuals_1;
				customerSourceOfFund.Company_PoliticallyExposedIndividuals_2 = addData.Company_PoliticallyExposedIndividuals_2;
				customerSourceOfFund.Company_PoliticallyExposedIndividuals_3 = addData.Company_PoliticallyExposedIndividuals_3;
				customerSourceOfFund.Company_SourceOfFund = addData.Company_SourceOfFund;
				customerSourceOfFund.Company_SourceOfFundIfOthers = addData.Company_SourceOfFundIfOthers;
				customerSourceOfFund.Company_ServiceLikeToUse = addData.Company_ServiceLikeToUse;
				customerSourceOfFund.Company_HearAboutUs = addData.Company_HearAboutUs;
				customerSourceOfFund.Company_PurposeOfIntendedTransactions = addData.Company_PurposeOfIntendedTransactions;
				customerSourceOfFund.CustomerParticularId = addData.CustomerParticularId;
				customerSourceOfFund.Natural_AnnualIncome = addData.Natural_AnnualIncome;
				customerSourceOfFund.Natural_PoliticallyExposedIndividuals_1 = addData.Natural_PoliticallyExposedIndividuals_1;
				customerSourceOfFund.Natural_PoliticallyExposedIndividuals_2 = addData.Natural_PoliticallyExposedIndividuals_2;
				customerSourceOfFund.Natural_PoliticallyExposedIndividuals_3 = addData.Natural_PoliticallyExposedIndividuals_3;
				customerSourceOfFund.Natural_SourceOfFund = addData.Natural_SourceOfFund;
				customerSourceOfFund.Natural_SourceOfFundIfOthers = addData.Natural_SourceOfFundIfOthers;
				customerSourceOfFund.Natural_ServiceLikeToUse = addData.Natural_ServiceLikeToUse;
				customerSourceOfFund.Natural_HearAboutUs = addData.Natural_HearAboutUs;
				customerSourceOfFund.Natural_PurposeOfIntendedTransactions = addData.Natural_PurposeOfIntendedTransactions;

				db.CustomerSourceOfFunds.Add(customerSourceOfFund);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, CustomerSourceOfFund updateData)
        {
            try
            {
                CustomerSourceOfFund data = db.CustomerSourceOfFunds.Find(id);

                data.Company_SourceOfFund = updateData.Company_SourceOfFund;
                data.Company_SourceOfFundIfOthers = updateData.Company_SourceOfFundIfOthers;
                data.Company_PoliticallyExposedIndividuals_1 = updateData.Company_PoliticallyExposedIndividuals_1;
                data.Company_PoliticallyExposedIndividuals_2 = updateData.Company_PoliticallyExposedIndividuals_2;
                data.Company_PoliticallyExposedIndividuals_3 = updateData.Company_PoliticallyExposedIndividuals_3;
				data.Company_ServiceLikeToUse = updateData.Company_ServiceLikeToUse;
				data.Company_HearAboutUs = updateData.Company_HearAboutUs;
				data.Company_PurposeOfIntendedTransactions = updateData.Company_PurposeOfIntendedTransactions;
                data.Natural_SourceOfFund = updateData.Natural_SourceOfFund;
                data.Natural_SourceOfFundIfOthers = updateData.Natural_SourceOfFundIfOthers;
                data.Natural_AnnualIncome = updateData.Natural_AnnualIncome;
                data.Natural_PoliticallyExposedIndividuals_1 = updateData.Natural_PoliticallyExposedIndividuals_1;
                data.Natural_PoliticallyExposedIndividuals_2 = updateData.Natural_PoliticallyExposedIndividuals_2;
                data.Natural_PoliticallyExposedIndividuals_3 = updateData.Natural_PoliticallyExposedIndividuals_3;
				data.Natural_ServiceLikeToUse = updateData.Natural_ServiceLikeToUse;
				data.Natural_HearAboutUs = updateData.Natural_HearAboutUs;
				data.Natural_PurposeOfIntendedTransactions = updateData.Natural_PurposeOfIntendedTransactions;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool UpdateReject(int id, Temp_CustomerSourceOfFunds updateData)
		{
			try
			{
				CustomerSourceOfFund data = db.CustomerSourceOfFunds.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.Company_PoliticallyExposedIndividuals_1 = updateData.Company_PoliticallyExposedIndividuals_1;
				data.Company_PoliticallyExposedIndividuals_2 = updateData.Company_PoliticallyExposedIndividuals_2;
				data.Company_PoliticallyExposedIndividuals_3 = updateData.Company_PoliticallyExposedIndividuals_3;
				data.Company_SourceOfFund = updateData.Company_SourceOfFund;
				data.Company_SourceOfFundIfOthers = updateData.Company_SourceOfFundIfOthers;
				data.Company_ServiceLikeToUse = updateData.Company_ServiceLikeToUse;
				data.Company_HearAboutUs = updateData.Company_HearAboutUs;
				data.Company_PurposeOfIntendedTransactions = updateData.Company_PurposeOfIntendedTransactions;
				data.CustomerParticularId = updateData.CustomerParticularId;
				data.Natural_AnnualIncome = updateData.Natural_AnnualIncome;
				data.Natural_PoliticallyExposedIndividuals_1 = updateData.Natural_PoliticallyExposedIndividuals_1;
				data.Natural_PoliticallyExposedIndividuals_2 = updateData.Natural_PoliticallyExposedIndividuals_2;
				data.Natural_PoliticallyExposedIndividuals_3 = updateData.Natural_PoliticallyExposedIndividuals_3;
				data.Natural_SourceOfFund = updateData.Natural_SourceOfFund;
				data.Natural_SourceOfFundIfOthers = updateData.Natural_SourceOfFundIfOthers;
				data.Natural_ServiceLikeToUse = updateData.Natural_ServiceLikeToUse;
				data.Natural_HearAboutUs = updateData.Natural_HearAboutUs;
				data.Natural_PurposeOfIntendedTransactions = updateData.Natural_PurposeOfIntendedTransactions;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateApproveKYC(int id, KYC_CustomerSourceOfFunds updateData)
		{
			try
			{
				CustomerSourceOfFund data = db.CustomerSourceOfFunds.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.Company_PoliticallyExposedIndividuals_1 = updateData.Company_PoliticallyExposedIndividuals_1;
				data.Company_PoliticallyExposedIndividuals_2 = updateData.Company_PoliticallyExposedIndividuals_2;
				data.Company_PoliticallyExposedIndividuals_3 = updateData.Company_PoliticallyExposedIndividuals_3;
				data.Company_SourceOfFund = updateData.Company_SourceOfFund;
				data.Company_SourceOfFundIfOthers = updateData.Company_SourceOfFundIfOthers;
				data.Company_ServiceLikeToUse = updateData.Company_ServiceLikeToUse;
				data.Company_HearAboutUs = updateData.Company_HearAboutUs;
				data.Company_PurposeOfIntendedTransactions = updateData.Company_PurposeOfIntendedTransactions;
				data.CustomerParticularId = updateData.CustomerParticularId;
				data.Natural_AnnualIncome = updateData.Natural_AnnualIncome;
				data.Natural_PoliticallyExposedIndividuals_1 = updateData.Natural_PoliticallyExposedIndividuals_1;
				data.Natural_PoliticallyExposedIndividuals_2 = updateData.Natural_PoliticallyExposedIndividuals_2;
				data.Natural_PoliticallyExposedIndividuals_3 = updateData.Natural_PoliticallyExposedIndividuals_3;
				data.Natural_SourceOfFund = updateData.Natural_SourceOfFund;
				data.Natural_SourceOfFundIfOthers = updateData.Natural_SourceOfFundIfOthers;
				data.Natural_ServiceLikeToUse = updateData.Natural_ServiceLikeToUse;
				data.Natural_HearAboutUs = updateData.Natural_HearAboutUs;
				data.Natural_PurposeOfIntendedTransactions = updateData.Natural_PurposeOfIntendedTransactions;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Delete(int id)
        {
            try
            {
                CustomerSourceOfFund data = db.CustomerSourceOfFunds.Find(id);

                db.CustomerSourceOfFunds.Remove(data);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool DeleteAll(int id)
		{
			try
			{
				db.CustomerSourceOfFunds.Where(e => e.CustomerParticularId == id).ToList().ForEach(csof => db.CustomerSourceOfFunds.Remove(csof));
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}
	}
}