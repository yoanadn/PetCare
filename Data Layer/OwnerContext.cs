using Business_Layer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data_Layer
{
    public class OwnerContext : IDb<Owner, int>
    {
        private readonly PetCareDbContext context;

        public OwnerContext()
        {
            context = new PetCareDbContext();
        }

        public void Create(Owner item)
        {
            context.Owners.Add(item);
            context.SaveChanges();
        }

        public Owner Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Owner> query = context.Owners;

            if (useNavigationalProperties)
                query = query.Include(o => o.Pets);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.FirstOrDefault(o => o.Id == key);
        }

        public List<Owner> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Owner> query = context.Owners;

            if (useNavigationalProperties)
                query = query.Include(o => o.Pets);

            if (isReadOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Update(Owner item, bool useNavigationalProperties = false)
        {
            Owner existing = Read(item.Id, useNavigationalProperties);

            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(item);

                if (useNavigationalProperties && item.Pets != null)
                    existing.Pets = item.Pets;

                context.SaveChanges();
            }
        }

        public void Delete(int key)
        {
            Owner item = Read(key);

            if (item != null)
            {
                context.Owners.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
