class Program
{
    static char[,] playerBoard = new char[10, 10];
    static char[,] computerBoard = new char[10, 10];
    static int playerShips = 20;
    static int computerShips = 20;

    static void Main(string[] args)
    {
        InitializeBoards();
        PlaceShips(playerBoard);
        PlaceShips(computerBoard);

        while (!GameIsOver())
        {
            Console.WriteLine("Ваше игровое поле:");
            PrintBoard(playerBoard);
            PrintComputerBoard(computerBoard);

            PlayerTurn();
            if (GameIsOver())
            {
                Console.WriteLine("Вы победили! Вы молодец");
                break;
            }

            ComputerTurn();
            if (GameIsOver())
            {
                Console.WriteLine("Вы проиграли! Очень жаль");
                break;
            }
        }

        Console.ReadLine();
    }

    static void InitializeBoards()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                playerBoard[i, j] = '-';
                computerBoard[i, j] = '-';
            }
        }
    }

    static void PlaceShips(char[,] board)
    {
        Random random = new Random();
        int[] shipCounts = { 4, 3, 2, 1 };
        int[] shipLengths = { 1, 2, 3, 4 };

        for (int i = 0; i < shipCounts.Length; i++)
        {
            int count = shipCounts[i];
            int length = shipLengths[i];
            int shipsPlaced = 0;

            while (shipsPlaced < count)
            {
                int direction = random.Next(2);
                int x = random.Next(10);
                int y = random.Next(10);

                if (CanPlaceShip(board, x, y, length, direction))
                {
                    PlaceShip(board, x, y, length, direction);
                    shipsPlaced++;
                }
            }
        }
    }
    static bool CanPlaceShip(char[,] board, int x, int y, int length, int direction)
    {
        if (direction == 0)
        {
            if (x + length > 10)
                return false;

            for (int i = x - 1; i < x + length + 1; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (i >= 0 && i < 10 && j >= 0 && j < 10)
                    {
                        if (board[i, j] != '-')
                            return false;
                    }
                }
            }

            for (int i = x; i < x + length; i++)
            {
                if ((y - 1 >= 0 && board[i, y - 1] != '-') || (y + 1 < 10 && board[i, y + 1] != '-'))
                    return false;
            }
        }
        else
        {
            if (y + length > 10)
                return false;

            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + length + 1; j++)
                {
                    if (i >= 0 && i < 10 && j >= 0 && j < 10)
                    {
                        if (board[i, j] != '-')
                            return false;
                    }
                }
            }

            for (int i = y; i < y + length; i++)
            {
                if ((x - 1 >= 0 && board[x - 1, i] != '-') || (x + 1 < 10 && board[x + 1, i] != '-'))
                    return false;
            }
        }

        return true;
    }


    static void PlaceShip(char[,] board, int x, int y, int length, int direction)
    {
        if (direction == 0)
        {
            for (int i = x; i < x + length; i++)
            {
                board[i, y] = 'S';
            }
        }
        else if (direction == 1)
        {
            for (int i = y; i < y + length; i++)
            {
                board[x, i] = 'S';
            }
        }
    }

    static void PrintBoard(char[,] board, bool showCoordinates = false)
    {
        Console.Write("  ");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((i + 1) + " ");
        }
        Console.WriteLine();

        for (int i = 0; i < 10; i++)
        {
            Console.Write((char)('A' + i) + " ");
            for (int j = 0; j < 10; j++)
            {
                if (showCoordinates && board[i, j] == 'S')
                    Console.Write("- ");
                else
                    Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    static void PlayerTurn()
    {
        bool playerGetsExtraTurn = false;

        do
        {
            Console.WriteLine("Ваш ход! Введите координаты для атаки (например, A5):");
            string attackCoordinates = Console.ReadLine().ToUpper();

            int x = attackCoordinates[0] - 'A';
            int y = int.Parse(attackCoordinates.Substring(1)) - 1;

            if (x < 0 || x >= 10 || y < 0 || y >= 10)
            {
                Console.WriteLine("Некорректные координаты! Попробуйте еще раз.");
                continue;
            }

            if (computerBoard[x, y] == 'S')
            {
                Console.WriteLine("Попадание!");
                computerBoard[x, y] = 'X';
                computerShips--;
                playerGetsExtraTurn = true;
                Console.WriteLine("Поле компьютера: ");
                PrintComputerBoard(computerBoard);
            }
            else if (computerBoard[x, y] == '-' || computerBoard[x, y] == 'O')
            {
                Console.WriteLine("Промах!");
                computerBoard[x, y] = 'O';
                playerGetsExtraTurn = false;
            }
        } while (playerGetsExtraTurn);
        if (playerShips == 0)
        {
            Console.WriteLine("Компьютер победил! Попробуйте еще раз.");
            return;
        }
    }

    static void ComputerTurn()
    {
        Random random = new Random();
        int x = random.Next(10);
        int y = random.Next(10);

        if (playerBoard[x, y] == 'S')
        {
            Console.WriteLine($"Компьютер попал в клетку {ConvertToCoordinates(x, y)}!");
            playerBoard[x, y] = 'X';
            playerShips--;
        }
        else if (playerBoard[x, y] == '-' || playerBoard[x, y] == 'O')
        {
            Console.WriteLine($"Компьютер промахнулся по клетке {ConvertToCoordinates(x, y)}!");
            playerBoard[x, y] = 'O';
        }
        if (computerShips == 0)
        {
            Console.WriteLine("Поздравляем! Вы выиграли!");
            return;
        }
    }
    static void PrintComputerBoard(char[,] board)
    {
        Console.WriteLine("Игровое поле компьютера:");
        Console.Write("  ");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((i + 1) + " ");
        }
        Console.WriteLine();
        for (int i = 0; i < 10; i++)
        {
            Console.Write((char)('A' + i) + " ");
            for (int j = 0; j < 10; j++)
            {
                if (board[i, j] == 'X' || board[i, j] == 'O')
                    Console.Write(board[i, j] + " ");
                else if (board[i, j] == 'S')
                    Console.Write("- "); //поменяйте "-" на "S" для более легкой проверки (показывает положение кораблей компьютера)
                else
                {
                    Console.Write("- ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    static string ConvertToCoordinates(int x, int y)
    {
        char xCoord = (char)('A' + x);
        int yCoord = y + 1;
        return $"{xCoord}{yCoord}";
    }

    static bool GameIsOver()
    {
        return playerShips == 0 || computerShips == 0;
    }
}