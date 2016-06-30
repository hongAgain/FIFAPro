using System;
using System.Text;
using System.Text.RegularExpressions;
public class URLEncoder
{
	private static string TAG = "URLEncoder";
	
	/*!
	 * @Encode string with encoding type
	 * @param {string} str
	 * @param {Encoding} encoding type
	 * @return {string} the value after encode
	 */
	public static string Encode(string str, Encoding e) 
	{
		string s = UrlEncoder(str, e);
		return s;
	}
	
	/*!
	 * @Encode string with default type UTF8.
	 * @param {string} str
	 * @param {Encoding} encoding type
	 * @return {string} the value after encode
	 */
	public static string Encode(string str) 
	{
		
		if (str == null) 
		{
			return str;
		}
		string s = "";
		try 
		{ 
			
			s = UrlEncoder(str, Encoding.UTF8).Replace("+", "%20"); 
			MatchEvaluator evaluator = new MatchEvaluator(ReplaceMe);
    		string pattern = "%(.{2})";
     		s = Regex.Replace(s, pattern, evaluator);
		} 
		catch (Exception e) 
		{
			MLog.e(TAG,"error", e);
		}
		return s;
	}
	
	private static string ReplaceMe(Match m)
	{
     	return m.Value.ToUpper();
 	}
	
	private static string UrlEncoder(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		return Encoding.ASCII.GetString(URLEncoderToBytes(str, e));
	}
	
	private static byte[] URLEncoderToBytes(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		byte[] buffer1 = e.GetBytes(str);
		return URLEncoderBytesToBytesInternal(buffer1, 0, buffer1.Length, false);
	}
	
	private static byte[] URLEncoderBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
	{
		int num1 = 0;
		int num2 = 0;
		for (int num3 = 0; num3 < count; num3++)
		{
			char ch1 = (char)bytes[offset + num3];
			if (ch1 == ' ')
			{
			num1++;
			}
		else if (!IsSafe(ch1))
		{
			num2++;
		}
		}
		if ((!alwaysCreateReturnValue && (num1 == 0)) && (num2 == 0))
		{
			return bytes;
		}
		byte[] buffer1 = new byte[count + (num2 * 2)];
		int num4 = 0;
		for (int num5 = 0; num5 < count; num5++)
		{
			byte num6 = bytes[offset + num5];
			char ch2 = (char)num6;
			if (IsSafe(ch2))
			{
				buffer1[num4++] = num6;
			}
			else if (ch2 == ' ')
			{
				buffer1[num4++] = 0x2b;
			}
			else
			{
				buffer1[num4++] = 0x25;
				buffer1[num4++] = (byte)IntToHex((num6 >> 4) & 15);
				buffer1[num4++] = (byte)IntToHex(num6 & 15);
			}
			}
			return buffer1;
		}
		
		private static char IntToHex(int n)
		{
			if (n <= 9)
			{
			return (char)((ushort)(n + 0x30));
			}
			return (char)((ushort)((n - 10) + 0x61));
		}
		
		internal static bool IsSafe(char ch)
		{
			if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
			{
				return true;
			}
			switch (ch)
			{
				case '\'':
				case '(':
				case ')':
				case '*':
				case '-':
				case '.':
				case '_':
				case '!':
				return true;
			}
			return false;
		}
}


