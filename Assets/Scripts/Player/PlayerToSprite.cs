using UnityEngine;

[ExecuteInEditMode]
public class PlayerToSprite : MonoBehaviour
{
    [SerializeField, Tooltip("Transform that serves as reference for forward vector")]
    public Transform forwardReference;

    [SerializeField, Tooltip("Local positions for tie placement")]
    private Vector3 localTiePositionRight, localTiePositionLeft;

    [SerializeField, Tooltip("The root objects")]
    private GameObject rootAss, rootHand, rootFeet;

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

    [SerializeField, Tooltip("Head sprites for each direction")]
    private Sprite[] headSprites;

    [SerializeField, Tooltip("Head sprites for each direction")]
    private Sprite[] bodySprites;

    [SerializeField, Tooltip("Tie sprites for each direction")]
    private Sprite[] tieSprites;

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

    /// <summary>
    /// Called each frame, used for updating visuals
    /// </summary>
    public void Update()
    {
        //Get the current heading direction
        Direction current = GetCurrentDirection();
        int index = (int)current;

        transform.position = forwardReference.transform.position;

        //Assign sprite based on direction
        headRenderer.sprite = headSprites[index];
        bodyRenderer.sprite = bodySprites[index];
        tieRenderer.sprite = tieSprites[index];

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

        //Ass logic
        if (current == Direction.NE)
        {
            rootAss.gameObject.SetActive(true);
            rootAss.gameObject.transform.localScale = Vector3.one;
        }
        else if (current == Direction.SW)
        {
            tieRenderer.gameObject.SetActive(true);
            rootAss.gameObject.SetActive(true);
            rootAss.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            rootAss.gameObject.SetActive(false);
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
    }

    /// <summary>
    /// Returns the current heading direction for this player
    /// </summary>
    private Direction GetCurrentDirection()
    {
        Vector3 f = forwardReference.forward;
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
