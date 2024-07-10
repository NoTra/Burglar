using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using burglar.managers;

namespace burglar.environment
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
            EventManager.FailSafeCrack += (safe) => OnFailSafeCrack(safe);
        }

        private void OnDisable()
        {
            EventManager.SuccessSafeCrack -= (safe) => OnSuccessSafeCrack(safe);
            EventManager.FailSafeCrack -= (safe) => OnFailSafeCrack(safe);
        }

        private void Start()
        {
            GenerateMatrix();
            
            GenerateCombination();
        }

        public void SetLevel(int newLevel)
        {
            _level = newLevel;
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
        }

        private void GenerateCombination()
        {
            _combination = new int[_combinationLength];
            // Generate a random possible combination from the matrix
            // knowing that you can't use the same position twice and
            // that the combination can be done be going north, south, east or west
            var random = new System.Random();
            var row = random.Next(0, _level);
            var col = random.Next(0, _level);

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
        }

        protected override void Interact()
        {
            if (!_isCracked)
            {
                EventManager.OnOpenSafe(this);
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

            UIManager.Instance.UpdateSafeGrid(this, coordinates);
        }

        public bool CheckCombination()
        {
            for (var i = 0; i < _combinationLength; i++)
            {
                if (_combination[i] != _matrix[(int)_selectedCombination[i].x, (int)_selectedCombination[i].y])
                {
                    return false;
                }
            }

            return true;
        }

        private void OnSuccessSafeCrack(Safe safe)
        {
            safe._isCracked = true;
            safe.GetComponent<Interactible>().enabled = false;
            
            EventManager.OnCreditCollected(safe._value);
        }

        private void OnFailSafeCrack(Safe safe)
        {
            _selectedCombination.Clear();
        }
    }
}
