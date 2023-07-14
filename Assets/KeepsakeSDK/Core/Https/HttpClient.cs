using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace KeepsakeSDK.Core.Https
{
    public class HttpClient
    {
        public async Task<string> PostRequest(string uri, string requestBody)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, ""))
            {
                byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(requestBody);

                www.uploadHandler = new UploadHandlerRaw(bodyData);
                www.disposeUploadHandlerOnDispose = true;
                www.SetRequestHeader("Content-Type", "application/json");

                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    return www.downloadHandler.text;
                }
                else
                {
                    return www.error;
                }
            }
        }

        public async Task<string> GetRequest(string uri, string requestBody)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(requestBody);

                www.uploadHandler = new UploadHandlerRaw(bodyData);
                www.disposeUploadHandlerOnDispose = true;
                www.SetRequestHeader("Content-Type", "application/json");

                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    return www.downloadHandler.text;
                }
                else
                {
                    return www.error;
                }
            }
        }

        public async Task<string> PostRequestWithAuthorization(string uri, string requestBody, string keyAuth, string valueAuth)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, ""))
            {
                byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(requestBody);

                www.uploadHandler = new UploadHandlerRaw(bodyData);
                www.disposeUploadHandlerOnDispose = true;
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader(keyAuth, valueAuth);

                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    return www.downloadHandler.text;
                }
                else
                {
                    return www.error;
                }
            }
        }

        public async Task<string> GetRequestWithAuthorization(string uri, string requestBody, string keyAuth, string valueAuth)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(requestBody);

                www.uploadHandler = new UploadHandlerRaw(bodyData);
                www.disposeUploadHandlerOnDispose = true;
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader(keyAuth, valueAuth);

                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    return www.downloadHandler.text;
                }
                else
                {
                    return www.error;
                }
            }
        }
    }

    public static class UnityWebRequestExtension
    {
        public static TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation reqOp)
        {
            TaskCompletionSource<UnityWebRequest.Result> tsc = new TaskCompletionSource<UnityWebRequest.Result>();
            reqOp.completed += asyncOp => tsc.TrySetResult(reqOp.webRequest.result);

            if (reqOp.isDone)
                tsc.TrySetResult(reqOp.webRequest.result);

            return tsc.Task.GetAwaiter();
        }
    }
}