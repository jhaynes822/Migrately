using Sabio.Data;
using Sabio.Data.Extensions;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Friends;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class FriendService : IFriendService
    {
        IDataProvider _data = null;

        public FriendService(IDataProvider data) { _data = data; }

        public Friend Get(int id)
        {
            string procName = "[dbo].[Friends_SelectById]";

            Friend friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                friend = MapSingleFriend(reader);
            }
            );
            return friend;
        }

        public FriendV3 GetV3(int id)
        {
            string procName = "[dbo].[Friends_SelectByIdV3]";

            FriendV3 friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                friend = MapSingleFriendV3(reader);
            }
            );
            return friend;

        }

        public List<Friend> GetAll()
        {
            string procName = "[dbo].[Friends_SelectAll]";

            List<Friend> listOfFriends = null;

            _data.ExecuteCmd(procName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Friend aFriend = MapSingleFriend(reader);

                    if (listOfFriends == null)
                    {
                        listOfFriends = new List<Friend>();
                    }
                    listOfFriends.Add(aFriend);
                });
            return listOfFriends;
        }

        public List<FriendV3> GetAllV3()
        {
            string procName = "[dbo].[Friends_SelectAllV3]";

            List<FriendV3> listOfFriends = null;

            _data.ExecuteCmd(procName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    FriendV3 aFriend = MapSingleFriendV3(reader);

                    if (listOfFriends == null)
                    {
                        listOfFriends = new List<FriendV3>();
                    }
                    listOfFriends.Add(aFriend);
                });
            return listOfFriends;
        }

        public int Add(FriendAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Friends_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

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
        
        public int FriendV3Add(FriendV3AddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Friends_InsertV3]";

            DataTable myParamValue = MapSkillsToTable(model.Skills);

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@SkillsList", myParamValue);

                AddCommonParamsV3(model, col);

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

        public void Update(FriendUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Friends_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        } 
        
        public void FriendV3Update(FriendV3UpdateRequest model, int userId)
        {
            string procName = "[dbo].[Friends_UpdateV3]";

            DataTable myParamValue = MapSkillsToTable(model.Skills);

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParamsV3(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@SkillsList", myParamValue);
                col.AddWithValue("@UserId", userId);
            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Friends_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        public Paged<Friend> Pagination(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Friends_Pagination]";
            Paged<Friend> pagedList = null;
            List<Friend> listOfFriends = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                Friend aFriend = MapSingleFriend(reader);
                totalCount = reader.GetSafeInt32(6);

                if (listOfFriends == null)
                {
                    listOfFriends = new List<Friend>();
                }
                listOfFriends.Add(aFriend);
            });
            if (listOfFriends != null)
            {
                pagedList = new Paged<Friend>(listOfFriends, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Friend> Search(int pageIndex, int pageSize, string query)
        {
            string procName = "[dbo].[Friends_Search]";
            Paged<Friend> pagedList = null;
            List<Friend> listOfFriends = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            }, (reader, recordSetIndex) =>
            {
                Friend aFriend = MapSingleFriend(reader);
                totalCount = reader.GetSafeInt32(6);

                if (listOfFriends == null)
                {
                    listOfFriends = new List<Friend>();
                }
                listOfFriends.Add(aFriend);
            });
            if (listOfFriends != null)
            {
                pagedList = new Paged<Friend>(listOfFriends, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<FriendV3> PaginationV3(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Friends_PaginationV3]";
            Paged<FriendV3> pagedList = null;
            List<FriendV3> listOfFriends = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                FriendV3 aFriend = MapSingleFriendV3(reader);
                totalCount = reader.GetSafeInt32(6);

                if (listOfFriends == null)
                {
                    listOfFriends = new List<FriendV3>();
                }
                listOfFriends.Add(aFriend);
            });
            if (listOfFriends != null)
            {
                pagedList = new Paged<FriendV3>(listOfFriends, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        } 
        
        public Paged<FriendV3> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            string procName = "[dbo].[Friends_Search_PaginationV3]";
            Paged<FriendV3> pagedList = null;
            List<FriendV3> listOfFriends = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            }, (reader, recordSetIndex) =>
            {
                FriendV3 aFriend = MapSingleFriendV3(reader);
                totalCount = reader.GetSafeInt32(6);

                if (listOfFriends == null)
                {
                    listOfFriends = new List<FriendV3>();
                }
                listOfFriends.Add(aFriend);
            });
            if (listOfFriends != null)
            {
                pagedList = new Paged<FriendV3>(listOfFriends, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        private static Friend MapSingleFriend(IDataReader reader)
        {
            Friend aFriend = new Friend();

            int startingIndex = 0;

            aFriend.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.Title = reader.GetSafeString(startingIndex++);
            aFriend.Bio = reader.GetSafeString(startingIndex++);
            aFriend.Summary = reader.GetSafeString(startingIndex++);
            aFriend.Headline = reader.GetSafeString(startingIndex++);
            aFriend.Slug = reader.GetSafeString(startingIndex++);
            aFriend.StatusId = reader.GetSafeInt32(startingIndex++);
            aFriend.UserId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
           
            return aFriend;
        }       
        
        private static FriendV3 MapSingleFriendV3(IDataReader reader)
        {
            FriendV3 aFriend = new FriendV3();

            aFriend.PrimaryImage = new Image();

            int startingIndex = 0;

            aFriend.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.Title = reader.GetSafeString(startingIndex++);
            aFriend.Bio = reader.GetSafeString(startingIndex++);
            aFriend.Summary = reader.GetSafeString(startingIndex++);
            aFriend.Headline = reader.GetSafeString(startingIndex++);
            aFriend.Slug = reader.GetSafeString(startingIndex++);
            aFriend.StatusId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.Url = reader.GetSafeString(startingIndex++);
            aFriend.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++);
            aFriend.UserId = reader.GetSafeInt32(startingIndex++);
            aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
           
            return aFriend;
        }
        
        private static void AddCommonParams(FriendAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@UserId", 1234);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);

        } 
        
        private static void AddCommonParamsV3(FriendV3AddRequest model, SqlParameterCollection col)
        {
            
 
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@ImageTypeId", model.ImageTypeId);
            col.AddWithValue("@ImageUrl", model.ImageUrl);
            col.AddWithValue("@StatusId", model.StatusId);


        }

        private DataTable MapSkillsToTable(List<string> SkillsToMap)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));

            foreach (string singleSkill in SkillsToMap)
            {
                DataRow dr = dt.NewRow();

                int startingIndex = 0;

                dr.SetField(startingIndex++, singleSkill);


                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
