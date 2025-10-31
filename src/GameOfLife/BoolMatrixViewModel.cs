using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameOfLife
{
    public sealed class BoolMatrixViewModel : INotifyPropertyChanged
    {
        public int N { get; }

        public bool[,] Matrix { get; }
        
        public ObservableCollection<BoolCell> Cells { get; }

        public BoolMatrixViewModel(int n)
        {
            N = n;
            Matrix = new bool[n, n];
            Cells = new ObservableCollection<BoolCell>();
            for (int r = 0; r < n; r++)
                for (int c = 0; c < n; c++)
                    Cells.Add(new BoolCell(this, r, c, Matrix[r, c]));
        }

        public void RefreshFromMatrix()
        {
            for (int r = 0; r < N; r++)
                for (int c = 0; c < N; c++)
                {
                    var cell = Cells[r * N + c];
                    if (cell.Value != Matrix[r, c])
                        cell.Value = Matrix[r, c]; // triggers UI updates
                }
        }

        internal void SetMatrixValue(int r, int c, bool value)
        {
            if (Matrix[r, c] == value) return;
            Matrix[r, c] = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
