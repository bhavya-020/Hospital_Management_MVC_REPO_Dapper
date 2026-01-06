using Dapper;
using Hospital_Management.Models;
using Microsoft.Data.SqlClient;
using System.Data;



namespace Hospital_Management.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _con;

        public AppointmentRepository(IConfiguration configuration)
        {
            _con = configuration.GetConnectionString("dbcon");
        }





        public (List<AppointmentModel>, int) GetFiltered(
            string searchType,
            string searchText,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize)
        {
            using var db = new SqlConnection(_con);

            var parameters = new DynamicParameters();
            parameters.Add("@SearchType", searchType);
            parameters.Add("@SearchText", searchText);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            using var multi = db.QueryMultiple(
                "sp_Appointment_GetAll_Filtered",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var data = multi.Read<AppointmentModel>().ToList();
            int total = multi.Read<int>().Single();

            return (data, total);
        }

        public AppointmentModel GetById(int id)
        {
            using var db = new SqlConnection(_con);

            return db.QueryFirstOrDefault<AppointmentModel>(
                "sp_Appointment_GetById",
                new { AppointmentId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int Insert(AppointmentModel m)
        {
            using var db = new SqlConnection(_con);

            var parameters = new DynamicParameters();
            parameters.Add("@DoctorId", m.DoctorId);
            parameters.Add("@PatientId", m.PatientId);
            parameters.Add("@AppointmentDate", m.AppointmentDate);
            parameters.Add("@AppointmentTime", m.AppointmentTime);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            db.Execute("sp_Appointment_Insert_With_Check", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }

        public int Update(AppointmentModel m)
        {
            using var db = new SqlConnection(_con);

            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentId", m.AppointmentId);
            parameters.Add("@DoctorId", m.DoctorId);
            parameters.Add("@PatientId", m.PatientId);
            parameters.Add("@AppointmentDate", m.AppointmentDate);
            parameters.Add("@AppointmentTime", m.AppointmentTime);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            db.Execute("sp_Appointment_Update_With_Check", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }

        public void Delete(int id)
        {
            using var db = new SqlConnection(_con);

            db.Execute(
                "sp_Appointment_Delete",
                new { AppointmentId = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
