using Neftm.TelegramMiniApp.Authorization.Helpers;

namespace Neftm.TelegramMiniApp.Authentication.Tests;

public class HashTests
{
	[Theory]
	[InlineData(
		"auth_date=1727941081\nchat_instance=-7780662050766671545\nchat_type=sender\nuser={\"id\":375114286,\"first_name\":\"test\",\"last_name\":\"test\",\"username\":\"test\",\"language_code\":\"ru\",\"allows_write_to_pm\":true}", 
		"7597163774:AAHcGplNPDFQaRZ4VGSGnZYS8BtsicL0Wi4", 
		"d056123e4f348a4d9bf486a1ddffdd70ebb300988cb1e6958e0e087d79d4aee8")]
	public void CalculateHashTest(string dataCheckString, string botToken, string result)
	{
		Assert.Equal(HashHelper.CalculateTmaHash(dataCheckString, botToken), result);
	}
}