using UnityEngine;

public class GameManager: MonoBehaviour
{
    public float speed = .5f;
    private Renderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector3(Time.time * speed,0);
        renderer.material.mainTextureOffset = offset;
    }
}
