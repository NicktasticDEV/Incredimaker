using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform defaultIcon;

    private Canvas canvas;

    [SerializeField]
    private RawImage regularIconImage;
    [SerializeField]
    private RawImage disabledIconImage;

    [SerializeField]
    public Texture2D regularIcon;
    [SerializeField]
    public Texture2D disabledIcon;

    public string characterName;

    void Awake()
    {
        // Get canvas from DefaultCanvas tag
        canvas = GameObject.FindGameObjectWithTag("DefaultCanvas").GetComponent<Canvas>();
    }

    void Start()
    {
        regularIconImage.texture = regularIcon;
        disabledIconImage.texture = disabledIcon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //PlayManager.instance.iconBeingDragged = true;
        //PlayManager.instance.selectedCharacter = characterName;
    }

    public void OnDrag(PointerEventData eventData)
    {
        defaultIcon.anchoredPosition += eventData.delta / canvas.scaleFactor; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if icon is over a collider of a character object.
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            // Check if the collider is a character object
            CharacterObject characterObject = hit.collider.GetComponent<CharacterObject>();
            if (characterObject != null)
            {
                // Set the character object's selected character to the dragged character
                characterObject.SetCharacter(ModManager.Instance.GetCharacter(characterName));
            }
        }

        LeanTween.move(defaultIcon, Vector2.zero, 0.5f).setEaseOutCubic();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
