using KeepsakeSDK.Example.Game.Models;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        private Cell _currentCell = new(0, 0);

        private bool _moving = false;
        private bool _touch = false;

        private bool _isGame;

        private GridModel _gridModel;

        private float _speed;
        private float _jumpStartTime = 0;
        private float _horizontalSpeed = 2.5f;
        private float _jumpHeight = 1f;
        private float _jumpDuration = 0.75f;
        private float _offsetDuration = 25f;

        public void OnTouch() { _touch = true; }

        public void OnSetupPlayer(float speed, Vector3 scale)
        {
            transform.position = new Vector3(transform.position.x, scale.y * 0.5f, transform.position.z);

            _speed = speed;
            _jumpDuration = _jumpDuration - _speed / _offsetDuration;
            transform.localScale = scale;
        }

        public void OnSetupGrid(GridModel gridModel) { _gridModel = gridModel; }

        public void OnSetupGame(bool isGame) { _isGame = isGame; } 

        private void MoveOnNextPosition()
        {
            if (_moving && !_isGame) return;

            int incrementX = Random.Range(-1, 2);
            int incrementY = Random.Range(-1, 2);

            if (_currentCell.RowNumber == 0) incrementX = Random.Range(0, 2);
            else if (_currentCell.RowNumber == 9) incrementX = Random.Range(-1, 1);

            if (_currentCell.ColumnNumber == 0) incrementY = Random.Range(0, 2);
            else if (_currentCell.ColumnNumber == 9) incrementY = Random.Range(-1, 1);

            if (incrementX == 0 && incrementY == 0)
            {
                int randomInt = Random.Range(0, 2);

                if (randomInt == 0 && _currentCell.ColumnNumber != 0)
                {
                    incrementY = -1;
                }
                else if (_currentCell.ColumnNumber != 9)
                {
                    incrementY = 1;
                }
            }

            _currentCell.RowNumber = _currentCell.RowNumber + incrementX;
            _currentCell.ColumnNumber = _currentCell.ColumnNumber + incrementY;

            _moving = true;
        }

        private void Update()
        {
            if (!_isGame) return;

            if (_touch && !_moving)
            {
                MoveOnNextPosition();

                _jumpStartTime = Time.time;

                _touch = false;
                _moving = true;
            }

            if (_moving)
            {
            if (_speed == 0f) _moving = false;
            float timeElapsed = Time.time - _jumpStartTime;

                float jumpHeightAtTime = Mathf.Sin(timeElapsed / _jumpDuration * Mathf.PI) * _jumpHeight;
                jumpHeightAtTime = jumpHeightAtTime + _gridModel.cellPositions[_currentCell.RowNumber, _currentCell.ColumnNumber].y;
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = 
                    new Vector3(
                        _gridModel.cellPositions[_currentCell.RowNumber, _currentCell.ColumnNumber].x + _horizontalSpeed * Time.deltaTime,
                        jumpHeightAtTime,
                        _gridModel.cellPositions[_currentCell.RowNumber, _currentCell.ColumnNumber].z + _horizontalSpeed * Time.deltaTime
                );

                transform.position = Vector3.MoveTowards(currentPosition, targetPosition, _horizontalSpeed * _speed * Time.deltaTime);
                if (Vector3.Distance(currentPosition, new Vector3(
                    targetPosition.x, _gridModel.cellPositions[_currentCell.RowNumber, _currentCell.ColumnNumber].y, targetPosition.z)) < 0.1f) 
                {
                    _moving = false;
                    _jumpStartTime = 0;
                }
            }
        }
    }
}