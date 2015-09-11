using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalWorkbench.Explorer
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            var fractal = new JuliaSet.JuliSetFractal(new System.Numerics.Complex(-0.70176, -0.3842));
            var fractalImage = new FractalImage(fractal);
            Controls.Add(fractalImage);
            fractalImage.Dock = DockStyle.Fill;
        }
    }
}
