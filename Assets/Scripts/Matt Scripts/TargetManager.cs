using UnityEngine;
using System.Collections.Generic;

public enum TargetManagerInputType
{
	Undefined = -1,
	Up,
	Down,
	Left,
	Right,
}

public class TargetManager : MonoBehaviour {

	public SpriteRenderer upSprite;
	public SpriteRenderer downSprite;
	public SpriteRenderer leftSprite;
	public SpriteRenderer rightSprite;

    public bool isFirstPlayer = true;
    public GameManager gm;

    public float alpha = 0.5f;

    [HideInInspector]
    public float spawnCount;
    [HideInInspector]
    public float caughtCount;
    [HideInInspector]
    public float accuracy;
    
	private List<Note> notes = new List<Note>();

	public void HandleInput(TargetManagerInputType inputType)
	{
		NoteType noteType = GetMappedNoteType(inputType);

		for(int i = 0; i < notes.Count; ++i)
		{
			Note currNote = notes[i];

			if(currNote.Type == noteType)
			{
				caughtCount++;
				UpdateAccuracy ();
				notes.Remove(currNote);
				--i;
				currNote.gameObject.SetActive(false);
			}
		}
	}

    public void OnButtonDown(TargetManagerInputType inputType)
    {
        SpriteRenderer sprite = GetMappedSprite(inputType);

        if(sprite != null)
        {
            Color color = sprite.color;
            color.a = 1f;
            sprite.color = color;
        }
    }

    public void OnButtonUp(TargetManagerInputType inputType)
    {
        SpriteRenderer sprite = GetMappedSprite(inputType);

        if(sprite != null)
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }

	public SpriteRenderer GetMappedSprite(TargetManagerInputType inputType)
	{
		switch(inputType)
		{
		case TargetManagerInputType.Up: return upSprite;
		case TargetManagerInputType.Down: return downSprite;
		case TargetManagerInputType.Left: return leftSprite;
		case TargetManagerInputType.Right: return rightSprite;
		default: return null;
		}
	}
	public NoteType GetMappedNoteType(TargetManagerInputType inputType)
	{
		switch(inputType)
		{
		case TargetManagerInputType.Up: return NoteType.High;
		case TargetManagerInputType.Down: return NoteType.Low;
		case TargetManagerInputType.Left: return NoteType.MidLow;
		case TargetManagerInputType.Right: return NoteType.MidHigh;
		default: return NoteType.Undefined;
		}
	}

	// Use this for initialization
	void Start () {
		spawnCount = 0;
		caughtCount = 0;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Note noteComp = collider.GetComponent<Note>();

		if(noteComp != null)
		{
			++spawnCount;
			notes.Add(noteComp);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		Note noteComp = collider.GetComponent<Note>();

		if(noteComp != null)
		{
			notes.Remove(noteComp);

			UpdateAccuracy ();
		}

        if (iTweenHelper.Instance)
            iTweenHelper.Instance.ShakeObjScale (collider.gameObject, 2, 4, 1);
	}

	public void UpdateAccuracy()
	{
		accuracy = caughtCount / spawnCount;

		if(isFirstPlayer)
		{
			gm.player1acc = accuracy;
		}
		else
		{
			gm.player2acc = accuracy;
		}

		gm.UpdateSacSlider ();
	}
		

	/*
	// Update is called once per frame
	void Update () {
	
		for(int i = 0; i < notes.Count; ++i)
		{
			Collider2D col = notes[i];

			if (col.gameObject.name == Note1Name && Input.GetKeyDown(note1Key)){
				caughtCount++;
				UpdateAccuracy ();
				notes.Remove(col);
				--i;
				Destroy (col.gameObject);
			}
			if (col.gameObject.name == Note2Name && Input.GetKeyDown(note2Key)){
				caughtCount++;
				UpdateAccuracy ();
				notes.Remove(col);
				--i;
				Destroy (col.gameObject);
			}
			if (col.gameObject.name == Note3Name && Input.GetKeyDown(note3Key)){
				caughtCount++;
				UpdateAccuracy ();
				notes.Remove(col);
				--i;
				Destroy (col.gameObject);
			}
			if (col.gameObject.name == Note4Name && Input.GetKeyDown(note4Key)){
				caughtCount++;
				UpdateAccuracy ();
				notes.Remove(col);
				--i;
				Destroy (col.gameObject);
			}
		}

	}
	*/


}
