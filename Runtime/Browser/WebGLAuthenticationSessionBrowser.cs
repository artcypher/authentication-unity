using System.Threading;
using System.Threading.Tasks;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Cdm.Authentication.Browser
{
    public class WebGLAuthenticationSessionBrowser : IBrowser
    {
        private TaskCompletionSource<BrowserResult> _taskCompletionSource;

        public async Task<BrowserResult> StartAsync(
            string loginUrl, string redirectUrl, CancellationToken cancellationToken = default)
        {
            _taskCompletionSource = new TaskCompletionSource<BrowserResult>();

            cancellationToken.Register(() =>
            {
                _taskCompletionSource?.TrySetCanceled();
            });

            using var authenticationSession = new WebGLAuthenticationSession(loginUrl, AuthenticationSessionCompletionHandler);

            return await _taskCompletionSource.Task;
        }

        private void AuthenticationSessionCompletionHandler(string callbackUrl)
        {
            _taskCompletionSource.SetResult(new BrowserResult(BrowserStatus.Success, callbackUrl));
        }
    }
}