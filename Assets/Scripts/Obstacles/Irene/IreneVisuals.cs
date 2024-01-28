using System;
using UnityEngine;

[ExecuteInEditMode]
public class IreneVisuals : MonoBehaviour
{
    [SerializeField, Tooltip("The Irene Movement component")]
    private IreneMovement ireneMovement;

    [SerializeField, Tooltip("The speech bubble reference.")]
    private SpriteRenderer speechBubbleImage;

    [SerializeField, Tooltip("Local positions for tie placement")]
    private Vector3 localTiePositionRight, localTiePositionLeft;

    [SerializeField, Tooltip("The root objects")]
    private GameObject rootHand, rootFeet;

    [SerializeField]
    private int leftHandLayer, rightHandLayer, altLeftHandLayer, altRightHandLayer;

    [SerializeField, Tooltip("Reference to the head sprite renderer")]
    private SpriteRenderer headRenderer;

    [SerializeField, Tooltip("Reference to the body sprite renderer")]
    private SpriteRenderer bodyRenderer;

    [SerializeField, Tooltip("Reference to the tie sprite renderer")]
    private SpriteRenderer tieRenderer;

    [SerializeField, Tooltip("Reference to the feet sprite renderers")]
    private SpriteRenderer leftFeetRenderer, rightFeetRenderer;

    [SerializeField, Tooltip("Reference to the hand sprite renderers")]
    private SpriteRenderer leftHandRenderer, rightHandRenderer;

    [field: SerializeField, Tooltip("Head sprites for each direction")]
    internal Sprite[] HeadSprites { get; private set; }

    [field: SerializeField, Tooltip("Head sprites for each direction")]
    internal Sprite[] BodySprites { get; private set; }

    [field: SerializeField, Tooltip("Tie sprites for each direction")]
    internal Sprite[] TieSprites { get; private set; }

    [field: SerializeField, Tooltip("Hand sprites for each direction.")]
    internal Sprite HandSprite { get; private set; }


    /// <summary>
    /// Enum direction value
    /// </summary>
    public enum Direction
    {
        SE,//South-east
        NE,//North-east
        NW,//North-west
        SW//South-west
    }

#if UNITY_EDITOR
    [SerializeField]
    private bool EDITOR_useDebugDirection;

    [SerializeField]
    private Direction EDITOR_direction;
#endif

    /// <summary>
    /// Called each frame, used for updating visuals
    /// </summary>
    public void LateUpdate()
    {
        Direction current = default;

        //Get the current heading direction
#if UNITY_EDITOR
        if (EDITOR_useDebugDirection)
            current = EDITOR_direction;
        else
#endif
            current = GetCurrentDirection();

        int index = (int)current;

        //Assign sprite based on direction
        headRenderer.sprite = HeadSprites[index];
        bodyRenderer.sprite = BodySprites[index];
        tieRenderer.sprite = TieSprites[index];
        leftHandRenderer.sprite = rightHandRenderer.sprite = HandSprite;

        //Hand logic
        if (current == Direction.NE)
        {
            leftHandRenderer.sortingOrder = altLeftHandLayer;
            rightHandRenderer.sortingOrder = altRightHandLayer;
        }
        else if (current == Direction.SW)
        {
            leftHandRenderer.sortingOrder = altLeftHandLayer;
            rightHandRenderer.sortingOrder = altRightHandLayer;
        }
        else if (current == Direction.NW)
        {
            leftHandRenderer.sortingOrder = leftHandLayer;
            rightHandRenderer.sortingOrder = rightHandLayer;
        }
        else
        {
            leftHandRenderer.sortingOrder = leftHandLayer;
            rightHandRenderer.sortingOrder = rightHandLayer;
        }

        //Tie logic
        if (current == Direction.SW)
        {
            tieRenderer.gameObject.SetActive(true);
        }

        //Tie logic
        if (current == Direction.NE)
        {
            rootHand.transform.localScale = new Vector3(-1, 1, 1);
            rootFeet.transform.localScale = new Vector3(-1, 1, 1);
            tieRenderer.gameObject.SetActive(false);
        }
        else if (current == Direction.SW)
        {
            rootHand.transform.localScale = new Vector3(1, 1, 1);
            rootFeet.transform.localScale = new Vector3(1, 1, 1);
            tieRenderer.gameObject.SetActive(false);
        }
        else if (current == Direction.NW)
        {
            rootHand.transform.localScale = new Vector3(-1, 1, 1);
            rootFeet.transform.localScale = new Vector3(-1, 1, 1);
            tieRenderer.gameObject.transform.localPosition = localTiePositionLeft;
            tieRenderer.gameObject.SetActive(true);
        }
        else
        {
            rootHand.transform.localScale = new Vector3(1, 1, 1);
            rootFeet.transform.localScale = new Vector3(1, 1, 1);
            tieRenderer.gameObject.transform.localPosition = localTiePositionRight;
            tieRenderer.gameObject.SetActive(true);
        }

        transform.localPosition = ireneMovement.transform.localPosition;
    }

    /// <summary>
    /// Toggles the speech bubble.
    /// </summary>
    internal void ToggleSpeechBubble(bool isOn)
    {
        speechBubbleImage.gameObject.SetActive(isOn);
    }

    /// <summary>
    /// Returns the current heading direction for this player
    /// </summary>
    public Direction GetCurrentDirection()
    {
        Vector3 f = ireneMovement.MoveVector;
        float angle = Mathf.Atan2(f.z, f.x) * Mathf.Rad2Deg;

        //Clamp angle in 0-360 range
        if (angle < 0)
            angle = 360 + angle;


        //45-135 - SE
        if (angle >= 45 && angle <= 135)
            return Direction.SE;
        //135-225 - NE
        else if (angle >= 135 && angle <= 225)
            return Direction.NE;
        //315-45 - SW
        else if ((angle >= 0 && angle <= 45) && (angle >= 315 && angle <= 360))
            return Direction.NW;
        else if ((angle >= 225 && angle <= 315))
            return Direction.SW;

        return Direction.NW;
    }
}
