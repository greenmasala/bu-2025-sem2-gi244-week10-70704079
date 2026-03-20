using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;
    private float normalSpeed;
    private float dashSpeed;

    private float leftBound = -15;

    private PlayerController playerController;

    void Start()
    {
        normalSpeed = speed;
        dashSpeed = speed * 2;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.IsDashing)
        {
            speed = dashSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

        if (!playerController.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
