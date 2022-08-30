
namespace BottomSheetMaui.Controls;

public partial class BottomSheet : ContentView
{
    public static BindableProperty SheetHeightProperty = BindableProperty.Create(nameof(SheetHeight), typeof(double), typeof(BottomSheet), defaultValue: default(double), defaultBindingMode: BindingMode.TwoWay);
    public static BindableProperty SheetContentProperty = BindableProperty.Create( nameof(SheetContent), typeof(View), typeof(BottomSheet), defaultValue: default(View), defaultBindingMode: BindingMode.TwoWay);
    
    public BottomSheet()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        PanContainerRef.Content.TranslationY = SheetHeight + 60;
    }

    public double SheetHeight
    {
        get { return (double)GetValue(SheetHeightProperty); }
        set { SetValue(SheetHeightProperty, value); OnPropertyChanged(); }
    }

    public View SheetContent
    {
        get { return (View)GetValue(SheetContentProperty); }
        set { SetValue(SheetContentProperty, value); OnPropertyChanged(); }
    }

    uint duration = 250;
    double openPosition = (DeviceInfo.Platform == DevicePlatform.Android) ? 20 : 60;
    double currentPosition = 0;

    public async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            currentPosition = e.TotalY;
            if (e.TotalY > 0)
            {
                PanContainerRef.Content.TranslationY = openPosition + e.TotalY;
            }
        }
        else if (e.StatusType == GestureStatus.Completed)
        {
            var threshold = SheetHeight * 0.55;

            if (currentPosition < threshold)
            {
                await OpenSheet();
            }
            else
            {
                await CloseSheet();
            }
        }
    }

    public async Task OpenSheet()
    {
        await Task.WhenAll
        (
            Backdrop.FadeTo(0.4, length: duration),
            Sheet.TranslateTo(0, openPosition, length: duration, easing: Easing.SinIn)
        );

        BottomSheetRef.InputTransparent = false;
        Backdrop.InputTransparent = false;
    }

    public async Task CloseSheet()
    {
        await Task.WhenAll
        (
            Backdrop.FadeTo(0, length: duration),
            PanContainerRef.Content.TranslateTo(x: 0, y: SheetHeight + 60, length: duration, easing: Easing.SinIn)
        );

        BottomSheetRef.InputTransparent = true;
        Backdrop.InputTransparent = true;
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await CloseSheet();
    }
}