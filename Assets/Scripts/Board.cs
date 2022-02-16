using System;
using UnityEngine;


namespace ThreeSpace.Chess
{
    public class Board : MonoBehaviour
    {
        // Camera Object
        CameraController camera;

        // Event Handlers
        public delegate void TileEventHandler(Tile tile);
        public delegate void PieceEventHandler(Piece piece);

        // Tile Events
        public event TileEventHandler OnMouseDown;
        public event TileEventHandler OnMouseUp;
        public event TileEventHandler OnMouseEnter;
        public event TileEventHandler OnMouseExit;

        // Game Variables
        // WHITE = false, BLACK = true
        public bool turn = false;

        // Current selected piece
        public Piece cur_piece;

        // Game loop Bool
        public bool isAlive = true;

        private Tile[,] _tiles;

        private Tile _lastHovered;

        private void Start()
        {
            GameObject view = GameObject.Find("Main Camera");
            camera = view.GetComponent<CameraController>();
        }

        void Awake()
        {
            _tiles = new Tile[6, 6];

            foreach (var tile in GetComponentsInChildren<Tile>())
            {
                _tiles[tile.Position.x, tile.Position.y] = tile;
            }
        }

        void Update()
        {
            if (isAlive)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    var tile = hit.collider.gameObject.GetComponent<Tile>();

                    UpdateHoveredTile(tile);

                    if (Input.GetMouseButtonDown(0))
                    {
                        OnMouseDown?.Invoke(tile);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        OnMouseUp?.Invoke(tile);
                    }
                }
                else
                {
                    UpdateHoveredTile(null);
                }
            }
            else
                End();
        }

        private void UpdateHoveredTile(Tile tile)
        {
            if (_lastHovered != tile)
            {
                OnMouseEnter?.Invoke(tile);
                OnMouseExit?.Invoke(_lastHovered);
                _lastHovered = tile;
            }
        }

        private void End()
        {
            // Added just for flavor and debugging
            string message;
            switch (turn)
            {
                case false:
                    message = "Black Wins!";
                    break;
                case true:
                    message = "White Wins!";
                    break;
            }

            Debug.Log(message);
            Application.Quit();
        }

        internal void changeTurn()
        {
            turn = !turn;
            camera.SwitchSides(turn);
        }
    }
}
