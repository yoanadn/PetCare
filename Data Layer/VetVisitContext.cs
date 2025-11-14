using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class VetVisitContext : IDb<VetVisit, int>
    {
        private readonly PetCareDbContext context;

        public VetVisitContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(VetVisit item)
        {
            context.VetVisits.Add(item);
            context.SaveChanges();
        }

        public VetVisit Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<VetVisit> query = context.VetVisits;

            if (useNavigationalProperties)
                query = query.Include(v => v.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(v => v.Id == key);
        }

        public List<VetVisit> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<VetVisit> query = context.VetVisits;

            if (useNavigationalProperties)
                query = query.Include(v => v.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(VetVisit item, bool useNavigationalProperties = false)
        {
            VetVisit existing = Read(item.Id, useNavigationalProperties);

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
            VetVisit item = Read(key);

            if (item != null)
            {
                context.VetVisits.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
