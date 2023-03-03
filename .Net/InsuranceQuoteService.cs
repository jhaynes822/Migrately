using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Appointments;
using Sabio.Models.Domain.Countries;
using Sabio.Models.Domain.Insurances;
using Sabio.Models.Domain.Licenses;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Appointments;
using Sabio.Models.Requests.InsuranceQuotes;
using Sabio.Services.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class InsuranceQuoteService : IInsuranceQuoteService
    {
        IDataProvider _data;
        IUserService _userService;

        public InsuranceQuoteService(IDataProvider dataProvider, IUserService userService)
        {
            _data = dataProvider;
            _userService = userService;
        }

        public InsuranceQuote GetById(int id)
        {
            string procName = "[dbo].[InsuranceQuotes_Select_ById]";
            InsuranceQuote insuranceQuote = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                insuranceQuote = MapSingleInsuranceQuote(reader, ref startingIndex);
            });
            return insuranceQuote;
        }

        public Paged<InsuranceQuote> GetAllPaginated(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[InsuranceQuotes_SelectAll_Paginated]";
            int totalCount = 0;
            Paged<InsuranceQuote> pagedList = null;
            List<InsuranceQuote> insuranceQuoteList = null;


            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                InsuranceQuote insuranceQuote = MapSingleInsuranceQuote(reader, ref startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex);

                if (insuranceQuoteList == null)
                {
                    insuranceQuoteList = new List<InsuranceQuote>();
                }
                insuranceQuoteList.Add(insuranceQuote);
            });

            if (insuranceQuoteList != null)
            {
                pagedList = new Paged<InsuranceQuote>(insuranceQuoteList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<InsuranceQuote> GetByCreatedByPaginated(string user, int pageIndex, int pageSize)
        {
            Paged<InsuranceQuote> pagedList = null;
            List<InsuranceQuote> insuranceQuoteList = null;
            int totalCount = 0;
            string procName = "[dbo].[InsuranceQuotes_Select_ByCreatedBy_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@User", user);
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                InsuranceQuote insuranceQuote = MapSingleInsuranceQuote(reader, ref startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex);

                if (insuranceQuoteList == null)
                {
                    insuranceQuoteList = new List<InsuranceQuote>();
                }

                insuranceQuoteList.Add(insuranceQuote);
            });

            if (insuranceQuoteList != null)
            {
                pagedList = new Paged<InsuranceQuote>(insuranceQuoteList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public int Add(InsuranceQuoteAddRequest insuranceQuote, int userId)
        {
            int id = 0;
            string procName = "[dbo].[InsuranceQuotes_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(insuranceQuote, col);
                    col.AddWithValue("@CreatedBy", userId);
                    col.AddWithValue("@ModifiedBy", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;
        }

        public void Update(InsuranceQuoteUpdateRequest insuranceQuote, int userId)
        {
            string procName = "[dbo].[InsuranceQuotes_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(insuranceQuote, col);
                    col.AddWithValue("@Id", insuranceQuote.Id);
                    col.AddWithValue("@ModifiedBy", userId);
                },
                returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[InsuranceQuotes_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        public List<Insurance> GetAllInsurance()
        {
            string procName = "[dbo].[Insurance_SelectAll]";
            List<Insurance> listOfInsurances = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Insurance insurance = new Insurance();
                LookUp insuranceType = new LookUp();

                insurance.Id = reader.GetSafeInt32(startingIndex++);
                insuranceType.Id = reader.GetSafeInt32(startingIndex++);
                insuranceType.Name = reader.GetSafeString(startingIndex++);
                insurance.InsuranceType = insuranceType;
                insurance.Name = reader.GetSafeString(startingIndex++);

                if (listOfInsurances == null)
                {
                    listOfInsurances = new List<Insurance>();
                }
                listOfInsurances.Add(insurance);
            });
            return listOfInsurances;
        }

        public List<Country> GetAllCountries()
        {
            string procName = "[dbo].[Countries_SelectAll]";
            List<Country> listOfCountries = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Country country = new Country();


                country.Id = reader.GetSafeInt32(startingIndex++);
                country.Code = reader.GetSafeString(startingIndex++);
                country.Name = reader.GetSafeString(startingIndex++);
                country.Flag = reader.GetSafeString(startingIndex++);

                if (listOfCountries == null)
                {
                    listOfCountries = new List<Country>();
                }
                listOfCountries.Add(country);
            });
            return listOfCountries;
        }

        public Paged<InsuranceQuote> GetByEmailPaginated(string userEmail, int pageIndex, int pageSize)
        {
            Paged<InsuranceQuote> pagedList = null;
            List<InsuranceQuote> insuranceQuoteList = null;
            int totalCount = 0;
            string procName = "[dbo].[InsuranceQuotes_Select_ByEmail_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@CreatedByEmail", userEmail);
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                InsuranceQuote insuranceQuote = MapSingleInsuranceQuote(reader, ref startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (insuranceQuoteList == null)
                {
                    insuranceQuoteList = new List<InsuranceQuote>();
                }

                insuranceQuoteList.Add(insuranceQuote);
            });

            if (insuranceQuoteList != null)
            {
                pagedList = new Paged<InsuranceQuote>(insuranceQuoteList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<InsuranceQuote> GetByDateRangePaginated(DateTime dateRangeStart, DateTime dateRangeEnd, int pageIndex, int pageSize)
        {
            Paged<InsuranceQuote> pagedList = null;
            List<InsuranceQuote> insuranceQuoteList = null;
            int totalCount = 0;
            string procName = "[dbo].[InsuranceQuotes_SelectDateRange_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {


                paramCollection.AddWithValue("@DateRangeStart", dateRangeStart);
                paramCollection.AddWithValue("@DateRangeEnd", dateRangeEnd);
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);

            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                InsuranceQuote insuranceQuote = MapSingleInsuranceQuote(reader, ref startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (insuranceQuoteList == null)
                {
                    insuranceQuoteList = new List<InsuranceQuote>();
                }

                insuranceQuoteList.Add(insuranceQuote);
            });

            if (insuranceQuoteList != null)
            {
                pagedList = new Paged<InsuranceQuote>(insuranceQuoteList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        
        public Paged<InsuranceQuote> GetByUserDateRangePaginated(string user, DateTime dateRangeStart, DateTime dateRangeEnd, int pageIndex, int pageSize)
        {
            Paged<InsuranceQuote> pagedList = null;
            List<InsuranceQuote> insuranceQuoteList = null;
            int totalCount = 0;
            string procName = "[dbo].[InsuranceQuotes_SelectDateRangeAndUser_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@User", user);
                paramCollection.AddWithValue("@DateRangeStart", dateRangeStart);
                paramCollection.AddWithValue("@DateRangeEnd", dateRangeEnd);
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);

            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                InsuranceQuote insuranceQuote = MapSingleInsuranceQuote(reader, ref startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (insuranceQuoteList == null)
                {
                    insuranceQuoteList = new List<InsuranceQuote>();
                }

                insuranceQuoteList.Add(insuranceQuote);
            });

            if (insuranceQuoteList != null)
            {
                pagedList = new Paged<InsuranceQuote>(insuranceQuoteList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        private InsuranceQuote MapSingleInsuranceQuote(IDataReader reader, ref int startingIndex)
        {
            InsuranceQuote insuranceQuote = new InsuranceQuote();
            Insurance insurance = new Insurance();
            LookUp insuranceType = new LookUp();
             
            Country citizenship = new Country();
            State mailingAddress = new State();
            State travelDestination = new State();
            LookUp visaTypeId = new LookUp();
            LookUp policyRange = new LookUp();


            insuranceQuote.Id = reader.GetSafeInt32(startingIndex++);
            insuranceType.Id = reader.GetSafeInt32(startingIndex++);
            insurance.Name = reader.GetSafeString(startingIndex++);
            insurance.InsuranceType = insuranceType;
            insurance.Id = reader.GetSafeInt32(startingIndex++);
            insurance.Name = reader.GetSafeString(startingIndex++);
            insuranceQuote.Insurance = insurance;
            insuranceQuote.CoverageStartDate = reader.GetSafeDateTime(startingIndex++);
            insuranceQuote.CoverageEndDate = reader.GetSafeDateTime(startingIndex++);
            citizenship.Code = reader.GetSafeString(startingIndex++);
            citizenship.Name = reader.GetSafeString(startingIndex++);
            citizenship.Flag = reader.GetSafeString(startingIndex++);
            insuranceQuote.Citizenship = citizenship;
            insuranceQuote.Age = reader.GetSafeInt32(startingIndex++);
            mailingAddress.Code = reader.GetSafeString(startingIndex++);
            mailingAddress.Name = reader.GetSafeString(startingIndex++);
            insuranceQuote.MailingAddress = mailingAddress;
            travelDestination.Code = reader.GetSafeString(startingIndex++);
            travelDestination.Name = reader.GetSafeString(startingIndex++);
            insuranceQuote.TravelDestination = travelDestination;
            policyRange.Id = reader.GetSafeInt32(startingIndex++);
            policyRange.Name = reader.GetSafeString(startingIndex++);
            insuranceQuote.PolicyRangeId = policyRange;
            insuranceQuote.IsArrivedInUSA = reader.GetSafeBool(startingIndex++);
            visaTypeId.Id = reader.GetSafeInt32(startingIndex++);
            visaTypeId.Name = reader.GetSafeString(startingIndex++);
            insuranceQuote.VisaTypeId = visaTypeId;
            insuranceQuote.DateCreated = reader.GetSafeDateTime(startingIndex++);
            insuranceQuote.DateModified = reader.GetSafeDateTime(startingIndex++);
            insuranceQuote.CreatedBy = _userService.MapSingleUser(reader, ref startingIndex);
            insuranceQuote.ModifiedBy = _userService.MapSingleUser(reader, ref startingIndex);
            

            
            return insuranceQuote;
        }

        private static void AddCommonParams(InsuranceQuoteAddRequest insuranceQuote, SqlParameterCollection col)
        {
            col.AddWithValue("@InsuranceId", insuranceQuote.InsuranceId);
            col.AddWithValue("@CoverageStartDate", insuranceQuote.CoverageStartDate);
            col.AddWithValue("@CoverageEndDate", insuranceQuote.CoverageEndDate);
            col.AddWithValue("@Citizenship", insuranceQuote.Citizenship);
            col.AddWithValue("@Age", insuranceQuote.Age);
            col.AddWithValue("@MailingAddress", insuranceQuote.MailingAddress);
            col.AddWithValue("@TravelDestination", insuranceQuote.TravelDestination);
            col.AddWithValue("@PolicyRangeId", insuranceQuote.PolicyRangeId);
            col.AddWithValue("@IsArrivedInUSA", insuranceQuote.IsArrivedInUSA);
            col.AddWithValue("@VisaTypeId", insuranceQuote.VisaTypeId);
            
        }
    }
}
