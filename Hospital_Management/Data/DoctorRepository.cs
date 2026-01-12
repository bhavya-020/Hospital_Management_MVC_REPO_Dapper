
using Dapper;
using Hospital_Management.Models;
using Hospital_Management.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Data
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _con;

        public DoctorRepository(IConfiguration configuration)
        {
            _con = configuration.GetConnectionString("dbcon");
        }

        public List<DoctorModel> GetAll()
        {
            using var db = new SqlConnection(_con);

            return db.Query<DoctorModel>(
                "sp_Doctor_GetAll",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public DoctorModel GetById(int id)
        {
            using var db = new SqlConnection(_con);

            return db.QueryFirstOrDefault<DoctorModel>(

                "sp_Doctor_GetById",
               new { DoctorId = id },
               commandType: CommandType.StoredProcedure

                //"SELECT * FROM Doctors WHERE DoctorId = @id",
                //new { id }
            );
        }

        public void Insert(DoctorModel m)
        {
            using var db = new SqlConnection(_con);

            var param = new DynamicParameters();
            param.Add("@DoctorName", m.DoctorName);
            param.Add("@Specialization", m.Specialization);
            param.Add("@WorkPlace", m.WorkPlace);
            param.Add("@Experience", m.Experience);
            //param.Add("@CreatedTime" , m.CreatedTime);

            db.Execute(
                "sp_Doctor_Insert",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Update(DoctorModel m)
        {
            using var db = new SqlConnection(_con);

            var param = new DynamicParameters();
            param.Add("@DoctorId", m.DoctorId);
            param.Add("@DoctorName", m.DoctorName);
            param.Add("@Specialization", m.Specialization);
            param.Add("@WorkPlace", m.WorkPlace);
            param.Add("@Experience", m.Experience);

            db.Execute(
                "sp_Doctor_Update",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Delete(int id)
        {
            using var db = new SqlConnection(_con);

            db.Execute(
                "sp_Doctor_Delete",
                new { DoctorId = id },
                commandType: CommandType.StoredProcedure
            );
        }



        //filter 

        public (List<DoctorModel> doctors, int totalCount) GetAllFiltered(
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
                "sp_Doctor_GetAll_Filtered",
                param,
                commandType: CommandType.StoredProcedure
            );

            var doctors = multi.Read<DoctorModel>().ToList();
            int totalCount = multi.Read<int>().Single();

            return (doctors, totalCount);
        }


    }
}
