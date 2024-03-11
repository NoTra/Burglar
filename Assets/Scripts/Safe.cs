﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace burglar
{
    public class Safe : Interactible
    {
        [Range(1, 5)]
        [SerializeField] private int _level;
        private int[,] _matrix;
        [SerializeField] private int _combinationLength;
        private int[] _combination;

        private List<Vector2> _selectedCombination = new List<Vector2>();

        private bool _isCracked = false;
        [SerializeField] private int _value = 500;


        private void OnEnable()
        {
            EventManager.SuccessSafeCrack += (safe) => OnSuccessSafeCrack(safe);
            EventManager.FailSafeCrack += () => OnFailSafeCrack();
        }

        private void OnSuccessSafeCrack(Safe safe)
        {
            safe._isCracked = true;
            EventManager.OnCreditCollected(safe._value);
        }

        private void OnDisable()
        {
            EventManager.SuccessSafeCrack -= (safe) => OnSuccessSafeCrack(safe);
            EventManager.FailSafeCrack -= () => OnFailSafeCrack();
        }

        private void OnFailSafeCrack()
        {
            _selectedCombination.Clear();

            Debug.Log("Combination failed");
        }

        void Start()
        {
            GenerateMatrix();
            
            GenerateCombination();
        }

        private void GenerateMatrix()
        {
            var nbRow = _level + 1;
            var nbCol = _level + 1;

            var random = new System.Random();
            _matrix = new int[nbRow, nbCol];

            for (int i = 0; i < nbRow; i++)
            {
                for (int j = 0; j < nbCol; j++)
                {
                    _matrix[i, j] = random.Next(0, 10);
                }
            }

            // Debug
            for (int i = 0; i < nbCol; i++)
            {
                for (int j = 0; j < nbRow; j++)
                {
                    Debug.Log("[" + i + "," + j + "] " + _matrix[i, j]);

                    if (i % _level + 1 == 0)
                    {
                        Debug.Log(" ");
                    }
                }
            }
        }

        private void GenerateCombination()
        {
            _combination = new int[_combinationLength];
            // Generate a random possible combination from the matrix
            // knowing that you can't use the same position twice and
            // that the combination can be done be going north, south, east or west
            var random = new System.Random();
            var row = random.Next(0, _level + 1);
            var col = random.Next(0, _level + 1);

            // Keep used positions
            var usedPositions = new List<int2>();

            // Starting position
            _combination[0] = _matrix[row, col];
            usedPositions.Add(new int2(row, col));

            for (int i = 1; i < _combinationLength; i++)
            {
                var direction = random.Next(0, 4);

                // Check if the direction is valid, if not generate a new one
                while (
                        direction == 0 && (row - 1 <= 0 || usedPositions.Contains(new int2(row - 1, col))) ||
                        direction == 1 && (row + 1 > _level || usedPositions.Contains(new int2(row + 1, col))) ||
                        direction == 2 && (col - 1 <= 0 || usedPositions.Contains(new int2(row, col - 1))) ||
                        direction == 3 && (col + 1 > _level || usedPositions.Contains(new int2(row, col + 1)))
                )
                {
                    direction = random.Next(0, 4);
                }

                switch (direction)
                {
                    // North
                    case 0:
                        if (row - 1 >= 0)
                        {
                            row--;
                            _combination[i] = _matrix[row, col];
                        }
                        break;
                    // South
                    case 1:
                        if (row + 1 <= _level)
                        {
                            row++;
                            _combination[i] = _matrix[row, col];
                        }
                        break;
                    // West
                    case 2:
                        if (col - 1 >= 0)
                        {
                            col--;
                            _combination[i] = _matrix[row, col];
                        }
                        break;
                    // East
                    case 3:
                        if (col + 1 <= _level)
                        {
                            col++;
                            _combination[i] = _matrix[row, col];
                        }
                        break;
                }

                usedPositions.Add(new int2(row, col));
            }

            // Show Combination found
            for (int j = 0; j < _combinationLength; j++)
            {
                Debug.Log("Combination : " + _combination[j]);
            }
        }

        protected override void Interact()
        {
            if (!_isCracked)
            {
                EventManager.OnOpenSafe(this);
            } else
            {
                Debug.Log("The safe is already cracked");
            }
        }

        public int GetLevel()
        {
            return _level;
        }

        public int[,] GetMatrix()
        {
            return _matrix;
        }

        public int[] GetCombination()
        {
            return _combination;
        }

        public int GetCombinationLength()
        {
            return _combinationLength;
        }

        public List<Vector2> GetSelectedCombination()
        {
            return _selectedCombination;
        }

        public int GetSelectedCombinationLength() {
            return _selectedCombination.Count;
        }

        public string GetStringCombination()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _combinationLength; i++)
            {
                sb.Append(_combination[i]);
            }

            return sb.ToString();
        }

        public void AddToCombination(Vector2 coordinates)
        {
            _selectedCombination.Add(coordinates);

            UIManager.Instance.UpdateGrid(this, coordinates);
        }

        public bool CheckCombination()
        {
            for (int i = 0; i < _combinationLength; i++)
            {
                if (_combination[i] != _matrix[(int)_selectedCombination[i].x, (int)_selectedCombination[i].y])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
