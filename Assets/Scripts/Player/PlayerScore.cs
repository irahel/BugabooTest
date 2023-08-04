using UnityEngine;
using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> score = new(0);
    public static event System.Action<int> ChangedLengthEvent;

    public override void OnNetworkSpawn()
    {
        score.OnValueChanged += ScoreChanged;
    }

    [ServerRpc(RequireOwnership = false)]
    public void addScoreServerRpc(int scorePoints)
    {
        score.Value += scorePoints;
    }

    private void ScoreChanged(int previousValue, int newValue)
    {
        if(!IsOwner) return;
        ChangedLengthEvent?.Invoke(score.Value);
    }
}
