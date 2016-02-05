using UnityEngine;
using System.Collections.Generic;

public class NoteCreator : MonoBehaviour {

	public float YSpacingPerFrame = 1f;
	public Note LowNotePrefab;
	public Note HighNotePrefab;
	public Note MidHighNotePrefab;
	public Note MidLowNotePrefab;

	//[Tooltip("To use note rank, otherwise will use weight/strength.")]
	//public bool UseNoteRank = true;

	//public bool UseStrength = false;

	public RhythmTool Analyzer;
	public AudioClip Song;


	private List<Note> noteList = new List<Note>();
	private int lastFrame = 0;

	// Use this for initialization
	void Start () {
		BuildNotes();
	}

	private void BuildNotes()
	{
		Analyzer.NewSong(Song);
		Analysis low = Analyzer.Low;
		Analysis high = Analyzer.High;
		int totFrames = Analyzer.TotalFrames;

		noteList.AddRange(BuildNotesFromPrefab(LowNotePrefab));
		noteList.AddRange(BuildNotesFromPrefab(HighNotePrefab));
		noteList.AddRange(BuildNotesFromPrefab(MidHighNotePrefab));
		noteList.AddRange(BuildNotesFromPrefab(MidLowNotePrefab));


		Analyzer.Play();
	}

	private List<Note> BuildNotesFromPrefab(Note notePrefab)
	{
		List<Note> retList = new List<Note>();
		Analysis soundData = null;

		if(notePrefab.Type == NoteType.Low)
			soundData = Analyzer.Low;
		else if(notePrefab.Type == NoteType.MidHigh || notePrefab.Type == NoteType.MidLow)
			soundData = Analyzer.Mid;
		else if(notePrefab.Type == NoteType.High)
			soundData = Analyzer.High;

		if(soundData != null)
		{
			for(int i = 0; i < soundData.onsets.Count; ++i)
			{
				Onset currOnset = soundData.onsets[i];
				int frameIndex = currOnset.index;

				//if(Mathf.Abs(currOnset.strength - notePrefab.weight) <= notePrefab.deviation)
				//{
				if(notePrefab.AnalyzeType == AnalyzeType.Rank && Mathf.Abs(notePrefab.targetRank - currOnset.rank) <= notePrefab.rankDeviation)
					continue;

				if(notePrefab.AnalyzeType == AnalyzeType.Strength && Mathf.Abs(currOnset.strength - notePrefab.strength) <= notePrefab.rankDeviation)
					continue;
				
					Note newNote = Instantiate(notePrefab) as Note;
				newNote.frameIndex = frameIndex;
					newNote.transform.position = new Vector3(notePrefab.transform.position.x, (frameIndex) * YSpacingPerFrame, 0);
					retList.Add(newNote);
				//}
			}
		}

		return retList;
	}

	void Update()
	{
		int currentFrame = Analyzer.CurrentFrame;
		noteList.ForEach(x => x.transform.position = new Vector3(x.transform.position.x, x.transform.position.y - (currentFrame - lastFrame)));
		// Vector3.Lerp(x.transform.position, new Vector3(x.transform.position.x, x.transform.position.y - (currentFrame - lastFrame)), Analyzer.Interpolation)); 

		if(currentFrame > lastFrame)
			lastFrame = currentFrame;
		/*
		for(int i = 0; i < 100; ++i)
		{
			int frameIndex = Mathf.Min(i+currentFrame, Analyzer.TotalFrames - 1);

			float y = currentFrame;//i - Analyzer.Interpolation;


		}*/
	}
}
