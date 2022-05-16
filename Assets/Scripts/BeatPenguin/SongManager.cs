using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;


public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError;//seconds

    public int inputDelayInMiliseconds;
    

    public string fileLocation= "";
    public float noteTime;
    public float spawnY, spawnX, spawnZ; //donde spawneaen 3D
    public float hitX; //franja de colision (probablemente no haga falta)
    public float despawnX // zona en z donde devolvemos al pool
    {
        get { return hitX - (spawnX - hitX); }
    }

    public static MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadFromFile();
        
    }
    private void ReadFromFile()
    {
        string midiPath = Application.dataPath + "/Assets/midiAssets/" + fileLocation;
        midiFile = MidiFile.Read(midiPath);
        GetDataFromMidi();
    }

    private void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach(var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong() {
        audioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
