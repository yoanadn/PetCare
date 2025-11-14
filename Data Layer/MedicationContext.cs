using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class MedicationContext : IDb<Medication, int>
    {
        private readonly PetCareDbContext context;

        public MedicationContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(Medication item)
        {
            context.Medications.Add(item);
            context.SaveChanges();
        }

        public Medication Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Medication> query = context.Medications;

            if (useNavigationalProperties)
                query = query.Include(m => m.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(m => m.Id == key);
        }

        public List<Medication> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Medication> query = context.Medications;

            if (useNavigationalProperties)
                query = query.Include(m => m.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(Medication item, bool useNavigationalProperties = false)
        {
            Medication existing = Read(item.Id, useNavigationalProperties);

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
            Medication item = Read(key);

            if (item != null)
            {
                context.Medications.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
