using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.System;

namespace Boomer.Models
{
    public class Worker
    {
        private const int ANIMATION_AMOUNT = 8;
        private List<CanvasBitmap> bitmapList;
        private Vector2 position;
        private CanvasAnimatedControl ctrl;

        public Worker(CanvasAnimatedControl ctrl, Vector2 windowSize)
        {
            this.ctrl = ctrl;

            var x = windowSize.X / 2;
            var y = windowSize.Y / 2;

            this.position = new Vector2(x, y);
            this.bitmapList = new List<CanvasBitmap>();
        }

        public async Task CreateResources()
        {
            string assetsFolder = AppDomain.CurrentDomain.BaseDirectory + $"\\Assets\\";

            CanvasBitmap canvasBitmap = await CanvasBitmap.LoadAsync(ctrl, assetsFolder + $"players\\workerstanding.png");
            bitmapList.Add(canvasBitmap);

            for (int index = 0; index < ANIMATION_AMOUNT; index++) // 1 - 8
            {
                canvasBitmap = await CanvasBitmap.LoadAsync(ctrl, assetsFolder + $"players\\workerforward{index}.png");
                bitmapList.Add(canvasBitmap);
            }

            for (int index = 0; index < ANIMATION_AMOUNT; index++) // 9 - 16
            {
                canvasBitmap = await CanvasBitmap.LoadAsync(ctrl, assetsFolder + $"players\\workerback{index}.png");
                bitmapList.Add(canvasBitmap);
            }

            for (int index = 0; index < ANIMATION_AMOUNT; index++) // 17 - 24
            {
                canvasBitmap = await CanvasBitmap.LoadAsync(ctrl, assetsFolder + $"players\\workerleft{index}.png");
                bitmapList.Add(canvasBitmap);
            }

            for (int index = 0; index < ANIMATION_AMOUNT; index++) // 25 - 32
            {
                canvasBitmap = await CanvasBitmap.LoadAsync(ctrl, assetsFolder + $"players\\workerright{index}.png");
                bitmapList.Add(canvasBitmap);
            }
        }

        public void Draw(Dictionary<VirtualKey, int> pressedKeyDic, CanvasAnimatedDrawEventArgs args)
        {
            if(bitmapList.Count == 33)
            {
                int index;
                if (pressedKeyDic.TryGetValue(VirtualKey.W, out index))
                {
                    float Y = position.Y - 3;
                    if (Y <= 0)
                        Y = float.Parse(ctrl.Size.Height.ToString());

                    if (index == 0 || index >= ANIMATION_AMOUNT + 9 - 1)
                        pressedKeyDic[VirtualKey.W] = 9;
                    else
                        pressedKeyDic[VirtualKey.W] = ++index;

                    position = new Vector2(position.X, Y);
                    args.DrawingSession.DrawImage(bitmapList[index], position);
                }
                else if (pressedKeyDic.TryGetValue(VirtualKey.S, out index))
                {
                    float Y = position.Y + 3;
                    if (Y >= ctrl.Size.Height)
                        Y = 0;

                    if (index == 0 || index >= ANIMATION_AMOUNT + 1 - 1)
                        pressedKeyDic[VirtualKey.S] = 1;
                    else
                        pressedKeyDic[VirtualKey.S] = ++index;

                    position = new Vector2(position.X, Y);
                    args.DrawingSession.DrawImage(bitmapList[index], position);
                }
                else if (pressedKeyDic.TryGetValue(VirtualKey.A, out index))
                {
                    float X = position.X - 3;
                    if (X <= 0)
                        X = float.Parse(ctrl.Size.Width.ToString());

                    if (index == 0 || index >= ANIMATION_AMOUNT + 17 - 1)
                        pressedKeyDic[VirtualKey.A] = 17;
                    else
                        pressedKeyDic[VirtualKey.A] = ++index;

                    position = new Vector2(X, position.Y);
                    args.DrawingSession.DrawImage(bitmapList[index], position);
                }
                else if (pressedKeyDic.TryGetValue(VirtualKey.D, out index))
                {
                    float X = position.X + 3;
                    if (X >= ctrl.Size.Width)
                        X = 0;

                    if (index == 0 || index >= ANIMATION_AMOUNT + 25 - 1)
                        pressedKeyDic[VirtualKey.D] = 25;
                    else
                        pressedKeyDic[VirtualKey.D] = ++index;

                    position = new Vector2(X, position.Y);
                    args.DrawingSession.DrawImage(bitmapList[index], position);
                }
                else
                {
                    args.DrawingSession.DrawImage(bitmapList[0], position);
                }
            }
        }
    }
}
