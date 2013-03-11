using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlockBusterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isLeftKeyPressed;
        bool isRightKeyPressed;
        bool isGameOver;
        public MainWindow()
        {
            InitializeComponent();
            
            // fix inner coordinates
            for (int i = 0; i < Container.Children.Count; i++)
            {
                if (Container.Children[i] is GameObject)
                {
                    ((GameObject)Container.Children[i]).YCoords = (uint)Canvas.GetTop(Container.Children[i]);
                    ((GameObject)Container.Children[i]).XCoords = (uint)Canvas.GetLeft(Container.Children[i]);
                }
            }


            Thread gameThread = new Thread(GameMethod);
            gameThread.IsBackground = true;
            gameThread.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                isLeftKeyPressed = true;
            }
            if (e.Key == Key.Right)
            {
                isRightKeyPressed = true;
            }
            if (isRightKeyPressed && isLeftKeyPressed)
            {
                MainRacket.IsStill = true;
            }
            else
            {
                if (isRightKeyPressed)
                {
                    MainRacket.IsStill = false;
                    MainRacket.IsMovingLeft = false;
                }
                if (isLeftKeyPressed)
                {
                    MainRacket.IsStill = false;
                    MainRacket.IsMovingLeft = true;
                }
            }
        }
        private void GameMethod()
        {
            while (!isGameOver)
            {
                Action a = () =>
                    {
                        foreach (var movableObject in this.Container.Children)
                        {
                            if (movableObject is IMovable)
                            {
                                ((IMovable)movableObject).Move();
                            }
                        }
                        // check if ball hit the racket on top
                        if (
                            MainBall.XCoords + MainBall.Width < MainRacket.XCoords + MainRacket.Width &&
                            MainBall.XCoords + MainBall.Width > MainRacket.XCoords && 
                            MainBall.YCoords+MainBall.Height<=MainRacket.YCoords+2 &&
                            MainBall.YCoords + MainBall.Height >= MainRacket.YCoords - 2
                            )
                        {
                            MainBall.IsMovingUp = true;
                        }
                        // check is ball hit racket on the side

                        if ( 
                            MainBall.XCoords + MainBall.Width < MainRacket.XCoords + MainRacket.Width &&
                            MainBall.XCoords + MainBall.Width > MainRacket.XCoords &&
                            MainBall.YCoords + MainBall.Height > MainRacket.YCoords + 2
                            )
                        {
                            if (MainBall.IsMovingLeft)
                            {
                                MainBall.IsMovingLeft = false;
                            }
                            else
                            {
                                MainBall.IsMovingLeft = true;
                            }
                        }


                        // check if ball touched the ground
                        if (
                            Math.Abs(MainBall.YCoords+MainBall.Height-Container.Height)<=2
                            )
                        {
                            MessageBox.Show("Game over");
                            isGameOver = true;
                        }

                        // check if ball hit a brick
                        CheckColisions();

                    };
                Dispatcher.Invoke(a);
                Thread.Sleep(10);
            }
        }

        private void CheckColisions()
        {
            foreach (var gameObject in Container.Children)
            {
                if (gameObject is GameObject)
                {
                    if (gameObject is Racket || gameObject is Ball) // we alreday have a check for this
                    {
                        continue;
                    }
                }

                // check if ball hit the brick on top
                if (
                    (
                        MainBall.XCoords + MainBall.Width < ((GameObject)gameObject).XCoords + ((GameObject)gameObject).Width &&
                        MainBall.XCoords + MainBall.Width > ((GameObject)gameObject).XCoords
                    ) &&
                    MainBall.YCoords + MainBall.Height <= ((GameObject)gameObject).YCoords + 2 &&
                    MainBall.YCoords + MainBall.Height >= ((GameObject)gameObject).YCoords - 2
                    )
                {
                    MainBall.IsMovingUp = true;
                    if (gameObject is IRemovable)
                    {
                        this.Container.Children.Remove((GameObject)gameObject);
                        CheckColisions();
                        return;
                    }
                }

                // check if ball hit the brick on bottom
                if (
                    MainBall.XCoords + MainBall.Width < ((GameObject)gameObject).XCoords + ((GameObject)gameObject).Width &&
                    MainBall.XCoords + MainBall.Width > ((GameObject)gameObject).XCoords &&
                    MainBall.YCoords <= ((GameObject)gameObject).YCoords + ((GameObject)gameObject).Height + 2 &&
                    MainBall.YCoords >= ((GameObject)gameObject).YCoords + ((GameObject)gameObject).Height - 2
                    )
                {
                    MainBall.IsMovingUp = false;
                    if (gameObject is IRemovable)
                    {
                        this.Container.Children.Remove((GameObject)gameObject);
                        CheckColisions();
                        return;
                    }
                }

                // check is ball hit brick on the right side

                if (
                    Math.Abs(MainBall.XCoords - (((GameObject)gameObject).XCoords + ((GameObject)gameObject).Width)) <= 2 &&

                    (
                        Math.Abs(MainBall.YCoords - ((GameObject)gameObject).YCoords) <= 2 ||
                        Math.Abs(MainBall.YCoords + MainBall.Height - (((GameObject)gameObject).YCoords + ((GameObject)gameObject).Height)) <= 2
                    )

                    )
                {
                    if (MainBall.IsMovingLeft)
                    {
                        MainBall.IsMovingLeft = false;
                    }
                    else
                    {
                        MainBall.IsMovingLeft = true;
                    }
                    if (gameObject is IRemovable)
                    {
                        this.Container.Children.Remove((GameObject)gameObject);
                        CheckColisions();
                        return;
                    }
                    
                }

                // check is ball hit brick on the left side

                if (
                    Math.Abs(MainBall.XCoords + MainBall.Width - ((GameObject)gameObject).XCoords) <= 2 &&

                    (
                        Math.Abs(MainBall.YCoords - ((GameObject)gameObject).YCoords) <= 2 ||
                        Math.Abs(MainBall.YCoords + MainBall.Height - (((GameObject)gameObject).YCoords + ((GameObject)gameObject).Height)) <= 2
                    )

                    )
                {
                    if (MainBall.IsMovingLeft)
                    {
                        MainBall.IsMovingLeft = false;
                    }
                    else
                    {
                        MainBall.IsMovingLeft = true;
                    }
                    if (gameObject is IRemovable)
                    {
                        this.Container.Children.Remove((GameObject)gameObject);
                        CheckColisions();
                        return;
                    }
                }



            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                isLeftKeyPressed = false;
            }
            if (e.Key == Key.Right)
            {
                isRightKeyPressed = false;
            }
            if (!isRightKeyPressed && !isLeftKeyPressed)
            {
                MainRacket.IsStill = true;
            }
            else
            {
                if (isRightKeyPressed)
                {
                    MainRacket.IsStill = false;
                    MainRacket.IsMovingLeft = false;
                }
                if (isLeftKeyPressed)
                {
                    MainRacket.IsStill = false;
                    MainRacket.IsMovingLeft = true;
                }
            }
        }
    }
}
