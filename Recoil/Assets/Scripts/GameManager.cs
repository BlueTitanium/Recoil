using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Animation pauseAnim;
    public Animation startAnim;
    public Animation deathAnim;
    public Animation victoryAnim;
    public bool started = false;
    public bool paused = false;
    public bool ended = false;
    public VolumeProfile profile;
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle CameraShake;
    public Toggle ChromaticAberration;
    public Toggle MotionBlur;
    public Toggle ParticleEffects;
    public GameObject[] InWorldParticles;
    public Toggle Animation;
    public Toggle Vignette;
    public Toggle FilmGrain;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        paused = false;
        gm = this;
        Time.timeScale = 1f;
        SetSettings();
        
    }

    void SetSettings()
    {
        masterVolume = Toggles.MasterVolume;
        musicVolume = Toggles.MusicVolume;
        sfxVolume = Toggles.SFXVolume;
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        CameraShake.isOn = Toggles.CameraShake;
        ChromaticAberration.isOn = Toggles.ChromaticAberration;
        MotionBlur.isOn = Toggles.MotionBlur;
        ParticleEffects.isOn = Toggles.ParticleEffects;
        Animation.isOn = Toggles.Animation;
        Vignette.isOn = Toggles.Vignette;
        FilmGrain.isOn = Toggles.FilmGrain;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if (paused && !ended)
            {
                UnpauseGame();
            } else if (!ended)
            {
                PauseGame();
            }
        }
    }
    public void StartGame()
    {
        GetComponent<AudioSource>().Play();
        startAnim.Play();
        started = true;
    }
    public void LoseGame()
    {
        if (!ended)
        {
            GetComponent<AudioSource>().Play();
            deathAnim.Play();
            paused = true;
            ended = true;
        }
        
    }
    public void WinGame()
    {
        if (!ended)
        {
            GetComponent<AudioSource>().Play();
            victoryAnim.Play();
            paused = true;
            ended = true;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        GetComponent<AudioSource>().Play();
        paused = true;
        if (Toggles.Animation)
        {
            pauseAnim["Pause_Open"].speed = 1;
            pauseAnim["Pause_Open"].time = 0;
            pauseAnim.Play("Pause_Open");
        }
        else
        {
            pauseAnim["Pause_Open"].speed = 1;
            pauseAnim["Pause_Open"].time = pauseAnim["Pause_Open"].length;
            pauseAnim.Play("Pause_Open");
        }
    }

    public void UnpauseGame()
    {
        GetComponent<AudioSource>().Play();
        paused = false;
        if (Toggles.Animation)
        {
            pauseAnim["Pause_Open"].speed = -1;
            pauseAnim["Pause_Open"].time = pauseAnim["Pause_Open"].length;
            pauseAnim.Play("Pause_Open");
        } else
        {
            pauseAnim["Pause_Open"].speed = -1;
            pauseAnim["Pause_Open"].time = 0f;
            pauseAnim.Play("Pause_Open");
        }
            
    }

    public void UpdateMixerVolume()
    {
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        Toggles.MasterVolume = masterVolume;
        UpdateMixerVolume();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        Toggles.MusicVolume = musicVolume;
        UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;
        Toggles.SFXVolume = sfxVolume;
        UpdateMixerVolume();
    }
    public void OnCameraShakeToggle()
    {
        Toggles.CameraShake = CameraShake.isOn;
    }
    public void OnChromaticAberrationToggle()
    {
        Toggles.ChromaticAberration = ChromaticAberration.isOn;
        ChromaticAberration chroma;
        profile.TryGet(out chroma);
        chroma.SetAllOverridesTo(Toggles.ChromaticAberration);
    }
    public void OnMotionBlurToggle()
    {
        Toggles.MotionBlur = MotionBlur.isOn;
        MotionBlur blur;
        profile.TryGet(out blur);
        blur.SetAllOverridesTo(Toggles.MotionBlur);
    }
    public void OnParticleEffectsToggle()
    {
        Toggles.ParticleEffects = ParticleEffects.isOn;
        foreach(GameObject o in InWorldParticles)
        {
            o.SetActive(Toggles.ParticleEffects);
        }
    }
    public void OnAnimationToggle()
    {
        Toggles.Animation = Animation.isOn;
        SlimeAI[] slimes = FindObjectsByType<SlimeAI>(FindObjectsSortMode.None);
        foreach (SlimeAI s in slimes)
        {
            if (Toggles.Animation)
            {
                s.anim.Play();
            }
            else
            {
                s.anim.Stop();
            }
        }
    }
    public void OnVignetteToggle()
    {
        Toggles.Vignette = Vignette.isOn;
        Vignette vignette;
        profile.TryGet(out vignette);
        vignette.SetAllOverridesTo(Toggles.Vignette);
    }

    public void OnFilmGrainToggle()
    {
        Toggles.FilmGrain = FilmGrain.isOn;
        FilmGrain film;
        profile.TryGet(out film);
        film.SetAllOverridesTo(Toggles.FilmGrain);
    }

}
