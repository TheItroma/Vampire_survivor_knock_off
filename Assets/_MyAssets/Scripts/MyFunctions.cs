using UnityEngine;

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
}
