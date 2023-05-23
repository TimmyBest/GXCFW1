using System.Threading.Tasks;
using UnityEngine.Networking;

namespace MarketplaceSDK.Https
{
    public class HttpClient
    {
        public async Task<string> PostRequest(string uri)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, ""))
            {
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