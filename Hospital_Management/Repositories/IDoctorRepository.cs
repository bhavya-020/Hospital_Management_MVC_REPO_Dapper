using Hospital_Management.Models;

namespace Hospital_Management.Repositories
{

    public interface IDoctorRepository
    {
        List<DoctorModel> GetAll();
        DoctorModel GetById(int id);
        void Insert(DoctorModel doctor);
        void Update(DoctorModel doctor);
        void Delete(int id);
    }
}
