using Business_Layer;
using Data_Layer;
using System.Collections.Generic;

namespace Service_Layer
{
    public class VetVisitService
    {
        private readonly IDb<VetVisit, int> db = new VetVisitContext();

        public void Create(VetVisit item) => db.Create(item);
        public VetVisit Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.Read(id, useNavigationalProperties, isReadOnly);
        public List<VetVisit> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.ReadAll(useNavigationalProperties, isReadOnly);
        public void Update(VetVisit item, bool useNavigationalProperties = false)
            => db.Update(item, useNavigationalProperties);
        public void Delete(int id) => db.Delete(id);
    }
}
