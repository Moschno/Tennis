using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using MexicanTennisSimulator.Classes;
using System.Windows.Media.Animation;
using System.Security.Cryptography;


namespace MexicanTennisSimulator
{
    public partial class MainWindow : DXWindow
    {
        private Player _playerOne;
        private Player _playerTwo;
        private Draw _drawOnCanvas;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            _playerOne = new Player(5, 5, 5, 5, 5);
            _playerTwo = new Player(5, 5, 5, 5, 5);
            _drawOnCanvas = new Draw(ref canvasCenter);
            _drawOnCanvas.DrawCourt();
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            WinDebug winDebug = new WinDebug(ref _playerOne, ref _playerTwo);
            winDebug.Show();
        }

        private void tbStrength_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
        }

        private void tbStrength_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
        }

        private void trackBarEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var trackBar = (DevExpress.Xpf.Editors.TrackBarEdit)sender;
            var gbGrid = (Grid)trackBar.Parent;
            var groupBox = (DevExpress.Xpf.LayoutControl.GroupBox)gbGrid.Parent;
            var stackPanel = (StackPanel)groupBox.Parent;
            var groupBoxOuter = (DevExpress.Xpf.LayoutControl.GroupBox)stackPanel.Parent;
            var col = (int)groupBoxOuter.GetValue(Grid.ColumnProperty);

            Player player;
            if (col == 0)
                player = _playerOne;
            else
                player = _playerTwo;

            int value = Convert.ToInt32(e.NewValue);
            if (stackPanel.Children[0].Equals(groupBox))
            {
                player.Strength = value;
            }
            else if (stackPanel.Children[1].Equals(groupBox))
            {
                player.Velocity = value;
            }
            else if (stackPanel.Children[2].Equals(groupBox))
            {
                player.Precision = value;
            }
            else if (stackPanel.Children[3].Equals(groupBox))
            {
                player.Service = value;
            }
            else if (stackPanel.Children[4].Equals(groupBox))
            {
                player.Retörn = value;
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (canvasCenter.IsLoaded)
            {
                _drawOnCanvas.ResizeCourt(); 
            }
        }        
    }
}
