using Business_Layer;
using Data_Layer;
using System.Collections.Generic;

namespace Service_Layer
{
    public class HealthRecordService
    {
        private readonly IDb<HealthRecord, int> db = new HealthRecordContext();

        public void Create(HealthRecord item) => db.Create(item);
        public HealthRecord Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.Read(id, useNavigationalProperties, isReadOnly);
        public List<HealthRecord> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
            => db.ReadAll(useNavigationalProperties, isReadOnly);
        public void Update(HealthRecord item, bool useNavigationalProperties = false)
            => db.Update(item, useNavigationalProperties);
        public void Delete(int id) => db.Delete(id);
    }
}
