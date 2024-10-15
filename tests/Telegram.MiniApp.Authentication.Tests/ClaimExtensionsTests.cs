using System.Security.Claims;
using FakeItEasy;
using Neftm.TelegramMiniApp.Authorization;
using Neftm.TelegramMiniApp.Authorization.Constants;
using Neftm.TelegramMiniApp.Authorization.Extensions;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace Neftm.TelegramMiniApp.Authentication.Tests;

public class ClaimExtensionsTests
{
	[Theory]
	[MemberData(nameof(Data))]
	public void TryGetTmaUserTest_Successful(List<Claim> claims, TmaUserPrincipals expectedOutput)
	{
		//Arrange
		var claimIdentity = A.Fake<ClaimsIdentity>();
		A.CallTo(() => claimIdentity.AuthenticationType).Returns(TmaDefaults.AuthenticationScheme);
		A.CallTo(() => claimIdentity.Claims).Returns(claims);
		
		var claimsPrincipal = A.Fake<ClaimsPrincipal>();
		A.CallTo(() => claimsPrincipal.Identities).Returns(new List<ClaimsIdentity>()
		{
			claimIdentity
		});
		
		//Act
		var actualResult = claimsPrincipal.TryGetTmaUser(out var actualOutput);

		//Assert
		Assert.True(actualResult);
		
		//Assert output
		Assert.NotNull(actualOutput);
		Assert.Equal(expectedOutput.UserId, actualOutput.UserId);
		Assert.Equal(expectedOutput.UserName, actualOutput.UserName);
		Assert.Equal(expectedOutput.FirstName, actualOutput.FirstName);
		Assert.Equal(expectedOutput.LastName, actualOutput.LastName);
		Assert.Equal(expectedOutput.ChatInstance, actualOutput.ChatInstance);
		Assert.Equal(expectedOutput.IsPremium, actualOutput.IsPremium);
	}
	
	[Fact]
	public void TryGetTmaUserTest_Failed()
	{
		//Arrange
		var claimIdentity = A.Fake<ClaimsIdentity>();
		A.CallTo(() => claimIdentity.AuthenticationType).Returns(TmaDefaults.AuthenticationScheme);
		A.CallTo(() => claimIdentity.Claims).Returns(new List<Claim>());
		
		var claimsPrincipal = A.Fake<ClaimsPrincipal>();
		A.CallTo(() => claimsPrincipal.Identities).Returns(new List<ClaimsIdentity>()
		{
			claimIdentity
		});
		
		//Act
		var actualResult = claimsPrincipal.TryGetTmaUser(out var actualOutput);

		//Assert
		Assert.False(actualResult);
		Assert.Null(actualOutput);
	}
	
	public static IEnumerable<object[]> Data =>
		new List<object[]>
		{
			new object[]
			{
				new List<Claim>()
				{
					new (TmaClaimTypes.UserId, "123"),
					new (TmaClaimTypes.UserName, "testUserName"),
					new (TmaClaimTypes.FirstName, "testFirstName"),
					new (TmaClaimTypes.IsPremium, "false"),
				},
				new TmaUserPrincipals()
				{
					UserId = 123,
					UserName = "testUserName",
					FirstName = "testFirstName",
					IsPremium = false,
				}
			}
		};
}