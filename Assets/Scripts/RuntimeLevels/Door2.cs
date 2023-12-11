using UnityEngine;

public class Door2 : MonoBehaviour, IDoor
{
    [SerializeField] BoxCollider blockingCollider;
    [SerializeField] MeshRenderer doorRenderer;
    private bool _isClosed = false;

    public void SetClosedState(bool isClosed)
    {
        _isClosed = isClosed;
        blockingCollider.enabled = _isClosed;
        doorRenderer.enabled = _isClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isClosed && other.CompareTag("Player") && other.GetComponent<PlayerInventory>().HasKeys())
        {
            SetClosedState(!_isClosed);
        }
    }
}
