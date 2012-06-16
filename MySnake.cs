using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading; // to slow the console execution
using System.IO;
//F.N. 71246  Petko Kostov

namespace Snake_Project
{

    public struct position
    {
        public int row;
        public int col;
        public position(int x, int y)
        {
            this.row = x;
            this.col = y;
        }
    }

    public class Snake
    {
        private Queue<position> snake_el = new Queue<position>();
        public Snake()
        {
            for (int i = 0; i <= 5; i++)
            {
                snake_el.Enqueue(new position(0, i));
            }
        }

        public void Display()
        {
            foreach (position pos in snake_el)
            {
                Console.SetCursorPosition(pos.col, pos.row);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("*");
            }
        }
        public bool Contains(position p)
        {
            return this.snake_el.Contains(p);
        }
        public position OldHead()
        {
            return snake_el.Last();
        }
        public int Count()
        {
            return snake_el.Count();
        }
        public void Put(position p_first)
        {
            snake_el.Enqueue(p_first);
        }
        public position RemoveLast()
        {
            return snake_el.Dequeue();
        }
    }

    public class Walls
    {
        private position pos = new position();
        public Walls(int x, int y)
        {
            pos.col = x;
            pos.row = y;
        }
        public int col()
        {
            return pos.col;
        }
        public int row()
        {
            return pos.row;
        }
        public void print_wall()
        {
            Console.SetCursorPosition(this.pos.col, this.pos.row);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("#");
        }
    }

    public class Apple
    {
        private position apple_pos = new position();
        private Random myRandom = new Random();
        public Apple(Snake snake, List<Walls> my_walls)
        {
            do
            {
                apple_pos = new position(myRandom.Next(0, Console.WindowHeight),
                       myRandom.Next(0, Console.WindowWidth));
            }
            while (check_pos(snake, apple_pos, my_walls))
            ;
        }

        private bool check_pos(Snake sn, position apple_pos, List<Walls> my_walls)
        {
            bool condition1 = sn.Contains(apple_pos);
            bool condition2 = true;
            foreach (Walls w in my_walls)
            {
                if (apple_pos.col == w.col() && apple_pos.row == w.row()) condition2 = false;
            }
            return condition1 || !condition2;
        }

        public Apple(Snake snake) //Constructor 2
        {
            do
            {
                apple_pos = new position(myRandom.Next(0, Console.WindowHeight),
                    myRandom.Next(0, Console.WindowWidth));
            }
            while (snake.Contains(apple_pos));// not create apple over the snake
        }

        public void Display()
        {
            Console.SetCursorPosition(apple_pos.col, apple_pos.row);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("@");
        }
        public int col()
        {
            return apple_pos.col;
        }
        public int row()
        {
            return apple_pos.row;
        }

    }

    // --------------------------------

    public class Level
    {
        public static Snake sn = new Snake();
        protected int direction = 0;

        public int show_best_result()
        {
            if (!File.Exists("eto.txt"))
            {
                StreamWriter writer = new StreamWriter("eto.txt");
                using (writer)
                {
                    string tmp = "0";
                    writer.WriteLine(tmp);
                }
            }
            using (TextReader reader = File.OpenText("eto.txt"))
            {
                return int.Parse(reader.ReadLine());
            }
        }

        public void get_direction(ConsoleKeyInfo userInput)
        {
            if (userInput.Key == ConsoleKey.RightArrow && direction != 1) direction = 0;
            if (userInput.Key == ConsoleKey.LeftArrow && direction != 0) direction = 1;
            if (userInput.Key == ConsoleKey.DownArrow && direction != 3) direction = 2;
            if (userInput.Key == ConsoleKey.UpArrow && direction != 2) direction = 3;
        }

        public void show_direction()
        {
            if (direction == 0) Console.Write(">");
            if (direction == 1) Console.Write("<");
            if (direction == 2) Console.Write("v");
            if (direction == 3) Console.Write("^");
        }

        public Snake Snake
        {
            get { return sn; }
        }

        public int Direction
        {
            get { return direction; }
        }

        public void snake_dead(DateTime startTime, int lev)
        {
            Console.SetCursorPosition(0, 0);
            int score = (sn.Count() - 6) * 10;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Game Over! Your score is: {0}", score);
            int temp_result = this.show_best_result();
            if (score > temp_result)
            {
                StreamWriter writer = new StreamWriter("eto.txt");
                using (writer)
                {
                    //string tmp = (string)score;
                    writer.WriteLine(score);
                }
            }
            int n = this.show_best_result();
            Console.WriteLine("Your best result is: {0}", n);
            Console.WriteLine("Difficulty: {0}", lev);
            TimeSpan runTime = DateTime.Now - startTime;
            Console.Write("Your playing time was: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0} seconds", Convert.ToString(runTime.Seconds));
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(0);
        }

    }

    public class Level1 : Level
    {

        public Apple ap = new Apple(sn);
        public bool check_snake_dead(position newhead)
        {
            return sn.Contains(newhead);
        }
        public Apple apple
        {
            get { return ap; }
            set { ap = value; }
        }
        public bool snake_eating(position newHead)
        {
            return (newHead.col == ap.col() && newHead.row == ap.row());
        }

    }

    public class Level2 : Level
    {
        static List<Walls> my_walls = new List<Walls>();
        public Level2()
        {
            Random myRandom = new Random();
            int until = myRandom.Next(4, 15);
            for (int i = 0; i <= until; i++) my_walls.Add(new Walls(myRandom.Next(2, Console.WindowWidth), myRandom.Next(2, Console.WindowHeight)));
        }
        public Apple ap = new Apple(sn, my_walls);

        public bool check_snake_dead(position newhead)
        {
            bool condition1 = (newhead.col < 0 || newhead.row < 0 ||
                        newhead.col >= Console.WindowWidth || newhead.row >= Console.WindowHeight ||
                        sn.Contains(newhead));
            bool condition2 = true;
            foreach (Walls w in my_walls)
            {
                if (condition2)
                {
                    if (newhead.col == w.col() && newhead.row == w.row()) condition2 = false;
                }
            }
            return condition1 || condition2 == false;
        }
        public void print_walls()
        {
            foreach (Walls w in my_walls)
            {
                w.print_wall();
            }
        }
        public Apple apple
        {
            get { return ap; }
            set { ap = value; }
        }
        public bool snake_eating(position newHead)
        {
            return (newHead.col == ap.col() && newHead.row == ap.row());
        }
    }

    class Program
    {
        public static int level;

        public static void welcome()
        {
            Console.WriteLine("Choose level of dificulty");
            Console.WriteLine("Easy - 1");
            Console.WriteLine("Hard - 2");
            Console.WriteLine("Written by 5_ko Ko_100_v ");
            ConsoleKeyInfo userInput = Console.ReadKey();
            if (userInput.Key == ConsoleKey.D1) level = 1;
            if (userInput.Key == ConsoleKey.D2) level = 2;
        }

        public static void set_new_h(ref position mynewHead)
        {
            if (mynewHead.col < 0) mynewHead.col = Console.WindowWidth - 1; // count from zero
            if (mynewHead.row < 0) mynewHead.row = Console.WindowHeight - 1;
            if (mynewHead.col >= Console.WindowWidth) mynewHead.col = 0;
            if (mynewHead.row >= Console.WindowHeight) mynewHead.row = 0;
        }

        public static void play(Level1 lev_one, position[] directions, double speed)
        {
            lev_one.ap.Display();
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    lev_one.get_direction(userInput);
                }
                position oldHead = new position();
                position next = new position();
                position newHead = new position();
                oldHead = lev_one.Snake.OldHead();
                next = directions[lev_one.Direction];

                // GAME OVER logic
                newHead = new position(oldHead.row + next.row,
                               oldHead.col + next.col);
                set_new_h(ref newHead);
                if (lev_one.check_snake_dead(newHead)) //<-samoiziajd
                {
                    if (lev_one.check_snake_dead(newHead))
                    {
                        lev_one.snake_dead(startTime, 1);
                    }
                }
                Console.SetCursorPosition(oldHead.col, oldHead.row);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("*");
                //include the new head in the queue
                lev_one.Snake.Put(newHead);
                Console.SetCursorPosition(newHead.col, newHead.row);
                lev_one.show_direction();
                if (lev_one.snake_eating(newHead))
                {
                    lev_one.apple = new Apple(lev_one.Snake);
                    lev_one.apple.Display();

                    //speed -= 0.01; // if we want to speed on every success
                }
                else
                {
                    position last = new position();
                    last = lev_one.Snake.RemoveLast();
                    lev_one.apple.Display();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }
                speed -= 0.01;
                Thread.Sleep((int)speed);
            }
        }

         public static void play(Level2 lev_two, position[] directions, double speed)
         {
	        lev_two.ap.Display();
	        DateTime startTime = DateTime.Now;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                   lev_two.get_direction(userInput);
                }
                position oldHead = new position();
                position next = new position();
                position newHead = new position();
				oldHead = lev_two.Snake.OldHead();
				next = directions[lev_two.Direction];                
                // GAME OVER logic
				lev_two.print_walls();        
				newHead = new position(oldHead.row + next.row,
						   oldHead.col + next.col);
				if (lev_two.check_snake_dead(newHead))
				{
					lev_two.snake_dead(startTime, 2);
				}               
                Console.SetCursorPosition(oldHead.col, oldHead.row);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("*");
                //include the new head in the queue
				lev_two.Snake.Put(newHead); 
                Console.SetCursorPosition(newHead.col, newHead.row);
				lev_two.show_direction();
                if (lev_two.snake_eating(newHead))
                {
                    // make the snake eat                   
                        lev_two.apple = new Apple(lev_two.Snake);
                        lev_two.apple.Display();                   
                    //speed -= 0.01; // if we want to speed on every success
                }
                else
                {
                    position last = new position();
					last = lev_two.Snake.RemoveLast();
					lev_two.apple.Display();                    
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }
                speed -= 0.01;
                Thread.Sleep((int)speed);
            }
        }
    

        // --------------------------------------------------------------------------------
        static void Main()
        {
            welcome();
            Console.Clear();
            Console.BufferHeight = Console.WindowHeight;

            position[] directions = new position[]
            {
                new position(0, 1),     //0 -> right    >
                new position(0, -1),    //1 -> left     <   
                new position(1, 0),     //2-> down      v
                new position(-1, 0)     //3 -> up       ^
            };

            double speed = new double();
            if (level == 1)
            {
                Level1 lev_one = new Level1();
                speed = 100;
                play(lev_one, directions, speed);
            }
            else
            {
                Level2 lev_two = new Level2();
                speed = 70;
                play(lev_two, directions, speed);
            }
        }
    }
}
