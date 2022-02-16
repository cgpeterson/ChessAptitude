using UnityEngine;

namespace ThreeSpace.Chess
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] public Vector2Int Position;
        // Access to the game variables
        Board board;

        // UI access to highlight tiles when cursor is over it.
        MeshRenderer cur_tile;
        Color color;

        void Start()
        {
            board = GameObject.Find("Board").GetComponent<Board>();
            cur_tile = gameObject.GetComponent<MeshRenderer>();
            // Store the starting color so we can reset to it when not hovered over.
            color = gameObject.GetComponent<MeshRenderer>().material.color;
        }

        void Update()
        {

        }

        public void OnMouseEnter()
        {
            // Highlight tile
            cur_tile.material.color = Color.gray;
        }

        public void OnMouseExit()
        {
            // Reset to starting color
            cur_tile.material.color = color;
        }

        public void OnMouseDown()
        {
            // Find the piece on the selected tile
            Piece piece = SelectPieceAt(Position);
            string cur_turn;

            if (board.turn == false) // White
                cur_turn = "White";
            else // Black
                cur_turn = "Black";

            if (piece == null || piece.tag != cur_turn)
            {
                switch (piece)
                {
                    // if no piece on clicked tile attempt move
                    case null:
                        if (board.cur_piece != null && board.cur_piece.Move(Position))
                        {
                            // Clear current piece
                            board.cur_piece = null;
                            // Switch turn
                            board.changeTurn();
                        }
                        break;
                    // if enemy piece on clicked tile attempt attack
                    default:
                        if (board.cur_piece != null && board.cur_piece.Attack(piece))
                        {
                            // Clear current piece
                            board.cur_piece = null;
                            // Switch turn
                            board.changeTurn();
                        }
                        break;
                }
            }
            else
                // Select piece for moving
                piece.OnPieceClick();
        }

        public Piece SelectPieceAt(Vector2Int position)
        {
            Piece[] gameObjects;
            Piece cur_piece = null;
            
            // Make a list of all pieces
            gameObjects = GameObject.FindObjectsOfType<Piece>();

            // Look through the list of pieces
            foreach (var item in gameObjects)
            {
                // If the piece is on this tile return it
                if (item.Position == Position)
                {
                    cur_piece = item;
                    break;
                }
            }
                
            return cur_piece;
        }
    }
}
