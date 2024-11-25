using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestCharacterSlot : MonoBehaviour, IDropHandler
{
    private UnityEngine.UI.Image image;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color highlightColor;

    void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped");
        // Delete the dragged object
        Destroy(eventData.pointerDrag);
        image.color = highlightColor;
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
