using System;
using System.Collections.Generic;
using AOT;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Cdm.Authentication.Browser
{
    public class WebGLAuthenticationSession : IDisposable
    {
        private static readonly Dictionary<string, WebGLAuthenticationSessionCompletionHandler>
        CompletionCallbacks = new Dictionary<string, WebGLAuthenticationSessionCompletionHandler>();

        public WebGLAuthenticationSession(string url, WebGLAuthenticationSessionCompletionHandler completionHandler)
        {
            StartAuth(url, OnAuthenticationSessionCompleted);

            CompletionCallbacks.Add(url, completionHandler);
        }

        public void Dispose()
        {
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void StartAuth(string url, AuthenticationSessionCompletedCallback completionHandler);
#else
        private static void StartAuth(string url, AuthenticationSessionCompletedCallback completionHandler)
        {
            throw new NotImplementedException("Only WebGL platform is supported.");
        }
#endif

        public delegate void WebGLAuthenticationSessionCompletionHandler(string callbackUrl);

        private delegate void AuthenticationSessionCompletedCallback(string url, string callbackUrl);

        [MonoPInvokeCallback(typeof(AuthenticationSessionCompletedCallback))]
        private static void OnAuthenticationSessionCompleted(string url, string callbackUrl)
        {
            if (CompletionCallbacks.TryGetValue(url, out var callback))
            {
                callback?.Invoke(callbackUrl);
            }
        }
    }
}