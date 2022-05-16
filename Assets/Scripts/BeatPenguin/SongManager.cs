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
    public float songDelayInSeconds;
    public double marginOfError;//seconds

    public int inputDelayInMiliseconds;
    

    public string fileLocation= "";
    public float noteTime;
    public float spawnY, spawnX, spawnZ; //donde spawneaen 3D
    public float hitZ; //franja de colision (probablemente no haga falta)
    public float despawnZ // zona en z donde devolvemos al pool
    {
        get { return hitZ - (spawnZ - hitZ); }
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
