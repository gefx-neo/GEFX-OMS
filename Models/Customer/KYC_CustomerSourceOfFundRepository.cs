using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class KYC_CustomerSourceOfFundRepository : IKYC_CustomerSourceOfFundRepository
	{
		private DataAccess.GreatEastForex db;

		public KYC_CustomerSourceOfFundRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<KYC_CustomerSourceOfFunds> Select()
		{
			var result = from c in db.KYC_CustomerSourceOfFunds select c;

			return result;
		}

		public KYC_CustomerSourceOfFunds GetSingle(int id)
		{
			try
			{
				IQueryable<KYC_CustomerSourceOfFunds> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerSourceOfFunds GetSourceOfFund(int CustomerParticularId)
		{
			try
			{
				IQueryable<KYC_CustomerSourceOfFunds> records = Select();

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
				KYC_CustomerSourceOfFunds temp = new KYC_CustomerSourceOfFunds();

				temp.Company_SourceOfFund = addData.Company_SourceOfFund;
				temp.Company_SourceOfFundIfOthers = addData.Company_SourceOfFundIfOthers;
				temp.Company_ServiceLikeToUse = addData.Company_ServiceLikeToUse;
				temp.Company_HearAboutUs = addData.Company_HearAboutUs;
				temp.Company_PurposeOfIntendedTransactions = addData.Company_PurposeOfIntendedTransactions;
				temp.Company_PoliticallyExposedIndividuals_1 = addData.Company_PoliticallyExposedIndividuals_1;
				temp.Company_PoliticallyExposedIndividuals_2 = addData.Company_PoliticallyExposedIndividuals_2;
				temp.Company_PoliticallyExposedIndividuals_3 = addData.Company_PoliticallyExposedIndividuals_3;
				temp.Natural_SourceOfFund = addData.Natural_SourceOfFund;
				temp.Natural_SourceOfFundIfOthers = addData.Natural_SourceOfFundIfOthers;
				temp.Natural_ServiceLikeToUse = addData.Natural_ServiceLikeToUse;
				temp.Natural_HearAboutUs = addData.Natural_HearAboutUs;
				temp.Natural_PurposeOfIntendedTransactions = addData.Natural_PurposeOfIntendedTransactions;
				temp.Natural_AnnualIncome = addData.Natural_AnnualIncome;
				temp.Natural_PoliticallyExposedIndividuals_1 = addData.Natural_PoliticallyExposedIndividuals_1;
				temp.Natural_PoliticallyExposedIndividuals_2 = addData.Natural_PoliticallyExposedIndividuals_2;
				temp.Natural_PoliticallyExposedIndividuals_3 = addData.Natural_PoliticallyExposedIndividuals_3;
				temp.CustomerParticularId = addData.CustomerParticularId;

				db.KYC_CustomerSourceOfFunds.Add(temp);

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
				KYC_CustomerSourceOfFunds data = db.KYC_CustomerSourceOfFunds.Where(e => e.ID == id).FirstOrDefault();

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

		public bool Delete(int id)
		{
			try
			{
				KYC_CustomerSourceOfFunds data = db.KYC_CustomerSourceOfFunds.Find(id);

				db.KYC_CustomerSourceOfFunds.Remove(data);

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