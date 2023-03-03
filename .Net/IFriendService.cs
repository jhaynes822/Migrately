using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFriendService
    {
        int Add(FriendAddRequest model, int userId);
        int FriendV3Add(FriendV3AddRequest model, int userId);
        void Delete(int id);
        Friend Get(int id);
        FriendV3 GetV3(int id);
        List<Friend> GetAll();
        List<FriendV3> GetAllV3();
        Paged<Friend> Pagination(int pageIndex, int pageSize);
        Paged<FriendV3> PaginationV3(int pageIndex, int pageSize);
        public Paged<Friend> Search(int pageIndex, int pageSize, string query);
        Paged<FriendV3> SearchPaginatedV3(int pageIndex, int pageSize, string query);
        void Update(FriendUpdateRequest model, int userId);
        void FriendV3Update(FriendV3UpdateRequest model, int userId);
    }
}