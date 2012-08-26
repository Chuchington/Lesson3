using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson3
{
    class Player
    {
        // Screen coord. Here we store where we want the player to be.
        // [0,0] [0,1] [0,2] [0,3] ...
        // [1,0] [1,1] [1,2] [1,3] ...
        public int m_xPos = 20;
        public int m_yPos = 10;

        // when the player dies, we want to him to stay dead for so many frames. if this value is greater then 1 then he is dead.
        public int m_deathTime = 0;

        public void Update()
        {
            if (m_deathTime > 0)
            {
                // if we are dead do not allow us to do anything
                m_deathTime--;
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
                        Program.s_ExitGame = true;
                    }

                    // move the player based on the arrow keys pressed
                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        m_yPos += 1;
                    }
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        m_yPos -= 1;
                    }
                    if (key.Key == ConsoleKey.RightArrow)
                    {
                        m_xPos += 1;
                    }
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        m_xPos -= 1;
                    }

                    // make sure the player position does not go off the screen
                    if (m_xPos < 0)
                        m_xPos = 0;
                    if (m_yPos < 0)
                        m_yPos = 0;
                    if (m_xPos > 40)
                        m_xPos = 40;
                    if (m_yPos > 20)
                        m_yPos = 20;
                }
            }
        }

        public void Draw()
        {
            // move the cursor to the screen position 
            Console.SetCursorPosition(m_xPos, m_yPos);
            
            if (m_deathTime > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X"); // X if he is dead
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write("O"); // O if he is alive
            }
        }
    }

    class Bullet
    {
        public int m_xPos = 20;
        public int m_yPos = 10;

        public void Update()
        {
            // now we are going to update the bullets movement
            m_xPos -= 1;

            // if the bullet hits the end we want it to wrap around
            if (m_xPos < 0)
            {
                m_xPos = 40;
                m_yPos = Program.s_RandomNumberGenerator.Next(20);
            }

            // now we can check if the bullet hit the player
            if (m_xPos == Program.s_player.m_xPos
                && m_yPos == Program.s_player.m_yPos)
            {
                // there is a hit if they occupy the same space!
                // we will handle this in the next exercise.

                Program.s_player.m_deathTime = 20;

                // move the bullet back
                m_xPos = 40;
                m_yPos = Program.s_RandomNumberGenerator.Next(20);
            }
        }

        public void Draw()
        {                
            // now we need to draw the bullet
            Console.SetCursorPosition(m_xPos, m_yPos);
            Console.Write("."); 
        }
    }

    class Program
    {
        // wheither or not exit
        public static bool s_ExitGame = false;

        // we will want to add some randomness to our game so we need a random number generator.
        public static Random s_RandomNumberGenerator = new Random();

        public static Player s_player = new Player();
        static Bullet[] s_bullet = new Bullet[5];

        static void Main(string[] args)
        {
            // we now need to initialize all the bullets
            for (int i = 0; i < s_bullet.Length; ++i)
            {
                s_bullet[i] = new Bullet();
                s_bullet[i].m_xPos = s_RandomNumberGenerator.Next(40);
                s_bullet[i].m_yPos = s_RandomNumberGenerator.Next(20);
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
            s_player.Update();

            // instead of update the 1 bullet we update each of the 5
            for (int i = 0; i < s_bullet.Length; ++i)
            {
                s_bullet[i].Update();
            }
        }

        static void DrawGraphics()
        {
            // clear the screen
            Console.Clear();

            // Draw the player
            s_player.Draw();

            // instead of drawing the 1 bullet we update each of the 5
            for (int i = 0; i < s_bullet.Length; ++i)
            {
                s_bullet[i].Draw();
            }
        }
    }
}
