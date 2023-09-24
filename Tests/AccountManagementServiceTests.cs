using Defender.IdentityService.Application.Common.Interfaces;
using NUnit.Framework;

namespace Defender.IdentityService.Tests;

public class AccountManagementServiceTests
{
    private IAccountManagementService _accountManagementService;

    [SetUp]
    public void Setup(IAccountManagementService accountManagementService)
    {
        _accountManagementService = accountManagementService;
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
