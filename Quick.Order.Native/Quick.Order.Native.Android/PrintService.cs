﻿using System;
using System.IO;
using Quick.Order.AppCore.Contracts;
using AndroidX.Print;
using Android.Graphics;
using Plugin.CurrentActivity;

namespace Quick.Order.Native.Droid
{
    public class PrintService : IPrintService
    {
        [Obsolete]
        void IPrintService.Print(byte[] content)
        {
            PrintHelper photoPrinter = new PrintHelper(CrossCurrentActivity.Current.Activity);
            photoPrinter.ScaleMode = PrintHelper.ScaleModeFit;
            Bitmap bitmap = BitmapFactory.DecodeStream(new MemoryStream(content));
            photoPrinter.PrintBitmap("QrCode", bitmap);
        }
    }
   
}