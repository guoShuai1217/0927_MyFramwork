/*
 *		   Description:  
 *
 *		   CreatedBy:  guoShuai
 *
 *		   DataTime:  2019.05
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class PathUtil  
{

	public static string GetAssetBundlePath()
	{
		string outPath = GetPlatformPath() + "/" + GetPlatformName();
		if (!Directory.Exists(outPath))
			Directory.CreateDirectory(outPath);

		return outPath;
	}

	public static string GetPlatformName()
	{
		switch (Application.platform)
		{
			 
			case RuntimePlatform.WindowsPlayer:				 
			case RuntimePlatform.WindowsEditor:
				return "Windows";
			 		 
			case RuntimePlatform.Android:
				return "Android";

			case RuntimePlatform.IPhonePlayer:
				return "IOS";

			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXEditor:
				return "OSX";
	 
			default:
				return null;
			 
		}
	}
	 

	public static string GetPlatformPath()
	{
		switch (Application.platform)
		{

			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				return Application.streamingAssetsPath;
		 
			case RuntimePlatform.Android:
				return Application.persistentDataPath;
 
			default:
				return null;
		}
	}
	 

	public static string GetWWWPath()
	{
		switch (Application.platform)
		{

			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				return "file://" + GetAssetBundlePath();
			 
			case RuntimePlatform.Android:
				return "jar:file//" + GetAssetBundlePath();
		 
			default:
				return null;
		}
	}
	 
}
