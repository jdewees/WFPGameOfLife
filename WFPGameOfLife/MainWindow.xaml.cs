using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WFPGameOfLife.Models;
using WFPGameOfLife.ViewModels;

namespace WFPGameOfLife
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GenerationViewModel generationViewModel;
        public MainWindow()
        {
            InitializeComponent();

            generationViewModel = new GenerationViewModel(WFPGameOfLife.Properties.Settings.Default.UniverseSize);

            CreateGridForUI(generationViewModel);

            DataContext = generationViewModel;
        }

        private void CreateGridForUI(GenerationViewModel generationViewModel)
        {
            for (int row = 0; row < generationViewModel.GameSize; row++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());

                for (int col = 0; col < generationViewModel.GameSize; col++)
                {
                    if (row == 0)
                    {
                        GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    TextBlock cellText = CreateFromCell(generationViewModel.GetCell(row, col));

                    Grid.SetRow(cellText, row);
                    Grid.SetColumn(cellText, col);

                    GameGrid.Children.Add(cellText);
                }

            }
        }

        private TextBlock CreateFromCell(Cell cell)
        {
            TextBlock cellText = new TextBlock();
            cellText.DataContext = cell;
            cellText.InputBindings.Add(CreateMouseClickInputBinding(cell));
            cellText.SetBinding(TextBlock.BackgroundProperty, CreateCellAliveBinding());

            return cellText;
        }
        private InputBinding CreateMouseClickInputBinding(Cell cell)
        {
            InputBinding ib = new InputBinding(generationViewModel.SwitchIsAliveCommand, new MouseGesture(MouseAction.LeftClick));

            ib.CommandParameter = string.Format("{0},{1}", cell.Row, cell.Column);
            return ib;
        }


        private Binding CreateCellAliveBinding()
        {
            return new Binding
            {
                Path = new PropertyPath("IsAlive"),
                Mode = BindingMode.TwoWay,
                Converter = new LifeColourConverter(
                    aliveColour: Brushes.Black,
                    deadColour: Brushes.White
                )
            };
        }

        private async void Animate_Button_Click_Async(object sender, RoutedEventArgs e)
        {
            //run a game loop in here basically
            await Task.Run(() => generationViewModel.AnimateCommand.Execute(null));

        }
    }
}
