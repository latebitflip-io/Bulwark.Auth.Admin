namespace Bulwark.Auth.Admin.Core.Tests.Utils
{
    public static class TestUtils
    {
        public static string GenerateEmail()
        {
            return Guid.NewGuid() + "@bulwark.test";
        }
    }
}
