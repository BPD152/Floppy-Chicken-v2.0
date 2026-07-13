using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject Item;
    public float SpawnRate;
    public float Offset;

    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < SpawnRate)
        {
            timer = timer + Time.deltaTime;
        }    
        else
        {
            SpawnPipe();
            timer = 0;
        }
    }

    void SpawnPipe()
    {
        float LowestPoint = transform.position.y - Offset;
        float HighestPoint = transform.position.y + Offset;

        Instantiate(Item, new Vector3(transform.position.x, Random.Range(HighestPoint,LowestPoint),0), transform.rotation);
    }    
}
