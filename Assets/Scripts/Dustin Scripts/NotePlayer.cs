using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class NotePlayer : MonoBehaviour {

	public Song songPrefab = null;
	public Note LowNotePrefab;
	public Note HighNotePrefab;
	public Note MidHighNotePrefab;
	public Note MidLowNotePrefab;

	public RhythmTool Analyzer;

	public float YSpacingPerFrame = 1f;

    public float TimeTilShow = 0.25f;
    public float TimeToMove = 0.33f; 

    public float NoteYStart = 1f;
    public float NoteYEnd = -1f;
    public float NoteSpeed = 1f;
    public int FrameDistance = 10;

    public Transform NoteOrigin;

	private int lastFrame = 0;

	private List<Note> noteList = new List<Note>();
    private Vector3 lowPos;
    private Vector3 lowMidPos;
    private Vector3 lowHighPos;
    private Vector3 highPos;

	// Use this for initialization
	void Start () {
		LoadNotes();
		Analyzer.NewSong(songPrefab.Clip);
		Analyzer.Play();

        lowPos = transform.position + LowNotePrefab.transform.localPosition;
        lowMidPos = transform.position + MidLowNotePrefab.transform.localPosition;
        lowHighPos = transform.position + MidHighNotePrefab.transform.localPosition;
        highPos = transform.position + HighNotePrefab.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update()
	{
		int currentFrame = Analyzer.CurrentFrame;

        MoveNotes(currentFrame);
        /*
        noteList.ForEach(x =>
        {
            x.transform.position += new Vector3(0, currentFrame * Analyzer.Interpolation); // new Vector3(x.transform.position.x, x.transform.position.y - (currentFrame - lastFrame))
        });
        */

		if(currentFrame > lastFrame)
			lastFrame = currentFrame;
	}

    private void MoveNotes(int currentFrame)
    {
        for (int i = 0; i < noteList.Count; ++i)
        {
            Note note = noteList[i];

            if (note.isMoving)
                continue;

            //int frameOffset = note.frameIndex - currentFrame;
            float currSec = Analyzer.TimeSeconds(currentFrame);
            float noteSec = Analyzer.TimeSeconds(note.frameIndex);
            float totalSec = Analyzer.TimeSeconds(Analyzer.TotalFrames - 1);

            float normTime = (currSec / noteSec);
            
            if(1 - normTime <= TimeToMove)
            {
                note.isMoving = true;
                StartCoroutine(MoveToStart(note));
            }

            //float seconds = Analyzer.TimeSeconds(note.frameIndex);
            
            /*
            if (frameOffset <= FrameDistance && frameOffset > 0)
            {              

                float totalTime = ((float)note.frameIndex / (float)Analyzer.TotalFrames);
                float localTime = ((float)currentFrame / (float)Analyzer.TotalFrames);


                float normTime = currSec / seconds; // localTime / totalTime;


            }*/
        }
    }

    IEnumerator MoveToStart(Note note)
    {
        float currMusicTime = Analyzer.TimeSeconds();
        float targetTime = (1f - TimeTilShow) * Analyzer.TimeSeconds(note.frameIndex);
        float normtarget = targetTime - currMusicTime;

        List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        sprites.AddRange(note.GetComponents<SpriteRenderer>());
        sprites.AddRange(note.GetComponentsInChildren<SpriteRenderer>());

        for(int i = 0; i < sprites.Count; ++i)
        {
            sprites[i].enabled = true;
        }
               
        if (NoteOrigin != null)
            note.transform.position = NoteOrigin.position;

        Vector3 startPos = note.transform.position;
        Vector3 endPos = GetNotePos(note.Type) + new Vector3(0, NoteYStart);

        Vector3 startSize = Vector3.zero;
        Vector3 endSize = note.transform.localScale;

        float currtime = 0;

        while(currtime <= 1)
        {
            note.transform.position = Vector3.Lerp(startPos, endPos, currtime);
            note.transform.localScale = Vector3.Lerp(startSize, endSize, currtime);

            float normCurrTime = Analyzer.TimeSeconds() - currMusicTime;
            currtime = normCurrTime / normtarget;

            yield return null;
        }

        yield return DoMove(note);
    }

    IEnumerator DoMove(Note note)
    {
        float currMusicTime = Analyzer.TimeSeconds();
        float targetTime = Analyzer.TimeSeconds(note.frameIndex);
        float normTarget = targetTime - currMusicTime;
        
        float currTime = 0;

        while (currTime <= 1)
        {
            Vector3 startPos = GetNotePos(note.Type) + new Vector3(0, NoteYStart);
            Vector3 endPos = GetNotePos(note.Type) + new Vector3(0, NoteYEnd);
            note.transform.position = Vector3.Lerp(startPos, endPos, currTime);

            float normCurrTime = Analyzer.TimeSeconds() - currMusicTime;
            currTime = normCurrTime / normTarget;

            yield return null;
        }
    }

    private Vector3 GetNotePos(NoteType noteType)
    {
        switch(noteType)
        {
            case NoteType.Low: return lowPos;
            case NoteType.MidLow: return lowMidPos;
            case NoteType.MidHigh: return lowHighPos;
            case NoteType.High: return highPos;
            default: return new Vector3();
        }
    }

	private void LoadNotes()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.OpenRead(Application.dataPath + "/SongData/" + songPrefab.DataFile);

		List<ManualNote> loadedNotes = bf.Deserialize(file) as List<ManualNote>;

		file.Close();

		BuildNotes(loadedNotes);
	}

	private void BuildNotes(List<ManualNote> loadedNotes)
	{
		foreach(ManualNote note in loadedNotes)
		{
			Note prefab = null;

			if(note.Type == NoteType.High)
				prefab = HighNotePrefab;
			else if(note.Type == NoteType.MidHigh)
				prefab = MidHighNotePrefab;
			else if(note.Type == NoteType.MidLow)
				prefab = MidLowNotePrefab;
			else if(note.Type == NoteType.Low)
				prefab = LowNotePrefab;

			if(prefab == null)
				continue;

			Note newNote = Instantiate(prefab) as Note;

			newNote.transform.parent = transform;
            newNote.transform.localPosition += transform.position + new Vector3(0, NoteYStart); // new Vector3(transform.position.x, YSpacingPerFrame * note.Frame);
            newNote.frameIndex = note.Frame;

			noteList.Add(newNote);
		}
	}

    void OnDrawGizmos()
    {
        lowPos = transform.position + LowNotePrefab.transform.localPosition;
        lowMidPos = transform.position + MidLowNotePrefab.transform.localPosition;
        lowHighPos = transform.position + MidHighNotePrefab.transform.localPosition;
        highPos = transform.position + HighNotePrefab.transform.localPosition;

        float radius = 0.5f;

        Gizmos.color = Color.green;
        Vector3 startOffset = new Vector3(0, NoteYStart);
        
        Gizmos.DrawWireSphere(lowPos + startOffset, radius);
        Gizmos.DrawWireSphere(lowMidPos + startOffset, radius);
        Gizmos.DrawWireSphere(lowHighPos + startOffset, radius);
        Gizmos.DrawWireSphere(highPos + startOffset, radius);

        Gizmos.color = Color.red;
        Vector3 endOffset = new Vector3(0, NoteYEnd);

        Gizmos.DrawWireSphere(lowPos + endOffset, radius);
        Gizmos.DrawWireSphere(lowMidPos + endOffset, radius);
        Gizmos.DrawWireSphere(lowHighPos + endOffset, radius);
        Gizmos.DrawWireSphere(highPos + endOffset, radius);
    }
}
