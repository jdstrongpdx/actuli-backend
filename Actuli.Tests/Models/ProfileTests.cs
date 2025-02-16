using Actuli.Api.Models;

namespace Actuli.Tests.Models
{
    public class ProfileTests
    {
        [Fact]
        public void Constructor_ShouldInitializeAllListsAsEmpty_AndElementsAsNotNull()
        {
            var profile = new Profile();

            Assert.NotNull(profile.Contact);
            Assert.NotNull(profile.Identity);
            Assert.NotNull(profile.Health);
            Assert.NotNull(profile.Finances);

            Assert.NotNull(profile.EducationList);
            Assert.Empty(profile.EducationList);

            Assert.NotNull(profile.WorkList);
            Assert.Empty(profile.WorkList);

            Assert.NotNull(profile.RelationshipsList);
            Assert.Empty(profile.RelationshipsList);

            Assert.NotNull(profile.ReligionsList);
            Assert.Empty(profile.ReligionsList);

            Assert.NotNull(profile.TravelsList);
            Assert.Empty(profile.TravelsList);

            Assert.NotNull(profile.ActivitiesList);
            Assert.Empty(profile.ActivitiesList);

            Assert.NotNull(profile.GivingList);
            Assert.Empty(profile.GivingList);
        }
    }
}