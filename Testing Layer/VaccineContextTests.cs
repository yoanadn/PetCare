using Business_Layer;
using Data_Layer;
using System;
using System.Linq;
using Xunit;

namespace Testing_Layer
{
    public class VaccineContextTests
    {
        private readonly OwnerContext ownerDb = new OwnerContext();
        private readonly PetContext petDb = new PetContext();
        private readonly VaccineContext vaccineDb = new VaccineContext();

        private Owner CreateOwner(string tag)
        {
            var owner = new Owner
            {
                FullName = "Owner " + tag,
                Phone = "0000000000",
                Address = "Test Address"
            };

            ownerDb.Create(owner);
            return ownerDb.ReadAll(isReadOnly: true).Last(o => o.FullName == owner.FullName);
        }

        private Pet CreatePet(int ownerId, string tag)
        {
            var pet = new Pet
            {
                Name = "Pet " + tag,
                Species = "Dog",
                Breed = "Mixed",
                BirthDate = new DateTime(2020, 1, 1),
                Weight = 10.5,
                OwnerId = ownerId
            };

            petDb.Create(pet);
            return petDb.ReadAll(isReadOnly: true).Last(p => p.Name == pet.Name);
        }

        private Vaccine CreateVaccine(int petId, string tag)
        {
            var vaccine = new Vaccine
            {
                Type = "Rabies " + tag,
                DateGiven = DateTime.Today,
                NextDoseDate = DateTime.Today.AddMonths(12),
                PetId = petId
            };

            vaccineDb.Create(vaccine);
            return vaccineDb.ReadAll(isReadOnly: true).Last(v => v.PetId == petId && v.Type == vaccine.Type);
        }

        [Fact]
        public void Create_ShouldInsertVaccine()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var vaccine = CreateVaccine(pet.Id, tag);

            var created = vaccineDb.ReadAll(isReadOnly: true).LastOrDefault(v => v.Id == vaccine.Id);
            Assert.NotNull(created);

            vaccineDb.Delete(vaccine.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Read_ShouldReturnVaccineById()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var vaccine = CreateVaccine(pet.Id, tag);

            var read = vaccineDb.Read(vaccine.Id, isReadOnly: true);
            Assert.NotNull(read);
            Assert.Equal(vaccine.Id, read.Id);

            vaccineDb.Delete(vaccine.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void ReadAll_ShouldReturnList()
        {
            var all = vaccineDb.ReadAll(isReadOnly: true);
            Assert.NotNull(all);
        }

        [Fact]
        public void Update_ShouldModifyVaccine()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var vaccine = CreateVaccine(pet.Id, tag);

            vaccine.Type = vaccine.Type + " UPDATED";
            vaccineDb.Update(vaccine);

            var updated = vaccineDb.Read(vaccine.Id, isReadOnly: true);
            Assert.NotNull(updated);
            Assert.Equal(vaccine.Type, updated.Type);

            vaccineDb.Delete(vaccine.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Delete_ShouldRemoveVaccine()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var vaccine = CreateVaccine(pet.Id, tag);

            vaccineDb.Delete(vaccine.Id);

            var deleted = vaccineDb.Read(vaccine.Id, isReadOnly: true);
            Assert.Null(deleted);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }
    }
}
