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
		public GameObject Player;
		public float setEfxVolume = 1.0f;
		public float setMusicVolume = 0.7f;

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
			MusicSource = Player.AddComponent<AudioSource> ();
            var loadAudio = Resources.LoadAll<AudioClip>("Sounds");
            foreach (AudioClip t in loadAudio)
            {
				_allAudio.Add(t.name,t);
            }

			MusicSource.clip = _allAudio ["bgm1"];
			MusicSource.loop = true;
			MusicSource.volume = setMusicVolume;
			MusicSource.Play ();
        }

        public static SoundManager Instance()
        {
            return _instance ?? (_instance = new SoundManager());
        }

		public void PlaySingle(string name)
        {
			EfxSource = gameObject.GetComponent<AudioSource> ();
			EfxSource.volume = setEfxVolume;
            EfxSource.clip = _allAudio[name];
            EfxSource.Play ();

        }

		public void PlaySingleDistance(GameObject emitter, string name)
		{
			bool desAudio = false;
			if (emitter.GetComponent<AudioSource> () != null) 
			{
				EfxSource = emitter.GetComponent<AudioSource> ();
			} else 
			{
				EfxSource = emitter.AddComponent<AudioSource> ();
				desAudio = true;
			}

			EfxSource.spatialBlend = 1;
			EfxSource.minDistance = 0.5f;
			EfxSource.maxDistance = 30.0f;
			EfxSource.volume = setEfxVolume;
			EfxSource.clip = _allAudio [name];
			EfxSource.Play ();

			if (desAudio == true)
			Destroy (emitter.GetComponent<AudioSource> (), EfxSource.clip.length + 1);
		}

		public void PlaySingleDistance(GameObject emitter, string name, float minDis, float maxDis)
		{
			bool desAudio = false;
			if (emitter.GetComponent<AudioSource> () != null) 
			{
				EfxSource = emitter.GetComponent<AudioSource> ();
			} else 
			{
				EfxSource = emitter.AddComponent<AudioSource> ();
				desAudio = true;
			}

			EfxSource.spatialBlend = 1;
			EfxSource.minDistance = minDis;
			EfxSource.maxDistance = maxDis;
			EfxSource.volume = setEfxVolume;
			EfxSource.clip = _allAudio [name];
			EfxSource.Play ();

			if (desAudio == true)
				Destroy (emitter.GetComponent<AudioSource> (), EfxSource.clip.length + 1);
		}
    }
}