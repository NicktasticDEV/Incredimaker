using System.Collections;
using System.Collections.Generic;
using Incredimaker;
using UnityEngine;

namespace Incredimaker
{
    public class VisualMetronome : MonoBehaviour
    {
        private Animator animator;
        public bool useTick;

        void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on this GameObject!");
                return;
            }
        }

        void Start()
        {
            if (useTick)
            {
                Metronome.Instance.onBeat += OnBeat;
            }
        }

        void Update()
        {
            animator.SetBool("isPlaying", Metronome.Instance.isActive);
            animator.SetInteger("bpm", Metronome.Instance.BPM);
            animator.SetBool("isPaused", Metronome.Instance.isPaused);
        }

        private void OnBeat()
        {
            if (animator != null)
            {
                Debug.Log("OnBeat triggered");
                animator.SetTrigger("tick");
            }
        }


    }
}