using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KompasPluginAxe.UI.Annotations;

namespace KompasPluginAxe.UI
{
    public class MainViewModel
    {
        private Axe _axe = new Axe();

        public Axe Axe
        {
            get => _axe;
            set
            {
                _axe = value;
            }
        }

        public void CreateAxe(float fullHeight, float fullLength, float edgeLength,
            float baseLength, float handleLength)
        {
            
        }
    }
}