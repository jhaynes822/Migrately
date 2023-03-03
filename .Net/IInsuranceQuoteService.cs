using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Countries;
using Sabio.Models.Domain.Insurances;
using Sabio.Models.Requests.InsuranceQuotes;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace Sabio.Services.Interfaces
{
    public interface IInsuranceQuoteService
    {
        int Add(InsuranceQuoteAddRequest insuranceQuote, int userId);
        void Delete(int id);
        Paged<InsuranceQuote> GetAllPaginated(int pageIndex, int pageSize);
        Paged<InsuranceQuote> GetByCreatedByPaginated(string user, int pageIndex, int pageSize);
        InsuranceQuote GetById(int id);
        void Update(InsuranceQuoteUpdateRequest insuranceQuote, int userId);
        List<Insurance> GetAllInsurance();
        List<Country> GetAllCountries();
        Paged<InsuranceQuote> GetByEmailPaginated(string userEmail, int pageIndex, int pageSize);
        Paged<InsuranceQuote> GetByDateRangePaginated(DateTime dateOne, DateTime dateTwo, int pageIndex, int pageSize);
        Paged<InsuranceQuote> GetByUserDateRangePaginated(string user, DateTime dateRangeStart, DateTime dateRangeEnd, int pageIndex, int pageSize);
    }
}