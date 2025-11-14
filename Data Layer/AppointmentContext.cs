using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class AppointmentContext : IDb<Appointment, int>
    {
        private readonly PetCareDbContext context;

        public AppointmentContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(Appointment item)
        {
            context.Appointments.Add(item);
            context.SaveChanges();
        }

        public Appointment Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Appointment> query = context.Appointments;

            if (useNavigationalProperties)
                query = query.Include(a => a.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(a => a.Id == key);
        }

        public List<Appointment> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Appointment> query = context.Appointments;

            if (useNavigationalProperties)
                query = query.Include(a => a.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(Appointment item, bool useNavigationalProperties = false)
        {
            Appointment existing = Read(item.Id, useNavigationalProperties);

            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                    existing.Pet = item.Pet;

                context.SaveChanges();
            }
        }

        public void Delete(int key)
        {
            Appointment item = Read(key);

            if (item != null)
            {
                context.Appointments.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
