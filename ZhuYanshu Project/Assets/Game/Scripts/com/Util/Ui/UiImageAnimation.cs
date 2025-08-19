﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com
{
    public class UiImageAnimation : MonoBehaviour
    {
        [SerializeField] float _interval;
        [SerializeField] UiImageAnimationClip[] _clips;
        [SerializeField] bool _startPlay;

        [SerializeField] Image _img;

        UiImageAnimationClip _clip;
        int _index;
        bool _playing;
        float _nextTimestamp;

        [System.Serializable]
        public class UiImageAnimationClip
        {
            public Sprite[] sps;
            public bool loop;
            public float speedRatio = 1;
        }

        // Use this for initialization
        void Start()
        {
            if (_startPlay)
                Play(0);
        }

        public void Play(int i)
        {
            Play(_clips[i]);
        }

        public void Stop()
        {
            _playing = false;
        }

        public void Play(UiImageAnimationClip c)
        {
            _clip = c;
            _index = 0;
            _playing = true;
            _nextTimestamp = Time.time;
        }

        private void Update()
        {
            if (!_playing)
                return;

            if (Time.time > _nextTimestamp)
            {
                _img.sprite = _clip.sps[_index++];
                _nextTimestamp = Time.time + _interval / _clip.speedRatio;
                if (_index >= _clip.sps.Length)
                {
                    if (_clip.loop)
                        _index = 0;
                    else
                        Stop();
                }
            }
        }
    }
}