using MauiAppPro.ViewsModel;
using System.Diagnostics; // เพิ่ม namespace สำหรับ Debug.WriteLine

namespace MauiAppPro.Pages;

[QueryProperty(nameof(UserId), "userId")]
public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private string _userId;
    public string UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnUserIdReceived(value);
        }
    }

    private void OnUserIdReceived(string userId)
    {
        Debug.WriteLine($"HomePage: Received userId: {userId}"); // เพิ่ม Debug.WriteLine
        Debug.WriteLine($"HomePage: userId before LoadData: {userId}"); // เพิ่ม Debug.WriteLine

        if (string.IsNullOrEmpty(userId))
        {
            Debug.WriteLine("HomePage: userId is null or empty. No data loaded.");
            return;
        }

        _viewModel.LoadData(userId);
    }
}