using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using burglar.environment;

namespace burglar.managers
{
    public class AudioManager : MonoBehaviour
    {
        [Header("AudioMix Groups")]
        public AudioMixerGroup masterMixerGroup;
        public AudioMixerGroup musicMixerGroup;
        public AudioMixerGroup sfxMixerGroup;
        
        [Header("Audio Sources")]
        public AudioSource musicAudioSource;
        public AudioSource musicAudioSource2;
        public AudioSource sfxAudioSource;
        public AudioSource sfxAudioSource2;
        public AudioSource sfxAudioSource3;
        public AudioSource sfxAudioSource4;
        
        // Singleton
        public static AudioManager Instance;

        [Header("Audio Clips")]
        [Header("Musics")]
        public AudioClip musicMenu;
        public AudioClip musicTuto;
        public AudioClip musicLevel;
        public AudioClip musicShop;
        [Header("SFX")]
        [Header("UI")]
        public AudioClip soundHover;
        public AudioClip soundDialogSlideIn;
        public AudioClip soundDialogSlideOut;
        public AudioClip soundTyping;
        public AudioClip soundClick;
        public AudioClip soundTransition;
        public AudioClip soundCredit;
        public AudioClip soundSuccess;
        public AudioClip soundFail;
        public AudioClip soundTeleport;
        public AudioClip soundTeleportOut;
        public AudioClip soundSwoosh;
        public AudioClip soundSlowDown;
        public AudioClip soundSlowDownInvert;
        
        [Header("Player")]
        public AudioClip soundFootstep;
        public AudioClip soundSneakyFootstep;
        
        [Header("Environment")]
        public AudioClip soundLightSwitch;
        public AudioClip soundAlarm;
        
        private float _musicVolume;
        private float _musicVolume2;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
            
            _musicVolume = musicAudioSource.volume;
            _musicVolume2 = musicAudioSource2.volume;
        }

        private void Start()
        {
            // Change the volume of the music mix groups according to the player's settings
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                masterMixerGroup.audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            }
            
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                musicMixerGroup.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            }
            
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                musicMixerGroup.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
            }
            
        }

        private void OnEnable()
        {
            EventManager.TogglePause += OnTogglePause;
            EventManager.ChangeGameState += OnChangeGameState;
            EventManager.SuccessSafeCrack += OnSuccessSafeCrack;
            EventManager.FailSafeCrack += OnFailSafeCrack;
            EventManager.LoadLevel += PlayLevelMusic;
        }

        private void OnDisable()
        {
            EventManager.TogglePause -= OnTogglePause;
            EventManager.ChangeGameState -= OnChangeGameState;
            EventManager.SuccessSafeCrack -= OnSuccessSafeCrack;
            EventManager.FailSafeCrack -= OnFailSafeCrack;
            EventManager.LoadLevel -= PlayLevelMusic;
        }
        
        private void PlayLevelMusic()
        {
            Debug.Log("Play level music (AudioManager)");
            var music = LevelManager.Instance._currentLevel.music;
            PlayMusicWithDelay(music, true, 1.3f);
        }

        private void OnFailSafeCrack(Safe arg0)
        {
            PlaySFX(soundFail);
        }

        private void OnSuccessSafeCrack(Safe arg0)
        {
            PlaySFX(soundSuccess);
        }

        private void OnChangeGameState(GameManager.GameState gameState)
        {
            if (gameState == GameManager.GameState.Alert)
            {
                musicAudioSource2.clip = soundAlarm;
                musicAudioSource2.Play();
                
                // Pitch current music at 1.1
                musicAudioSource.pitch = 1.1f;
            }
            else
            {
                musicAudioSource2.Stop();
                
                // Pitch current music at 1.5
                musicAudioSource.pitch = 1f;
            }
        }

        private void OnTogglePause()
        {
            // Play slowdown sound
            PlaySFX((Time.timeScale == 0) ? soundSlowDown : soundSlowDownInvert);
            
            // Reduce the pitch of the music when the game is paused
            if (Time.timeScale == 0)
            {
                musicAudioSource.pitch = 0.75f;
                musicAudioSource2.pitch = 0.75f;
            }
            else
            {
                musicAudioSource.pitch = 1f;
                musicAudioSource2.pitch = 1f;
            }
        }
        
        

        public void PlayMusic(AudioClip music, bool crossFade, bool doFade = true)
        {
            if (crossFade)
            {
                // if a music is already playing, make a fade out of the current music and fade in of the new one
                if (musicAudioSource.isPlaying || musicAudioSource2.isPlaying)
                {
                    if (musicAudioSource.isPlaying)
                    {
                        musicAudioSource2.clip = music;
                        musicAudioSource2.Play();
                    
                        StartCoroutine(CrossFade(musicAudioSource, musicAudioSource2, 1f));
                    }
                    else
                    {
                        musicAudioSource.clip = music;
                        musicAudioSource.Play();
                        
                        StartCoroutine(CrossFade(musicAudioSource2, musicAudioSource, 1f));
                    }
                }
                else
                {
                    musicAudioSource.clip = music;
                    musicAudioSource.Play();
                
                    StartCoroutine(FadeIn(musicAudioSource, music, 1f));
                }
            }
            else
            {
                if (doFade)
                {
                    StartCoroutine(FadeOutThanIn(music, 1f, 1f));                    
                }
                else
                {
                    if (musicAudioSource.isPlaying)
                    {
                        musicAudioSource.Stop();
                    }
                    else if (musicAudioSource2.isPlaying)
                    {
                        musicAudioSource2.Stop();
                    }

                    musicAudioSource.clip = music;
                    musicAudioSource.Play();
                }
                
            }
        }
        
        private void PlayMusicWithDelay(AudioClip music, bool crossFade, float delay)
        {
            StartCoroutine(PlayMusicWithDelayCoroutine(music, crossFade, delay));
        }

        private IEnumerator PlayMusicWithDelayCoroutine(AudioClip music, bool crossFade, float delay)
        {
            // Wait for the delay
            yield return new WaitForSeconds(delay);
            
            PlayMusic(music, crossFade);
        }

        private IEnumerator FadeOutThanIn(AudioClip music, float fadeTimeSeconds = 1f, float waitTimeSeconds = 0f)
        {
            // First fade out the current music
            if (musicAudioSource.isPlaying)
            {
                yield return StartCoroutine(FadeOut(musicAudioSource, fadeTimeSeconds));
            }
            else if (musicAudioSource2.isPlaying)
            {
                yield return StartCoroutine(FadeOut(musicAudioSource2, fadeTimeSeconds));
            }


            if (waitTimeSeconds > 0f)
            {
                yield return new WaitForSeconds(waitTimeSeconds);
            }
            
            // Then fade in the new music
            StartCoroutine(FadeIn(musicAudioSource, music, fadeTimeSeconds));
        }

        public AudioSource PlaySFX(AudioClip sfx)
        {
            var sfxAudioSourceFree = 
                (sfxAudioSource.isPlaying) ? (
                    (sfxAudioSource2.isPlaying) ? (
                        (sfxAudioSource3.isPlaying) ? sfxAudioSource4 : sfxAudioSource3
                    ) : sfxAudioSource2
                ) : sfxAudioSource;
            
            sfxAudioSourceFree.clip = sfx;
            sfxAudioSourceFree.Play();
            
            return sfxAudioSourceFree;
        }

        private IEnumerator CrossFade(AudioSource audioSourcePlaying, AudioSource audioSourceFree, float fadeTime)
        {
            // Fade out the audio source playing and fade in the free one
            var startVolume = audioSourcePlaying.volume;
            
            // Fade out the audio source playing and fade in the free one for fadeTime seconds
            while (audioSourcePlaying.volume > 0)
            {
                audioSourcePlaying.volume -= startVolume * Time.deltaTime / fadeTime;
                audioSourceFree.volume += Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        
        private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        {
            // Fade out the audio source
            float startVolume = audioSource.volume;
            
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        
        private IEnumerator FadeIn(AudioSource audioSource, AudioClip music, float fadeTime)
        {
            var audioSourceTarget = audioSource == musicAudioSource ? _musicVolume : _musicVolume2;
            
            // Fade in the audio source
            audioSource.clip = music;
            audioSource.Play();
            audioSource.volume = 0f;
            
            while (audioSource.volume < audioSourceTarget)
            {
                audioSource.volume += Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        
        
        
    }
}
