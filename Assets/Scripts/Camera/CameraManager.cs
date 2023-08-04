using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _finalPos;
    private Transform _player;

    private Vector3 targetPosition;
    private Vector3 targetRotation;

    private bool _isMoving = false;
    private float _moveSpeed = 2.0f;

    void Update()
    {
        if (_isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * _moveSpeed * 0.5f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f && Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRotation)) < 1f)
            {
                transform.SetParent(_player);
                _isMoving = false;
                _player.GetComponent<Player>().EndCamAnim();
            }
        }
    }

    public void SetPlayerReference(Transform finalPosition, Transform newPlayer)
    {
        _finalPos = finalPosition;
        targetPosition = new Vector3(_finalPos.position.x + 0.5f, _finalPos.position.y + 3f, _finalPos.position.z - 2.5f);
        targetRotation = new Vector3(0f, 0f, 0f);
        _player = newPlayer;
        _isMoving = true;
    }
}