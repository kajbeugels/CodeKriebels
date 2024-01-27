using UnityEngine;

public class PlayerToSprite : MonoBehaviour
{
    [SerializeField, Tooltip("Transform that serves as reference for forward vector")]
    public Transform forwardReference;

    [SerializeField, Tooltip("Reference to the head sprite renderer")]
    private SpriteRenderer headRenderer;

    [SerializeField, Tooltip("Reference to the body sprite renderer")]
    private SpriteRenderer bodyRenderer;

    [SerializeField, Tooltip("Reference to the feet sprite renderers")]
    private SpriteRenderer leftFeetRenderer, rightFeetRenderer;

    [SerializeField, Tooltip("Reference to the hand sprite renderers")]
    private SpriteRenderer leftHandRenderer, rightHandRenderer;

    [SerializeField, Tooltip("Head sprites for each direction")]
    private Sprite[] headSprites;

    [SerializeField, Tooltip("Head sprites for each direction")]
    private Sprite[] bodySprites;

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

        //Assign sprite based on direction
        headRenderer.sprite = headSprites[index];
        bodyRenderer.sprite = bodySprites[index];
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
            return Direction.SW;
        else
            return Direction.SW;
    }
}
