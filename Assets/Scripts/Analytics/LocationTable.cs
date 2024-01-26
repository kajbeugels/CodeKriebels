namespace CodeKriebels.Analytics
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "LocationTable", menuName = "Analytics/LocationTable")]
    public class LocationTable : ScriptableObject
    {
        [field: SerializeField, Tooltip("All the locations that can be the result.")]
        internal string[] Locations { get; private set; }
    }
}