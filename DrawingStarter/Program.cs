using System;
using System.Threading;
using System.Windows.Input;
using DrawPanelLibrary; // ...so we can create DrawingPanels to draw into
using System.Drawing; // ...so we can actually draw (.NET GDI+ graphics)

namespace DrawingStarter
{
    class Program
    {
        static int x = 100;
        static int y = 100;
        static int fruitEaten = 1;
        static int[] snakePossibleLengthX = new int[1000];
        static int[] snakePossibleLengthY = new int[1000];
        static int direction = 0;
        static Random xRandom = new Random();
        static Random yRandom = new Random();
        static int fruitX = xRandom.Next(75, 725);
        static int fruitY = yRandom.Next(75, 425);
        static Font font1 = new Font("Comic Sans", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        static ConsoleKey keyData;

        static void Main(string[] args)
        {
            DrawingPanel panel = new DrawingPanel(800, 500);
            Graphics g = panel.GetGraphics();
            int a = 0;

            while (a == 0)
            {
                MovingSnake(g, panel, x, y, keyData, fruitX, fruitY);
            }

            DrawingPanel.Pause();
        }

        //Draws Fruit and creates new random coordinates
        static void Fruit(Graphics g, DrawingPanel panel, bool fruitEaten)
        {
            if (fruitEaten == true)
            {
                fruitX = xRandom.Next(75, 725);
                fruitY = yRandom.Next(75, 425);
                fruitEaten = false;
            }

            g.FillRectangle(Brushes.Green, fruitX + 7, fruitY + 17, 10, 10);
        }

        //Detects if player has hit fruit
        static void HitDetection(Graphics g, DrawingPanel panel, int x, int y, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (x == snakePossibleLengthX[i])
                {
                    if (y == snakePossibleLengthY[i])
                    {
                        panel.RefreshDisplay();
                        g.DrawString("Game Over", font1, Brushes.Black, 300, 165);
                        g.DrawString($"Final Score: {fruitEaten - 1}", font1, Brushes.Black, 300, 200);
                        panel.RefreshDisplay();
                        Thread.Sleep(5000);
                        Environment.Exit(1);
                    }
                }
            }


            if (x > fruitX - 20 && x < fruitX + 20)
            {
                Fruit(g, panel, false);

                if (y > fruitY - 20 && y < fruitY + 20)
                {
                    Fruit(g, panel, true);
                    fruitEaten++;
                }
            } else
            {
                Fruit(g, panel, false);
            }
        }

        //Keeps track of snake's past coordinates
        static void SnakePossibleArray(int x, int y, int length)
        {
            for (int i = length; i > 0; i--)
            {
                snakePossibleLengthX[i] = snakePossibleLengthX[i - 1];
            }

            for (int i = length; i > 0; i--)
            {
                snakePossibleLengthY[i] = snakePossibleLengthY[i - 1];
            }

            snakePossibleLengthX[0] = x;
            snakePossibleLengthY[0] = y;
        }

        //Increases snakes length
        static void SnakeLengthIncrease(Graphics g, DrawingPanel panel, int length)
        {
            int x = snakePossibleLengthX[length];
            int y = snakePossibleLengthY[length];

            g.FillRectangle(Brushes.Red, x, y, 25, 25);
        }

        //Repeats "SnakeLengthIncrease" based on snake's length
        static void NumberOfSnakeLength(Graphics g, DrawingPanel panel, int length)
        {
            for (int i = 0; i < length; i++)
            {
                SnakeLengthIncrease(g, panel, i);
            }
        }

        //Takes care of controlling and drawing snake(Player)
        static void MovingSnake(Graphics g, DrawingPanel panel, int x, int y, ConsoleKey keyData, int fruitX, int fruitY)
        {
            for (; ; )
            {
                if (x > 800)
                {
                    x -= 1050;
                }
                if (x < 0)
                {
                    x += 1050;
                }
                if (y > 500)
                {
                    y -= 725;
                }
                if (y < 0)
                {
                    y += 725;
                }

                if (panel.Input.KeyAvailable)
                {
                    char key = panel.Input.ReadKey();

                    if (panel.Input.KeyDown('w'))
                    {
                        direction = 1;
                    }

                    if (panel.Input.KeyDown('s'))
                    {
                        direction = 2;
                    }

                    if (panel.Input.KeyDown('d'))
                    {
                        direction = 3;
                    }

                    if (panel.Input.KeyDown('a'))
                    {
                        direction = 4;
                    }

                    switch (direction)
                    {
                        case 1:
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Red, x, y -= 25, 25, 25);
                            HitDetection(g, panel, x, y, fruitEaten);
                            SnakePossibleArray(x, y, fruitEaten);
                            NumberOfSnakeLength(g, panel, fruitEaten);
                            g.DrawString($"Score: {fruitEaten - 1}", font1, Brushes.Black, 660, 10);
                            panel.RefreshDisplay();
                            Thread.Sleep(10);
                            break;
                        case 2:
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Red, x, y += 25, 25, 25);                        
                            HitDetection(g, panel, x, y, fruitEaten);
                            SnakePossibleArray(x, y, fruitEaten);
                            NumberOfSnakeLength(g, panel, fruitEaten);
                            g.DrawString($"Score: {fruitEaten - 1}", font1, Brushes.Black, 660, 10);
                            panel.RefreshDisplay();
                            Thread.Sleep(10);
                            break;
                        case 3:
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Red, x += 25, y, 25, 25);
                            HitDetection(g, panel, x, y, fruitEaten);
                            SnakePossibleArray(x, y, fruitEaten);
                            NumberOfSnakeLength(g, panel, fruitEaten);
                            g.DrawString($"Score: {fruitEaten - 1}", font1, Brushes.Black, 660, 10);
                            panel.RefreshDisplay();
                            Thread.Sleep(10);
                            break;
                        case 4:
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Red, x -= 25, y, 25, 25);
                            HitDetection(g, panel, x, y, fruitEaten);
                            SnakePossibleArray(x, y, fruitEaten);
                            NumberOfSnakeLength(g, panel, fruitEaten);
                            g.DrawString($"Score: {fruitEaten - 1}", font1, Brushes.Black, 660, 10);
                            panel.RefreshDisplay();
                            Thread.Sleep(10);
                            break;
                        default:
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Red, x, y -= 25, 25, 25);
                            HitDetection(g, panel, x, y, fruitEaten);
                            SnakePossibleArray(x, y, fruitEaten);
                            NumberOfSnakeLength(g, panel, fruitEaten);
                            g.DrawString($"Score: {fruitEaten - 1}", font1, Brushes.Black, 660, 10);
                            panel.RefreshDisplay();
                            Thread.Sleep(10);
                            break;
                    }
                }

                Thread.Sleep(100);
                panel.RefreshDisplay();
            }
        }
    }
}