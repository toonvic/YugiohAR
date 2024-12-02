using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public AudioClip[] audioClips; // Array para armazenar os clipes de áudio
    private AudioSource audioSource;
    private List<AudioSource> audioSources = new List<AudioSource>();

    void Start()
    {
        // Obtém ou adiciona um AudioSource ao GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Audio Source Atribuido");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Método para tocar um áudio específico pelo índice
    public void PlayAudio(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            Debug.Log($"Tocando áudio: {audioClips[clipIndex].name}");
            audioSource.clip = audioClips[clipIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Índice do áudio inválido! Tamanho do array: {audioClips.Length}, Índice fornecido: {clipIndex}");
        }
    }

    // Toca o áudio sem substituir o clipe atual
    public void PlayAudioOneShot(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            audioSource.PlayOneShot(audioClips[clipIndex]);
        }
        else
        {
            Debug.LogWarning("Índice do áudio inválido!");
        }
    }

    // Método para tocar em loop
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
            Debug.LogWarning("Índice do áudio inválido!");
        }
    }

    // Método para parar o áudio
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
            Debug.LogWarning("Índice do áudio inválido!");
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

    // Pausa o áudio
    public void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // Retoma o áudio
    public void ResumeAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
