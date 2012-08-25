using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson3
{
    class Program
    {
        // wheither or not exit
        static bool s_ExitGame = false;

        // we will want to add some randomness to our game so we need a random number generator.
        static Random s_RandomNumberGenerator = new Random();

        // Screen coord. Here we store where we want the player to be.
        // [0,0] [0,1] [0,2] [0,3] ...
        // [1,0] [1,1] [1,2] [1,3] ...
        static int s_PlayerXPos = 20;
        static int s_PlayerYPos = 10;

        // when the player dies, we want to him to stay dead for so many frames. if this value is greater then 1 then he is dead.
        static int s_PlayerDeathTime = 0;

        // Screen coord. Here we are going to want to store the position of the bullet.
        // we want this to be an array of 5. So we can have different bullets running at the same time.
        static int[] s_bulletXPos = new int[5];
        static int[] s_bulletYPos = new int[5];

        static void Main(string[] args)
        {
            // we now need to initialize all the bullets
            for (int i = 0; i < 5; ++i)
            {
                s_bulletXPos[i] = s_RandomNumberGenerator.Next(40);
                s_bulletYPos[i] = s_RandomNumberGenerator.Next(20);
            }

            // this is the game loop... every frame until the game exits
            while (!s_ExitGame)
            {
                // update any game logic... ai's, input, etc...
                UpdateGameLogic();

                // draw all the graphics
                DrawGraphics();

                // to keep the program from running too fast, we will wait 1/60th of a sec
                System.Threading.Thread.Sleep(16); 
            }
        }

        static void UpdateGameLogic()
        {
            if (s_PlayerDeathTime > 0)
            {
                // if we are dead do not allow us to do anything
                s_PlayerDeathTime--;
            }
            else
            {
                // we need to check if the user has pressed any keys
                if (Console.KeyAvailable)
                {
                    // get the key that was pressed, if the key was escape
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        s_ExitGame = true;
                    }

                    // move the player based on the arrow keys pressed
                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        s_PlayerYPos += 1;
                    }
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        s_PlayerYPos -= 1;
                    }
                    if (key.Key == ConsoleKey.RightArrow)
                    {
                        s_PlayerXPos += 1;
                    }
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        s_PlayerXPos -= 1;
                    }

                    // make sure the player position does not go off the screen
                    if (s_PlayerXPos < 0)
                        s_PlayerXPos = 0;
                    if (s_PlayerYPos < 0)
                        s_PlayerYPos = 0;
                    if (s_PlayerXPos > 40)
                        s_PlayerXPos = 40;
                    if (s_PlayerYPos > 20)
                        s_PlayerYPos = 20;
                }
            }

            // instead of update the 1 bullet we update each of the 5
            for (int i=0; i<5; ++i)
            {
                // now we are going to update the bullets movement
                s_bulletXPos[i] -= 1;

                // if the bullet hits the end we want it to wrap around
                if (s_bulletXPos[i] < 0)
                {
                    s_bulletXPos[i] = 40;
                    s_bulletYPos[i] = s_RandomNumberGenerator.Next(20);
                }

                // now we can check if the bullet hit the player
                if (s_bulletXPos[i] == s_PlayerXPos
                    && s_bulletYPos[i] == s_PlayerYPos)
                { 
                    // there is a hit if they occupy the same space!
                    // we will handle this in the next exercise.

                    s_PlayerDeathTime = 20;

                    // move the bullet back
                    s_bulletXPos[i] = 40;
                    s_bulletYPos[i] = s_RandomNumberGenerator.Next(20);
                }
            }
        }

        static void DrawGraphics()
        {
            // clear the screen
            Console.Clear();

            // move the cursor to the screen position 
            Console.SetCursorPosition(s_PlayerXPos, s_PlayerYPos);
            
            // Draw the player
            if (s_PlayerDeathTime > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X"); // X if he is dead
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write("O"); // O if he is alive
            }

            // instead of drawing the 1 bullet we update each of the 5
            for (int i = 0; i < 5; ++i)
            {
                // now we need to draw the bullet
                Console.SetCursorPosition(s_bulletXPos[i], s_bulletYPos[i]);
                Console.Write(".");
            }
        }
    }
}
