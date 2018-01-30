using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public static class Extensions
{
	#region GameObject Extensions

	public static bool HasComponent <T>(this GameObject go) where T : Component
	{
		if (go != null)
		{
			return go.GetComponent<T>() != null;
		}
		return false;
	}

	public static T[] GetComponentsFromArray<T>(this GameObject[] gos) where T : Component
	{
		T[] result = new T[gos.Length];
		for(int i = 0; i < gos.Length; i++)
		{
			result[i] = gos[i].GetComponent<T>();
			if(result[i] == null)
			{
				gos[i].AddComponent<T>();
			}
		}
		return result;
	}

	#endregion

	#region Int

	public static int SetIfDifferent(this int curentValue, int newValue)
	{
		if(curentValue != newValue)
		{
			return newValue;
		}

		return curentValue;
	}

	#endregion

	#region Float

	/// <summary>
	/// Rounds a float to the specified number of decimal places
	/// </summary>
	/// <param name="digits">The amount of decimal places you want to round to</param>
	public static float RoundToDP(this float value, int digits)
	{
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}

	#endregion

	#region Time Extensions

	public static string FloatToTimeInMMSS(this float time)
	{
		int minutes = (int)time / 60;
		int seconds = (int)time % 60;

		string minSec = string.Format("{0:00}:{1:00}", minutes, seconds);
		return minSec;
	}

	public static string FloatToTimeInMMSSMS(this float time)
	{
		int minutes = (int)time / 60;
		int seconds = (int)time % 60;
		float miliseconds = time * 100;
		miliseconds = miliseconds % 100;

		string minSec = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);
		return minSec;
	}

	public static string FloatToTimeInSSMS(this float time)
	{
		int seconds = (int)time % 60;
		float miliseconds = time * 100;
		miliseconds = miliseconds % 100;

		string minSec = string.Format("{0:00}:{1:00}", seconds, miliseconds);
		return minSec;
	}

	#endregion

	#region Rect Transforms

	public static RectTransform Resize(this RectTransform transform, float percentage, float arcSize)
	{
		RectTransform parent = transform.parent as RectTransform;
		float arcPercentage = arcSize / parent.rect.width;
		transform.sizeDelta = new Vector2(parent.rect.width * Mathf.Min(percentage, arcPercentage), parent.rect.height * Mathf.Min(percentage, arcPercentage));
		return transform;
	}

	public static RectTransform AnchorsToCorners(this RectTransform transform)
	{
		RectTransform t = transform as RectTransform;
		RectTransform pt = t.parent as RectTransform;

		if (t == null || pt == null) return transform;

		Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
											t.anchorMin.y + t.offsetMin.y / pt.rect.height);
		Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
											t.anchorMax.y + t.offsetMax.y / pt.rect.height);

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2(0, 0);
		return t;
	}

	public static RectTransform CornersToAnchors(this RectTransform transform)
	{
		RectTransform t = transform as RectTransform;

		t.offsetMin = t.offsetMax = new Vector2(0, 0);

		return t;
	}

	public static Vector2 ViewportToWorldSpaceUI(this RectTransform transform, Vector3 pos, Vector3 dir)
	{
		Vector2 result = Vector2.zero;

		Vector3 localPos = transform.InverseTransformPoint(pos);
		Vector3 localDir = transform.InverseTransformDirection(dir);

		result = (Vector2)localPos + ((Vector2)localDir * (Mathf.Abs(localPos.z) / localDir.z));
		
		return result;
	}

	#endregion

	#region String
	public static string VarNameToScreenName(this string varName)
	{
		return Regex.Replace(varName, @"(\B[A-Z])", " $1").TrimStart("m_ ".ToCharArray());
	}

	public static string RemoveSpaces(this string screenName)
	{
		string noSpaces = screenName.Replace(" ", "");
		return noSpaces;
	}

	public static string SpaceBeforeCapitals(this string value)
	{
		return Regex.Replace(value, "([a-z])([A-Z])", "$1 $2");
	}

	public static string EnumToString(this string value)
	{
		string spaced = Regex.Replace(value, "_", " ");
		return Regex.Replace(spaced, @"(\B[A-Z]\w+)\b", m=>m.ToString().ToLower());
	}


	public static string RemoveNameSpaces(this string className)
	{
		int lastPeriod = className.LastIndexOf('.');
		return className.Remove(0, lastPeriod + 1);
	}
	#endregion

	#region Text

	public static int SmallestFontSize(this Text[] values)
	{
		int smallestFont = -1;

		int sizeOfValues = values.Length;

		for(int i = 0; i < sizeOfValues; i++)
		{
			if(smallestFont == -1)
			{
				smallestFont = values[i].fontSize;
			}
			else
			{
				if(smallestFont > values[i].fontSize)
				{
					smallestFont = values[i].fontSize;
				}
			}
		}

		return smallestFont;
	}

	public static void SetFontSizes(this Text[] values, int fontSize)
	{
		int sizeOfValues = values.Length;

		for (int i = 0; i < sizeOfValues; i++)
		{
			values[i].resizeTextForBestFit = false;
			values[i].fontSize = fontSize;
		}
	}

	#endregion

	#region Bool

	public static bool In<T>(this T obj, params T[] args)
	{
		return args.Contains(obj);
	}

	#endregion

	#region List

	#endregion
}
