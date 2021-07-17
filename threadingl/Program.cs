using System;
using System.Threading;
using System.Threading.Tasks;

namespace threadingl
{
    class Program
    {
        static int y;
        static object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Thread worker = new Thread(() =>
            {

                // receive message.
                while (true)
                {
                    //Thread.Sleep(2000);
                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(2000);
                    });

                    t.Wait();

                    lock (locker)
                    {
                        y = Console.CursorTop;

                        // Force a scroll if we're at the end of the buffer
                        if (y == Console.BufferHeight - 1)
                        {
                            Console.WriteLine();
                            Console.SetCursorPosition(0, --y);
                        }

                        //if (Console.CursorTop != y)//for multiple input, if the user input something, y will change.
                        //{
                        //    continue;
                        //}

                        int x = Console.CursorLeft;
                        Console.MoveBufferArea(0, y, Console.WindowWidth, 1, 0, y + 1);//move the current line to next line, if the console start a new line, will move only the line at the bottom.
                        Console.SetCursorPosition(0, y);//back to previous line
                        Console.Write("I just waited 2 seconds.");
                        Console.SetCursorPosition(x, y + 1);//move the cursor to where it was

                        //Console.WriteLine("doing things ...");
                    }
                };
            });

            worker.Start();

            string input;
            while(true)
            {
                lock(locker)
                {
                    Console.Write("Input(enter [empty] to quit): ");
                }
                input = Console.ReadLine();
                if(String.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                lock (locker)
                {
                    Console.WriteLine("You typed: " + input);
                }
                //line_input++;
            }

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
