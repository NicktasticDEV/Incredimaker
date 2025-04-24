using System.Collections;
using System.Collections.Generic;
using Incredimaker;
using UnityEngine;


namespace Incredimaker
{
    public class VisualDynamicMetronome : MonoBehaviour
    {
        private Animator animator;
        
        [SerializeField] private DynamicMetronome metronome;

        private bool isReady = false;
        public bool autoSync = true;

        void Awake()
        {
            animator = GetComponent<Animator>();

            metronome.OnBeat += onBeat;
            metronome.OnRestart += onRestart;
            
        }

        void Start()
        {

        }

        void Update()
        {
            animator.SetFloat("bpm", (float)metronome.bpm);
            animator.SetBool("isPaused", metronome.isPaused);
        }

        void onBeat(int beatIndex, int measureCount)
        {
            if (!autoSync)
            {
                if (!isReady)
                {
                    Debug.Log("Metronome started");
                    isReady = true;
                    animator.SetBool("isPlaying", true);
                }
            }
            else
            {
                animator.Play("TickRight", 0, 0.0f);
            }
        }

        void onRestart()
        {
            // Play TickLeft animation
            animator.Play("TickLeft", 0, 0.0f);
        }

    }
}