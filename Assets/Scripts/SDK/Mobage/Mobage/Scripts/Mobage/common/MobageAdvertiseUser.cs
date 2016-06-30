using UnityEngine;
using System.Collections;

public class MobageAdvertiseUser {

	public string accessToken {get;set;}
	public string userId {get;set;}
	public string birthday{get;set;}
	public string name {get;set;}
	public string username {get;set;}
	public string img {get;set;}
	public string type {get;set;}
	public long expiationData {get;set;}

	public override string ToString ()
	{
		return string.Format ("[MobageAdvertiseUser: accessToken={0}, userId={1}, birthday={2}, name={3}, username={4}, img={5}, type={6}, expiationData={7}]", accessToken, userId, birthday, name, username, img, type, expiationData);
	}
}
