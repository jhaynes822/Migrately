using Sabio.Data.Providers;
using Sabio.Models.Requests.Addresses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Addresses;
using Sabio.Data;
using Sabio.Models.Domain.Concerts;
using Sabio.Models.Requests.Concerts;

namespace Sabio.Services
{
    public class ConcertService
    {
        IDataProvider _data = null;
        public ConcertService(IDataProvider data) { _data = data; }

        public int Add(ConcertAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Concerts_Insert]";

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

        

        public Concert GetById(int id)
        {

            string procName = "[dbo].[Concerts_SelectById]";

            Concert concert = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection) //inputParamMapper, int >> param (int)
            {

                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set) //Single Record Mapper, reader from DB >> Address (Hydrating model)
            {
                SingleConcertMapper(reader);

            }
            );

            return concert;
        }

        

        public List<Concert> GetAll()
        {
            string procName = "[dbo].[Concerts_SelectAll]";

            List<Concert> concertList = null;

            _data.ExecuteCmd(procName, inputParamMapper: null,

            singleRecordMapper: delegate (IDataReader reader, short set) 
            {
                Concert aConcert = SingleConcertMapper(reader);

                if (concertList == null)
                {
                    concertList = new List<Concert>();
                }

                concertList.Add(aConcert);
            }
           );

            return concertList;
        }


        public void Update(ConcertUpdateRequest model)
        {
            string procName = "[dbo].[Concerts_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Concerts_Delete]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void AddCommonParams(ConcertAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@IsFree", model.IsFree);
            col.AddWithValue("@Address", model.Address);
            col.AddWithValue("@Cost", model.Cost);
            col.AddWithValue("@DateOfEvent", model.DateOfEvent);
        }
        private static Concert SingleConcertMapper(IDataReader reader)
        {
            Concert aConcert = new Concert();

            int startingIndex = 0;

            aConcert.Id = reader.GetSafeInt32(startingIndex++);
            aConcert.Name = reader.GetSafeString(startingIndex++);
            aConcert.Description = reader.GetSafeString(startingIndex++);
            aConcert.IsFree = reader.GetSafeBool(startingIndex++);
            aConcert.Address = reader.GetSafeString(startingIndex++);
            aConcert.Cost = reader.GetSafeInt32(startingIndex++);
            aConcert.DateOfEvent = reader.GetSafeDateTime(startingIndex++);
            return aConcert;
        }
    }
    
}
