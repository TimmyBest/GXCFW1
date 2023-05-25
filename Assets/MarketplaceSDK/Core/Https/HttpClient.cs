using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MarketplaceSDK.Https
{
    public class HttpClient
    {
        public async Task<string> PostRequest(string uri, string requestBody)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, "POST"))
            {
                byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(requestBody);

                www.uploadHandler = new UploadHandlerRaw(bodyData);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.disposeUploadHandlerOnDispose = true;
                www.disposeDownloadHandlerOnDispose = true;
                www.SetRequestHeader("Content-Type", "application/json");

                var asyncOperation = www.SendWebRequest();

                while (!asyncOperation.isDone)
                {
                    await Task.Yield();
                }

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
}