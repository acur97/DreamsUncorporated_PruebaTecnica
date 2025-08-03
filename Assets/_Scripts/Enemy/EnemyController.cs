using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void MoveHorizontal(bool right)
    {
        transform.localPosition = right ? new Vector2(transform.localPosition.x + 0.2f, transform.localPosition.y) :
                                            new Vector2(transform.localPosition.x - 0.2f, transform.localPosition.y);
    }

    public void MoveDown()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 0.2f);
    }
}