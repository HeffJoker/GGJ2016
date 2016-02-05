//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;


namespace AssemblyCSharp
{
	public static class MathUtil
	{
		public static Vector3 AngleToVector(float angle)
		{
			angle *= Mathf.Deg2Rad;
			return new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		/*public static Vector2 AngleToVector(float angle)
		{
			angle *= Mathf.Deg2Rad;
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}*/

		public static float VectorToAngle(Vector2 vector)
		{
			return (float)Math.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		}

		public static float VectorToAngle(Vector3 vector)
		{
			return (float)Math.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		}
	}
}
