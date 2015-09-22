using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GuesturesSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        GestureRecognizer gestureRecognizer = new GestureRecognizer();
        UIElement element;
        private void Page_Loaded(object sender,RoutedEventArgs e)
        {
            GestureInputProcessor(gestureRecognizer, LayoutRoot);
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            gestureRecognizer.Holding -= GestureRecognizer_Holding;
            gestureRecognizer.Tapped -= GestureRecognizer_Tapped;
            gestureRecognizer.RightTapped -= GestureRecognizer_RightTapped;
        }

        public void GestureInputProcessor(GestureRecognizer gr,UIElement target)
        {
            this.gestureRecognizer = gr;
            this.element = target;
            this.gestureRecognizer.GestureSettings = GestureSettings.Tap | GestureSettings.Hold | GestureSettings.RightTap | GestureSettings.CrossSlide;
            this.element.PointerCanceled += MainPage_PointerCanceled;
            this.element.PointerPressed += MainPage_PointerPressed;
            this.element.PointerReleased += MainPage_PointerReleased;
            this.element.PointerMoved += Element_PointerMoved;
            gestureRecognizer.Holding += GestureRecognizer_Holding;
            gestureRecognizer.Tapped += GestureRecognizer_Tapped;
            gestureRecognizer.RightTapped += GestureRecognizer_RightTapped;

            CrossSlideThresholds cst = new CrossSlideThresholds();
            cst.SelectionStart = 2;
            cst.SpeedBumpStart = 3;
            cst.SpeedBumpEnd = 4;
            cst.RearrangeStart = 5;
            gestureRecognizer.CrossSlideHorizontally = true;
            gestureRecognizer.CrossSlideThresholds = cst;
            gestureRecognizer.CrossSliding += GestureRecognizer_CrossSliding;
        }

        private void GestureRecognizer_CrossSliding(GestureRecognizer sender, CrossSlidingEventArgs args)
        {
            TxtGestureNotes.Text = "Slide/swipe gesture recognized on cross horizontal";
        }

        private void GestureRecognizer_RightTapped(GestureRecognizer sender, RightTappedEventArgs args)
        {
            TxtGestureNotes.Text = "Right Tap gesture recognized";
        }

        private void GestureRecognizer_Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            TxtGestureNotes.Text = "Tap gesture recognized";
        }

        private void GestureRecognizer_Holding(GestureRecognizer sender, HoldingEventArgs args)
        {
            TxtGestureNotes.Text = "Gesture Holding";

        }

        private void Element_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            this.gestureRecognizer.ProcessMoveEvents(e.GetIntermediatePoints(this.element));
        }

        private void MainPage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.gestureRecognizer.ProcessUpEvent(e.GetCurrentPoint(this.element));
            e.Handled = true;
        }

        private void MainPage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.gestureRecognizer.ProcessDownEvent(e.GetCurrentPoint(this.element));
            this.element.CapturePointer(e.Pointer);
            e.Handled = true;
        }

        private void MainPage_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            this.gestureRecognizer.CompleteGesture();
            e.Handled = true;   
        }
    }
}
