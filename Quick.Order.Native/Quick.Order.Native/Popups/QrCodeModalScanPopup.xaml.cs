﻿using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeModalScanPopup
    {
        public QrCodeModalScanPopup()
        {
            InitializeComponent();
            IsAnimationEnabled = false;

     
        }
    }
}