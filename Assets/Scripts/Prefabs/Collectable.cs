using UnityEngine;
using Unity.Netcode;

public class Collectable : NetworkBehaviour
{
    [SerializeField] private int points = 100;
    private bool actived = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || actived) return;

        if (other.TryGetComponent(out PlayerScore playerScore))
        {
            playerScore.addScoreServerRpc(points);
            actived = true;
        }

        DespawnCollectableServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnCollectableServerRpc()
    {
        DespawnCollectableClientRpc();
        GetComponent<Animator>().SetTrigger("Break");
        GetComponent<BoxCollider>().enabled = false;
    }

    [ClientRpc]
    public void DespawnCollectableClientRpc()
    {
        if (!IsOwner) return;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().SetTrigger("Break");
        Invoke("DestroyItem", 1.2f);
    }

    private void DestroyItem()
    {
        NetworkObject.Despawn();
    }
}