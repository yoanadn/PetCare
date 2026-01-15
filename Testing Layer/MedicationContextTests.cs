using Business_Layer;
using Data_Layer;
using System;
using System.Linq;
using Xunit;

namespace Testing_Layer
{
    public class MedicationContextTests
    {
        private readonly OwnerContext ownerDb = new OwnerContext();
        private readonly PetContext petDb = new PetContext();
        private readonly MedicationContext medicationDb = new MedicationContext();

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
                Species = "Cat",
                Breed = "Mixed",
                BirthDate = new DateTime(2020, 1, 1),
                Weight = 4.2,
                OwnerId = ownerId
            };

            petDb.Create(pet);
            return petDb.ReadAll(isReadOnly: true).Last(p => p.Name == pet.Name);
        }

        private Medication CreateMedication(int petId, string tag)
        {
            var med = new Medication
            {
                Name = "Antibiotic " + tag,
                Dosage = "2x daily",
                DurationDays = 5,
                StartDate = DateTime.Today,
                PetId = petId
            };

            medicationDb.Create(med);
            return medicationDb.ReadAll(isReadOnly: true).Last(m => m.PetId == petId && m.Name == med.Name);
        }

        [Fact]
        public void Create_ShouldInsertMedication()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var med = CreateMedication(pet.Id, tag);

            var created = medicationDb.ReadAll(isReadOnly: true).LastOrDefault(m => m.Id == med.Id);
            Assert.NotNull(created);

            medicationDb.Delete(med.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Read_ShouldReturnMedicationById()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var med = CreateMedication(pet.Id, tag);

            var read = medicationDb.Read(med.Id, isReadOnly: true);
            Assert.NotNull(read);
            Assert.Equal(med.Id, read.Id);

            medicationDb.Delete(med.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void ReadAll_ShouldReturnList()
        {
            var all = medicationDb.ReadAll(isReadOnly: true);
            Assert.NotNull(all);
        }

        [Fact]
        public void Update_ShouldModifyMedication()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var med = CreateMedication(pet.Id, tag);

            med.Dosage = "3x daily";
            medicationDb.Update(med);

            var updated = medicationDb.Read(med.Id, isReadOnly: true);
            Assert.NotNull(updated);
            Assert.Equal("3x daily", updated.Dosage);

            medicationDb.Delete(med.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Delete_ShouldRemoveMedication()
        {
            var tag = Guid.NewGuid().ToString();
            var owner = CreateOwner(tag);
            var pet = CreatePet(owner.Id, tag);
            var med = CreateMedication(pet.Id, tag);

            medicationDb.Delete(med.Id);

            var deleted = medicationDb.Read(med.Id, isReadOnly: true);
            Assert.Null(deleted);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }
    }
}
