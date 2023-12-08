using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dragable : MonoBehaviour
{
    private Rigidbody2D _rb;

    public void PositionSet()
    {
        _rb.velocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void DragTowards(Vector2 point)
    {
        _rb.velocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 direction = point - (Vector2)transform.position;
        _rb.MovePosition((Vector2)transform.position + direction * Time.deltaTime * 200f);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
}
