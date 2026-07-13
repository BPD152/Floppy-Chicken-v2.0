using UnityEngine;

public class BackgroundControler : MonoBehaviour
{
    public float MoveSpeed;

    public float EndX;
    public float RespawnX;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
       
        if (transform.position.x < EndX)
        {
            transform.position = new Vector3(RespawnX, transform.position.y, transform.position.z);
        }
    }
}

