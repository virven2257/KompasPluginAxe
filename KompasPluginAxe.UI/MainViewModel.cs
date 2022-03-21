using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using KompasPluginAxe.UI.Annotations;

namespace KompasPluginAxe.UI
{
    /// <summary>
    /// Визуальная модель окна
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private ICommand _createAxeCommand;
        
        private Axe _axe = new Axe();
        private bool _enabled = true;

        private readonly AxeCreator _creator =
            new AxeCreator();

        
        /// <summary>
        /// Экземпляр топора
        /// </summary>
        public Axe Axe
        {
            get => _axe;
            set
            {
                _axe = value;
            }
        }

        /// <summary>
        /// Включены ли элементы интерфейса
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Команда, вызывающая построение топора
        /// </summary>
        public ICommand CreateAxeCommand =>
            _createAxeCommand ??= new RelayCommand(async task =>
            {
                Enabled = false;
                await Task.Run(CreateAxe);
                Enabled = true;
            });

        /// <summary>
        /// Построение топора
        /// </summary>
        public void CreateAxe()
        {
            _creator.Axe = _axe;
            _creator.Init();
            _creator.CreateAxe();
        }

        #region  INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}