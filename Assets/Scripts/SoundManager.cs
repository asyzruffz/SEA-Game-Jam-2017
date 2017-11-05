using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject manager = new GameObject("| SoundManager |");
                _instance = manager.AddComponent<SoundManager>();
                _instance.BGM_Master = manager.AddComponent<AudioSource>();
                _instance.BGM_Master.loop = true;
                _instance.SFX_Master = manager.AddComponent<AudioSource>();
                _instance.SFX_Master.loop = false;
                _instance.LoadFromResource();
                DontDestroyOnLoad(manager);
            }
            return _instance;
        }
    }

    List<AudioSource> SFX_LoopingList = new List<AudioSource>();
    public Dictionary<string, AudioClip> AudioPieceList = new Dictionary<string, AudioClip>();
    public AudioSource BGM_Master;
    public AudioSource SFX_Master;

    public void LoadFromResource()
    {
        Object[] _TempList = Resources.LoadAll("Sound", typeof(AudioClip));
        AudioPieceList.Clear();

        foreach (Object _t in _TempList)
        {
            AudioPieceList.Add(_t.name, (AudioClip)_t);
        }
    }

    public AudioClip ReturnSoundOnName(string _audioName)
    {
        AudioClip _ac = null;
        AudioPieceList.TryGetValue(_audioName, out _ac);
        return _ac;
    }

    AudioSource CreateAudioSource()
    {
        GameObject _temp = Instantiate(new GameObject(), transform.parent) as GameObject;
        AudioSource _actemp = _temp.AddComponent<AudioSource>();
        _temp.name = "AudioSource" + SFX_LoopingList.Count;
        return _actemp;
    }

    AudioSource GetOneSFXFromList(ref int _ret)
    {
        AudioSource _returnsource = null;

        if(SFX_LoopingList.Count <= 0)
        {
            _returnsource = CreateAudioSource();
            SFX_LoopingList.Add(_returnsource);
            _ret = SFX_LoopingList.Count - 1;
        }
        else
        {
            foreach(AudioSource _r in SFX_LoopingList)
            {
                if (_r.isPlaying) continue;
                _returnsource = _r;
                break;
            }

            if (_returnsource == null)
            { 
                _returnsource = CreateAudioSource();
                SFX_LoopingList.Add(_returnsource);
                _ret = SFX_LoopingList.Count - 1;
            }
        }

        return _returnsource;
    }

    //! Sound effect will play on instance;
    public void PlaySFX(string _audioName, AudioSource _alternatePlayer = null)
    {
        AudioSource _theSource = _alternatePlayer ? _alternatePlayer : SFX_Master;
        _theSource.PlayOneShot(ReturnSoundOnName(_audioName), (float) PlayerPrefs.GetInt("VolumeControl", 1));
        BGM_Master.volume = (float) PlayerPrefs.GetInt("VolumeControl", 1);
    }

    //! Sound effect will either play on generated or alternatively
    public int PlayLoopingSFX(string _audioName, AudioSource _alternatePlayer = null)
    {
        int _temp = 0;
        AudioSource _theSource = _alternatePlayer ? _alternatePlayer : GetOneSFXFromList(ref _temp);
        _theSource.clip = ReturnSoundOnName(_audioName);
        _theSource.Play();
        _theSource.loop = true;
        return _temp;
    }

    public void StopLoopingSFX(int _stopid)
    {
        if (SFX_LoopingList.Count <= 0) return;
        SFX_LoopingList[_stopid].Stop();
    }

    public void StopAllLoopingSFX()
    {
        foreach(AudioSource _sources in SFX_LoopingList)
        {
            _sources.Stop();
        }
    }

    public void PlayBGM(string _audioName, float _volumeControl = 1f)
    {
        AudioClip _ac = null;
        AudioPieceList.TryGetValue(_audioName, out _ac);

        BGM_Master.clip = _ac;
        BGM_Master.volume = (float) PlayerPrefs.GetInt("VolumeControl", 1);
        BGM_Master.Play();
    }

    public void StopBGM()
    {
        BGM_Master.Stop();
    }

    public void PauseBGM()
    {
        BGM_Master.Pause();
    }

    public void UnPauseBGM()
    {
//        BGM_Master.UnPause();
        BGM_Master.Play();
    }

}
