using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
		private Dictionary<string,AudioClip> _allAudio;

        public AudioSource EfxSource;
        public AudioSource MusicSource;
        public Camera Camera;
		public float setEfxVolume = 1.0f;
		public float setMusicVolume = 0.7f;
        public float volumeChange = 1.0f;
        public bool soundEnabled = true;

        private void Awake()
        {
            if (_instance != null && _instance != this) {
                Destroy (this.gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad (this.gameObject);
            }
        }

        private void Start()
        {
			_allAudio = new Dictionary<string, AudioClip>();
			gameObject.AddComponent<AudioSource> ();
			MusicSource = Camera.gameObject.AddComponent<AudioSource> (); 
            var loadAudio = Resources.LoadAll<AudioClip>("Sounds");
            foreach (AudioClip t in loadAudio)
            {
				_allAudio.Add(t.name,t);
            }
        }

        public static SoundManager Instance()
        {
            return _instance ?? (_instance = new SoundManager());
        }

        public void PlayBGM()
        {
            if (soundEnabled == true)
            {
                MusicSource.clip = _allAudio["bgm1"];
                MusicSource.loop = true;
                MusicSource.volume = setMusicVolume * volumeChange;
                MusicSource.Play();
            }
        }

		public void PlaySingle(string name)
        {
            if (soundEnabled == true)
            {
                EfxSource = gameObject.GetComponent<AudioSource>();
                EfxSource.volume = setEfxVolume * volumeChange;
                EfxSource.clip = _allAudio[name];
                EfxSource.Play();
            }
        }

		public void PlaySingleDistance(GameObject emitter, string name)
		{
            if (soundEnabled == true)
            {
                bool desAudio = false;
                if (emitter.GetComponent<AudioSource>() != null)
                {
                    EfxSource = emitter.GetComponent<AudioSource>();
                }
                else
                {
                    EfxSource = emitter.AddComponent<AudioSource>();
                    desAudio = true;
                }

                EfxSource.spatialBlend = 1;
                EfxSource.minDistance = 0.5f;
                EfxSource.maxDistance = 30.0f;
                EfxSource.volume = setEfxVolume * volumeChange;
                EfxSource.clip = _allAudio[name];
                EfxSource.Play();

                if (desAudio == true)
                    Destroy(emitter.GetComponent<AudioSource>(), EfxSource.clip.length + 1);
            }
		}

		public void PlaySingleDistance(GameObject emitter, string name, float minDis, float maxDis)
		{
            if (soundEnabled == true)
            {
                bool desAudio = false;
                if (emitter.GetComponent<AudioSource>() != null)
                {
                    EfxSource = emitter.GetComponent<AudioSource>();
                }
                else
                {
                    EfxSource = emitter.AddComponent<AudioSource>();
                    desAudio = true;
                }

                EfxSource.spatialBlend = 1;
                EfxSource.minDistance = minDis;
                EfxSource.maxDistance = maxDis;
                EfxSource.volume = setEfxVolume * volumeChange;
                EfxSource.clip = _allAudio[name];
                EfxSource.Play();

                if (desAudio == true)
                    Destroy(emitter.GetComponent<AudioSource>(), EfxSource.clip.length + 1);
            }
		}

        public void ToogleSound()
        {
            soundEnabled = !soundEnabled;
           /* if (soundEnabled == true)
                soundEnabled = false;
            else if (soundEnabled = false)
                soundEnabled = true;*/
        }

        public void ChangeVolume(float volume)
        {
            volumeChange = volume;
        }
    }
}