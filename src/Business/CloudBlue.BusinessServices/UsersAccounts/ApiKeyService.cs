using System.Security.Cryptography;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.UsersAccounts;

public class ApiKeyService : IApiKeyService
{
	private const string Prefix = "Mimo-";
	private const int NumberOfSecureBytesToGenerate = 64;
	private const int LengthOfKey = 64;

	public string GenerateApiKey()
	{
		var bytes = RandomNumberGenerator.GetBytes(NumberOfSecureBytesToGenerate);

		var base64String = Convert.ToBase64String(bytes)
			.Replace("+", "-")
			.Replace("/", "_");

		var keyLength = LengthOfKey - Prefix.Length;

		return Prefix + base64String[..keyLength];
	}
}