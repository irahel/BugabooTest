using UnityEngine;
using TMPro;
using Unity.Netcode;

public class Timer : NetworkBehaviour
{
    private float totalTime = 20f; // 5min
    private float _currentTimeInSeconds;
    private TextMeshProUGUI _timerText;

    private bool _started = false;

    public NetworkVariable<bool> _timeOver = new(false);
    public static event System.Action<bool> TimerOverEvent;

    [SerializeField] private GameObject WaitText;

    public override void OnNetworkSpawn()
    {
        _timeOver.OnValueChanged += TimerOverChanged;
    }

    private void OnEnable()
    {
        _currentTimeInSeconds = totalTime;
        _timerText = GetComponent<TextMeshProUGUI>();
        _timerText.text = "5:00";

        GameManager.StartTimerEvent += ChangeTimerEvent;
    }

    private void OnDisable()
    {
        GameManager.StartTimerEvent -= ChangeTimerEvent;
    }

    private void ChangeTimerEvent(bool changed)
    {
        _started = true;
        Destroy(WaitText);
    }

    private void Update()
    {
        if (!_started) return;

        if (_currentTimeInSeconds > 0f)
        {
            _currentTimeInSeconds -= Time.deltaTime;
            UpdateTimerText();
        }
        else if (!_timeOver.Value)
        {
            _timerText.text = "0:00";
            _timeOver.Value = true;
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_currentTimeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(_currentTimeInSeconds % 60f);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimerOverChanged(bool previousValue, bool newValue)
    {
        TimerOverEvent?.Invoke(_timeOver.Value);
    }
}
