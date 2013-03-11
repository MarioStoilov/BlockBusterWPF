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
    /// Interaction logic for Racket.xaml
    /// </summary>
    public partial class Racket : GameObject, IMovable
    {
        public Racket()
        {
            InitializeComponent();
            this.IsStill = true;
            //this.XCoords = 0;
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
                    this.XCoords -= 4;
                }
                catch (ArgumentOutOfRangeException)
                {

                    return;
                }
            }
            else
            {
                try
                {
                    this.XCoords += 4;
                }
                catch (ArgumentOutOfRangeException)
                {

                    return;
                }
            }
        }
    }
}
