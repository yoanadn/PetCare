using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class HealthRecordContext : IDb<HealthRecord, int>
    {
        private readonly PetCareDbContext context;

        public HealthRecordContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(HealthRecord item)
        {
            context.HealthRecords.Add(item);
            context.SaveChanges();
        }

        public HealthRecord Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<HealthRecord> query = context.HealthRecords;

            if (useNavigationalProperties)
                query = query.Include(h => h.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(h => h.Id == key);
        }

        public List<HealthRecord> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<HealthRecord> query = context.HealthRecords;

            if (useNavigationalProperties)
                query = query.Include(h => h.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(HealthRecord item, bool useNavigationalProperties = false)
        {
            HealthRecord existing = Read(item.Id, useNavigationalProperties);

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
            HealthRecord item = Read(key);

            if (item != null)
            {
                context.HealthRecords.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
