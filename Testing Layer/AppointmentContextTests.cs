using Business_Layer;
using Data_Layer;
using System;
using System.Linq;
using Xunit;

namespace Testing_Layer
{
    public class AppointmentContextTests
    {
        private readonly OwnerContext ownerDb = new OwnerContext();
        private readonly PetContext petDb = new PetContext();
        private readonly AppointmentContext appointmentDb = new AppointmentContext();

        private Owner CreateOwner()
        {
            var owner = new Owner
            {
                FullName = "Owner " + Guid.NewGuid(),
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

        private Appointment CreateAppointment(int petId)
        {
            var appt = new Appointment
            {
                DateTime = DateTime.Now.AddDays(1),
                Location = "Clinic",
                Reason = "Checkup",
                PetId = petId
            };

            appointmentDb.Create(appt);
            return appointmentDb.ReadAll(isReadOnly: true).Last(a => a.PetId == petId && a.Reason == appt.Reason);
        }

        [Fact]
        public void Create_ShouldInsertAppointment()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);
            var appt = CreateAppointment(pet.Id);

            var created = appointmentDb.ReadAll(isReadOnly: true).LastOrDefault(a => a.Id == appt.Id);
            Assert.NotNull(created);

            appointmentDb.Delete(appt.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Read_ShouldReturnAppointmentById()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);
            var appt = CreateAppointment(pet.Id);

            var read = appointmentDb.Read(appt.Id, isReadOnly: true);
            Assert.NotNull(read);
            Assert.Equal(appt.Id, read.Id);

            appointmentDb.Delete(appt.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void ReadAll_ShouldReturnList()
        {
            var all = appointmentDb.ReadAll(isReadOnly: true);
            Assert.NotNull(all);
        }

        [Fact]
        public void Update_ShouldModifyAppointment()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);
            var appt = CreateAppointment(pet.Id);

            appt.Location = "New Clinic";
            appointmentDb.Update(appt);

            var updated = appointmentDb.Read(appt.Id, isReadOnly: true);
            Assert.NotNull(updated);
            Assert.Equal("New Clinic", updated.Location);

            appointmentDb.Delete(appt.Id);
            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }

        [Fact]
        public void Delete_ShouldRemoveAppointment()
        {
            var owner = CreateOwner();
            var pet = CreatePet(owner.Id);
            var appt = CreateAppointment(pet.Id);

            appointmentDb.Delete(appt.Id);

            var deleted = appointmentDb.Read(appt.Id, isReadOnly: true);
            Assert.Null(deleted);

            petDb.Delete(pet.Id);
            ownerDb.Delete(owner.Id);
        }
    }
}
