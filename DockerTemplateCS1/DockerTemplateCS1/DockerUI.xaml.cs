﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
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
using corel = Corel.Interop.VGCore;

namespace DockerTemplateCS1
{

    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;
        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
        }

        private void doSmartTrimMark(object sender, RoutedEventArgs e)
        {

        }
    }
}
