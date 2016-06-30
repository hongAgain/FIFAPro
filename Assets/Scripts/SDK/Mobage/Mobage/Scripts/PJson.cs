using System;
using System.Text;
using System.Collections.Generic;

public abstract class PJObject
{
	public static implicit operator PJObject(int i)
	{
		return new PJInt (){Value = i};
	}

	public static implicit operator PJObject(string i)
	{
		return new PJString(){Value = i.ToString()};
	}
	/*
	public static implicit operator int(PJObject i)
	{
		return i;
	}
	
	public static implicit operator string(PJObject i)
	{
		return i;
	}
*/
	public abstract String ToJString ();
	

}

public class PJson : PJObject
{
	private Dictionary<string, PJObject> dic = new Dictionary<string, PJObject>();

	public PJson ()
	{


	}

	public PJObject this[string key]
	{
		get
		{
			return dic[key];
		}
		set
		{
			dic [key] = value;
		}
	}

	public bool ContainsKey(string key)
	{
		return this.dic.ContainsKey (key);
	}

	public override string ToString()
	{
		return ToJString ();
	}

	public override string ToJString ()
	{
		string ret = "{";
		bool first = true;
		foreach(KeyValuePair<string, PJObject> kv in this.dic)
		{
			if(first) first = false;
			else ret += ",";
			ret += "\"" + kv.Key.Replace(@"\", @"\\") +"\"";
			ret += ":";
			ret += kv.Value.ToJString();
		}
		ret += "}";
		return ret;
	}
}

public class PJString : PJObject
{
	public string Value { get; set;}
	public PJString()
	{
		this.Value = "";
	}
	public static implicit operator PJString(string str)
	{
		return new PJString {Value = str};
	}

	public static implicit operator string(PJString pjstring)
	{
		return pjstring.Value;
	}

	public override string ToJString ()
	{
		return "\"" + this.Value.Replace(@"\", @"\\") +"\"";
	}

	public override string ToString ()
	{
		return this.Value;
	}

	/*
	public void ParamJString (string jString)
	{
		int BEFOR_CONTEXT = 0;
		int CONTEXT = 1;
		int AFTER_CONTEXT = 2;

		int stutes = BEFOR_CONTEXT;
		char start_quot_type;
		StringBuilder sb = new StringBuilder ();
		for(int i = 0 ; i < jString.Length ; i++)
		{
			char c = jString[i];
			if(stutes == BEFOR_CONTEXT)
			{
				if(c == ' ' || c == '\n') continue;
				if(c == '\"' || c == '\'')
				{
					start_quot_type = c;
					stutes = CONTEXT;
					continue;
				}
				throw new ArgumentException("Parse JString Error, unexpect charector [" + c +"], at index " + i + ", string:" + jString);
			}
			else if(stutes == CONTEXT)
			{
				if(c == '\\')
				{
					i++;
					continue;
				}
				if(c == start_quot_type)
				{
					stutes = AFTER_CONTEXT;
					continue;
				}
				sb.Append(c);
			}
			else if(stutes == AFTER_CONTEXT)
			{
				if(c == ' ' || c == '\n') continue;
				throw new ArgumentException("Parse JString Error, unexpect charector [" + c +"], at index " + i + ", string:" + jString);
			}
		}
		return this.Value = sb.ToString ();
	}
	*/
}

public class PJArray : PJObject
{
	List<PJObject> list = new List<PJObject>();

	public override string ToJString ()
	{
		string ret = "";
		ret += "[";
		bool first = true;
		foreach (PJObject o in list) 
		{
			if(first) first = false;
			else ret += ",";
			ret += o.ToJString();
		}
		ret += "]";
		return ret;
	}

	public override string ToString ()
	{
		return ToJString ();
	}

	public PJObject this[int index]
	{
		get{return this.list[index];}
		set{this.list[index] = value;}
	}

	public int Count()
	{
		return list.Count;
	}

	/*
	public override void ParamJString (string jString)
	{
		int BEFOR_CONTEXT = 0;
		int CONTEXT = 1;
		int AFTER_CONTEXT = 2;

		int status = BEFOR_CONTEXT;

		int BEFOR_OBJECT = 0;
		int AFTER_OBJECT = 1;
		int IN_STRING = 2;
		int IN_BIG_QUOT = 3;
		int s = BEFOR_OBJECT;

		char _in_string_start_mark;

		List<string> list = new List<string> ();
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < jString.Length; i++) 
		{
			char c = jString[i];
			if(status == BEFOR_CONTEXT)
			{
				if(c == ' ' || c == '\n') continue;
				if(c == '[')
				{
					status = CONTEXT;
					continue;
				}
				throw new ArgumentException("Parase PJArrary Error, unexpect charector [" + c + "] at index " + i);
			}
			else if(status == CONTEXT)
			{
				if(s == BEFOR_OBJECT)
				{
					if(c == ' ' || c == '\n') continue;
					if(c == ']')
					{
						status = AFTER_CONTEXT;
						continue;
					}
					if(c == '\'' || c == '\"')
					{
						s = IN_STRING;
						_in_string_start_mark = c;
						sb.Append(c);
						continue;
					}
					if(c == "{")
					{
						s = IN_BIG_QUOT;
						sb.Append(c);
						continue;
					}
				}
				else if(s == IN_STRING)
				{
					sb.Append(c);
					if(c == '\\')
					{
						i++;
						sb.Append(jString[i]);
						continue;
					}
					if(c == _in_string_start_mark)
					{
						s = AFTER_OBJECT;
						list.Add(sb.ToString());
						sb = new StringBuilder();
						continue;
					}
				}
				else if(s == IN_BIG_QUOT)
				{
					int start_quot = 1;
					int end_quot = 0;
					bool success = false;

					bool in_string;
					char in_string_start_mark;
					for (int j = i ; j<jString.Length; j++)
					{
						char c2 = jString[j];
						sb.Append(c2);
						if(c2 == '\'' || c2 == '\"')
						{
							in_string = true;
							in_string_start_mark = c2;
						}
						if(c2 == '}')
						{
							end_quot++;
							if(start_quot == end_quot)
							{
								success = true;
								break;
							}
						}
						if(c2 == "{")
						{
							start_quot ++;
						}
					}
					if(!success)
					{
						throw new ArgumentException("Parse JArray Error, [}] not fount , string = "  + jString );
					}
					list.Add(sb.ToString());
					sb = new StringBuilder();
					s = AFTER_OBJECT;
				}
				else if(s == AFTER_OBJECT)
				{
					if(c == ' ' || c == '\n') continue;
					if(c == ']')
					{
						status = AFTER_CONTEXT;
						continue;
					}
					if(c == ',')
					{
						status = BEFOR_CONTEXT;
						continue;
					}
				}

	
			}
			else if(status == AFTER_CONTEXT)
			{
				if(c == ' ' || c == '\n') continue;
				throw new ArgumentException("Parase PJArrary Error, unexpect charector [" + c + "] at index " + i);
			}
		}

	}
	*/
}

public class PJInt : PJObject
{
	public int Value { get; set;}

	public static implicit operator int(PJInt i)
	{
		return i.Value;
	}

	public static implicit operator PJInt(int i)
	{
		return new PJInt (){Value = i};
	}

	public override string ToJString ()
	{
		return Value.ToString();
	}

	public override string ToString ()
	{
		return ToJString ();
	}

	/*
	public override void ParamJString (string jString)
	{
		this.Value = Int32.Parse (jString);
	}
	*/
}


public class PJBool : PJObject
{
	public bool Value { get; set;}
	
	public static implicit operator bool(PJBool i)
	{
		return i.Value;
	}
	
	public static implicit operator PJBool(bool i)
	{
		return new PJBool (){Value = i};
	}
	
	public override string ToJString ()
	{
		return Value ? "true" : "false";
	}
	
	public override string ToString ()
	{
		return ToJString ();
	}
	
	/*
	public override void ParamJString (string jString)
	{
		this.Value = Int32.Parse (jString);
	}
	*/
}