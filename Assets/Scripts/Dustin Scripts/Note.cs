using UnityEngine;
using System.Collections;
using System;

public enum NoteType
{
	Undefined = -1,
	Low,
	MidLow,
	MidHigh,
	High,
}

public enum AnalyzeType
{
	Undefined = -1,
	Rank,
	Strength,
}

public class Note : MonoBehaviour 
{
	public NoteType Type = NoteType.Undefined;
	public AnalyzeType AnalyzeType = AnalyzeType.Undefined;

	public float strength = 0.1f;
	public float strengthDeviation = 0.1f;
	public int targetRank = 1;
	public int rankDeviation = 1;

	public int frameIndex = 0;

    [HideInInspector]
    public bool isMoving = false;
}
