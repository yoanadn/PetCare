using Business_Layer;
using Data_Layer;
using System.Collections.Generic;

namespace Service_Layer
{
    public class PetService
    {
        private readonly IDb<Pet, int> db = new PetContext();

        public void Create(Pet item) => db.Create(item);
        public Pet Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.Read(id, useNavigationalProperties, isReadOnly);
        public List<Pet> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.ReadAll(useNavigationalProperties, isReadOnly);
        public void Update(Pet item, bool useNavigationalProperties = false)
            => db.Update(item, useNavigationalProperties);
        public void Delete(int id) => db.Delete(id);
    }
}
