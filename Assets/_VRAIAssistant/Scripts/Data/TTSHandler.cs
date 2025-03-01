using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TTSHandler : Singleton<TTSHandler>
{
    public string[] speakerList;
    TTSResponseData ttsResult;
    AudioSource audioSource;
    AudioClip ttsResultClip;
    [SerializeField] TMP_Text ttsText;
    [SerializeField] NPCAI npc;

    private void Update()
    {
        if (Keyboard.current[Key.Y].wasReleasedThisFrame)
        {
            //DoTextToSpeech(StringExtension.RemoveLineEndings(dummy));
        }
    }

    private void Start()
    {
        GetSpeakerList();
        audioSource = FindFirstObjectByType<AudioSource>();
        npc = FindFirstObjectByType<NPCAI>();   
    }

    public void GetSpeakerList()
    {
        APIManager.Instance.GetTTSData("speaker", OnGotSpeakerList);
    }

    public void OnGotSpeakerList(string result)
    {
        speakerList =  JsonConvert.DeserializeObject<string[]>(result);
    }

    public void DoTextToSpeech(string inputText)
    {
        APIManager.Instance.PostDataTTS("tospeech_with_viseme", speakerList[0], inputText, OnGotSpeechData);
        ttsText.text = "Begin Text To Speech";
    }

    private void OnGotSpeechData(string result)
    {
        if (StaticData.requestError)
        {
            GlobalValues.Instance.SetIsAIRunning(false);
            return;
        }
        string text = StringExtension.RemoveLineEndings(result);
        ttsResult = JsonConvert.DeserializeObject<TTSResponseData>(text);
        Debug.Log($"OnGotSpeechData {ttsResult.audio_id}");
        ttsText.text = "Processing Text To Speech . . .";
        GetAudioResult();
    }

    public void GetAudioResult()
    {
        APIManager.Instance.GetTTSAudioResult($"result?audio_id={ttsResult.audio_id}", OnGotSpeechResult);
        ttsText.text = "Fetching Audio . . .";
    }

    private void OnGotSpeechResult(AudioClip clip)
    {
        if (StaticData.requestError)
        {
            GlobalValues.Instance.SetIsAIRunning(false);
            return;
        }
        ttsResultClip = clip;
        if ( audioSource != null ) 
            audioSource.PlayOneShot(ttsResultClip);
        ttsText.text = "Speaking . . ."; 

        if (npc != null)
        {
            npc.StartAnimation(true);
        }
        else
        {
            Debug.Log("NPC Not Found");
        }
    }



    private IEnumerator WaitFoSpeech()
    {
        yield return new WaitForEndOfFrame();   
        while (audioSource)
        {

        }

    }

    /*
    public void CreateAudioClip ()
    {
        AudioClip trimmedClip = AudioClip.Create(audioClip.name + "_Trimmed", trimmedSamples.Count, channels, frequency, false);
        trimmedClip.SetData(trimmedSamples.ToArray(), 0);
        audioClip = trimmedClip; // Replace the old clip with the trimmed clip
        Debug.Log("-> TrimSilence() - " + PrintAudioClipDetail(audioClip));
    }

    public void ConvertToMono()
    {
        int channels = audioClip.channels; // Typically 2 for stereo
        int samples = audioClip.samples;   // Number of samples per channel

        float[] stereoData = new float[samples * channels];
        audioClip.GetData(stereoData, 0);

        float[] monoData = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            float sum = 0f;

            // Sum all the channel values for this sample
            for (int j = 0; j < channels; j++)
            {
                sum += stereoData[i * channels + j];
            }

            // Average the sum to get the mono sample value
            monoData[i] = sum / channels;
        }

        // Create a new AudioClip in mono and set the data
        AudioClip monoClip = AudioClip.Create(audioClip.name + "_Mono", samples, 1, audioClip.frequency, false);
        monoClip.SetData(monoData, 0);
        audioClip = monoClip; // Replace the old clip with the trimmed clip
        Debug.Log("-> ConvertToMono() - " + PrintAudioClipDetail(audioClip));
    }*/
}
