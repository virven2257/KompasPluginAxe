using System;
using System.Runtime.InteropServices;
using Kompas6API5;

namespace KompasPluginAxe.Core
{
            /// <summary>
    /// Реализует создание экземпляра КОМПАС-3D и подключение к нему
    /// </summary>
            public static class KompasConnector
    {
        /// <summary>
        /// Данная строка передаётся в метод GetActiveObject() объекта
        /// Marshal для получения экземпляра КОМПАС-3D
        /// </summary>
        private const string Api5Name = "KOMPAS.Application.5";

        /// <summary>
        /// Пытается получить существующий экземпляр КОМПАС-3D
        /// </summary>
        /// <param name="kompas">Объект КОМПАС-3D, куда будет помещён </param>
        /// <returns></returns>
        private static bool TryGetActiveKompas(out KompasObject kompas)
        {
            kompas = null;
            try
            {
                // Попытка получить экземпляр КОМПАС-3D через COM
                kompas = (KompasObject)Marshal.GetActiveObject(Api5Name);
                return true;
            }
            catch (COMException)
            {
                return false;
            }
        }

        /// <summary>
        /// Пытается создать новый экземпляр приложения КОМПАС-3D
        /// </summary>
        private static bool TryCreateKompasInstance(out KompasObject kompas)
        {
            try
            {
                // Попытка создать новый экземпляр КОМПАС-3D
                var type = Type.GetTypeFromProgID(Api5Name);
                kompas = (KompasObject)Activator.CreateInstance(type);
                return true;
            }
            catch (COMException)
            {
                kompas = null;
                return false;
            }
        }

        /// <summary>
        /// Пытается получить экземпляр открытого 3D-документа КОМПАС-3D
        /// </summary>
        /// <returns>Экземпляр 3D-документа</returns>
        /// <exception cref="KompasInstanceException">Выбрасывается в случае, 
        /// если документ открыть не удалось</exception>
        public static ksDocument3D GetActiveDocument3D(KompasObject kompas)
        {
            if (kompas == null)
            {
                throw new Exception("Не удалось создать новый экземпляр КОМПАС-3D.");
            }
            var document3d = (ksDocument3D)kompas.Document3D();
            document3d.Create();
            return document3d;
        }
        
        /// <summary>
        /// Получает существующий экземпляр КОМПАС-3D
        /// </summary>
        /// <returns>Истина, если экземпляр КОМПАС-3D был найден и успешно
        /// назначен свойству KompasObject</returns>
        /// <exception cref="KompasInstanceException">Не найден COM-объект КОМПАС-3D</exception>
        public static KompasObject GetKompasInstance()
        {
            var found = TryGetActiveKompas(out var kompas);
            if (!found)
            {
                var created = TryCreateKompasInstance(out kompas);
                if (!created)
                {
                    throw new Exception("Не удалось создать новый экземпляр КОМПАС-3D.");
                }
            }
            kompas.Visible = true;
            kompas.ActivateControllerAPI();
            return kompas;
        }
    }
}