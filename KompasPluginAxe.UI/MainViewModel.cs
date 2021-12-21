using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using KompasPluginAxe.Core;
using KompasPluginAxe.UI.Annotations;

namespace KompasPluginAxe.UI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ICommand _createAxeCommand;
        
        private Axe _axe = new Axe();
        private bool _enabled = true;

        private readonly AxeCreator _creator =
            new AxeCreator();

        public Axe Axe
        {
            get => _axe;
            set
            {
                _axe = value;
            }
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateAxeCommand =>
            _createAxeCommand ??= new RelayCommand(async (i) =>
            {
                Enabled = false;
                await Task.Run(CreateAxe)
                    .ContinueWith((t) =>
                    {
                        Enabled = true;
                    });
            });

        public void CreateAxe()
        {
            _creator.Axe = _axe;
            _creator.Init();
            _creator.CreateAxe();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}