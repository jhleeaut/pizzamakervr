using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{

    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance;
        public AudioSource bgm;
        public AudioSource effect;
        [SerializeField] private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        private void Awake()
        {
            Instance = this;
            audioClips.Add(
                "물끓는소리",
                Resources.Load("물끓는소리") as AudioClip
                );
            audioClips.Add(
                "배경음악",
                Resources.Load("배경음악") as AudioClip
                );
            audioClips.Add(
                "배경음악2",
                Resources.Load("배경음악2") as AudioClip
                );
            audioClips.Add(
                "배경음악3",
                Resources.Load("배경음악3") as AudioClip
                );
            audioClips.Add(
                "배경음악4",
                Resources.Load("배경음악4") as AudioClip
                );
            audioClips.Add(
                "배경음악5",
                Resources.Load("배경음악5") as AudioClip
                );
            audioClips.Add(
                "배경음악6",
                Resources.Load("배경음악6") as AudioClip
                );
            audioClips.Add(
                "알람소리1",
                Resources.Load("알람소리1") as AudioClip
                );
            audioClips.Add(
                "알람소리2",
                Resources.Load("알람소리2") as AudioClip
                );
            audioClips.Add(
                "알람소리3",
                Resources.Load("알람소리3") as AudioClip
                );
            audioClips.Add(
                "알람소리4",
                Resources.Load("알람소리4") as AudioClip
                );
            audioClips.Add(
                "오븐소리",
                Resources.Load("오븐소리") as AudioClip
                );
            audioClips.Add(
                "온갖알람소리",
                Resources.Load("온갖알람소리") as AudioClip
                );
            audioClips.Add(
                "완료음악",
                Resources.Load("완료음악") as AudioClip
                );
            audioClips.Add(
                "익는소리",
                Resources.Load("익는소리") as AudioClip
                );
            audioClips.Add(
                "효과음",
                Resources.Load("효과음") as AudioClip
                );
#if UNITY_EDITOR
            print(audioClips.Count);
#endif
        }

        private void Start()
        {
            StartCoroutine("BGMLoop");
        }

        IEnumerator BGMLoop()
        {

            while (true)
            {
                if(!bgm.isPlaying)
                {
                    int ranidx = Random.Range(1, 7);
                    if (ranidx == 1)
                    {
                        PlayBGM("배경음악");
                    }
                    else
                    {
                        PlayBGM("배경음악"+ranidx);
                    }
                }
                yield return new WaitForSeconds(5.0f);
            }
        }

        /// <summary>
        /// 배경음악, 배경음악2, 배경음악3, 배경음악4, 배경음악5, 배경음악6
        /// </summary>
        /// <param name="audioName">오디오 이름</param>
        public void PlayBGM(string audioName)
        {
            bgm.clip = audioClips[audioName];
            bgm.Play();
        }
        /// <summary>
        /// 물끓는소리, 알람소리1, 알람소리2, 알람소리3, 알람소리4, 오븐소리, 온갖알람소리, 완료음악, 익는소리, 효과음
        /// </summary>
        /// <param name="audioName">오디오 이름</param>
        public void PlayEffect(string audioName)
        {
            StopEffect();
            EffectVolume(audioName);
            effect.clip = audioClips[audioName];
            effect.Play();
        }
        /// <summary>
        /// 물끓는소리, 알람소리1, 알람소리2, 알람소리3, 알람소리4, 오븐소리, 온갖알람소리, 완료음악, 익는소리, 효과음
        /// </summary>
        /// <param name="audioName">오디오 이름</param>
        public void PlayEffect(string audioName, float startTime)
        {
            StopEffect();
            EffectVolume(audioName);
            effect.clip = audioClips[audioName];
            effect.time = startTime;
            effect.Play();
        }
        /// <summary>
        /// 물끓는소리, 알람소리1, 알람소리2, 알람소리3, 알람소리4, 오븐소리, 온갖알람소리, 완료음악, 익는소리, 효과음
        /// </summary>
        /// <param name="audioName">오디오 이름</param>
        public void PlayEffect(string audioName, float startTime, float exitTime)
        {
            StopEffect();
            EffectVolume(audioName);
            effect.clip = audioClips[audioName];
            effect.time = startTime;
            effect.Play();
            Invoke("StopEffect", exitTime);
        }

        void StopEffect()
        {
            effect.Stop();
        }

        void EffectVolume(string audioName)
        {
            if (audioName.Equals("완료음악"))
            {
                effect.volume = 0.5f;
            }
            else
            {
                effect.volume = 1.0f;
            }
        }
    }
}
