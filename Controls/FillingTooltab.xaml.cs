﻿using System;
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

namespace ComputerGraphics_Rasterization.Controls
{
    /// <summary>
    /// Interaction logic for FillingTooltab.xaml
    /// </summary>
    public partial class FillingTooltab : UserControl
    {
        public FillingTooltab()
        {
            InitializeComponent();
        }
        public Color SelectedColor => FillColorPicker.SelectedColor ?? Colors.White;
    }
}
