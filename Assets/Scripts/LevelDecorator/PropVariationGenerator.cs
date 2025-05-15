using UnityEngine;

public class PropVariationGenerator : MonoBehaviour
{
    [ContextMenu("Generate Variation")]
    internal void GenerateVariation()
    {
        var xorProps = GetComponents<PropSelectionXor>();
        foreach (var prop in xorProps)
        {
            prop.GenerateVariation();
        }

        var orProps = GetComponents<PropVariationOr>();
        foreach (var prop in orProps)
        {
            prop.GenerateVariation();
        }
    }
}
