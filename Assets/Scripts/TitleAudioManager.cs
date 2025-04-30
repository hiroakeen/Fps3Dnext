using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _titleAudioSource;
    private readonly Dictionary<string, AudioClip> _titleClips = new Dictionary<string, AudioClip>();

    public void Awake()
    {

        var audioClips = Resources.LoadAll<AudioClip>("2D_SE");
        foreach (var clip in audioClips)
        {
            _titleClips.Add(clip.name, clip);
        }
    }

    public void onTitlePlay(string clipName)
    {
        if (!_titleClips.ContainsKey(clipName))
        {
            throw new Exception("Sound" + clipName + "is not defind");
        }

        _titleAudioSource.clip = _titleClips[clipName];
        _titleAudioSource.Play();
    }
}
