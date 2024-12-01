using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public AudioClip[] audioClips; // Array para armazenar os clipes de �udio
    private AudioSource audioSource;
    private List<AudioSource> audioSources = new List<AudioSource>();

    void Start()
    {
        // Obt�m ou adiciona um AudioSource ao GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Audio Source Atribuido");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // M�todo para tocar um �udio espec�fico pelo �ndice
    public void PlayAudio(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            Debug.Log($"Tocando �udio: {audioClips[clipIndex].name}");
            audioSource.clip = audioClips[clipIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"�ndice do �udio inv�lido! Tamanho do array: {audioClips.Length}, �ndice fornecido: {clipIndex}");
        }
    }

    // Toca o �udio sem substituir o clipe atual
    public void PlayAudioOneShot(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            audioSource.PlayOneShot(audioClips[clipIndex]);
        }
        else
        {
            Debug.LogWarning("�ndice do �udio inv�lido!");
        }
    }

    // M�todo para tocar em loop
    public void PlayAudioLoop(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            audioSource.clip = audioClips[clipIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("�ndice do �udio inv�lido!");
        }
    }

    // M�todo para parar o �udio
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Cria um novo AudioSource para tocar o som
    public void PlayAudioInNewSource(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = audioClips[clipIndex];
            newSource.Play();
            StartCoroutine(RemoveAudioSourceAfterPlay(newSource));
        }
        else
        {
            Debug.LogWarning("�ndice do �udio inv�lido!");
        }
    }

    private IEnumerator RemoveAudioSourceAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(source);
    }

    // Controla o volume
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }

    // Pausa o �udio
    public void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // Retoma o �udio
    public void ResumeAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
