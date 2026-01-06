using Hospital_Management.Models;

namespace Hospital_Management.Repositories
{
    public interface IAppointmentRepository
    {
        (List<AppointmentModel> Data, int TotalRecords) GetFiltered(
            string searchType,
            string searchText,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        );

        AppointmentModel GetById(int id);

        int Insert(AppointmentModel model);

        int Update(AppointmentModel model);

        void Delete(int id);
    }
}
