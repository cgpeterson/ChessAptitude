using System;
using UnityEngine;

namespace ThreeSpace.Chess
{
    public class Piece : MonoBehaviour
    {
        // Access board to use game variables
        Board board;

        [SerializeField] public Vector2Int Position;

        // Assign piece type
        enum Class
        {
            Pawn,
            Knight,
            King
        }
        [SerializeField] Class Type;

        void Start()
        {
            board = GameObject.Find("Board").GetComponent<Board>();
        }

        public void OnPieceClick()
        {
            // Assign piece as selected piece to use
            board.cur_piece = gameObject.GetComponent<Piece>();
        }

        public bool Move(Vector2Int pos)
        {
            bool flag = false;
            // Movement based on piece type
            switch (Type)
            {
                case Class.Pawn:
                    bool movingForward = false;
                    int startPosition;
                    switch (board.turn)// determine pawn movement direction and start position
                    {
                        case false:// White
                            if (pos[1] > Position[1])
                                movingForward = true;
                            startPosition = 1;
                            break;
                        case true:// Black
                            if (pos[1] < Position[1])
                                movingForward = true;
                            startPosition = 4;
                            break;
                    }

                    if (pos[0] == Position[0] && movingForward)
                    {
                        // Determine if it is the pawn's first move
                        int moveLimit = 1;
                        if (Position[1] == startPosition)
                            moveLimit = 2;
                        
                        int moveDist = pos[1] - Position[1];
                        
                        if (Math.Abs(moveDist) <= moveLimit)
                        {
                            // Move the pawn
                            Position[1] = pos[1];
                            transform.position = transform.position + new Vector3((moveDist * 5), 0, 0);
                            // Mark as Successful
                            flag = true;
                        }
                    }
                    break;
                case Class.Knight:
                    int xDiff = Position[0] - pos[0];
                    int yDiff = pos[1] - Position[1];
                    int totalDiff = Math.Abs(xDiff) + Math.Abs(yDiff);

                    // Check the pattern
                    bool patternMatch = false;
                    if (Math.Abs(xDiff) > 0 && Math.Abs(yDiff) > 0)
                        patternMatch = true;
                    
                    // If it is an 'L' and the movement amount is exactly 3 spaces
                    if (totalDiff == 3 && patternMatch)
                    {
                        // Move the knight
                        Position = new Vector2Int(pos[0], pos[1]);
                        transform.position = transform.position + new Vector3(yDiff, 0, xDiff) * 5;
                        // Mark as Successful
                        flag = true;
                    }
                    break;
                case Class.King:
                    int xMove = Position[0] - pos[0];
                    int yMove = pos[1] - Position[1];
                    int totalMove = Math.Abs(xMove) + Math.Abs(yMove);

                    // if the piece is not already in the selected square and the space is adjacent.
                    if (totalMove > 0 && Math.Abs(xMove) <= 1 && Math.Abs(yMove) <= 1)
                    {
                        // Move the king
                        Position = new Vector2Int(pos[0], pos[1]);
                        transform.position = transform.position + new Vector3(yMove, 0, xMove) * 5;
                        // Mark as Successful
                        flag = true;
                    }
                    break;
            }
            return flag;
        }

        public bool Attack(Piece enemy)
        {
            bool flag = false;
            // Attack based on type
            switch (Type)
            {
                case Class.Pawn:
                    bool inFront = false;
                    int forwardMove = 0;
                    // Check if enemy is in front of pawn
                    switch (board.turn)
                    {
                        case false:// White
                            if (enemy.Position[1] == (Position[1] + 1))
                            {
                                inFront = true;
                                forwardMove = 1;
                            }
                            break;
                        case true:// Black
                            if (enemy.Position[1] == (Position[1] - 1))
                            {
                                inFront = true;
                                forwardMove = -1;
                            }
                            break;
                    }

                    int sideDiff = Position[0] - enemy.Position[0];

                    // if it is in front of the pawn and off to one side
                    if (inFront && Math.Abs(sideDiff) == 1)
                    {
                        enemy.Die();
                        // Move the pawn to new position
                        Position[0] -= sideDiff;
                        Position[1] += forwardMove;
                        Vector3 move = new Vector3(forwardMove, 0, sideDiff) * 5;
                        transform.position = transform.position + move;
                        // Mark as Successful
                        flag = true;
                    }
                    break;
                case Class.Knight:
                    // if the knight can move to enemy position
                    if (Move(enemy.Position))
                    {
                        enemy.Die();
                        // Mark as Successful
                        flag = true;
                    }
                    break;
                case Class.King:
                    // if the king can move to enemy position
                    if (Move(enemy.Position))
                    {
                        enemy.Die();
                        // Mark as Successful
                        flag = true;
                    }
                    break;
            }
            return flag;
        }

        private void Die()
        {
            // if the king dies end game loop
            if (Type == Class.King)
                board.isAlive = false;
            // Delete the piece off the board
            Destroy(gameObject);
        }
    }
}
