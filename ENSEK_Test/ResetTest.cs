using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.Helpers;

namespace ENSEK_Test;

public class ResetTest
{

    private APIClient apiClient;

    [SetUp]
    public async Task Setup()
    {
        // Set environment and initialize API client
        EnvironmentConfig.CurrentEnvironment = Environment.GetEnvironmentVariable("ENSEK_ENV") ?? "QA";
        ApiCredentials.ClearOverrides();
        apiClient = new APIClient(useAuthentication: true);

        // Reset system state before each test
        var response = await apiClient.Reset();
        Assert.That((int)response.StatusCode, Is.EqualTo(200), "Reset failed.");
    }
    [TearDown]
    public void AfterTest()
    {
        apiClient.Dispose();
    }

    [Test]
    public async Task VerifyValidUserCanReset()
    {
        var apiClient = new APIClient(useAuthentication: true);
        var response = await apiClient.Reset();

        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        CommonHelper.LogAndValidateResponse<dynamic>(response);

    }

    [Test]
    public async Task VerifyInvalidUserCanNotReset()
    {
        ApiCredentials.OverrideUsername = "special_user";
        ApiCredentials.OverridePassword = "special_password";
        var apiClient = new APIClient(useAuthentication: true);
        var response = await apiClient.Reset();

        Assert.That((int)response.StatusCode, Is.EqualTo(401));
        CommonHelper.LogAndValidateResponse<dynamic>(response);
    }

    [Test]
    public async Task VerifyUserCannotResetWithoutAuthentication()
    {
        var apiClient = new APIClient(useAuthentication: false);
        var response = await apiClient.Reset();

        Assert.That((int)response.StatusCode, Is.EqualTo(401));
        CommonHelper.LogAndValidateResponse<dynamic>(response);

    }
}
