using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Cdm.Authentication.Utils
{
    public static class UserInfoParser
    {
        public static async Task<IUserInfo> GetUserInfoAsync<T>(HttpClient httpClient, string url,
            AuthenticationHeaderValue authenticationHeader, CancellationToken cancellationToken = default)
            where T : IUserInfo
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var authenticationHeaderString = authenticationHeader.ToString();

            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", authenticationHeaderString);
#else
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = authenticationHeader;
#endif

#if UNITY_EDITOR
            Debug.Log($"{request}");
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            await RequestEnd(request, cancellationToken);

            if (request.result == UnityWebRequest.Result.Success) {
                var content = request.downloadHandler.text;
                var userInfo = JsonConvert.DeserializeObject<T>(content);
                return userInfo;
            }
#else
            var response = await httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<T>(content);

#if UNITY_EDITOR
                Debug.Log(content);
#endif

                return userInfo;
            }
#endif

            throw new Exception("User info could not parsed.");
        }

        private static async Task RequestEnd(UnityWebRequest request, CancellationToken cancellationToken = default)
        {
            request.SendWebRequest();

            while (!request.isDone) {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                await Task.Yield();
            }
        }
    }
}