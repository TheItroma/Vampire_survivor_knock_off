using UnityEngine;
using System.Collections.Generic;

public static class MyFunctions
{
    public static float GetRandomizedByPercentage(float p_normal, float p_percentage)
    {
	float offset = p_normal * p_percentage;
	return Random.Range(p_normal - offset, p_normal + offset);
    }


    public static Vector2 MakeVectorUsingAngle(float p_norm, float p_angle)
    {
	return new Vector2((Mathf.Cos(p_angle) * p_norm), (Mathf.Sin(p_angle) * p_norm));
    }


    public static List<GameObject> GetDrops(List<GameObject> p_drops, List<float> p_percentages, int p_dropAmount)
    {
	if (p_drops.Count != p_percentages.Count)
	{
	    Debug.Log("Table de loot pas bonne");
	}

	float RandVal = Random.value;

	List<GameObject> Possible = new List<GameObject>();
	for (int i = 0; i < p_percentages.Count; i++)
	{
	    if (p_percentages[i] >= RandVal)
	    {
		Possible.Add(p_drops[i]);
	    }
	}

	List<GameObject> Returned = new List<GameObject>();
	for (int i = 0; i < p_dropAmount; i++)
	{
	    Returned.Add(Possible[Random.Range(0, Possible.Count)]);
	}
	return Returned;
    }
}
