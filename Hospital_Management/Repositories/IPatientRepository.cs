
using Hospital_Management.Models;

namespace Hospital_Management.Repositories
{
    public interface IPatientRepository
    {
        List<PatientModel> GetAll();
        PatientModel GetById(int id);
        void Insert(PatientModel patient);
        void Update(PatientModel patient);
        void Delete(int id);
    }
}
