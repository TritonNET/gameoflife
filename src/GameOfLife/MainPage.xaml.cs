using System.Windows.Input;

namespace GameOfLife
{
    public partial class MainPage : ContentPage
    {
        public ICommand ToggleCellCommand { get; }

        public BoolMatrixViewModel VM { get; }

        public MainPage()
        {
            InitializeComponent();

            VM = new BoolMatrixViewModel(n: 10);

            ToggleCellCommand = new Command<BoolCell>(cell =>
            {
                if (cell is null) return;
                cell.Value = !cell.Value;
            });

            BindingContext = this; 
            
            this.SetBinding(BindingContextProperty, new Binding("."));
         
            this.BindingContext = VM;
        }
    }
}
