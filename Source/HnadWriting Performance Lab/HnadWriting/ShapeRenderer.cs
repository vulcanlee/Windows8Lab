using HnadWriting.ComonDX;
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
    public class ShapeRenderer
    {
        TextFormat textFormat;
        Brush sceneColorBrush;
        Brush sceneColorBrush1;
        PathGeometry1 pathGeometry1;
        Stopwatch clock;
        int xx = 300;
        int switchColor = 1;
        int switchFlash = 0;
        bool testMode = false;

        public ShapeRenderer()
        {
            EnableClear = true;
            Show = true;
        }

        public bool EnableClear { get; set; }

        public bool Show { get; set; }

        public virtual void Initialize(DeviceManager deviceManager)
        {

            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, SharpDX.Color.Red);
            sceneColorBrush1 = new SolidColorBrush(deviceManager.ContextDirect2D, SharpDX.Color.Blue);

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

                if (EnableClear)
                {
                    context2D.Clear(SharpDX.Color.White);
                }

                #region 原來的範例 繪製文字與線條

                Debug.WriteLine("Switch Flash {0}", switchFlash);
                if ((switchFlash % 3) == 0)
                {
                    switchFlash++;

                    var sizeX = (float)target.RenderTargetBounds.Width;
                    var sizeY = (float)target.RenderTargetBounds.Height;
                    sizeX = 768;
                    sizeY = 1024;
                    var globalScaling = Matrix.Scaling(Math.Min(sizeX, sizeY));

                    var centerX = (float)(target.RenderTargetBounds.X + sizeX / 2.0f);
                    var centerY = (float)(target.RenderTargetBounds.Y + sizeY / 2.0f);

                    if (textFormat == null)
                    {
                        // Initialize a TextFormat
                        textFormat = new TextFormat(target.DeviceManager.FactoryDirectWrite, "Calibri", 96 * sizeX / 1920) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };
                    }

                    if (pathGeometry1 == null)
                    {
                        var sizeShape = sizeX / 4.0f;

                        // Creates a random geometry inside a circle
                        pathGeometry1 = new PathGeometry1(target.DeviceManager.FactoryDirect2D);
                        var pathSink = pathGeometry1.Open();
                        var startingPoint = new Vector2(sizeShape * 0.5f, 0.0f);
                        pathSink.BeginFigure(startingPoint, FigureBegin.Hollow);
                        for (int i = 0; i < 6; i++)
                        {
                            float angle = (float)i / 128.0f * (float)Math.PI * 2.0f;
                            float R = (float)(Math.Cos(angle) * 0.1f + 0.4f);
                            R *= sizeShape;
                            Vector2 point1 = new Vector2(R * (float)Math.Cos(angle), R * (float)Math.Sin(angle));

                            if ((i & 1) > 0)
                            {
                                R = (float)(Math.Sin(angle * 6.0f) * 0.1f + 0.9f);
                                R *= sizeShape;
                                point1 = new Vector2(R * (float)Math.Cos(angle + Math.PI / 12), R * (float)Math.Sin(angle + Math.PI / 12));
                            }
                            pathSink.AddLine(point1);
                        }
                        pathSink.EndFigure(FigureEnd.Open);
                        pathSink.Close();
                    }

                    context2D.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
                    float t = clock.ElapsedMilliseconds / 1000.0f;
                    xx += 50;
                    t = xx / 1000.0f;
                    context2D.Transform = Matrix.RotationZ((float)(Math.Cos(t * 2.0f * Math.PI * 0.5f))) * Matrix.Translation(centerX, centerY, 0);

                    context2D.DrawText("可愛\nDirect2D1\nDirectWrite", textFormat, new RectangleF(-sizeX / 2.0f, -sizeY / 2.0f, +sizeX / 2.0f, sizeY / 2.0f), sceneColorBrush);

                    float scaling = (float)(Math.Cos(t * 2.0 * Math.PI * 0.25) * 0.5f + 0.5f) * 0.5f + 0.5f;
                    context2D.Transform = Matrix.Scaling(scaling) * Matrix.RotationZ(t * 1.5f) * Matrix.Translation(centerX, centerY, 0);

                    if ((switchColor % 3) != 0)
                    {
                        context2D.DrawGeometry(pathGeometry1, sceneColorBrush, 10.0f);
                        switchColor++;
                    }
                    else
                    {
                        context2D.DrawGeometry(pathGeometry1, sceneColorBrush1, 10.0f);
                        switchColor = 1;
                    }
                }
                else
                {
                    switchFlash++;
                }
                #endregion
                context2D.EndDraw();
                #endregion
            }
        }

    }
}
