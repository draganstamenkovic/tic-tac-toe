using System;
using Controller;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace View
{
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        public async void ShowTime()
        {
            try
            {
                gameObject.SetActive(true);
                await UpdateTimeLoopAsync();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void HideTime()
        {
            gameObject.SetActive(false);
        }

        private async UniTask UpdateTimeLoopAsync()
        {
            while (gameObject.activeInHierarchy)
            {
                try
                {
                    var time = await TimeService.GetCurrentLocalTime();
                    if (timeText != null)
                    {
                        var dateTime = DateTime.Parse(time.datetime);
                        timeText.text = $"Current Time: {dateTime.Hour:D2}:{dateTime.Minute:D2}:{dateTime.Second:D2}";
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to fetch time: {e.Message}");
                    if (timeText != null)
                        timeText.text = "Current Time: Unavailable";
                }

                await UniTask.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }
}