using System;
using KompasPluginAxe.UI;
using NUnit.Framework;

namespace KompasPluginAxe.UnitTests
{
    [TestFixture]
    public class AxeTests
    {
        private Axe _axe;

        [SetUp]
        public void SetUp()
        {
            _axe = new Axe();
        }
        
        [Test(Description = "Позитивный тест общей высоты топора")]
        [TestCase(160, 70, Description = "Стандартные значения FullHeight и KnobHeight")]
        [TestCase(170, 80, Description = "Правильные значения FullHeight и KnobHeight")]
        [TestCase(160, 100, Description = "Правильное значение FullHeight и неправильное KnobHeight")]
        public void FullHeightCorrectValues(double fullHeight, double knobHeight)
        {
            _axe.FullHeight = fullHeight;
            var actualHeight = _axe.FullHeight;
            var actualKnobHeight = _axe.KnobHeight;
            var heightMatches = actualHeight == fullHeight;
            var knobHeightMatches = actualKnobHeight == fullHeight - 90;
            Assert.True(heightMatches && knobHeightMatches);
        }

        [Test(Description = "Негативный тест высоты топора: значения за пределами допустимых")]
        [TestCase(100, Description = "Меньше минимума")]
        [TestCase(200, Description = "Больше максимума")]
        public void FullHeightOutOfRange(double fullHeight)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _axe.FullHeight = fullHeight;
            });
        }
        
        [Test(Description = "Позитивный тест высоты края топорища")]
        [TestCase(70, 160, Description = "Стандартные значения KnobHeight и FullHeight")]
        [TestCase(80, 170, Description = "Правильные значения KnobHeight и FullHeight")]
        public void KnobHeightCorrectValues(double knobHeight, double fullHeight)
        {
            _axe.KnobHeight = knobHeight;
            _axe.FullHeight = fullHeight;
            var actualKnobHeight = _axe.KnobHeight;
            var actualHeight = _axe.FullHeight;
            var knobHeightMatches = actualKnobHeight == knobHeight;
            var heightMatches = actualHeight == knobHeight + 90;
            Assert.True(heightMatches && knobHeightMatches);
        }
            
        [Test(Description = "Негативный тест высоты края топорища: значения за пределами допустимых")]
        [TestCase(20, Description = "Меньше минимума")]
        [TestCase(100, Description = "Больше максимума")]
        public void KnobHeightOutOfRange(double fullHeight)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _axe.FullHeight = fullHeight;
            });
        }

        [Test(Description = "Негативный тест высоты топора и его крвя: значения за пределами допустимых")]
        [TestCase(70, 180, Description = "Корректные, но несовместимые значения")]
        public void KnobAndFullHeightsIncompatible(double knobHeight, double fullHeight)
        {
            _axe.KnobHeight = knobHeight;
            _axe.FullHeight = fullHeight;
            var actualKnobHeight = _axe.KnobHeight;
            var actualHeight = _axe.FullHeight;
            var knobHeightMatches = actualKnobHeight == knobHeight;
            var heightMatches = actualHeight == knobHeight + 90;
            Assert.False(heightMatches && knobHeightMatches);
            }
        
        [Test(Description = "Позитивный тест длины лезвия топора")]
        [TestCase(120, 450,
            Description = "Стандартное значения BladeLength и fullLength")]
        public void BladeLengthCorrectValues(double bladeLength, double fullLength)
        {
            _axe.BladeLength = bladeLength;
            _axe.FullLength = fullLength;
            var actualBladeLength = _axe.BladeLength;
            var actualFullLength = _axe.FullLength;
            var bladeLengthMatches = actualBladeLength == bladeLength;
            var fullLengthMatches = actualFullLength == fullLength;
            Assert.True(bladeLengthMatches && fullLengthMatches);
        }

        [Test(Description = "Негативный тест длины лезвия: значения за пределами допустимых")]
        [TestCase(100, Description = "Меньше минимума")]
        [TestCase(200, Description = "Больше максимума")]
        public void BladeLengthOutOfRange(double length)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _axe.FullHeight = length;
            });
        }
        
        [Test(Description = "Позитивный тест длины основания топора")]
        [TestCase(50, Description = "Стандартное значения BladeLength и fullLength")]
        public void ButtLengthCorrectValues(double length)
        {
            _axe.ButtLength = length;
            var actualButtLength = _axe.ButtLength;
            Assert.True(length == actualButtLength);
        }
        
        [Test(Description = "Негативный тест длины основания: значения за пределами допустимых")]
        [TestCase(40, Description = "Меньше минимума")]
        [TestCase(65, Description = "Больше максимума")]
        public void ButtLengthOutOfRange(double length)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _axe.ButtLength = length;
            });
        }
    }
}