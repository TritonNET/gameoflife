using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameOfLife
{
    public sealed class BoolCell : INotifyPropertyChanged
    {
        private readonly int _row;

        private readonly int _col;

        private readonly BoolMatrixViewModel _owner;

        private bool _value;
        
        public int Row => _row;
        
        public int Col => _col;

        public bool Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                _owner.SetMatrixValue(_row, _col, value); // keep matrix in sync
                OnPropertyChanged();
            }
        }

        internal BoolCell(BoolMatrixViewModel owner, int row, int col, bool initial)
        {
            _owner = owner;
            _row = row;
            _col = col;
            _value = initial;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
