using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;
using Sabio.Models.Domain.Addresses;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class AddressService : IAddressService
    {
        IDataProvider _data = null;
        public AddressService(IDataProvider data) { _data = data; }


        public void Update(AddressUpdateRequest model)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }
        
        public int Add(AddressAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Sabio_Addresses_Insert]";

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

        public Address Get(int id)
        {

            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection) //inputParamMapper, int >> param (int)
            {

                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set) //Single Record Mapper, reader from DB >> Address (Hydrating model)
            {
                address = MapSingleAddress(reader);
            }
            );

            return address;
        }

        public List<Address> GetRandomAddresses()
        {
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            List<Address> listOfAddresses = null;

            _data.ExecuteCmd(procName, inputParamMapper: null,

             singleRecordMapper: delegate (IDataReader reader, short set) //Single Record Mapper, reader from DB >> Address (Hydrating model)
             {
                 Address anAddress = MapSingleAddress(reader);

                 if (listOfAddresses == null)
                 {
                     listOfAddresses = new List<Address>();
                 }

                 listOfAddresses.Add(anAddress);
             }
            );

            return listOfAddresses;
        }

        private static Address MapSingleAddress(IDataReader reader)
        {
            Address anAddress = new Address();

            int startingIndex = 0;

            anAddress.Id = reader.GetSafeInt32(startingIndex++);
            anAddress.LineOne = reader.GetSafeString(startingIndex++);
            anAddress.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            anAddress.City = reader.GetSafeString(startingIndex++);
            anAddress.State = reader.GetSafeString(startingIndex++);
            anAddress.PostalCode = reader.GetSafeString(startingIndex++);
            anAddress.IsActive = reader.GetSafeBool(startingIndex++);
            anAddress.Lat = reader.GetSafeDouble(startingIndex++);
            anAddress.Long = reader.GetSafeDouble(startingIndex++);
            return anAddress;
        }

        private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@SuiteNumber", model.SuiteNumber);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@State", model.State);
            col.AddWithValue("@PostalCode", model.PostalCode);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@Lat", model.Lat);
            col.AddWithValue("@Long", model.Long);
        }
    }
}
