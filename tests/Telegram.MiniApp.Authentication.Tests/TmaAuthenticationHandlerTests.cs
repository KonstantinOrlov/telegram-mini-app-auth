using System.Collections.Specialized;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Neftm.TelegramMiniApp.Authorization;
using FakeItEasy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neftm.TelegramMiniApp.Authorization.Constants;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace Neftm.TelegramMiniApp.Authentication.Tests;

public class TmaAuthenticationHandlerTests
{
	private TmaAuthenticationHandler _handler;

	public TmaAuthenticationHandlerTests()
	{
		var options = A.Fake<IOptionsMonitor<TmaAuthenticationOptions>>();
		A.CallTo(() => options.Get(A<string>.Ignored)).Returns(new TmaAuthenticationOptions()
		{
			BotToken = "1234:test"
		});
		
		var logger = A.Fake<ILoggerFactory>();
		var encoder = A.Fake<UrlEncoder>();
		
		_handler = new TmaAuthenticationHandler(options, logger, encoder);
	}
	
	[Theory]
	[MemberData(nameof(Data))]
	public async Task HandleAuthenticateTest_Successful(string token, List<Claim> claims)
	{
		//Arrange
		var context = new DefaultHttpContext();
		context.HttpContext.Request.Headers.Append("Authorization", token);
		await _handler.InitializeAsync(new AuthenticationScheme(TmaDefaults.AuthenticationScheme, null, typeof(TmaAuthenticationHandler)), context);

		//Act
		var result = await _handler.AuthenticateAsync();
		
		//Assert
		Assert.True(result.Succeeded);
		Assert.NotNull(context.Features.Get<NameValueCollection>());
		Assert.Equal(result.Principal.Claims.Count(), claims.Count());
		Assert.All(result.Principal.Claims, actual =>
		{
			var expected = claims.FirstOrDefault(c => c.Type == actual.Type);
			Assert.NotNull(expected);
			Assert.Equal(expected.Value.ToLowerInvariant(), actual.Value.ToLowerInvariant());
		});
	}
	
	[Theory]
	[InlineData("auth_date=1728978781&hash=85c8e6f88ae131053facd61f402fc1dc85b34c3974cb55bfcf1fc65daa4c7554")]
	public async Task HandleAuthenticateTest_None(string token)
	{
		//Arrange
		var context = new DefaultHttpContext();
		context.HttpContext.Request.Headers.Append("Authorization", token);
		
		await _handler.InitializeAsync(new AuthenticationScheme(TmaDefaults.AuthenticationScheme, null, typeof(TmaAuthenticationHandler)), context);

		//Act
		var result = await _handler.AuthenticateAsync();
		
		//Assert
		Assert.True(result.None);
	}
	
	[Theory]
	[InlineData("tma auth_date=1728978781")] // no hash
	[InlineData("tma hash=85c8e6f88ae131053facd61f402fc1dc85b34c3974cb55bfcf1fc65daa4c7554")] // no auth_data
	[InlineData("tma query_id=AAAa2aAAAAAAAAAbAA9JKMy3auth_date=1728978781&hash=85c8e6f88ae131053facd61f402fc1dc85b34c3974cb55bfcf1fc65daa4c7554")] // wrong hash
	public async Task HandleAuthenticateTest_Failed(string token)
	{
		//Arrange
		var context = new DefaultHttpContext();
		context.HttpContext.Request.Headers.Append("Authorization", token);
		await _handler.InitializeAsync(new AuthenticationScheme(TmaDefaults.AuthenticationScheme, null, typeof(TmaAuthenticationHandler)), context);

		//Act
		var result = await _handler.AuthenticateAsync();

		//Assert
		Assert.NotNull(result.Failure);
	}
	
	public static IEnumerable<object[]> Data =>
		new List<object[]>
		{
			new object[]
			{
				"TMA auth_date=1728978781&hash=85c8e6f88ae131053facd61f402fc1dc85b34c3974cb55bfcf1fc65daa4c7554", 
				new List<Claim>()
			},
			new object[]
			{
				"TMA user=%7B%22id%22%3A111111111%2C%22first_name%22%3A%22testName%22%2C%22last_name%22%3A%22%22%2C%22username%22%3A%22testusername%22%2C%22language_code%22%3A%22ru%22%2C%22is_premium%22%3Atrue%2C%22allows_write_to_pm%22%3Atrue%7D&chat_instance=-8800555353566677788&auth_date=1728978781&hash=b03a48abcf7bbe0dbd941f7e4b1d55a6d52e975448600b2b5559667ff59d372f",
				new List<Claim>()
				{
					new (TmaClaimTypes.UserId, "111111111"),
					new (TmaClaimTypes.FirstName, "testName"),
					new (TmaClaimTypes.IsPremium, "True"),
					new (TmaClaimTypes.UserName, "testusername"),
					new (TmaClaimTypes.ChatInstance, "-8800555353566677788"),
				}
				
			},
			new object[]
			{
				"TMA query_id=AAAa2aAAAAAAAAAbAA9JKMy3&user=%7B%22id%22%3A111111111%2C%22first_name%22%3A%22testName%22%2C%22last_name%22%3A%22%22%2C%22username%22%3A%22testusername%22%2C%22language_code%22%3A%22ru%22%2C%22is_premium%22%3Atrue%2C%22allows_write_to_pm%22%3Atrue%7D&auth_date=1728978781&hash=51df8bfcb486d1028e929a0c7f704ead9875ca333b6dae28038b219fdc18cdde",
				new List<Claim>()
				{
					new (TmaClaimTypes.UserId, "111111111"),
					new (TmaClaimTypes.FirstName, "testName"),
					new (TmaClaimTypes.IsPremium, "True"),
					new (TmaClaimTypes.UserName, "testusername"),
				}
				
			},
		};
}