﻿using Caliburn.Micro;
using System.Windows;
using System.Windows.Input;
using TAN.Helpers;

namespace TAN.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window, IShellView
    {
        private AddPartiesView _addPartiesView { get; set; }
        private PaymentInPageVIew _paymentInPageVIew { get; set; }
        private SalePageView _salePageView { get; set; }
        private PurchasePageVIew _purchasePageView { get; set; }
        private PaymentOutPageView _paymentOutPageView { get; set; }
        private CreditViewPageVIew _creditView { get; set; }
        private DebitPageView _debitPageView { get; set; }





        public ShellView()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;


        }
        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        this.WindowState = WindowState.Normal;
                        NormalWindowButton.Visibility = Visibility.Visible;
                        MaximizeWindowButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.WindowState = WindowState.Maximized;
                        NormalWindowButton.Visibility = Visibility.Collapsed;
                        MaximizeWindowButton.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows[0].WindowState = WindowState.Minimized;
        }
        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            NormalWindowButton.Visibility = Visibility.Visible;
            MaximizeWindowButton.Visibility = Visibility.Collapsed;
            Application.Current.Windows[0].WindowState = WindowState.Normal;
        }
        private void NormalWindow(object sender, RoutedEventArgs e)
        {
            NormalWindowButton.Visibility = Visibility.Collapsed;
            MaximizeWindowButton.Visibility = Visibility.Visible;
            Application.Current.Windows[0].WindowState = WindowState.Maximized;
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows[0].Close();
        }
        public void addPartiesInShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _addPartiesView = new AddPartiesView(events, aPIHelper);
            ShellGridMain.Children.Add(_addPartiesView);
        }
        public void clearChildOfShellView()
        {
            ShellGridMain.Children.Remove(_addPartiesView);
        }

        public void addSalePagetoShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _salePageView = new SalePageView(events, aPIHelper);
            ShellGridMain.Children.Add(_salePageView);
        }
        public void removeSalePagefromShellView()
        {
            ShellGridMain.Children.Remove(_salePageView);
        }
        public void addPurchaseePagetoShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _purchasePageView = new PurchasePageVIew(events, aPIHelper);
            ShellGridMain.Children.Add(_purchasePageView);
        }
        public void removePurchasePagefromShellView()
        {
            ShellGridMain.Children.Remove(_purchasePageView);
        }


        public void addPaymentInShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _paymentInPageVIew = new PaymentInPageVIew(events, aPIHelper, true);
            ShellGridMain.Children.Add(_paymentInPageVIew);
        }
        public void removePaymentChildOfShellView()
        {
            ShellGridMain.Children.Remove(_paymentInPageVIew);
        }

        public void addPaymentOutShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _paymentOutPageView = new PaymentOutPageView(events, aPIHelper, true);
            ShellGridMain.Children.Add(_paymentOutPageView);
        }
        public void removePaymentOutChildOfShellView()
        {
            ShellGridMain.Children.Remove(_paymentOutPageView);
        }

        public void addCreditNoteToShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _creditView = new CreditViewPageVIew(events, aPIHelper);
            ShellGridMain.Children.Add(_creditView);
        }
        public void removeCreditNoteFromShellView()
        {
            ShellGridMain.Children.Remove(_creditView);
        }
        public void addDebitNoteToShellView(IEventAggregator events, IAPIHelper aPIHelper)
        {
            _debitPageView = new DebitPageView(events, aPIHelper);
            ShellGridMain.Children.Add(_debitPageView);
        }
        public void removeDebitNoteFromShellView()
        {
            ShellGridMain.Children.Remove(_debitPageView);
        }

        
        
    }
}
