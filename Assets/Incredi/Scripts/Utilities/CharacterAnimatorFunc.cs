using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorFunc : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private CharacterObject characterObject;

    void Awake()
    {
        //Get animator from parent
        animator = GetComponentInParent<Animator>();
        characterObject = GetComponentInParent<CharacterObject>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetCharacter()
    {
        characterObject.SetCharacter(ModManager.Instance.GetCharacter("Default"));
    }
}
