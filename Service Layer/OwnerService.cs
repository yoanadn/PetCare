using Business_Layer;
using Data_Layer;
using System.Collections.Generic;

namespace Service_Layer
{
    public class OwnerService
    {
        private readonly IDb<Owner, int> db = new OwnerContext();

        public void Create(Owner item) => db.Create(item);
        public Owner Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.Read(id, useNavigationalProperties, isReadOnly);
        public List<Owner> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.ReadAll(useNavigationalProperties, isReadOnly);
        public void Update(Owner item, bool useNavigationalProperties = false)
            => db.Update(item, useNavigationalProperties);
        public void Delete(int id) => db.Delete(id);
    }
}
