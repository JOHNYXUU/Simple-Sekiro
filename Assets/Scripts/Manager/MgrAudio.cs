using UnityEngine;

namespace Manager
{
    public class MgrAudio
    {
        public AudioSource AudioSource;

        public MgrAudio()
        {
            AudioSource = GameObject.Find("Audio").GetComponent<AudioSource>();
            
        }

        public bool SetAudioClip(AudioClip audio)
        {
            AudioSource.clip = audio;

            return AudioSource.clip != null;
        }
        
        public void Play()
        {
            if(AudioSource.clip != null)
                AudioSource.Play();
        }
        
        public void Stop()
        {
            if(AudioSource.clip != null)
                AudioSource.Stop();
        }
        
        public void Pause()
        {
            if(AudioSource.clip != null)
                AudioSource.Pause();
        }

        public void PlayOneShot(AudioClip clip, float volumeScale = 1f)
        {
            // Debug.LogError(clip.name);
            if(AudioSource.clip != null)
                AudioSource.PlayOneShot(clip,volumeScale);
        }

        public AudioClip GetRandomAudioClip(string basePath, int totalNum)
        {
            var randInt = Random.Range(1, totalNum + 1);
            
            // Debug.LogError(basePath + randInt);

            return (AudioClip)Resources.Load(basePath + randInt);
        }
    }
}