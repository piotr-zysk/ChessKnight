using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

/*
 *  policz ile minimum ruchów skoczkiem trzeba zrobic by  przedostac sie z lewego gornego na dolnty prawy rog planszy A
 *  1 oznaczone sa pola zajete
 * 
 * 
 * algorytm znajduje wszelkie mozliwe sciezki
 */

namespace Codility5
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] A = new int[4][]; 

            A[0] = new int[] { 0, 0, 0 };
            A[1] = new int[] { 0, 0, 1 };
            A[2] = new int[] { 1, 0, 0 };
            A[3] = new int[] { 0, 0, 0 };

            WriteLine($"number of moves in the shortest path: {new Solution().solution(A)}");
        }
    }

    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    internal class PositionNode
    {
        public PositionNode(byte moveNr, int x, int y)
        {
            this.MoveNr = moveNr;
            this.X = x;
            this.Y = y;
        }

        public byte MoveNr { get; set; }            
        public int X { get; set; }
        public int Y { get; set; }
    }
       
    

    class Solution
    {
        
        Stack<PositionNode> PosHistory = new Stack<PositionNode>();
        
        public int solution(int[][] A)
        {
            int failResult = -1;
            int Result = failResult;
            
            var startPos = new Position(0,0);
            var currentPos = startPos;
            var newPos = currentPos;
            

            var targetPos = new Position(A[0].Length - 1,A.Length - 1);
                        
           //WriteLine($"{targetPos.X+1}X{targetPos.Y+1}");

            byte startNr = 0;
            PositionNode posNode;
            int counter = 0;

            do
            {
                newPos = nextMove(currentPos, A, startNr);
                if (PosHistory.Count() == 0) return Result; // stop the loop if no move possible


                //Console.WriteLine($"{++counter}:{PosHistory.Count()} / {startNr}");

                startNr = 0;

             

                if ((newPos.X==targetPos.X) && (newPos.Y==targetPos.Y))
                {
                    if ((Result == failResult) || (PosHistory.Count() < Result)) Result = PosHistory.Count();

                    //Console.WriteLine(PosHistory.Count());
                    currentPos = newPos;
                }


                if (currentPos == newPos) //if no move possible or path completed
                {
                    if (PosHistory.Count()>0)
                    {
                        do
                        {
                            posNode = PosHistory.Pop();
                            startNr = (byte)(1+posNode.MoveNr);
                            currentPos.X = posNode.X;
                            currentPos.Y = posNode.Y;
                        }
                        while ((startNr > 7) && (PosHistory.Count() > 0));
                    }
                    
                    
                }                    
                else
                {                    
                    currentPos = newPos;                    
                    
                }

            }
            while (currentPos.X < targetPos.X || currentPos.Y < targetPos.Y);

            return Result;
        }

        Position nextMove(Position currentPos, int[][] A, byte startNr)
        {
            Position newPos = currentPos;

            Position[] newMove = new Position[8];

            newMove[0] = new Position(currentPos.X + 1, currentPos.Y + 2);
            newMove[1] = new Position(currentPos.X + 2, currentPos.Y + 1);

            newMove[2] = new Position(currentPos.X - 1, currentPos.Y + 2);
            newMove[3] = new Position(currentPos.X + 2, currentPos.Y - 1);

            newMove[4] = new Position(currentPos.X + 1, currentPos.Y - 2);
            newMove[5] = new Position(currentPos.X - 1, currentPos.Y - 2);

            newMove[6] = new Position(currentPos.X - 2, currentPos.Y + 1);
            newMove[7] = new Position(currentPos.X - 2, currentPos.Y - 1);


            for (byte i = startNr; i < 8; i++)
            {
                if (isValidPosition(newMove[i], A))
                {
                    PosHistory.Push(new PositionNode(i, currentPos.X, currentPos.Y));
                    newPos = newMove[i];
                    break;
                }
            }

            
            return newPos;
        }
         
        bool isValidPosition(Position pos, int[][] A)
        {            
            if (pos.X >= A[0].Length) return false;
            if (pos.Y >= A.Length) return false;
            if (pos.X < 0) return false;
            if (pos.Y < 0) return false;
            
            if (A[pos.Y][pos.X] == 1) return false;

            var historyCounter = 0;
            foreach(PositionNode p in PosHistory)
            {
                if ((p.X == pos.X) && (p.Y == pos.Y)) historyCounter++;
            }
            if (historyCounter > 0) return false;

            return true;
        }
        


      
        

    }
}
