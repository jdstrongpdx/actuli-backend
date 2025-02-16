using Actuli.Api.Models;

namespace Actuli.Tests.Models
{
    public class AppUserTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithGivenIdAndDefaultValues()
        {
            var id = "test-id";
            var appUser = new AppUser(id);

            Assert.Equal(id, appUser.Id);
            Assert.NotNull(appUser.Profile);
            Assert.NotNull(appUser.Overview);
            Assert.NotNull(appUser.Goals);
            Assert.NotNull(appUser.Accomplishments);
            Assert.NotNull(appUser.CreatedAt);
            Assert.InRange(appUser.CreatedAt.Value, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow);
            Assert.NotNull(appUser.ModifiedAt);
            Assert.InRange(appUser.ModifiedAt.Value, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow);
        }

        [Fact]
        public void MarkAsModified_ShouldUpdateModifiedAtToCurrentUtcTime()
        {
            var appUser = new AppUser("test-id");
            var initialModifiedAt = appUser.ModifiedAt;

            var beforeMarking = DateTime.UtcNow;
            appUser.MarkAsModified();
            var afterMarking = DateTime.UtcNow;

            Assert.NotNull(appUser.ModifiedAt);
            Assert.True(appUser.ModifiedAt >= beforeMarking && appUser.ModifiedAt <= afterMarking);
            Assert.NotEqual(initialModifiedAt, appUser.ModifiedAt);
        }
    }
}