using Newtonsoft.Json;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudBlue.Utilities;

public static class UtilityFunctions
{
    public static bool CompareObjects<T>(T leftModel, T rightModel)
    {
        var properties = TypeDescriptor.GetProperties(typeof(T));

        foreach (PropertyDescriptor property in properties)
        {
            try
            {
                var leftValue = property.GetValue(leftModel);
                var rightValue = property.GetValue(rightModel);

                if (leftValue == null || rightValue == null || string.IsNullOrEmpty(leftValue.ToString()) ||
                   string.IsNullOrEmpty(rightValue.ToString()) ||
                   leftValue.ToString()!.Equals(rightValue.ToString(), StringComparison.InvariantCulture) == false)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    public static string ConstructMessage(List<string> messages)
    {
        var message = string.Empty;

        if (messages.Count == 0)
        {
            return message;
        }

        var regex = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        var newMessages = new string[messages.Count];
        var idx = 0;

        foreach (var str in messages)
        {
            newMessages[idx++] = regex.Replace(str, " ");
        }

        message = string.Join("; ", newMessages)
            .Trim()
            .ToLower();

        message = $"{message.Substring(0, 1).ToUpper()}{message.Substring(1)}";

        return message;
    }

    public static T? DeserializeJson<T>(string json)
    {
        try
        {
            var obj = JsonConvert.DeserializeObject<T>(json);

            return obj!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }

    public static string FixApiPath(string path)
    {
        var arr = path.Split('/');

        for (var i = 0; i < arr.Length; i++)
        {
            if (long.TryParse(arr[i]
                   .Replace("_", ""), out _))
            {
                arr[i] = "{id}";
            }
        }

        return string.Join("/", arr);
    }

    public static string GenerateSalt()
    {
        var guid = Guid.NewGuid()
            .ToString()
            .Substring(0, 12);

        return GetBase64(guid);
    }

    public static string GetCompositeId(long? defaultItemId, long? itemId)
    {
        defaultItemId ??= 0;
        itemId ??= 0;

        return $"{defaultItemId}_{itemId}";
    }

    public static string HashPassword(string password, string salt)
    {
        var bytes = Encoding.Unicode.GetBytes(password);
        var src = Convert.FromBase64String(salt);
        var dst = new byte[src.Length + bytes.Length];
        Buffer.BlockCopy(src, 0, dst, 0, src.Length);
        Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
        var algorithm = SHA1.Create();
        var inArray = algorithm.ComputeHash(dst);

        return Convert.ToBase64String(inArray);
    }

    public static string RandomPassPhrase(string email, int length = 22)
    {
        var r = new Random();
        var allowedChars = $"{email.Substring(0, email.IndexOf("@", StringComparison.Ordinal))}0123456789!@$?_-";

        return new string(Enumerable.Repeat(allowedChars, length)
            .Select(s => s[r.Next(s.Length)])
            .ToArray());
    }

    public static string SerializeJson<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);

        return json;
    }

    private static string GetBase64(string value)
    {
        var base64EncryptedStr = Convert.ToBase64String(Encoding.Unicode.GetBytes(value));

        return base64EncryptedStr;
    }

    public static string? PopulateCompositePhone(string? countryCode, string? areaCode, string? phone)
    {
        if (string.IsNullOrEmpty(countryCode) || string.IsNullOrEmpty(areaCode) || string.IsNullOrEmpty(phone))
        {
            return null;
        }

        return $"{countryCode}{areaCode.TrimStart('0')}{phone}";
    }

    public static string? GetCompositePhoneForSearch(string? countryCode, string? areaCode, string? phone)
    {
        if (string.IsNullOrEmpty(countryCode) || string.IsNullOrEmpty(areaCode) || string.IsNullOrEmpty(phone))
        {
            return null;
        }

        return $"({countryCode}) {areaCode} {phone}";
    }

    public static DateTime? ConvertToUtc(DateTime? date)
    {
        if (date == null)
        {
            return null;
        }

        return DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
    }

    public static int GetIntegerFromDate(DateTime? date)
    {
        if (date == null)
        {
            return 0;
        }

        return int.Parse(date.Value.ToString("yyyyMMdd"));
    }

    public static long GetLongFromDate(DateTime? date)
    {
        if (date == null)
        {
            return 0;
        }

        return long.Parse(date.Value.ToString("yyyyMMddHHmmss"));
    }

    public static DateTime? GetDateFromNumeric(long dateNumeric)
    {
        var dateStr = dateNumeric.ToString();

        if (dateStr.Length < 8)
        {
            return null;
        }

        var year = int.Parse(dateStr.Substring(0, 4));
        var month = int.Parse(dateStr.Substring(4, 2));
        var day = int.Parse(dateStr.Substring(6, 2));
        var hour = 0;
        var minute = 0;
        var second = 0;

        if (dateStr.Length == 14)
        {
            hour = int.Parse(dateStr.Substring(8, 2));
            minute = int.Parse(dateStr.Substring(10, 2));
            second = int.Parse(dateStr.Substring(12, 2));
        }

        return new DateTime(year, month, day, hour, minute, second);
    }
}