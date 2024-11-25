using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterIcon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform defaultIcon;

    [SerializeField]
    private Canvas canvas;

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        defaultIcon.anchoredPosition += eventData.delta / canvas.scaleFactor; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        defaultIcon.anchoredPosition = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
