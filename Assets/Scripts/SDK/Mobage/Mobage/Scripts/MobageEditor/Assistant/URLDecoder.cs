using System;
using System.Text;
public class URLDecoder
{
	/*!
	 * @Decode string with default type UTF8
	 * @param {string} str
	 * @return {string}  the value after decode
	 */
	public static string Decode(string str)
	{
		if(str == null)
			return null;
		else return Decode(str, Encoding.UTF8);
	}
	
	/*!
	 * @Decode string with encoding type
	 * @param {string} str
	 * @param {Encoding} encoding type
	 * @return {string} the value after decode
	 */
	public static string Decode(string str, Encoding e)
	{
		if(str == null)
			return null;
		return UrlDecodeStringFromStringInternal(str, e);
	}
	
	private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
	{
	    int length = s.Length;
	    UrlDecoder decoder = new UrlDecoder(length, e);
	    for (int i = 0; i < length; i++)
	    {
	        char ch = s[i];
	        if (ch == '+')
	        {
	            ch = ' ';
	        }
	        else if ((ch == '%') && (i < (length - 2)))
	        {
	            if ((s[i + 1] == 'u') && (i < (length - 5)))
	            {
	                int num3 = HexToInt(s[i + 2]);
	                int num4 = HexToInt(s[i + 3]);
	                int num5 = HexToInt(s[i + 4]);
	                int num6 = HexToInt(s[i + 5]);
	                if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
	                {
	                    goto Label_0106;
	                }
	                ch = (char) ((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
	                i += 5;
	                decoder.AddChar(ch);
	                continue;
	            }
	            int num7 = HexToInt(s[i + 1]);
	            int num8 = HexToInt(s[i + 2]);
	            if ((num7 >= 0) && (num8 >= 0))
	            {
	                byte b = (byte) ((num7 << 4) | num8);
	                i += 2;
	                decoder.AddByte(b);
	                continue;
	            }
	        }
Label_0106:
	        if ((ch & 0xff80) == 0)
	        {
	            decoder.AddByte((byte) ch);
	        }
	        else
	        {
	            decoder.AddChar(ch);
	        }
	    }
	    return decoder.GetString();
	}
	 
	private static int HexToInt(char h)
	{
	    if ((h >= '0') && (h <= '9'))
	    {
	        return (h - '0');
	    }
	    if ((h >= 'a') && (h <= 'f'))
	    {
	        return ((h - 'a') + 10);
	    }
	    if ((h >= 'A') && (h <= 'F'))
	    {
	        return ((h - 'A') + 10);
	    }
	    return -1;
	}
	
	private class UrlDecoder
	{
	    // Fields
	    private int _bufferSize;
	    private byte[] _byteBuffer;
	    private char[] _charBuffer;
	    private Encoding _encoding;
	    private int _numBytes;
	    private int _numChars;
	
	    // Methods
		public  UrlDecoder(int bufferSize, Encoding encoding)
		{
		    this._bufferSize = bufferSize;
		    this._encoding = encoding;
		    this._charBuffer = new char[bufferSize];
		}
		internal void AddChar(char ch)
		{
		    if (this._numBytes > 0)
		    {
		        this.FlushBytes();
		    }
		    this._charBuffer[this._numChars++] = ch;
		}
		
		internal void AddByte(byte b)
		{
		    if (this._byteBuffer == null)
		    {
		        this._byteBuffer = new byte[this._bufferSize];
		    }
		    this._byteBuffer[this._numBytes++] = b;
		}
		
		 private void FlushBytes()
		 {
		     if (this._numBytes > 0)
		     {
		         this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
		         this._numBytes = 0;
		     }
		 }
		
		 internal string GetString()
		 {
		     if (this._numBytes > 0)
		     {
		         this.FlushBytes();
		     }
		     if (this._numChars > 0)
		     {
		         return new string(this._charBuffer, 0, this._numChars);
		     }
		     return string.Empty;
		 }
	}
}




