using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BlockBusterWPF
{
    public class GameObject : UserControl
    {
        protected uint xCoords, yCoords;
        public uint XCoords
        {
            get
            {
                return this.xCoords;
            }
            set
            {
                if (value + this.Width > ((Canvas)this.Parent).Width || value<0)
                {
                    throw new ArgumentOutOfRangeException("xCoords");
                }
                else
                {
                    this.xCoords = value;
                    Canvas.SetLeft(this, value);
                }
            }
        }
        public uint YCoords
        {
            get
            {
                return this.yCoords;
            }
            set
            {
                if (value + this.Height > ((Canvas)this.Parent).Height || value < 0)
                {
                    throw new ArgumentOutOfRangeException("xCoords");
                }
                else
                {
                    this.yCoords = value;
                    Canvas.SetTop(this, value);
                }
            }
        }
        public bool IsMovingLeft
        {
            get;
            set;
        }
        public bool IsMovingUp
        {
            get;
            set;
        }
        public bool IsStill
        {
            get;
            set;
        }
    }
}
