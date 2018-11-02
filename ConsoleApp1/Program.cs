using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        private static void Write(Coord pos, char inputChar = ' ')
        {
            try
            {
                Console.SetCursorPosition(left: pos.x, top: pos.y);
                Console.Write(value: inputChar);
            }
            catch (ArgumentOutOfRangeException)
            {
                WriteLn(Coordinates(0, 21), "WrChExc();" + pos.x + " " + pos.y + inputChar);
            }
        }

        private static void WriteLn(Coord pos, string inputStr = "")
        {

            try
            {
                Console.SetCursorPosition(left: pos.x, top: pos.y);
                Console.WriteLine(value: inputStr);
            }
            catch (ArgumentOutOfRangeException)
            {
                WriteLn(Coordinates(0, 21), "WrLnExc();" + pos.x + " " + pos.y + inputStr);
            }
        }

        public struct Coord
        {
            public int x, y;

            public Coord(int x = 0, int y = 0)
            {
                this.x = x;
                this.y = y;
            }
        }

        public static Coord Coordinates(int x, int y) => new Coord(x, y);

        public static Coord ConvertDirection(int direction)
        {
            switch (direction)
            {
                case 0://up
                    return Coordinates(0, -1);
                case 1://right
                    return Coordinates(1, 0);
                case 2://down
                    return Coordinates(0, 1);
                case 3://left
                    return Coordinates(-1, 0);
                default:
                    return Coordinates(-127, -127);
            }
        }

        class WormPart
        {
            public Coord position = new Coord();
            public char imgSymb = new char();
            public ConsoleColor foreground, background;

            private void Paint(ConsoleColor f_ground, ConsoleColor b_ground)
            {
                foreground = f_ground;
                background = b_ground;
            }

            public WormPart(Coord position, char symb = 'x')
            {
                this.position = position;
                imgSymb = symb;
            }
        }

        class Worm
        {
            public int direction = 0;
            private int numberOfParts = 0;
            private static readonly WormPart head = new WormPart(Coordinates(0, 0), '0');
            public WormPart[] parts = { head };

            public Worm(Coord pos, int numberOfParts = 0, int direction = 0)
            {
                head.position = pos;
                Grow(numberOfParts);
            }

            public void Show()
            {
                try
                {
                    for (int count = parts.GetLength(0) - 1; count > -1; count--)
                    {
                        Console.BackgroundColor = parts[count].background;
                        Console.ForegroundColor = parts[count].foreground;

                        Write(pos: parts[count].position, inputChar: parts[count].imgSymb);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    WriteLn(Coordinates(0, 21), "ShowExc();");
                }
            }

            public void Hide()
            {
                try
                {
                    for (int count = 0; count < numberOfParts; count++)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                        Write(pos: parts[count].position, inputChar: ' ');
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    WriteLn(Coordinates(0, 21), "HideExc();");
                }
            }

            public void Grow(int segments)
            {
                Coord tempPos;

                if (numberOfParts + segments > 0)
                {
                    Hide();

                    numberOfParts += segments;
                    Array.Resize(array: ref parts, newSize: numberOfParts);

                    if (segments > 0)
                    {
                        for (int count = 1; count < numberOfParts; count++)
                        {
                            tempPos = Coordinates(x: parts[count - 1].position.x + ConvertDirection(direction).x, y: parts[count - 1].position.y + ConvertDirection(direction).y);

                            if (parts[count] == null)
                            {
                                parts[count] = new WormPart(position: tempPos);
                            }
                        }
                    }

                    Show();
                }
            }

            public void Move(int direction)
            {
                Hide();

                if (parts[0].position.x + ConvertDirection(direction).x >= 0 && parts[0].position.y + ConvertDirection(direction).y >= 0)
                {
                    int count = numberOfParts - 1;

                    while (true)
                    {
                        if (count == 0)
                        {
                            break;
                        }

                        parts[count].position.x = parts[count - 1].position.x;
                        parts[count].position.y = parts[count - 1].position.y;

                        count--;
                    }

                    parts[0].position.x = parts[0].position.x + ConvertDirection(direction).x;
                    parts[0].position.y = parts[0].position.y + ConvertDirection(direction).y;
                }

                Show();
            }
        }

        public static int Dir = 3;

        public static void Input()
        {
            char c;

            while (true)
            {
                c = Console.ReadKey(true).KeyChar;

                switch (c)
                {
                    case 'w':
                        if (Dir != 2)
                        {
                            Dir = 0;
                        }
                        break;
                    case 'd':
                        if (Dir != 3)
                        {
                            Dir = 1;
                        }
                        break;
                    case 's':
                        if (Dir != 0)
                        {
                            Dir = 2;
                        }
                        break;
                    case 'a':
                        if (Dir != 1)
                        {
                            Dir = 3;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        static public int _max(int a, int b) => b > a ? b : a;

        public static void Main(string[] args)
        {
            Worm First = new Worm(numberOfParts: 30, pos: Coordinates( _max(Console.WindowWidth / 2, 30), Console.WindowHeight / 2), direction: Dir);
            Console.CursorVisible = false;
            Thread inputThread = new Thread( new ThreadStart( Input ) );
            inputThread.Start();

            First.Show();

            while (true)
            {
                First.Move(direction: Dir);
                Thread.Sleep(100);
            }
        }
    }
}
