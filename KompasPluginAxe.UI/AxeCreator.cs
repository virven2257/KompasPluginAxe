using Kompas6API5;
using KompasPluginAxe.Core;
using KompasPluginAxe.Core.Enums;
using KompasPluginAxe.Core.Models;

namespace KompasPluginAxe.UI
{
    public class AxeCreator
    {
        /// <summary>
        /// Экземпляр КОМПАС-3D
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Корневое тело
        /// </summary>
        private ksPart _root;

        /// <summary>
        /// Рабочий документ
        /// </summary>
        private ksDocument3D _document;

        /// <summary>
        /// Модель топора
        /// </summary>
        public Axe Axe { get; set; } = new Axe();      
        
        /// <summary>
        /// Инициализация КОМПАС-3D и всех необходимых объектов для
        /// дальнейшей работы
        /// </summary>
        public void Init()
        {
            _kompas = KompasConnector.GetKompasInstance();
            _document = KompasConnector.GetActiveDocument3D(_kompas);
            _root = _document.GetRootPart();
        }

        /// <summary>
        /// Последовательное построение топора
        /// </summary>
        public void CreateAxe()
        {
            CreateBladeSlice();
            CreateBladeSide();
            CreateButt();
            CreateHandleBaseline();
            CreateHandle();
        }

        /// <summary>
        /// Строит рубящую часть топора
        /// </summary>
        private void CreateBladeSlice()
        {
            var sketch = _root.CreateSketch(Plane.Yz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();
            var topPointLeft = new Point2D()
            {
                X = 0,
                Y = -Axe.BladeWidth * 0.5
            };
            var topPointRight = new Point2D()
            {
                X = 0,
                Y = Axe.BladeWidth * 0.5
            };
            var midPointLeft = new Point2D()
            {
                X = Axe.StraightBladeHeight,
                Y = -Axe.BladeWidth * 0.5
            };
            var midPointRight = new Point2D()
            {
                X = Axe.StraightBladeHeight,
                Y = Axe.BladeWidth * 0.5
            };
            var tip = new Point2D()
            {
                X = Axe.BladeHeight,
                Y = 0
            };
            editable.CreateLineSegment(topPointLeft, topPointRight, LineStyle.Main);
            editable.CreateLineSegment(topPointLeft, midPointLeft, LineStyle.Main);
            editable.CreateLineSegment(topPointRight, midPointRight, LineStyle.Main);
            editable.CreateLineSegment(midPointLeft, tip, LineStyle.Main);
            editable.CreateLineSegment(midPointRight, tip, LineStyle.Main);
            definition.EndSketchEditing();
            
            sketch.Extrude(_root, ExtrusionDirection.MiddlePlane,Axe.BladeLength);
        }

        /// <summary>
        /// Строит эскиз режущей части топора сбоку и корректирует его форму
        /// </summary>
        private void CreateBladeSide()
        {
            var sketch = _root.CreateSketch(Plane.Xz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();
            
            var topPointLeft = new Point2D()
            {
                X = -(Axe.ButtLength * 0.5),
                Y = 0
            };
            var topPointRight = new Point2D()
            {
                X = (Axe.ButtLength/2),
                Y = 0
            };
            var bottomPointLeft = new Point2D()
            {
                X = -(Axe.BladeLength/2),
                Y = Axe.BladeHeight
            };
            var bottomPointRight = new Point2D()
            {
                X = (Axe.BladeLength/2),
                Y = Axe.BladeHeight
            };
            
            editable.CreateLineSegment(topPointLeft, bottomPointLeft, LineStyle.Main);
            editable.CreateLineSegment(topPointRight, bottomPointRight, LineStyle.Main);
            editable.CreateLineSegment(bottomPointLeft, bottomPointRight, LineStyle.Main);
            definition.EndSketchEditing();
            sketch.CutBySketch(_root, CutDirection.Reverse);
            
        }

        /// <summary>
        ///  Строит основание топора
        /// </summary>
        private void CreateButt()
        {
            var sketch = _root.CreateSketch(Plane.Yz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();

            var topPointLeftLine = new Point2D()
            {
                X = -Axe.ButtHeight + Axe.ButtRadius,
                Y = -Axe.ButtWidth * 0.5
            };
            
            var leftPointTopLine = new Point2D()
            {
                X = -Axe.ButtHeight,
                Y = -Axe.ButtWidth * 0.5 + Axe.ButtRadius
            };
            
            var rightPointTopLine = new Point2D()
            {
                X = -Axe.ButtHeight,
                Y = Axe.ButtWidth * 0.5  - Axe.ButtRadius
            };

            var topPointRightLine = new Point2D()
            {
                X = -Axe.ButtHeight + Axe.ButtRadius,
                Y = Axe.ButtWidth * 0.5
            };

            var leftCenterRadiusPoint = new Point2D()
            {
                X = topPointLeftLine.X,
                Y = leftPointTopLine.Y
            };

            var rightCenterRadiusPoint = new Point2D()
            {
                X = topPointRightLine.X,
                Y = rightPointTopLine.Y
            };
            
            var midPointLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.StraightSlice1Height),
                Y = - (Axe.Slice1Width * 0.5 + Axe.SpaceAroundEye)
            };
            
            var midPointRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.StraightSlice1Height),
                Y = (Axe.Slice1Width * 0.5 + Axe.SpaceAroundEye)
            };
            
            var bottomPointLeft = new Point2D()
            {
                X = 0,
                Y = - Axe.BladeWidth * 0.5
            };
            var bottomPointRight = new Point2D()
            {
                X = 0,
                Y = Axe.BladeWidth * 0.5
            };

            var eyeTopLineRight = new Point2D()
            {

                X = -(Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = Axe.Slice1Width * 0.5 - Axe.SliceRadius
            };

            var eyeTopLineLeft = new Point2D()
            {

                X = -(Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = -Axe.Slice1Width * 0.5 + Axe.SliceRadius
            };

            var eyeTopPointLeftLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height) + Axe.SliceRadius,
                Y = -Axe.Slice1Width * 0.5
            };

            var eyeTopPointRightLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height) + Axe.SliceRadius,
                Y = Axe.Slice1Width * 0.5
            };

            var leftEyeRadiusPoint = new Point2D()
            {
                X = eyeTopPointLeftLine.X,
                Y = eyeTopLineLeft.Y
            };

            var rightEyeRadiusPoint = new Point2D()
            {
                X = eyeTopPointRightLine.X,
                Y = eyeTopLineRight.Y
            };

            var eyeMidLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = -Axe.Slice1Width * 0.5
            };

            var eyeMidRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = Axe.Slice1Width * 0.5
            };

            var eyeBottom = new Point2D()
            {
                X = -Axe.SpaceBelowEye,
                Y = 0
            };

            Point2D[] eyeBezier =
            {
                eyeMidLeft,
                eyeBottom,
                eyeMidRight
            };
            
            Point2D[] leftBezier =
            {
                topPointLeftLine,
                midPointLeft,
                bottomPointLeft
            };
            
            Point2D[] rightBezier =
            {
                topPointRightLine,
                midPointRight,
                bottomPointRight
            };

            editable.CreateLineSegment(leftPointTopLine, rightPointTopLine, LineStyle.Main);
            editable.CreateLineSegment(bottomPointLeft, bottomPointRight, LineStyle.Main);
            editable.CreateArcByPoint(leftCenterRadiusPoint, Axe.ButtRadius,
                leftPointTopLine, topPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            editable.CreateArcByPoint(rightCenterRadiusPoint, Axe.ButtRadius,
                rightPointTopLine, topPointRightLine,
                Direction.Clockwise, LineStyle.Main);

            editable.CreateBezier(_kompas, leftBezier, LineStyle.Main);
            editable.CreateBezier(_kompas, rightBezier, LineStyle.Main);
            
            editable.CreateLineSegment(eyeTopLineLeft, eyeTopLineRight, LineStyle.Main);
            editable.CreateLineSegment(eyeTopPointLeftLine, eyeMidLeft, LineStyle.Main);
            editable.CreateLineSegment(eyeTopPointRightLine, eyeMidRight, LineStyle.Main);
            editable.CreateArcByPoint(leftEyeRadiusPoint, Axe.SliceRadius,
                eyeTopLineLeft, eyeTopPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            editable.CreateArcByPoint(rightEyeRadiusPoint, Axe.SliceRadius,
                eyeTopLineRight, eyeTopPointRightLine,
                Direction.Clockwise, LineStyle.Main);

            editable.CreateBezier(_kompas, eyeBezier, LineStyle.Main);
            
            definition.EndSketchEditing();

            sketch.Extrude(_root, ExtrusionDirection.MiddlePlane,Axe.ButtLength);
        }

         /// <summary>
         /// Строит основную линию топорища
         /// </summary>
        private void CreateHandleBaseline()
        {
            var sketch = _root.CreateSketch(Plane.Xz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();

            Point2D[] bezier =
            {
                Axe.Slice3Point,
                Axe.Slice4Point,
                Axe.Slice5Point,
                Axe.Slice6Point
            };

            editable.CreateLineSegment(Axe.Slice1Point, Axe.Slice3Point,LineStyle.Main);
            editable.CreateBezier(_kompas, bezier, LineStyle.Main);

            definition.EndSketchEditing();
        }

         /// <summary>
         /// Строит топорище
         /// </summary>
         private void CreateHandle()
         {
            //Координаты плоскостей
             var s1Point3d = new Point3D()
             {
                 X = Axe.Slice1Point.X,
                 Y = 0,
                 Z = -Axe.Slice1Point.Y
             };
             
             var s2Point3d = new Point3D()
             {
                 X = Axe.Slice2Point.X,
                 Y = 0,
                 Z = -Axe.Slice2Point.Y
             };
             
             var s3Point3d = new Point3D()
             {
                 X = Axe.Slice3Point.X,
                 Y = 0,
                 Z = -Axe.Slice3Point.Y
             };
             
             var s4Point3d = new Point3D()
             {
                 X = Axe.Slice4Point.X,
                 Y = 0,
                 Z = -Axe.Slice4Point.Y
             };
             
             var s5Point3d = new Point3D()
             {
                X = Axe.Slice5Point.X,
                Y = 0,
                Z = -Axe.Slice5Point.Y
             };
             
             var s6Point3d = new Point3D()
             {
                 X = Axe.Slice6Point.X,
                 Y = 0,
                 Z = -Axe.Slice6Point.Y
             };
             
             #region Slice 1
            var s1Plane = _root.CreatePerpendicularPlane(s1Point3d,s1Point3d, _kompas);
            var s1Sketch = _root.CreateSketchOnPlane(s1Plane);
            var s1Definition = s1Sketch.GetSketchDefinition();
            var s1Editable = s1Definition.EditSketch();
            
            var s1TopLineRight = new Point2D()
            {

                X = (Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = Axe.Slice1Width * 0.5 - Axe.SliceRadius
            };

            var s1TopLineLeft = new Point2D()
            {

                X = (Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = -Axe.Slice1Width * 0.5 + Axe.SliceRadius
            };

            var s1TopPointLeftLine = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height) - Axe.SliceRadius,
                Y = -Axe.Slice1Width * 0.5
            };

            var s1TopPointRightLine = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height) - Axe.SliceRadius,
                Y = Axe.Slice1Width * 0.5
            };

            var s1LeftRadiusPoint = new Point2D()
            {
                X = s1TopPointLeftLine.X,
                Y = s1TopLineLeft.Y
            };

            var s1RightRadiusPoint = new Point2D()
            {
                X = s1TopPointRightLine.X,
                Y = s1TopLineRight.Y
            };

            var s1MidLeft = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = -Axe.Slice1Width * 0.5
            };

            var s1MidRight = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = Axe.Slice1Width * 0.5
            };

            var s1Bottom = new Point2D()
            {
                X = Axe.SpaceBelowEye,
                Y = 0
            };

            Point2D[] s1Bezier =
            {
                s1MidLeft,
                s1Bottom,
                s1MidRight
            };
            s1Editable.CreateLineSegment(s1TopLineLeft, s1TopLineRight, LineStyle.Main);
            s1Editable.CreateLineSegment(s1TopPointLeftLine, s1MidLeft, LineStyle.Main);
            s1Editable.CreateLineSegment(s1TopPointRightLine, s1MidRight, LineStyle.Main);
            s1Editable.CreateArcByPoint(s1LeftRadiusPoint, Axe.SliceRadius,
                s1TopLineLeft, s1TopPointLeftLine,
                Direction.Clockwise, LineStyle.Main);
            s1Editable.CreateArcByPoint(s1RightRadiusPoint, Axe.SliceRadius,
                s1TopLineRight, s1TopPointRightLine,
                Direction.Counterclockwise, LineStyle.Main);

            s1Editable.CreateBezier(_kompas, s1Bezier, LineStyle.Main);

            s1Definition.EndSketchEditing();

             #endregion

             #region Slice 2
            var s2Plane = _root.CreatePerpendicularPlane(s2Point3d,s2Point3d, _kompas);
            var s2Sketch = _root.CreateSketchOnPlane(s2Plane);
            var s2Definition = s2Sketch.GetSketchDefinition();
            var s2Editable = s2Definition.EditSketch();
            
            var s2TopLineRight = new Point2D()
            {

                X = (Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = Axe.Slice1Width * 0.5 - Axe.SliceRadius
            };

            var s2TopLineLeft = new Point2D()
            {

                X = (Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = -Axe.Slice1Width * 0.5 + Axe.SliceRadius
            };

            var s2TopPointLeftLine = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height) - Axe.SliceRadius,
                Y = -Axe.Slice1Width * 0.5
            };

            var s2TopPointRightLine = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height) - Axe.SliceRadius,
                Y = Axe.Slice1Width * 0.5
            };

            var s2LeftRadiusPoint = new Point2D()
            {
                X = s2TopPointLeftLine.X,
                Y = s2TopLineLeft.Y
            };

            var s2RightRadiusPoint = new Point2D()
            {
                X = s2TopPointRightLine.X,
                Y = s2TopLineRight.Y
            };

            var s2MidLeft = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = -Axe.Slice1Width * 0.5
            };

            var s2MidRight = new Point2D()
            {
                X = (Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = Axe.Slice1Width * 0.5
            };

            var s2Bottom = new Point2D()
            {
                X = Axe.SpaceBelowEye,
                Y = 0
            };

            Point2D[] s2Bezier =
            {
                s2MidLeft,
                s2Bottom,
                s2MidRight
            };
            s2Editable.CreateLineSegment(s2TopLineLeft, s2TopLineRight, LineStyle.Main);
            s2Editable.CreateLineSegment(s2TopPointLeftLine, s2MidLeft, LineStyle.Main);
            s2Editable.CreateLineSegment(s2TopPointRightLine, s2MidRight, LineStyle.Main);
            s2Editable.CreateArcByPoint(s2LeftRadiusPoint, Axe.SliceRadius,
                s2TopLineLeft, s2TopPointLeftLine,
                Direction.Clockwise, LineStyle.Main);
            s2Editable.CreateArcByPoint(s2RightRadiusPoint, Axe.SliceRadius,
                s2TopLineRight, s2TopPointRightLine,
                Direction.Counterclockwise, LineStyle.Main);

            s2Editable.CreateBezier(_kompas, s2Bezier, LineStyle.Main);

            s2Definition.EndSketchEditing();

             #endregion

             #region Slice 3
            var s3Plane = _root.CreatePerpendicularPlane(s4Point3d,s3Point3d, _kompas);
            var s3Sketch = _root.CreateSketchOnPlane(s3Plane);
            var s3Definition = s3Sketch.GetSketchDefinition();
            var s3Editable = s3Definition.EditSketch();
            var fixHeight3 = 23.820483;
            
            var s3TopLineRight = new Point2D()
            {

                X = -(Axe.SpaceBelowEye + Axe.Slice3Height) - fixHeight3,
                Y = Axe.Slice3Width * 0.5 - Axe.SliceRadius
            };

            var s3TopLineLeft = new Point2D()
            {

                X = -(Axe.SpaceBelowEye + Axe.Slice3Height) - fixHeight3,
                Y = -Axe.Slice3Width * 0.5 + Axe.SliceRadius
            };

            var s3TopPointLeftLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice3Height) + Axe.SliceRadius - fixHeight3,
                Y = -Axe.Slice3Width * 0.5
            };

            var s3TopPointRightLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice3Height) + Axe.SliceRadius - fixHeight3,
                Y = Axe.Slice3Width * 0.5
            };

            var s3LeftRadiusPoint = new Point2D()
            {
                X = s3TopPointLeftLine.X,
                Y = s3TopLineLeft.Y
            };

            var s3RightRadiusPoint = new Point2D()
            {
                X = s3TopPointRightLine.X,
                Y = s3TopLineRight.Y
            };

            var s3MidLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice3Height - Axe.StraightSlice3Height) - fixHeight3,
                Y = -Axe.Slice3Width * 0.5
            };

            var s3MidRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice3Height - Axe.StraightSlice3Height) - fixHeight3,
                Y = Axe.Slice3Width * 0.5
            };

            var s3Bottom = new Point2D()
            {
                X = -Axe.SpaceBelowEye - fixHeight3,
                Y = 0
            };

            Point2D[] s3Bezier =
            {
                s3MidLeft,
                s3Bottom,
                s3MidRight
            };
            s3Editable.CreateLineSegment(s3TopLineLeft, s3TopLineRight, LineStyle.Main);
            s3Editable.CreateLineSegment(s3TopPointLeftLine, s3MidLeft, LineStyle.Main);
            s3Editable.CreateLineSegment(s3TopPointRightLine, s3MidRight, LineStyle.Main);
            s3Editable.CreateArcByPoint(s3LeftRadiusPoint, Axe.SliceRadius,
                s3TopLineLeft, s3TopPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            s3Editable.CreateArcByPoint(s3RightRadiusPoint, Axe.SliceRadius,
                s3TopLineRight, s3TopPointRightLine,
                Direction.Clockwise, LineStyle.Main);

            s3Editable.CreateBezier(_kompas, s3Bezier, LineStyle.Main);

            s3Definition.EndSketchEditing();

             #endregion

             #region Slice 4
            var s4Plane = _root.CreatePerpendicularPlane(s4Point3d,s4Point3d, _kompas);
            var s4Sketch = _root.CreateSketchOnPlane(s4Plane);
            var s4Definition = s4Sketch.GetSketchDefinition();
            var s4Editable = s4Definition.EditSketch();
            var fixHeight4 = 23.77121;


            var s4TopLineRight = new Point2D()
            {
            
                X = -(Axe.SpaceBelowEye + Axe.Slice4Height) - fixHeight4,
                Y = Axe.Slice4Width * 0.5 - Axe.SliceRadius
            };
            
            var s4TopLineLeft = new Point2D()
            {
            
                X = -(Axe.SpaceBelowEye + Axe.Slice4Height) - fixHeight4,
                Y = -Axe.Slice4Width * 0.5 + Axe.SliceRadius
            };
            
            var s4TopPointLeftLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice4Height) + Axe.SliceRadius - fixHeight4,
                Y = -Axe.Slice4Width * 0.5
            };
            
            var s4TopPointRightLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice4Height) + Axe.SliceRadius - fixHeight4,
                Y = Axe.Slice4Width * 0.5
            };
            
            var s4LeftRadiusPoint = new Point2D()
            {
                X = s4TopPointLeftLine.X,
                Y = s4TopLineLeft.Y
            };
            
            var s4RightRadiusPoint = new Point2D()
            {
                X = s4TopPointRightLine.X,
                Y = s4TopLineRight.Y
            };
            
            var s4MidLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice4Height - Axe.StraightSlice4Height) - fixHeight4,
                Y = -Axe.Slice4Width * 0.5
            };
            
            var s4MidRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice4Height - Axe.StraightSlice4Height) - fixHeight4,
                Y = Axe.Slice4Width * 0.5
            };
            
            var s4Bottom = new Point2D()
            {
                X = -Axe.SpaceBelowEye - fixHeight4,
                Y = 0
            };
            
            Point2D[] s4Bezier =
            {
                s4MidLeft,
                s4Bottom,
                s4MidRight
            };
            s4Editable.CreateLineSegment(s4TopLineLeft, s4TopLineRight, LineStyle.Main);
            s4Editable.CreateLineSegment(s4TopPointLeftLine, s4MidLeft, LineStyle.Main);
            s4Editable.CreateLineSegment(s4TopPointRightLine, s4MidRight, LineStyle.Main);
            s4Editable.CreateArcByPoint(s4LeftRadiusPoint, Axe.SliceRadius,
                s4TopLineLeft, s4TopPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            s4Editable.CreateArcByPoint(s4RightRadiusPoint, Axe.SliceRadius,
                s4TopLineRight, s4TopPointRightLine,
                Direction.Clockwise, LineStyle.Main);
            
            s4Editable.CreateBezier(_kompas, s4Bezier, LineStyle.Main);
            
            s4Definition.EndSketchEditing();

             #endregion
             
             #region Slice 5
            var s5Plane = _root.CreatePerpendicularPlane(s5Point3d,s5Point3d, _kompas);
            var s5Sketch = _root.CreateSketchOnPlane(s5Plane);
            var s5Definition = s5Sketch.GetSketchDefinition();
            var s5Editable = s5Definition.EditSketch();
            var fixHeight5 = 27.510067;


            var s5TopLineRight = new Point2D()
            {
            
                X = -(Axe.SpaceBelowEye + Axe.Slice5Height) - fixHeight5,
                Y = Axe.Slice5Width * 0.5 - Axe.SliceRadius
            };
            
            var s5TopLineLeft = new Point2D()
            {
            
                X = -(Axe.SpaceBelowEye + Axe.Slice5Height) - fixHeight5,
                Y = -Axe.Slice5Width * 0.5 + Axe.SliceRadius
            };
            
            var s5TopPointLeftLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice5Height) + Axe.SliceRadius - fixHeight5,
                Y = -Axe.Slice5Width * 0.5
            };
            
            var s5TopPointRightLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice5Height) + Axe.SliceRadius - fixHeight5,
                Y = Axe.Slice5Width * 0.5
            };
            
            var s5LeftRadiusPoint = new Point2D()
            {
                X = s5TopPointLeftLine.X,
                Y = s5TopLineLeft.Y
            };
            
            var s5RightRadiusPoint = new Point2D()
            {
                X = s5TopPointRightLine.X,
                Y = s5TopLineRight.Y
            };
            
            var s5MidLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice5Height - Axe.StraightSlice5Height) - fixHeight5,
                Y = -Axe.Slice5Width * 0.5
            };
            
            var s5MidRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice5Height - Axe.StraightSlice5Height) - fixHeight5,
                Y = Axe.Slice5Width * 0.5
            };
            
            var s5Bottom = new Point2D()
            {
                X = -Axe.SpaceBelowEye - fixHeight5,
                Y = 0
            };
            
            Point2D[] s5Bezier =
            {
                s5MidLeft,
                s5Bottom,
                s5MidRight
            };
            s5Editable.CreateLineSegment(s5TopLineLeft, s5TopLineRight, LineStyle.Main);
            s5Editable.CreateLineSegment(s5TopPointLeftLine, s5MidLeft, LineStyle.Main);
            s5Editable.CreateLineSegment(s5TopPointRightLine, s5MidRight, LineStyle.Main);
            s5Editable.CreateArcByPoint(s5LeftRadiusPoint, Axe.SliceRadius,
                s5TopLineLeft, s5TopPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            s5Editable.CreateArcByPoint(s5RightRadiusPoint, Axe.SliceRadius,
                s5TopLineRight, s5TopPointRightLine,
                Direction.Clockwise, LineStyle.Main);
            
            s5Editable.CreateBezier(_kompas, s5Bezier, LineStyle.Main);
            
            s5Definition.EndSketchEditing();

             #endregion
             
             #region Slice 6
            var s6Plane = _root.CreatePerpendicularPlane(s6Point3d, s6Point3d, _kompas);
            var s6Sketch = _root.CreateSketchOnPlane(s6Plane);
            var s6Definition = s6Sketch.GetSketchDefinition();
            var s6Editable = s6Definition.EditSketch();
            var fixHeight6 = 98.235407;


            var s6TopLineRight = new Point2D()
            {
            
                X = -(Axe.SpaceBelowEye + Axe.Slice6Height) - fixHeight6,
                Y = Axe.Slice6Width * 0.5 - Axe.SliceRadius
            };
            
            var s6TopLineLeft = new Point2D()
            {
            
                X = -(Axe.SpaceBelowEye + Axe.Slice6Height) - fixHeight6,
                Y = -Axe.Slice6Width * 0.5 + Axe.SliceRadius
            };
            
            var s6TopPointLeftLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice6Height) + Axe.SliceRadius - fixHeight6,
                Y = -Axe.Slice6Width * 0.5
            };
            
            var s6TopPointRightLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice6Height) + Axe.SliceRadius - fixHeight6,
                Y = Axe.Slice6Width * 0.5
            };
            
            var s6LeftRadiusPoint = new Point2D()
            {
                X = s6TopPointLeftLine.X,
                Y = s6TopLineLeft.Y
            };
            
            var s6RightRadiusPoint = new Point2D()
            {
                X = s6TopPointRightLine.X,
                Y = s6TopLineRight.Y
            };
            
            var s6MidLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice6Height - Axe.StraightSlice6Height) - fixHeight6,
                Y = -Axe.Slice6Width * 0.5
            };
            
            var s6MidRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice6Height - Axe.StraightSlice6Height) - fixHeight6,
                Y = Axe.Slice6Width * 0.5
            };
            
            var s6Bottom = new Point2D()
            {
                X = -Axe.SpaceBelowEye - fixHeight6,
                Y = 0
            };
            
            Point2D[] s6Bezier =
            {
                s6MidLeft,
                s6Bottom,
                s6MidRight
            };
            s6Editable.CreateLineSegment(s6TopLineLeft, s6TopLineRight, LineStyle.Main);
            s6Editable.CreateLineSegment(s6TopPointLeftLine, s6MidLeft, LineStyle.Main);
            s6Editable.CreateLineSegment(s6TopPointRightLine, s6MidRight, LineStyle.Main);
            s6Editable.CreateArcByPoint(s6LeftRadiusPoint, Axe.SliceRadius,
                s6TopLineLeft, s6TopPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            s6Editable.CreateArcByPoint(s6RightRadiusPoint, Axe.SliceRadius,
                s6TopLineRight, s6TopPointRightLine,
                Direction.Clockwise, LineStyle.Main);
            
            s6Editable.CreateBezier(_kompas, s6Bezier, LineStyle.Main);
            
            s6Definition.EndSketchEditing();

             #endregion
             
             var sketches = new[] { s1Sketch, s2Sketch, s3Sketch, s4Sketch, s5Sketch, s6Sketch };
             sketches.BossLoft(_root);
         }
    }
}