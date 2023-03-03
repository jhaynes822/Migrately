using Sabio.Data.Providers;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Users;
using System.ComponentModel;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class UserServiceV1 : IUserServiceV1
    {
        IDataProvider _data = null;
        public UserServiceV1(IDataProvider data) { _data = data; }

        public User Get(int id)
        {
            string procName = "[dbo].[Users_SelectById]";

            User user = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection) //inputParamMapper, int >> param (int)
            {

                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set) //Single Record Mapper, reader from DB >> Address (Hydrating model)
            {
                user = MapSingleUser(reader);

            }
           );

            return user;
        }

        public List<User> GetAll()
        {
            string procName = "[dbo].[Users_SelectAll]";

            List<User> listOfUsers = null;

            _data.ExecuteCmd(procName, inputParamMapper: null,
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                User aUser = MapSingleUser(reader);


                if (listOfUsers == null)
                {
                    listOfUsers = new List<User>();
                }
                listOfUsers.Add(aUser);
            });
            return listOfUsers;
        }

        public int Add(UserAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Users_Insert]";

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



        public void Update(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);

        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Users_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
            col.AddWithValue("@TenantId", model.TenantId);
            col.AddWithValue("@Password", model.Password);

        }

        private static User MapSingleUser(IDataReader reader)
        {
            User aUser = new User();

            int startingIndex = 0;

            aUser.Id = reader.GetSafeInt32(startingIndex++);
            aUser.FirstName = reader.GetSafeString(startingIndex++);
            aUser.LastName = reader.GetSafeString(startingIndex++);
            aUser.Email = reader.GetSafeString(startingIndex++);
            aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
            aUser.TenantId = reader.GetSafeString(startingIndex++);
            aUser.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aUser.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aUser;
        }
    }
}
