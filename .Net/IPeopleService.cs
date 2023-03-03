using Sabio.Models;
using Sabio.Models.Domain.Practice;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IPeopleService
    {
        PracticePeople Get(int id);
        PracticePeopleV3 GetV3(int id);
        List<PracticePeople> GetAll();
        List<PracticePeopleV3> GetAllV3();
        Paged<PracticePeopleV3> SearchPaginatedV3(int pageIndex, int pageSize, string query);
    }
}