using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KompasPluginAxe.UI.Annotations;

namespace KompasPluginAxe.UI
{
    public class Axe : INotifyPropertyChanged
    {
        #region Constants
        
        private const float MinFullHeight = 145;
        private const float MaxFullHeight = 185;
        private const float MinFullLength = 400;
        private const float MaxFullLength = 550;
        private const float MinBladeLength = 110;
        private const float MaxBladeLength = 150;
        private const float MinButtLength = 45;
        private const float MaxButtLength = 60;
        private const float MinKnobHeight = 25;
        private const float MaxKnobHeight = 70;
        
        #endregion

        private float _fullHeight = 160;
        private float _fullLength = 439;
        private float _bladeLength = 120;
        private float _buttLength = 50;
        private float _knobHeight = 35;

        
        public float FullHeight
        {
            get => _fullHeight;
            set
            {
                if (CheckValue(MinFullHeight, value, MaxFullHeight))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                _fullHeight = value;
                if (CheckKnobHeight(value, KnobHeight))
                    throw new ArgumentException("Значение находится вне заданного диапазона");
                OnPropertyChanged();
            }
        }

        public float FullLength
        {
            get => _fullLength;
            set
            {
                if (CheckValue(MinFullLength, value, MaxFullLength))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                if (CheckBladeLength(value, BladeLength))
                    throw new ArgumentException("Значение находится вне заданного диапазона");
                _fullLength = value;
                OnPropertyChanged();
            }
        }

        public float BladeLength
        {
            get => _bladeLength;
            set
            {
                if (CheckValue(MinBladeLength, value, MaxBladeLength))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                if (CheckBladeLength(FullLength, value))
                    throw new ArgumentException("Значение находится вне заданного диапазона");
                _bladeLength = value;
                OnPropertyChanged();
            }
        }

        public float ButtLength
        {
            get => _buttLength;
            set
            {
                if (CheckValue(MinButtLength, value, MaxButtLength))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                _buttLength = value;
                OnPropertyChanged();
            }
        }

        public float KnobHeight
        {
            get => _knobHeight;
            set
            {
                if (CheckValue(MinKnobHeight, value, MaxKnobHeight))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                if (CheckKnobHeight(FullHeight, value))
                    throw new ArgumentException("Значение находится вне заданного диапазона");
                _buttLength = value;
                OnPropertyChanged();
            }
        }

        private bool CheckValue(float min, float value, float max)
        {
            return ((value > max) || (value < min));
        }

        private bool CheckBladeLength(float fullLength, float bladeLength)
        {
            return (bladeLength >= (fullLength / 3));
        }

        private bool CheckKnobHeight(float fullHeight, float knobHeight)
        {
            return (knobHeight >= (fullHeight - 95));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}