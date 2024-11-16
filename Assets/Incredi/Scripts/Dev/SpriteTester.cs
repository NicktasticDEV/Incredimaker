using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImageSequenceAnimator))]
public class SpriteTester : MonoBehaviour
{
    private ImageSequenceAnimator imageSequenceAnimator;
    
    void Awake()
    {
        imageSequenceAnimator = GetComponent<ImageSequenceAnimator>();
    }

    void Start()
    {
        imageSequenceAnimator.PlayAnimation("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
