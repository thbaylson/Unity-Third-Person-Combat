using UnityEngine;
using Random = System.Random;

public class PropSelectionXor : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;

    public void GenerateVariation()
    {
        Random random = SharedLevelData.Instance.Rand;
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }

        int randomIndex = random.Next(gameObjects.Length);
        gameObjects[randomIndex].SetActive(true);
    }
}
