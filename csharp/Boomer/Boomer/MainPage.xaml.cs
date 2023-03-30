using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas;
using Windows.System;
using System.Threading;
using Windows.UI.Core;
using Boomer.Models;

namespace Boomer
{
    public sealed partial class MainPage : Page
    {
        private Dictionary<VirtualKey, int> pressedKeyDic;
        private CanvasBitmap canvasBitmap;
        private Worker worker;
        private List<List<GridViewItem>> foregroudMesh;

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;


            double width = Window.Current.Bounds.Width / 29;
            double height = Window.Current.Bounds.Height / 38;
            pressedKeyDic = new Dictionary<VirtualKey, int>();
            worker = new Worker(CanvasAnimatredCtrl, ActualSize);

            foregroudMesh = new List<List<GridViewItem>>();
            for (int i = 0; i < (int)height; i++)
            {
                foregroudMesh.Add(new List<GridViewItem>());
                for(int j = 0; j < (int)width; j++)
                {
                    foregroudMesh[i].Add(new GridViewItem()
                    {
                        MinHeight = 38,
                        MinWidth = 29,
                        Height = 38,
                        Width = 29,
                        MaxHeight = 38,
                        MaxWidth = 29
                    });
                }
            }
            GridMesh.DataContext = foregroudMesh;
        }

        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (pressedKeyDic.ContainsKey(args.VirtualKey))
            {
                pressedKeyDic.Remove(args.VirtualKey);
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (!pressedKeyDic.ContainsKey(args.VirtualKey))
            {
                pressedKeyDic.Add(args.VirtualKey, 0);
            }
        }

        private async void CanvasAnimatredCtrl_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            string assetsFolder = AppDomain.CurrentDomain.BaseDirectory + $"\\Assets\\";
            canvasBitmap = await CanvasBitmap.LoadAsync(CanvasAnimatredCtrl, assetsFolder + $"maps\\map.png");

            await worker.CreateResources();
        }

        private void CanvasAnimatredCtrl_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if(null != canvasBitmap)
            {
                args.DrawingSession.DrawImage(canvasBitmap);
            }

            worker.Draw(pressedKeyDic, args);
            Thread.Sleep(90);
        }

        private void CanvasAnimatredCtrl_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {

        }

    }
}
