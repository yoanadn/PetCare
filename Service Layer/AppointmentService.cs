using Business_Layer;
using Data_Layer;
using System.Collections.Generic;

namespace Service_Layer
{
    public class AppointmentService
    {
        private readonly IDb<Appointment, int> db = new AppointmentContext();

        public void Create(Appointment item) => db.Create(item);
        public Appointment Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.Read(id, useNavigationalProperties, isReadOnly);
        public List<Appointment> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.ReadAll(useNavigationalProperties, isReadOnly);
        public void Update(Appointment item, bool useNavigationalProperties = false)
            => db.Update(item, useNavigationalProperties);
        public void Delete(int id) => db.Delete(id);
    }
}
