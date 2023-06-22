using MarketplaceSDK.Example.Game.Creator;
using MarketplaceSDK.Example.Game.Enum;
using MarketplaceSDK.Example.Game.Input;
using MarketplaceSDK.Example.Game.Models;
using MarketplaceSDK.Example.Game.Particle.Provider;
using MarketplaceSDK.Example.Game.Player.Movement;
using MarketplaceSDK.Example.Game.Provider;
using MarketplaceSDK.Example.Game.UI;
using MarketplaceSDK.Example.Interfaces;
using MarketplaceSDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MarketplaceSDK.Example.Game
{
    public class GameContext : MonoBehaviour
    {
        private IPersonCreator _personCreator = new PersonCreator();

        private Action<bool> _actionGame;

        [SerializeField] private GameObject _prefabPlayer;
        [SerializeField] private GameObject _prefabCritter;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private GameObject _particleSplash;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _timerText;
        [SerializeField] private UIContext _UIContext;

        private GameObject _currentPlayer;

        private GridModel _gridModel;
        private CritterCell[,] _cellCritters = new CritterCell[10, 10];

        private Vector3[] _vectorOffsets = new Vector3[] {
            new Vector3(-0.35f, 0f, 0.35f), new Vector3(0f, 0f, 0.35f), new Vector3(0.35f, 0f, 0.35f),
            new Vector3(-0.35f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0.35f, 0f, 0f),
            new Vector3(-0.35f, 0f, -0.35f), new Vector3(0f, 0f, -0.35f), new Vector3(0.35f, 0f, -0.35f),
        };

        private int _score = 0;
        private float _timer = 1f;
        private float _mainTimer = 60f;
        private float _multiplyRotateCamera = 10f;
        private bool _isGame = false;

        private const string CUBIX_NAME = "Cubix";
        private const string SPHERIX_NAME = "Spherix";

        private string _nickname = "";
        private string _walletId = "";

        private void isGame(bool isGame) { _isGame = isGame; }


        private byte[] SerializeToByteArray(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        private T DeserializeFromByteArray<T>(byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(stream);
            }
        }


        private async void Awake()
        {
            //string sessionToken = await MarketplaceSDK.OnCreateSession("anewsecretkey");
            //string walletId = await MarketplaceSDK.OnCreateWallet(sessionToken, "tim_new_forwardgame");
            //string walletId = await MarketplaceSDK.GetWallet("tim_new_forwardgame");
            //string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            //string signature = await MarketplaceSDK.SignPersonalMessage("tim_new_forwardgame", timestamp, sessionToken);
            //string token = await MarketplaceSDK.LoginToKeepsake(signature, timestamp);
            //string gaslessTxKiosk = await MarketplaceSDK.MakeObKiosk(token);
            //ResultDev rootDev = await MarketplaceSDK.DevInspectTransactionBlock(walletId, gaslessTxKiosk);
            //await MarketplaceSDK.ExecuteGaslessTransactionBlock("tim_new_forwardgame", sessionToken, gaslessTxKiosk, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);
            //CoinRootOwned coinRoot = await MarketplaceSDK.GetOwnedObjectCoins(walletId);
            //KioskRootOwned kioskRoot = await MarketplaceSDK.GetOwnedObjectKiosk(walletId);
            //string gaslessTx = await MarketplaceSDK.BuildBuyTransaction(token, result.Id, coinRoot.Result.Data[0].Data.ObjectId, kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            //ResultDev rootDev = await MarketplaceSDK.DevInspectTransactionBlock(walletId, gaslessTx);
            //string response = await MarketplaceSDK.ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);
            //Debug.Log(response);
            //Debug.Log(await MarketplaceSDK.GetMyListing(token));
            /*
            byte[] bytes = SerializeToByteArray(result);

            // Convert the byte array to a Base64 string
            string base64String = Convert.ToBase64String(bytes);

            // Save the Base64 string in PlayerPrefs
            PlayerPrefs.SetString("Result", base64String);

            // Save PlayerPrefs to disk (optional)
            PlayerPrefs.Save();

            Debug.Log("Custom class saved to PlayerPrefs.");

            // Load the Base64 string from PlayerPrefs
            string bas64String = PlayerPrefs.GetString("Result");

            if (!string.IsNullOrEmpty(bas64String))
            {
                // Convert the Base64 string to a byte array
                byte[] byes = Convert.FromBase64String(bas64String);

                // Deserialize the byte array back to your custom class
                Result loadedData = DeserializeFromByteArray<Result>(byes);

                // Use the loaded data
                Debug.Log("Value: " + loadedData.Nft.Name);
                Debug.Log("Text: " + loadedData.Nft.Description);
            }
            else
            {
                Debug.Log("No custom class data found in PlayerPrefs.");
            }
            */

            //_UIContext.OpenMainMenu();
            _UIContext.OpenLoginWindow(AuthorizationAPI);
            OnInitializeCell();
            //_actionGame += isGame;

            //InitGame(result);
        }

        public async void BuyNFT(string nftId)
        {
            _UIContext.ActivityIndicatorItem.Open();
            string sessionToken = await MarketplaceSDK.OnCreateSession("anewsecretkey");
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await MarketplaceSDK.SignPersonalMessage(_nickname, timestamp, sessionToken);
            string token = await MarketplaceSDK.LoginToKeepsake(signature, timestamp);
            CoinRootOwned coinRoot = await MarketplaceSDK.GetOwnedObjectCoins(_walletId);
            KioskRootOwned kioskRoot = await MarketplaceSDK.GetOwnedObjectKiosk(_walletId);
            string gaslessTx = await MarketplaceSDK.BuildBuyTransaction(token, nftId, FindHighestNumber(coinRoot.Result.Data), kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            ResultDev rootDev = await MarketplaceSDK.DevInspectTransactionBlock(_walletId, gaslessTx);
            string response = await MarketplaceSDK.ExecuteGaslessTransactionBlock(_nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);
            Debug.Log(response);

            AuthorizationAPI(_nickname);
        }

        public async void UnlistAsset(string nftId)
        {
            _UIContext.ActivityIndicatorItem.Open();
            string sessionToken = await MarketplaceSDK.OnCreateSession("anewsecretkey");
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await MarketplaceSDK.SignPersonalMessage(_nickname, timestamp, sessionToken);
            string token = await MarketplaceSDK.LoginToKeepsake(signature, timestamp);
            string gaslessTx = await MarketplaceSDK.UnlistAsset(nftId, token);
            ResultDev rootDev = await MarketplaceSDK.DevInspectTransactionBlock(_walletId, gaslessTx);
            string response = await MarketplaceSDK.ExecuteGaslessTransactionBlock(_nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);
            Debug.Log(response);

            AuthorizationAPI(_nickname);
        }

        public async void SellNFT(string nftId, double amount)
        {
            _UIContext.ActivityIndicatorItem.Open();
            string sessionToken = await MarketplaceSDK.OnCreateSession("anewsecretkey");
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await MarketplaceSDK.SignPersonalMessage(_nickname, timestamp, sessionToken);
            string token = await MarketplaceSDK.LoginToKeepsake(signature, timestamp);
            KioskRootOwned kioskRoot = await MarketplaceSDK.GetOwnedObjectKiosk(_walletId);
            string gaslessTx = await MarketplaceSDK.BuildSellTransaction(token, nftId, (amount * 1000000000).ToString(), kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            ResultDev rootDev = await MarketplaceSDK.DevInspectTransactionBlock(_walletId, gaslessTx);
            string response = await MarketplaceSDK.ExecuteGaslessTransactionBlock(_nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);
            Debug.Log(response);

            AuthorizationAPI(_nickname);
        }

        public async void AuthorizationAPI(string nickname)
        {
            _UIContext.ActivityIndicatorItem.Open();

            string walletId = await MarketplaceSDK.GetWallet(nickname);
            RootBalance coinRoot = await MarketplaceSDK.GetWalletBalance(walletId);
            double balance = coinRoot.Result.TotalBalance / 1000000000;

            _nickname = nickname;
            _walletId = walletId;

            _UIContext.ActivityIndicatorItem.Close();
            _UIContext.OpenMainMenu(nickname, walletId, balance.ToString());
        }

        public async Task UpdateWindows()
        {
            _UIContext.ActivityIndicatorItem.Open();

            Root root = await MarketplaceSDK.OnSearchListing(1, "6462c8af23a2b24070683fd1", "Whacky Cube Smash", 10000000, 10000000000000);

            KioskRootOwned kioskRoot = await MarketplaceSDK.GetOwnedObjectKiosk(_walletId);
            RootDynamic rootDynamic = await MarketplaceSDK.GetDynamicField(kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            RootObjectType rootObjectType = await MarketplaceSDK.GetObjectType("6462c8af23a2b24070683fd1");
            List<string> objects = new();

            foreach (DataDynamic answer in rootDynamic.Result.Data)
            {
                if (answer.ObjectType == rootObjectType.Collection.FullType)
                {
                    objects.Add(answer.ObjectId);
                }
            }

            RootMulti rootNft = await MarketplaceSDK.GetMultiObjects(objects.ToArray());

            string sessionToken = await MarketplaceSDK.OnCreateSession("anewsecretkey");
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await MarketplaceSDK.SignPersonalMessage(_nickname, timestamp, sessionToken);
            string token = await MarketplaceSDK.LoginToKeepsake(signature, timestamp);
            Root rootMyListing = await MarketplaceSDK.GetMyListing(token);

            _UIContext.OpenMyNFT(InitGame, SellNFT, UnlistAsset, rootNft.Result, rootMyListing.Results, kioskRoot.Result.Data[0].Data.Display.Data.Kiosk, false);
            _UIContext.OpenMarket(BuyNFT, UnlistAsset, root.Results, kioskRoot.Result.Data[0].Data.Display.Data.Kiosk, false);

            _UIContext.ActivityIndicatorItem.Close();
        }

        public void TunningPersonAction(Result result)
        {
            Vector3 scale = new Vector3(result.Nft.Fields.Size, result.Nft.Fields.Size, result.Nft.Fields.Size);
            float speed = result.Nft.Fields.Speed;

            int sideColor = (int)ColorType.Parse(typeof(ColorType), result.Nft.Fields.SideColor);
            int edgeColor = (int)ColorType.Parse(typeof(ColorType), result.Nft.Fields.EdgeColor);

            _gridModel = new GridModel(scale.y);

            if (_currentPlayer.TryGetComponent(out PlayerMovement movement))
            {
                movement.OnSetupPlayer(speed, scale);
                movement.OnSetupGrid(_gridModel);
            }

            _personCreator.TunningPerson(_currentPlayer, sideColor, edgeColor);

            _isGame = true;
            _UIContext.OpenInfoWindow();

            StartGame();
        }

        public void InitGame(Result result)
        {
            Vector3 scale = new Vector3(result.Nft.Fields.Size, result.Nft.Fields.Size, result.Nft.Fields.Size);
            float speed = result.Nft.Fields.Speed;

            _gridModel = new GridModel(scale.y);

            _currentPlayer = _personCreator.CreatePerson(_prefabPlayer, new Vector3(_gridModel.cellPositions[0, 0].x, 0f, _gridModel.cellPositions[0, 0].z),
                CUBIX_NAME, 1, 1);

            if (_currentPlayer.TryGetComponent(out PlayerMovement movement))
            {
                movement.OnSetupPlayer(speed, scale);
                movement.OnSetupGrid(_gridModel);

                _playerInput.onFire += movement.OnTouch;
                _actionGame += movement.OnSetupGame;
            }

            if (_currentPlayer.TryGetComponent(out PlayerComponentProvider provider))
            {
                provider.playerDetect.action += TouchTheCritter;
            }
            _playerInput.onHold += RotateCamera;

            TunningPersonAction(result);
        }

        private void OnApplicationQuit()
        {
            UnsubscribeActions();
        }

        private void UnsubscribeActions()
        {
            if (_currentPlayer != null)
            {
                if (_currentPlayer.TryGetComponent(out PlayerMovement movement))
                {
                    _playerInput.onFire -= movement.OnTouch;
                    _actionGame -= movement.OnSetupGame;
                }

                if (_currentPlayer.TryGetComponent(out PlayerComponentProvider provider))
                {
                    provider.playerDetect.action -= TouchTheCritter;
                }
            }
            _playerInput.onHold -= RotateCamera;
        }

        private void Update()
        {
            if (_isGame)
            {
                if (_mainTimer <= 0)
                {
                    SceneManager.LoadScene(0);
                }

                if (_timer <= 0)
                {
                    CreateCritter();
                    _timer = 1f;
                }

                _timer -= Time.deltaTime;
                _mainTimer -= Time.deltaTime;
                TimeSpan time = TimeSpan.FromSeconds(_mainTimer);
                string formattedTime = time.ToString("mm\\:ss");
                _timerText.text = formattedTime;
            }
        }

        private void RotateCamera()
        {
            if (_isGame)
            {
                float mouseY = UnityEngine.Input.GetAxis("Mouse X") * _multiplyRotateCamera;

                _cameraTransform.Rotate(Vector3.up, mouseY, Space.World);
            }
        }

        public void StartGame()
        {
            _actionGame?.Invoke(true);
        }

        public void FinishGame()
        {
            _actionGame?.Invoke(false);

            if (_currentPlayer.TryGetComponent(out PlayerMovement movement))
            {
                _playerInput.onFire -= movement.OnTouch;
                _actionGame -= movement.OnSetupGame;
            }

            if (_currentPlayer.TryGetComponent(out PlayerComponentProvider provider))
            {
                provider.playerDetect.action -= TouchTheCritter;
            }
            _playerInput.onHold -= RotateCamera;
        }

        private void OnInitializeCell()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _cellCritters[i, j] = new CritterCell(i, j);
                }
            }
        }

        public string FindHighestNumber(List<CoinDataOwned> coins)
        {
            if (coins == null || coins.Count == 0)
            {
                throw new ArgumentException("The array is null or empty.");
            }
            double highestNumber = double.Parse(coins[0].Data.Content.fields.Balance, CultureInfo.InvariantCulture.NumberFormat);
            string result = coins[0].Data.ObjectId;

            for (int i = 1; i < coins.Count; i++)
            {
                double currentNumber = double.Parse(coins[i].Data.Content.fields.Balance, CultureInfo.InvariantCulture.NumberFormat);

                if (currentNumber > highestNumber)
                {
                    highestNumber = currentNumber;
                    result = coins[i].Data.ObjectId;
                }
            }

            return result;
        }

        private void TouchTheCritter(Collider collider)
        {
            Vector3 posSplash = new Vector3(collider.transform.position.x, 0.025f, collider.transform.position.z);
            Color color = collider.GetComponent<MeshRenderer>().materials[1].color;
            GameObject splash = Instantiate(_particleSplash, posSplash, collider.transform.rotation);
            if (splash.TryGetComponent(out ParticleProvider provider))
            {
                provider.SetColorParticles(color);
            }
            Destroy(collider.gameObject);
            _score++;
            _scoreText.text = "Score: " + _score;
        }

        private void CreateCritter()
        {
            bool existFree = false;
            List<CritterCell> cells = new List<CritterCell>();
            foreach (CritterCell cell in _cellCritters)
            {
                existFree = cell.ExistFreeCell();
                if(existFree)
                    cells.Add(cell);
            }

            if (!existFree) return;

            int cellRandom = UnityEngine.Random.Range(0, cells.Count);
            CritterCell nextCell = cells[cellRandom];

            if (Vector3.Distance(_currentPlayer.transform.position, _gridModel.cellPositions[nextCell.RowNumber, nextCell.ColumnNumber]) < 0.5f) return;

            Vector3 newPos = _gridModel.cellPositions[nextCell.RowNumber, nextCell.ColumnNumber];
            List<CellModel> cellModels = new List<CellModel>();

            foreach (CellModel model in nextCell.ListOfCritters)
            {
                if (!model.engaged)
                {
                    cellModels.Add(model);
                }
            }
            if (cellModels.Count > 0)
            {
                int number = UnityEngine.Random.Range(0, cellModels.Count);
                newPos += _vectorOffsets[cellModels[number].number];
                newPos.y = 0.15f;
                CellModel newCritter = new CellModel(_personCreator.CreatePerson(_prefabCritter, newPos, SPHERIX_NAME, UnityEngine.Random.Range(1, 11),
                    UnityEngine.Random.Range(1, 11)), cellModels[number].number, newPos);

                _cellCritters[nextCell.RowNumber, nextCell.ColumnNumber].ListOfCritters[cellModels[number].number] = newCritter;
            }
        }
    }
}