using System;

namespace ConsoleApp1
{
    class Program
    {
        public struct Coord
        {
            public int x, y;

            public Coord( int x = 0, int y = 0)
            {
                this.x = x;
                this.y = y;
            }
        }

        class wormPart
        {
            public Coord position = new Coord();
            public char imgSymb = new char();

            public wormPart(Coord position, char symb = 'x')
            {
                this.position = position;
                imgSymb = symb;
            }
        }

        class Worm
        {
            public int direction = 0;
            private int numberOfParts = 1;
            private static readonly wormPart head = new wormPart(coordinates(0,0), '0');
            public readonly wormPart[] parts = { head };

            public Worm(int numberOfParts, Coord pos, int direction)
            {
                Coord newPos = new Coord(0,0);
                head.position = coordinates(0, 0);
                this.numberOfParts = numberOfParts;
                Array.Resize(ref parts, numberOfParts);

                for ( int count = 1; count < this.numberOfParts; count++ )
                {
                    newPos.y = parts[0].position.y + count;
                    newPos.x = parts[0].position.x + count;
                    parts[count] = new wormPart(newPos);
                }
            }

            public void Show()
            {
                try
                {
                    for (int count = 0; count < parts.GetLength(0); count++)
                    {
                        Write(pos: parts[count].position, inputChar: parts[count].imgSymb);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }

            public void Hide()
            {
                try
                {
                    for (int count = 0; count < numberOfParts; count++)
                    {
                        Write(pos: parts[count].position, inputChar: ' ');
                    }
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }

            public void Move(int direction)
            {
                Hide();

                if (parts[0].position.x + ConvertDirection(direction).x >= 0)
                {

                    if (parts[0].position.y + ConvertDirection(direction).y >= 0)
                    {
                        //Сделай нормально просчёт координат.

                        parts[0].position.x = parts[0].position.x + ConvertDirection(direction).x;
                        parts[0].position.y = parts[0].position.y + ConvertDirection(direction).y;
                    }
                }

                Show();
            }
        }

        public static Coord ConvertDirection( int direction )
        {
            int dx = 0, dy = 0;

            switch (direction)
            {
                case 0://up
                    dx = 0;
                    dy = -1;
                    break;
                case 1://right
                    dx = 1;
                    dy = 0;
                    break;
                case 2://down
                    dx = 0;
                    dy = 1;
                    break;
                case 3://left
                    dx = -1;
                    dy = 0;
                    break;
                default:
                    break;
            }

            return coordinates(dx, dy);
        }

        public static Coord coordinates( int x, int y )
        {
            return new Coord(x, y);
        }

        private static void Write(Coord pos, char inputChar = ' ')
        {
            try
            {
                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(inputChar);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        private static void WriteLn(Coord pos, string inputStr = "")
        {

            try
            {
                Console.SetCursorPosition(pos.x, pos.y);
                Console.WriteLine(inputStr);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        public static void Main(string[] args)
        {
            char c;
            int Dir = 0;
            Coord pos = new Coord(0, 0);
            Worm First = new Worm(3, pos, Dir);
            Console.CursorVisible = false;

            First.Show();

            while (true)
            {
                c = Console.ReadKey().KeyChar;
                Console.Clear();

                switch (c)
                {
                    case 'w':
                        Dir = 0;
                        break;
                    case 'd':
                        Dir = 1;
                        break;
                    case 's':
                        Dir = 2;
                        break;
                    case 'a':
                        Dir = 3;
                        break;
                }

                First.Move(Dir);
            }
        }
    }
}
