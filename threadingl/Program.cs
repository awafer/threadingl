using System;
using System.Threading;

namespace threadingl
{
    class Program
    {
        static int y;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Thread worker = new Thread(() =>
            {
                // do work
                while (true)//!workerShouldStop)
                {
                    //TimedReadline();
                    y = Console.CursorTop;

                    // Force a scroll if we're at the end of the buffer
                    if (y == Console.BufferHeight - 1)
                    {
                        Console.WriteLine();
                        Console.SetCursorPosition(0, --y);
                    }

                    Thread.Sleep(2000);
                    if (Console.CursorTop != y)
                        continue;
                    int x = Console.CursorLeft;
                    Console.MoveBufferArea(0, y, Console.WindowWidth, 1, 0, y + 1);
                    Console.SetCursorPosition(0, y);
                    Console.Write("I just waited 2 seconds.");
                    Console.SetCursorPosition(x, y + 1);

                    //Console.WriteLine("doing things ...");
                };
            });

            worker.Start();

            string input;
            do
            {
                Console.Write("Input(enter [empty] to quit): ");
                input = Console.ReadLine();
                Console.WriteLine("You typed: " + input);
                //line_input++;
            } while (!String.IsNullOrWhiteSpace(input));

            System.Environment.Exit(0);
        }

        static void TimedReadline()
        {
            int y = Console.CursorTop;
            int seconds = 2;
            //string prompt = "Please input something: ";

            // Force a scroll if we're at the end of the buffer
            if (y == Console.BufferHeight - 1)
            {
                Console.WriteLine();
                Console.SetCursorPosition(0, --y);
            }

            // Setup the timer       
            using (var tmr = new System.Timers.Timer(1000 * seconds))
            {
                tmr.AutoReset = false;
                tmr.Elapsed += (s, e) => {
                    if (Console.CursorTop != y) return;
                    int x = Console.CursorLeft;
                    Console.MoveBufferArea(0, y, Console.WindowWidth, 1, 0, y + 1);
                    Console.SetCursorPosition(0, y);
                    Console.Write("I just waited {0} seconds", seconds);
                    Console.SetCursorPosition(x, y + 1);
                };
                tmr.Enabled = true;
                // Write the prompt and obtain the user's input       
                //Console.Write(prompt);        
                //return Console.ReadLine();    
            }
        }
    }
}
