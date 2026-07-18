using System;
using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace Controller
{
    public static class TimeService
    {
        private const string IpApiUrl = "https://ipwho.is/";
        private const string TimeApiUrl = "https://time.now/developer/api/timezone/";

        public static async UniTask<TimeApiResponse> GetCurrentLocalTime()
        {
            using var ipRequest = UnityWebRequest.Get(IpApiUrl);

            await ipRequest.SendWebRequest();

            if (ipRequest.result != UnityWebRequest.Result.Success)
                throw new Exception(ipRequest.error);

            var ipData = JsonUtility.FromJson<IpApiResponse>(
                ipRequest.downloadHandler.text);

            if (ipData == null || ipData.timezone == null || string.IsNullOrEmpty(ipData.timezone.id))
                throw new Exception("Failed to get timezone from IP");

            var timezone = ipData.timezone.id;

            using var timeRequest = UnityWebRequest.Get(
                TimeApiUrl + timezone);

            await timeRequest.SendWebRequest();

            if (timeRequest.result != UnityWebRequest.Result.Success)
                throw new Exception(
                    $"{timeRequest.responseCode}: {timeRequest.downloadHandler.text}");
            
            return JsonUtility.FromJson<TimeApiResponse>(
                timeRequest.downloadHandler.text);
        }
    }
}