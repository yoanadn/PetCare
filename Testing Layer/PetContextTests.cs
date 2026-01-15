using Business_Layer;
using Data_Layer;
using System;
using System.Linq;
using Xunit;

namespace Testing_Layer
{
    public class PetContextTests
    {
        private readonly OwnerContext ownerDb;
        private readonly PetContext petDb;

        public PetContextTests()
        {
            ownerDb = new OwnerContext();
            petDb = new PetContext();
        }

        private Owner CreateOwner()
        {
            var owner = new Owner
            {
                FullName = "Pet Owner " + Guid.NewGuid(),
                Phone = "0000000000",
                Address = "Test Address"
            };

            ownerDb.Create(owner);

            return ownerDb.ReadAll(isReadOnly: true).Last(o => o.FullName == owner.FullName);
        }

        private Pet CreatePet(int ownerId)
        {
            var pet = new Pet
            {
                Name = "Pet " + Guid.NewGuid(),
                Species = "Dog",
                Breed = "Mixed",
                BirthDate = new DateTime(2020, 1, 1),
                Weight = 10.5,
                OwnerId = ownerId
            };

            petDb.Create(pet);

            return petDb.ReadAll(isReadOnly: true).Last(p => p.Name == pet.Name);
        }

        [Fact]
        public void Create_ShouldInsertPet()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);

            var created = petDb.ReadAll(isReadOnly: true).LastOrDefault(p => p.Id == pet.Id);
            Assert.NotNull(created);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Read_ShouldReturnPetById()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);

            var read = petDb.Read(pet.Id, isReadOnly: true);
            Assert.NotNull(read);
            Assert.Equal(pet.Id, read.Id);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void ReadAll_ShouldReturnList()
        {
            var all = petDb.ReadAll(isReadOnly: true);
            Assert.NotNull(all);
        }

        [Fact]
        public void Update_ShouldModifyPet()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);

            pet.Name = pet.Name + " UPDATED";
            pet.Weight = 12.3;

            petDb.Update(pet);

            var updated = petDb.Read(pet.Id, isReadOnly: true);
            Assert.NotNull(updated);
            Assert.Equal(pet.Name, updated.Name);
            Assert.Equal(pet.Weight, updated.Weight);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Delete_ShouldRemovePet()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);

            petDb.Delete(pet.Id);

            var deleted = petDb.Read(pet.Id, isReadOnly: true);
            Assert.Null(deleted);

            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Read_WithNavigations_ShouldIncludeOwner()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);

            var read = petDb.Read(pet.Id, useNavigationalProperties: true, isReadOnly: true);
            Assert.NotNull(read);
            Assert.NotNull(read.Owner);
            Assert.Equal(owner.Id, read.Owner.Id);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }
    }
}
