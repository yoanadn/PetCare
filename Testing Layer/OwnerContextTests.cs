using Business_Layer;
using Data_Layer;
using System;
using System.Linq;
using Xunit;

namespace Testing_Layer
{
    public class OwnerContextTests
    {
        private readonly OwnerContext db;

        public OwnerContextTests()
        {
            db = new OwnerContext();
        }

        [Fact]
        public void Create_ShouldInsertOwner()
        {
            var unique = Guid.NewGuid().ToString();
            var owner = new Owner
            {
                FullName = "DL Create " + unique,
                Phone = "0000000000",
                Address = "Test Address"
            };

            db.Create(owner);

            var created = db.ReadAll(isReadOnly: true).LastOrDefault(o => o.FullName == owner.FullName);
            Assert.NotNull(created);

            db.Delete(created.Id);
        }

        [Fact]
        public void Read_ShouldReturnOwnerById()
        {
            var unique = Guid.NewGuid().ToString();
            var owner = new Owner
            {
                FullName = "DL Read " + unique,
                Phone = "0000000000",
                Address = "Test Address"
            };

            db.Create(owner);

            var created = db.ReadAll(isReadOnly: true).Last(o => o.FullName == owner.FullName);
            var read = db.Read(created.Id, isReadOnly: true);

            Assert.NotNull(read);
            Assert.Equal(created.Id, read.Id);

            db.Delete(created.Id);
        }

        [Fact]
        public void ReadAll_ShouldReturnList()
        {
            var all = db.ReadAll(isReadOnly: true);
            Assert.NotNull(all);
        }

        [Fact]
        public void Update_ShouldModifyOwner()
        {
            var unique = Guid.NewGuid().ToString();
            var owner = new Owner
            {
                FullName = "DL Update " + unique,
                Phone = "0000000000",
                Address = "Test Address"
            };

            db.Create(owner);

            var created = db.ReadAll().Last(o => o.FullName == owner.FullName);
            created.FullName += " UPDATED";

            db.Update(created);

            var updated = db.Read(created.Id, isReadOnly: true);
            Assert.NotNull(updated);
            Assert.Equal(created.FullName, updated.FullName);

            db.Delete(created.Id);
        }

        [Fact]
        public void Delete_ShouldRemoveOwner()
        {
            var unique = Guid.NewGuid().ToString();
            var owner = new Owner
            {
                FullName = "DL Delete " + unique,
                Phone = "0000000000",
                Address = "Test Address"
            };

            db.Create(owner);

            var created = db.ReadAll().Last(o => o.FullName == owner.FullName);

            db.Delete(created.Id);

            var deleted = db.Read(created.Id, isReadOnly: true);
            Assert.Null(deleted);
        }
    }
}
