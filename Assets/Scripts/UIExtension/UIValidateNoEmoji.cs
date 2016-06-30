using UnityEngine;
using System.Text.RegularExpressions;

public class UIValidateNoEmoji : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	    UIInput input = GetComponent<UIInput>();
	    input.onValidate = Validate;
	}
	
    public static char Validate(string text, int length, char c)
    {
        Regex charactor = new Regex("[\u4e00-\u9fa5]");
        if (charactor.IsMatch(c.ToString()))
        {
            return c;
        }
        else
        {
            if ((int)c >= 0 && (int)c <= 255)
            {
                return c;
            }
            else
            {
                return (char)0;
            }
        }
    }
}
