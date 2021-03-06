﻿using HnadWriting.ComonDX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HnadWriting
{
    public class 頁面手寫物件軌跡ShapeRender
    {
        TextFormat textFormat;
        Brush sceneColorBrush;
        Brush sceneColorBrush1;
        Brush sceneColorBrush2;
        Brush sceneColorBrush3;
        PathGeometry1 pathGeometry1;
        Stopwatch clock;
        int xx = 300;
        int switchColor = 1;
        int switchFlash = 0;
        bool testMode = false;
       public 頁面手寫物件軌跡 頁面手寫物件軌跡 = new 頁面手寫物件軌跡();

        int totalDrawing = 0;

        public 頁面手寫物件軌跡ShapeRender()
        {
            EnableClear = true;
            Show = true;
        }

        public bool EnableClear { get; set; }

        public bool Show { get; set; }

        public virtual void Initialize(DeviceManager deviceManager)
        {

            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, SharpDX.Color.Red);
            sceneColorBrush1 = new SolidColorBrush(deviceManager.ContextDirect2D, SharpDX.Color.GreenYellow);
            sceneColorBrush2 = new SolidColorBrush(deviceManager.ContextDirect2D, SharpDX.Color.White);
            sceneColorBrush3 = new SolidColorBrush(deviceManager.ContextDirect2D, SharpDX.Color.Transparent);

            clock = Stopwatch.StartNew();
        }

        public virtual void Render(TargetBase target)
        {
            if (testMode == true)
            {
            }
            else
            {
                #region 原始範例

                if (!Show)
                    return;

                var context2D = target.DeviceManager.ContextDirect2D;

                context2D.BeginDraw();

                //if (totalDrawing == 0)
                //{
                //    totalDrawing++;
                //    Debug.WriteLine("Start");
                //}
                //else if ((totalDrawing % 2) == 0)
                //{
                //    context2D.DrawGeometry(pathGeometry1, sceneColorBrush, 10.0f);
                //    context2D.EndDraw();
                //    context2D.BeginDraw();
                //    totalDrawing = 2;
                //    Debug.WriteLine("單數");
                //}
                //else
                //{
                //    //context2D.EndDraw();
                //    //context2D.BeginDraw();
                //    totalDrawing++;
                //    Debug.WriteLine("偶數");
                //}

                if (EnableClear)
                {
                    context2D.Clear(SharpDX.Color.Transparent);
                }

                #region 原來的範例 繪製文字與線條

                //Debug.WriteLine("Switch Flash {0}", switchFlash);
                //if ((switchFlash % 3) == 0)
                if (true)
                {
                    switchFlash++;

                    var sizeX = (float)target.RenderTargetBounds.Width;
                    var sizeY = (float)target.RenderTargetBounds.Height;
                    sizeX = 768;
                    sizeY = 1024;
                    var globalScaling = Matrix.Scaling(Math.Min(sizeX, sizeY));

                    var centerX = (float)(target.RenderTargetBounds.X + sizeX / 2.0f);
                    var centerY = (float)(target.RenderTargetBounds.Y + sizeY / 2.0f);

                    //if (textFormat == null)
                    //{
                    //    // Initialize a TextFormat
                    //    textFormat = new TextFormat(target.DeviceManager.FactoryDirectWrite, "Calibri", 96 * sizeX / 1920) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };
                    //}

                    //if (pathGeometry1 == null)
                    //{
                    //    var sizeShape = sizeX / 4.0f;

                    //    // Creates a random geometry inside a circle
                    //    pathGeometry1 = new PathGeometry1(target.DeviceManager.FactoryDirect2D);
                    //    var pathSink = pathGeometry1.Open();
                    //    var startingPoint = new Vector2(sizeShape * 0.5f, 0.0f);
                    //    pathSink.BeginFigure(startingPoint, FigureBegin.Hollow);
                    //    //for (int i = 0; i < 6; i++)
                    //    //{
                    //    //    float angle = (float)i / 128.0f * (float)Math.PI * 2.0f;
                    //    //    float R = (float)(Math.Cos(angle) * 0.1f + 0.4f);
                    //    //    R *= sizeShape;
                    //    //    Vector2 point1 = new Vector2(R * (float)Math.Cos(angle), R * (float)Math.Sin(angle));

                    //    //    if ((i & 1) > 0)
                    //    //    {
                    //    //        R = (float)(Math.Sin(angle * 6.0f) * 0.1f + 0.9f);
                    //    //        R *= sizeShape;
                    //    //        point1 = new Vector2(R * (float)Math.Cos(angle + Math.PI / 12), R * (float)Math.Sin(angle + Math.PI / 12));
                    //    //    }
                    //    //    pathSink.AddLine(point1);
                    //    //}
                    //    pathSink.EndFigure(FigureEnd.Open);
                    //    pathSink.Close();
                    //}


                    //context2D.DrawGeometry(pathGeometry1, sceneColorBrush, 10.0f);
                    Random randomGenerator = new Random((int)DateTime.Now.Ticks);

                    int mm = randomGenerator.Next(1500, 2000);
                    int p1x = 0;
                    int p1y = 0;
                    int p2x = 0;
                    int p2y = 0;
                    //for (int ii = 0; ii < mm; ii++)
                    //{
                    //     p1x = randomGenerator.Next(10, 760);
                    //     p1y = randomGenerator.Next(10, 1000);
                    //     p2x = randomGenerator.Next(10, 760);
                    //     p2y = randomGenerator.Next(10, 1000);
                    //    context2D.DrawLine(new Vector2(p1x, p1y), new Vector2(p2x, p2y), sceneColorBrush1, 1);
                    //    //context2D.DrawLine(new Vector2(10, ii), new Vector2(1010, ii), sceneColorBrush1, 1);
                    //}
                    //context2D.PrimitiveBlend = PrimitiveBlend.Copy;
                    //for (int ii = 0; ii < 50; ii++)
                    //{
                    //     p1x = randomGenerator.Next(10, 760);
                    //     p1y = randomGenerator.Next(10, 1000);
                    //     p2x = randomGenerator.Next(10, 760);
                    //     p2y = randomGenerator.Next(10, 1000);
                    //    context2D.DrawLine(new Vector2(p1x, p1y), new Vector2(p2x, p2y), sceneColorBrush3, 10);
                    //}

                    foreach (var item in 頁面手寫物件軌跡.手寫物件軌跡s)
                    {
                        foreach (var item1 in item.手寫物件s)
                        {
                            p1x = (int)item1.X1;
                            p1y = (int)item1.Y1;
                            p2x = (int)item1.X2;
                            p2y = (int)item1.Y2;
                            context2D.DrawLine(new Vector2(p1x, p1y), new Vector2(p2x, p2y), sceneColorBrush1, (float)item.StrokeThickness);
                        }
                    }
                }
                else
                {
                    switchFlash++;
                }
                #endregion

                context2D.Flush();
                context2D.Flush();
                context2D.Flush();
                context2D.EndDraw();
                #endregion
            }
        }
    }
}
