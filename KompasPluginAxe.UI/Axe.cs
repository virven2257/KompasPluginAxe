using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KompasPluginAxe.Core.Models;
using KompasPluginAxe.UI.Annotations;

namespace KompasPluginAxe.UI
{
    public class Axe : INotifyPropertyChanged
    {
        #region Feilds
        
        #region Constants
        
        //Константы
        private const double MinFullHeight = 145;
        private const double MaxFullHeight = 185;
        private const double MinFullLength = 400;
        private const double MaxFullLength = 550;
        private const double MinBladeLength = 110;
        private const double MaxBladeLength = 150;
        private const double MinButtLength = 45;
        private const double MaxButtLength = 60;
        private const double MinKnobHeight = 55;
        private const double MaxKnobHeight = 95;
        
        #endregion

        #region  Editable
        
        //Представленные в окне значения
        private double _fullHeight = 160;
        private double _fullLength = 439;
        private double _bladeLength = 120;
        private double _buttLength = 50;
        private double _knobHeight = 70;
        
        #endregion
        
        #endregion

        #region Properties

        #region Readonly Properties
        
        //Топор (основание)
        #region Butt

        public double ButtHeight => 72;

        public double ButtWidth => 27;

        public double SpaceAboveEye => 9;

        public double SpaceAroundEye => 3;

        public double SpaceBelowEye => 16;

        public double ButtRadius = 6;
        
        #endregion

        //Лезвие топора
        #region Blade

        public double BladeWidth = 9;
        public double BladeHeight => (FullHeight - ButtHeight);

        public double StraightBladeHeight => BladeHeight - EdgeHeight;

        //public double BladeRadius => 25;

        public double EdgeHeight => 16;

        #endregion

        //Рукоять
        #region Handle

        public double HandleTip => 3;
        
        public double HandleLength => 
            FullLength + HandleTip - (BladeLength - ButtLength)/2;

        public double SpaceBetweenSlices =>
            (FullLength - (FullLength / 2.5)) / 2;

        public double HeightKnobSpaces =>
            KnobHeight / 4;
        
        public Point2D Slice1Point => new Point2D()
        {
            X = -(ButtLength/2 + HandleTip),
            Y = -(ButtHeight - SpaceAboveEye)
        };

        public Point2D Slice2Point => new Point2D()
        {
            X = ButtLength / 2 + 10,
            Y = -(ButtHeight - SpaceAboveEye)
        };

        public Point2D Slice3Point => new Point2D()
        {
            X = (HandleLength / 3.5) + Slice1Point.X,
            Y = Slice2Point.Y
        };

        public Point2D Slice4Point => new Point2D()
        {
            X = (HandleLength / 2.5) + Slice1Point.X,
            Y = Slice3Point.Y + 12
        };

        public Point2D Slice5Point => new Point2D()
        {
            X = Slice4Point.X + SpaceBetweenSlices,
            Y = Slice4Point.Y + 12 - 6.682818
        };

        public Point2D Slice6Point => new Point2D()
        {
            X = Slice5Point.X + SpaceBetweenSlices,
            Y = BladeHeight - KnobHeight - 22.819118
        };

        public double SliceRadius => 6;

        public double SliceWidth => 21;
        
        #endregion
        
        //Срезы рукояти
        #region Slice 1 & 2/Eye

        public double Slice1Width => SliceWidth;

        public double Slice1Height => 47;
        public double StraightSlice1Height => 15;
        
        #endregion

        #region Slice 3

        public double Slice3Width => SliceWidth;

        public double Slice3Height => 47;

        public double StraightSlice3Height => 23.5;

        #endregion

        #region Slice 4

        public double Slice4Width => 18;

        public double Slice4Height => 36.5;

        public double StraightSlice4Height => 25;

        #endregion

        #region Slice 5

        public double Slice5Width => SliceWidth;

        public double Slice5Height => 46;

        public double StraightSlice5Height => 30;

        #endregion

        #region Slice 6

        public double Slice6Width => SliceWidth;

        public double Slice6Height => 56;

        public double StraightSlice6Height => 23;

        #endregion

        #endregion
        
        /// <summary>
        /// Высота всего топора
        /// </summary>
        /// <exception cref="ArgumentException">Недопустимое значение
        /// по MinFullHeight/MaxFullHeight или значение не соответствует зависимому параметру</exception>
        public double FullHeight
        {
            get => _fullHeight;
            set
            {
                if (ValueIsIncorrect(MinFullHeight, value, MaxFullHeight))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                _fullHeight = value;
                if (KnobHeightIsIncorrect(value, KnobHeight))
                    // throw new ArgumentException("Значение не соответствует величине K");
                    KnobHeight = FullHeight - 90;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Длина всего топора
        /// </summary>
        /// <exception cref="ArgumentException">Недопустимое значение
        /// по MinFullLength/MaxFullLength или значение не соответствует зависимому параметру</exception>
        public double FullLength
        {
            get => _fullLength;
            set
            {
                if (ValueIsIncorrect(MinFullLength, value, MaxFullLength))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                if (BladeLengthIsIncorrect(value, BladeLength))
                    throw new ArgumentException("Значение не соответствует величине L1");
                _fullLength = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Длина лезвия
        /// </summary>
        /// <exception cref="ArgumentException">Недопустимое значение
        /// по MinBladeLength/MaxBladeLength или значение не соответствует зависимому параметру</exception>
        public double BladeLength
        {
            get => _bladeLength;
            set
            {
                if (ValueIsIncorrect(MinBladeLength, value, MaxBladeLength))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                if (BladeLengthIsIncorrect(FullLength, value))
                    throw new ArgumentException("Значение не соответствует величине L");
                _bladeLength = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Длина основания 
        /// </summary>
        /// <exception cref="ArgumentException">Недопустимое значение по min/max</exception>
        public double ButtLength
        {
            get => _buttLength;
            set
            {
                if (ValueIsIncorrect(MinButtLength, value, MaxButtLength))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                _buttLength = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Высота края топорища
        /// </summary>
        /// <exception cref="ArgumentException">Недопустимое значение
        /// по Min/Max или значение не соответствует зависимому параметру</exception>
        public double KnobHeight
        {
            get => _knobHeight;
            set
            {
                if (ValueIsIncorrect(MinKnobHeight, value, MaxKnobHeight))
                    throw new ArgumentException("Значение находится вне допустимого диапазона");
                if (KnobHeightIsIncorrect(FullHeight, value))
                    // throw new ArgumentException("Значение не соответствует величине H");
                    FullHeight = 90 + value;
                _knobHeight = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Проверяет, находится ли значение в заданном диапазоне 
        /// </summary>
        /// <param name="min">Минимальное возможное значение</param>
        /// <param name="value">Проверяемое значение</param>
        /// <param name="max">Максимальное возможное значение</param>
        /// <returns>true -- недопустимое значение
        /// false -- допустимое</returns>
        private bool ValueIsIncorrect(double min, double value, double max)
        {
            return ((value > max) || (value < min));
        }

        /// <summary>
        /// Проверяет, соответствут ли значения заданной формуле
        /// </summary>
        /// <param name="fullLength">Длина всего изделия</param>
        /// <param name="bladeLength">Длина лезвия</param>
        /// <returns>true -- недопустимое значение
        /// false -- допустимое</returns>
        private bool BladeLengthIsIncorrect(double fullLength, double bladeLength)
        {
            return (bladeLength > (fullLength / 3));
        }

        /// <summary>
        /// Проверяет, соответствут ли значения заданной формуле
        /// </summary>
        /// <param name="fullHeight">Высота всего изделия</param>
        /// <param name="knobHeight">Высота края топорща</param>
        /// <returns>true -- недопустимое значение
        /// false -- допустимое</returns>
        private bool KnobHeightIsIncorrect(double fullHeight, double knobHeight)
        {
            return Math.Abs(fullHeight - 90 - knobHeight) > double.MinValue;
        }

        #endregion
        
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