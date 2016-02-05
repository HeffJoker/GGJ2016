using UnityEngine;
using System.Collections.Generic;
using InControl;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ManualNoteCreator : MonoBehaviour {

	public Note LowNotePrefab;
	public Note HighNotePrefab;
	public Note MidHighNotePrefab;
	public Note MidLowNotePrefab;
	public SpriteRenderer debugSprite;

	public RhythmTool Analyzer;
	public AudioClip Song;
	public InputDevice device;

	public float PixelsPerFrame = 1.0f;
	public float NoteSize = 0.25f;

	private List<ManualNote> noteList = new List<ManualNote>();


	void Start()
	{
		Analyzer.NewSong(Song);
		Analyzer.Play();
	}

	// Update is called once per frame
	void Update () {
		int currSongFrame = Analyzer.CurrentFrame;
		device = InputManager.ActiveDevice;

		if(currSongFrame < Analyzer.TotalFrames)
		{
			Note prefab = null;

			if(device.LeftTrigger.WasPressed)
			{
				prefab = LowNotePrefab;
			}

			if(device.LeftBumper.WasPressed)
			{
				prefab = MidLowNotePrefab;
			}

			if(device.RightBumper.WasPressed)
			{
				prefab = MidHighNotePrefab;
			}

			if(device.RightTrigger.WasPressed)
			{
				prefab = HighNotePrefab;
			}

			if(prefab != null)
			{
				ManualNote note = CreateNoteFromPrefab(prefab);
				noteList.Add(note);

				Vector3 position = new Vector3(prefab.transform.position.x, PixelsPerFrame * note.Frame / Analyzer.TotalFrames);
				Note newNote = Instantiate(prefab, position, Quaternion.identity) as Note;
				newNote.transform.localScale = new Vector3(NoteSize, NoteSize, 0);
			}
		}
	}

	private ManualNote CreateNoteFromPrefab(Note prefab)
	{
		ManualNote retNote = new ManualNote();
		retNote.Frame = Analyzer.CurrentFrame; 
		retNote.Type = prefab.Type;

		return retNote;
	}

	void OnDestroy()
	{
		SaveNotes();
	}

	private void SaveNotes()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath + "/SongData/" + Song.name + ".dat");
		bf.Serialize(file, noteList);
		file.Close();
	}
}

