using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Rings : MonoBehaviour
{
    //CALIDAD DE LA ANILLA
    private RingQuality quality;

    //POSICION EN LA MATRIZ
    private int col;
    private int row;

    //ANCLAJES DE ANILLA
    [SerializeField] private GameObject upHandle;
    [SerializeField] private GameObject downHandle;
    [SerializeField] private GameObject leftHandle;
    [SerializeField] private GameObject rightHandle;
    private Collider2D upHandleCollider; //O(1)
    private Collider2D downHandleCollider;
    private Collider2D leftHandleCollider;
    private Collider2D rightHandleCollider;
    private SpriteRenderer spriteRenderer;


    public void InitializationRing(RingObject newRing)
    {
        quality = newRing.quality;
        this.GetComponent<SpriteRenderer>().sprite = newRing.ringSprite;
        upHandleCollider = upHandle.GetComponent<Collider2D>();
        downHandleCollider = downHandle.GetComponent<Collider2D>();
        leftHandleCollider = leftHandle.GetComponent<Collider2D>();
        rightHandleCollider = rightHandle.GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void DisableHandlesByGridPos(int maxR, int maxC)
    {
        rightHandleCollider.enabled = true;
        leftHandleCollider.enabled = true;
        upHandleCollider.enabled = true;
        downHandleCollider.enabled = true;

        if (col <= 0) leftHandleCollider.enabled = false;
        if (col >= maxC - 1) rightHandleCollider.enabled = false;
        if (row <= 0) upHandleCollider.enabled = false;
        if (row >= maxR - 1) downHandleCollider.enabled = false;
    }

    public void DisableHandleByDirection(string direction)
    {
        switch (direction)
        {
            case "up":
                upHandleCollider.enabled = false;
                break;
            case "down":
                downHandleCollider.enabled = false;
                break;
            case "left":
                leftHandleCollider.enabled = false;
                break;
            case "right":
                rightHandleCollider.enabled = false;
                break;
        }
    }

    public void DisableHandleByAttachment(List<bool> blockDirection)
    {
        //arriba abajo izquieda derecha

        if (blockDirection[0])
        {
            upHandleCollider.enabled = false;
        }
        if (blockDirection[1])
        {
            downHandleCollider.enabled = false;
        }
        if (blockDirection[2])
        {
            leftHandleCollider.enabled = false;
        }
        if (blockDirection[3])
        {
            rightHandleCollider.enabled = false;
        }
    }
    
    public RingQuality GetQuality()
    {
        return quality;
    }

    public List<int> GetColRow()
    {
        List<int> pos = new List<int> { row, col };
        return pos;
    }

    public void SetColRowInGrid(int r, int c, int maxR, int maxC) //al establecer la fila y la columna de la anilla en el grid se modifican los colliders (handles)
    {
        row = r;
        col = c;
        DisableHandlesByGridPos(maxR, maxC); //deshabilitar handles en base a la posicion
    }
    
    public string GetHandleHitDirection(Collider2D hitHandle)
    {
        string handleDirection = "";

        if (hitHandle == upHandleCollider)
        {
            handleDirection = "up";
        }
        if (hitHandle == downHandleCollider)
        {
            handleDirection = "down";
        }
        if (hitHandle == leftHandleCollider)
        {
            handleDirection = "left";
        }
        if (hitHandle == rightHandleCollider)
        {
            handleDirection = "right";
        }
        //else //error
        //{
        //    handleDirection = "desconocido";
        //}

        return handleDirection;
    }

    public int GetOrderInLayer()
    {
        return spriteRenderer.sortingOrder;
    }

    public void ChangeSpriteColor()
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#B8B8B8", out newColor))
        {
            spriteRenderer.color = newColor;
        }
    }

    public void SetOrderInLayer(int order)
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = order;
    }
}
