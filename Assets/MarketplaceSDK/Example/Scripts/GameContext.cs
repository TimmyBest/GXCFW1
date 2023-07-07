using MarketplaceSDK.Example.Game.Creator;
using MarketplaceSDK.Example.Game.Enum;
using MarketplaceSDK.Example.Game.Input;
using MarketplaceSDK.Example.Game.Models;
using MarketplaceSDK.Example.Game.Particle.Provider;
using MarketplaceSDK.Example.Game.Player.Movement;
using MarketplaceSDK.Example.Game.Provider;
using MarketplaceSDK.Example.Game.UI;
using MarketplaceSDK.Example.Interfaces;
using MarketplaceSDK.Core.Models;
using System;
using System.Collections.Generic;
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

        private void Awake()
        {
            OnInitializeCell();

            _UIContext.OpenLoginWindow();
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