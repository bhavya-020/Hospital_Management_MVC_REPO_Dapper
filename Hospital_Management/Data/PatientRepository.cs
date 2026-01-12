using Dapper;
using Hospital_Management.Models;
using Hospital_Management.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;



namespace Hospital_Management.Data
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _con;

        public PatientRepository(IConfiguration configuration)
        {
            _con = configuration.GetConnectionString("dbcon");
        }

        public List<PatientModel> GetAll()
        {
            using var db = new SqlConnection(_con);

            return db.Query<PatientModel>(
                "sp_Patient_GetAll",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public PatientModel GetById(int id)
        {
            using var db = new SqlConnection(_con);

            return db.QueryFirstOrDefault<PatientModel>(

                "sp_Patient_GetById",
               new { PatientId = id },
               commandType: CommandType.StoredProcedure


            );
        }

        public void Insert(PatientModel m)
        {
            using var db = new SqlConnection(_con);

            var param = new DynamicParameters();
            param.Add("@PatientId", m.PatientId);
            param.Add("@PatientName", m.PatientName);
            param.Add("@Age", m.Age);
            param.Add("@Gender", m.Gender);
            param.Add("@Contact", m.Contact);
            //param.Add("@CreatedTime" , m.CreatedTime);

            db.Execute(
                "sp_Patient_Insert",
                param,
                commandType: CommandType.StoredProcedure
            );
            
        }

        public void Update(PatientModel m)
        {
            using var db = new SqlConnection(_con);

            var param = new DynamicParameters();
            param.Add("@PatientId", m.PatientId);
            param.Add("@PatientName", m.PatientName);
            param.Add("@Age", m.Age);
            param.Add("@Gender", m.Gender);
            param.Add("@Contact", m.Contact);
            db.Execute(
                "sp_Patient_Update",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Delete(int id)
        {
            using var db = new SqlConnection(_con);

            db.Execute(
                "sp_Patient_Delete",
                new { PatientId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        ////
        ///

        public (List<PatientModel> patients, int totalCount) GetAllFiltered(
    string search,
    int page,
    int pageSize)
        {
            using var db = new SqlConnection(_con);

            var param = new DynamicParameters();
            param.Add("@Search", search);
            param.Add("@Page", page);
            param.Add("@PageSize", pageSize);

            using var multi = db.QueryMultiple(
                "sp_Patient_GetAll_Filtered",
                param,
                commandType: CommandType.StoredProcedure
            );

            var patients = multi.Read<PatientModel>().ToList();
            int totalCount = multi.Read<int>().Single();

            return (patients, totalCount);
        }

    }
}
