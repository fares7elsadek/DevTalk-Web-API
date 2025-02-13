
using System.Globalization;
using System.Text;

namespace DevTalk.Application.Posts;

public static class DateTimeCursorOperations
{
    public static DateTime? Decode(string encodedDate)
    {
        if (string.IsNullOrEmpty(encodedDate))
            return null;

        try
        {
            byte[] bytes = Convert.FromBase64String(encodedDate);
            string dateTimeString = Encoding.UTF8.GetString(bytes);
            if (DateTime.TryParse(dateTimeString, null, DateTimeStyles.RoundtripKind, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    public static string Encode(DateTime dateTime)
    {
        string dateTimeString = dateTime.ToString("o");
        byte[] bytes = Encoding.UTF8.GetBytes(dateTimeString);
        return Convert.ToBase64String(bytes);
    }
}
