using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class VaccineContext : IDb<Vaccine, int>
    {
        private readonly PetCareDbContext context;

        public VaccineContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(Vaccine item)
        {
            context.Vaccines.Add(item);
            context.SaveChanges();
        }

        public Vaccine Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Vaccine> query = context.Vaccines;

            if (useNavigationalProperties)
                query = query.Include(v => v.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(v => v.Id == key);
        }

        public List<Vaccine> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Vaccine> query = context.Vaccines;

            if (useNavigationalProperties)
                query = query.Include(v => v.Pet);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(Vaccine item, bool useNavigationalProperties = false)
        {
            Vaccine existing = Read(item.Id, useNavigationalProperties);

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
            Vaccine item = Read(key);

            if (item != null)
            {
                context.Vaccines.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
