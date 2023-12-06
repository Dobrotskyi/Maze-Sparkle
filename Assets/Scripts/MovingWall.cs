using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] private Vector3 _firstPoint;
    [SerializeField] private Vector3 _secondPoint;
    [SerializeField] private float _speed;
    private bool _movingToSecond = true;

    private void Awake()
    {
        transform.position = _firstPoint;
    }

    private void Update()
    {
        if (_movingToSecond)
            MoveWallTowards(_secondPoint);
        else
            MoveWallTowards(_firstPoint);
    }

    private void MoveWallTowards(Vector3 point)
    {
        transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, point) < 0.001f)
            _movingToSecond = !_movingToSecond;
    }
}
