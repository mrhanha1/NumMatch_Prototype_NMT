using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class AudioService
{
    private readonly AudioSource _musicSource;
    private readonly AudioSource _sfxSource;
    private readonly Dictionary<string, AudioClip> _clips = new();

    public AudioService()
    {
        var go = new GameObject("AudioService");
        Object.DontDestroyOnLoad(go);
        _musicSource = go.AddComponent<AudioSource>();
        _sfxSource = go.AddComponent<AudioSource>();
    }

    public void RegisterClip(string key, AudioClip clip) => _clips[key] = clip;

    public void PlaySFX(string key)
    {
        if (_clips.TryGetValue(key, out var clip))
            _sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(string key, bool loop = true)
    {
        if (!_clips.TryGetValue(key, out var clip)) return;
        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource.Stop();
}