using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorfulRooms : MonoBehaviour
{
    MeshRenderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponentsInChildren<MeshRenderer>().Where(mr => mr.name.Equals("Plane")).First();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Color rooms according to their position in the world
        m_renderer.material.color = new Color(ColorClamp(transform.position.x), ColorClamp(transform.position.x * transform.position.z), ColorClamp(transform.position.z));
    }

    // Returns a value between 0 - 1
    float ColorClamp(float f)
    {
        return (f % 255f) / 255;
    }
}
