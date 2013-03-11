using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Ball.xaml
    /// </summary>
    public partial class Ball : GameObject, IMovable
    {
        public Ball()
        {
            InitializeComponent();
        }
        public void Move()
        {
            if (this.IsStill)
            {
                return;
            }
            if (this.IsMovingLeft)
            {
                try
                {
                    this.XCoords -= 2;
                }
                catch (ArgumentOutOfRangeException)
                {

                    this.IsMovingLeft = false;
                    //this.Move();
                    return;
                }
            }
            else
            {
                try
                {
                    this.XCoords += 2;
                }
                catch (ArgumentOutOfRangeException)
                {
                    this.IsMovingLeft = true;
                    //this.Move();
                    return;
                }
            }
            if (this.IsMovingUp)
            {
                try
                {
                    this.YCoords -= 2;
                }
                catch (ArgumentOutOfRangeException)
                {

                    this.IsMovingUp = false;
                    //this.Move();
                    return;
                }
            }
            else
            {
                try
                {
                    this.YCoords += 2;
                }
                catch (ArgumentOutOfRangeException)
                {
                    this.IsMovingUp = true;
                    //this.Move();
                    return;
                }
            }


        }
    }
}
