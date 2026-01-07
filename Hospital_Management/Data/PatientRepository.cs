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

            db.Execute(
                "sp_Patient_Insert",
                m,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Update(PatientModel m)
        {
            using var db = new SqlConnection(_con);

            db.Execute(
                "sp_Patient_Update",
                m,
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
    }
}
