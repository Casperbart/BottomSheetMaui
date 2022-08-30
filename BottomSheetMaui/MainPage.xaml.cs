namespace BottomSheetMaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
        await Sheet.OpenSheet();
	}

	private async void Button_ClickedAsync(object sender, EventArgs e)
	{
        await Sheet.OpenSheet();
    }

	private async void CloseButton_ClickedAsync(object sender, EventArgs e)
	{
		await Sheet.CloseSheet();
    }
}

