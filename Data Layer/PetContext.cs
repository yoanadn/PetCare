using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class PetContext : IDb<Pet, int>
    {
        private readonly PetCareDbContext context;

        public PetContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(Pet item)
        {
            context.Pets.Add(item);
            context.SaveChanges();
        }

        public Pet Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Pet> query = context.Pets;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.Owner)
                    .Include(p => p.VetVisits)
                    .Include(p => p.Vaccines)
                    .Include(p => p.Medications)
                    .Include(p => p.Appointments)
                    .Include(p => p.HealthRecords);
            }

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(p => p.Id == key);
        }

        public List<Pet> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Pet> query = context.Pets;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.Owner)
                    .Include(p => p.VetVisits)
                    .Include(p => p.Vaccines)
                    .Include(p => p.Medications)
                    .Include(p => p.Appointments)
                    .Include(p => p.HealthRecords);
            }

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(Pet item, bool useNavigationalProperties = false)
        {
            Pet existing = Read(item.Id, useNavigationalProperties);

            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
                    existing.VetVisits = item.VetVisits;
                    existing.Vaccines = item.Vaccines;
                    existing.Medications = item.Medications;
                    existing.Appointments = item.Appointments;
                    existing.HealthRecords = item.HealthRecords;
                }

                context.SaveChanges();
            }
        }

        public void Delete(int key)
        {
            Pet item = Read(key);

            if (item != null)
            {
                context.Pets.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
