using System;
using System.Security.Principal;
using Kompas6API5;
using KompasAPI7;
using Kompas6Constants;
using Kompas6Constants3D;
using KompasPluginAxe.Core.Enums;
using KompasPluginAxe.Core.Models;
using Point3D = KompasPluginAxe.Core.Models.Point3D;

namespace KompasPluginAxe.Core
{
    public static class Sketcher
    {

        /// <summary>
        /// Получение корневого элемента документа
        /// </summary>
        /// <returns>Корневой элемент документа</returns>
        public static ksPart GetRootPart(this ksDocument3D document)
        {
            return (ksPart)document.GetPart((short)Part_Type.pTop_Part);
        }

        /// <summary>
        /// Создаёт сущность эскиза.
        /// </summary>
        /// <param name="part">Элемент, на котором требуется разместить эскиз</param>
        /// <returns>Сущность эскиза</returns>
        internal static ksEntity CreateSketchEntity(this ksPart part)
        {
            return (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
        }

        /// <summary>
        /// Получает и возвращает объект плоскости
        /// </summary>
        /// <param name="part">Часть, располагаемая на искомой плоскости</param>
        /// <param name="plane">Плоскость (XOY, XOZ или YOZ)</param>
        /// <returns>Объект плоскости</returns>
        internal static object GetPlaneObject(this ksPart part, Plane plane)
        {
            return part.GetDefaultEntity((short)plane);
        }


        /// <summary>
        /// Возвращает свойства эскиза.
        /// </summary>
        /// <param name="sketch">Эскиз</param>
        /// <returns>Свойства эскиза</returns>
        public static ksSketchDefinition GetSketchDefinition(this ksEntity sketch)
        {
            return (ksSketchDefinition)sketch.GetDefinition();
        }


        /// <summary>
        /// Устанавливает плоскость для эскиза.
        /// </summary>
        /// <param name="sketchDefinition">Объект свойств эскиза</param>
        /// <param name="part">Часть, на которой создаётся эскиз</param>
        /// <param name="plane">Плоскость</param>
        /// <returns>Истина, если установить плоскость для эскиза удалось.</returns>
        public static bool SetSketchPlane(this ksSketchDefinition sketchDefinition,
            ksPart part, Plane plane)
        {
            var planeObj = part.GetPlaneObject(plane);
            return sketchDefinition.SetPlane(planeObj);
        }

        /// <summary>
        /// Создаёт эскиз для заданных элемента и плоскости.
        /// </summary>
        /// <param name="part">Элемент</param>
        /// <param name="plane">Плоскость</param>
        /// <returns>Сущность эскиза</returns>
        public static ksEntity CreateSketch(this ksPart part, Plane plane)
        {
            var sketch = part.CreateSketchEntity();
            // Получаем интерфейс свойств эскиза
            var sketchDefinition = sketch.GetSketchDefinition();
            // Устанавливаем плоскость эскиза
            sketchDefinition.SetSketchPlane(part, plane);
            // Создание эскиза
            sketch.Create();
            return sketch;
        }

        /// <summary>
        /// Создаёт эскиз на указанной поверхности
        /// </summary>
        /// <param name="plane">Поверхность</param>
        /// <param name="part">Часть</param>
        /// <returns></returns>
        public static ksEntity CreateSketchOnPlane(this ksPart part, ksEntity plane)
        {
            var sketch = CreateSketchEntity(part);
            var sketchDefinition = GetSketchDefinition(sketch);
            sketchDefinition.SetPlane(plane);
            sketch.Create();
            return sketch;
        }

        public static ksEntity SetSketchPlacement(this ksEntity entity, Point3D origin)
        {
            var sketchDefinition = GetSketchDefinition(entity);
            var surface = (ksSurface)sketchDefinition.GetSurface();
            var surfaceParam = (ksPlaneParam)surface.GetSurfaceParam();
            var placement = (ksPlacement)surfaceParam.GetPlacement();
            placement.SetOrigin(origin.X, origin.Y, origin.Z);
            return entity;
        }

        public static ksEntity CreatePoint3D(this ksPart part, Point3D point, KompasObject kompas)
        {
            var entity = (ksEntity)part.NewEntity((short)Obj3dType.o3d_point3D);
            entity.Create();
        
            var point3D = (IPoint3D)kompas.TransferInterface(entity, (int)ksAPITypeEnum.ksAPI7Dual, 0);
        
            point3D.X = point.X;
            point3D.Y = point.Y;
            point3D.Z = point.Z;
            point3D.Update();
            return entity;
        }
        
        public static ksEntity CreatePerpendicularPlane(this ksPart part, Point3D linePoint, Point3D targetPoint,
            KompasObject kompas)
        {
            var edgeCollection = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_edge);
            edgeCollection.SelectByPoint(linePoint.X, linePoint.Y, linePoint.Z);
        
            var targetPointEntity = CreatePoint3D(part, targetPoint, kompas);
            
            var entity = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planePerpendicular);
            var definition = (ksPlanePerpendicularDefinition)entity.GetDefinition();
            definition.SetPoint(targetPointEntity);
            definition.SetEdge(edgeCollection.First());
            entity.Create();
            return entity;
        }

        /// <summary>
        /// Открывает редактирование эскиза
        /// </summary>
        /// <param name="definition">Свойства эскиза</param>
        /// <returns>Объект эскиза, доступного для редактирования</returns>
        public static ksDocument2D EditSketch(this ksSketchDefinition definition)
        {
            return (ksDocument2D)definition.BeginEdit();
        }

        /// <summary>
        /// Закрывает редактирование эскиза
        /// </summary>
        /// <param name="definition">Свойства эскиза</param>
        /// <returns>Истина, если завершить редактирование эскиза удалось</returns>
        public static bool EndSketchEditing(this ksSketchDefinition definition)
        {
            return definition.EndEdit();
        }

        /// <summary>
        /// Создаёт отрезок
        /// </summary>
        /// <param name="document">2D-документ (эскиз)</param>
        /// <param name="start">Первая точка</param>
        /// <param name="end">Вторая точка</param>
        /// <param name="style">Стиль линии</param>
        /// <returns></returns>
        public static ksDocument2D CreateLineSegment(this ksDocument2D document,
            Point2D start, Point2D end, LineStyle style)
        {
            document.ksLineSeg(start.X, start.Y,
                end.X, end.Y, (int)style);
            return document;
        }
        
        /// <summary>
        /// Создаёт дугу по центру и конечным точкам
        /// </summary>
        /// <param name="document"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="direction"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static ksDocument2D CreateArcByPoint(this ksDocument2D document,
            Point2D center,
            double radius,
            Point2D start,
            Point2D end,
            Direction direction,
            LineStyle style)
        {
            document.ksArcByPoint(center.X, center.Y,
                radius, start.X, start.Y, end.X, end.Y,
                (short)direction, (int)style);
            return document;
        }

        public static ksDocument2D CreateBezier(this ksDocument2D document,
            KompasObject kompas,
            Point2D[] points,
            LineStyle style,
            bool closed = false)
        {
            var isClosed = (short)(closed ? 1 : 0);

            document.ksBezier(isClosed, (int)style);

            var pointParam =
                (ksBezierPointParam)kompas.GetParamStruct(
                    (short)Kompas6Constants.StructType2DEnum.ko_BezierPointParam);

            foreach (var point in points)
            {
                pointParam.x = point.X;
                pointParam.y = point.Y;
                document.ksBezierPoint(pointParam);
            }

            document.ksEndObj();
            return document;
        }
}
}