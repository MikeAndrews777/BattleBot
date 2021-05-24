const int MAZE_SIZE = 6;
static int[] allowed_move_row = { 0, -1, 0, 1 };
static int[] allowed_move_col = { 1, 0, -1, 0 };
const int MAX_ALLOWED_MOVES = 4;
static int[,] Optmaze = new int[MAZE_SIZE, MAZE_SIZE];
static int[,] maze  = new int[MAZE_SIZE, MAZE_SIZE] { 
                       { -2 , 0  ,  0   , 0  ,  0  ,  -1 },
                       { -1 , 0  , -1   , 0  , -1  ,  0 } ,
                       { -1 , -1 , -1   , 0  ,  0  ,  0 } ,
                       { -1 , 0  , -1   , -1 ,  0  ,  -1 },
                       { -1 , -1 , -1   , 0  ,  0  ,  0 } ,
                       { -1 , 0  , -1   , -3 ,  0  ,  0 } };

static bool Solve(int previous_row, int previous_col, int next_step_no)
{
     for (int i = 0; i < MAX_ALLOWED_MOVES; i++)
     {
          int col = previous_col + allowed_move_col[i];
          int row = previous_row + allowed_move_row[i];
          if (col < 0 || col >= MAZE_SIZE) 
              continue;
          if (row < 0 || row >= MAZE_SIZE) 
              continue;

          if (maze[row, col] == -3)
              return true;

          if (maze[row, col] != 0) 
              continue;

          maze[row, col] = next_step_no;

          if (Solve(row, col, next_step_no + 1))
              return true;

          else maze[row, col] = 0;
     }
            return false;
}

static void Main(string[] args)
{
    Print(maze);
    Console.WriteLine("-----------Solve-------------");
    if (Solve (0, 0, 1) )
        Print( maze);
    else
        Console.WriteLine("No Result");
}

static void Print(int[,] maze)
{
    for (int i = 0; i < maze.GetLength(0); i++)
    {
        for (int j = 0; j < maze.GetLength(1); j++)
        {
            Console.Write(" " + String.Format("{0,3}",maze[i, j]) + " ");
        }
        Console.WriteLine();
    }
}



//Output
/*
	   -2    0    0    0    0   -1
       -1    0   -1    0   -1    0
       -1   -1   -1    0    0    0
       -1    0   -1   -1    0   -1
       -1   -1   -1    0    0    0
       -1    0   -1   -3    0    0
       -----------Solve-------------
       -2    1    2    3    0   -1
       -1    0   -1    4   -1    0
       -1   -1   -1    5    6    0
       -1    0   -1   -1    7   -1
       -1   -1   -1    0    8    9
       -1    0   -1   -3   11   10
*/