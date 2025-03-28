using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using MauiApp1.model;
using System.Text.Json;
using System.IO;
using System;

namespace MauiAppPro.ViewsModel;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string fullName;

    [ObservableProperty]
    private int year;

    [ObservableProperty]
    private string major;

    [ObservableProperty]
    private string studentId;

    public HomeViewModel()
    {
    }

    public async void LoadData(string userId)
    {
        Debug.WriteLine($"HomeViewModel: LoadData started with userId: {userId}");

        if (!string.IsNullOrEmpty(userId))
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("users.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                Debug.WriteLine($"HomeViewModel: JSON data loaded: {json}");

                var users = JsonSerializer.Deserialize<List<User>>(json);

                if (users != null)
                {
                    Debug.WriteLine($"HomeViewModel: Users list count: {users.Count}");
                    var user = users.Find(u => u.StudentId.Trim().ToLower() == userId.Trim().ToLower());

                    if (user != null)
                    {
                        Debug.WriteLine($"HomeViewModel: User found: {user.FirstName} {user.LastName}");

                        if (user.FirstName != null && user.LastName != null)
                        {
                            FullName = $"{user.FirstName} {user.LastName}";
                        }
                        else
                        {
                            FullName = "ไม่พบชื่อ";
                        }

                        if (user.Year != null)
                        {
                            Year = (int)user.Year;
                        }
                        else
                        {
                            Year = 0;
                        }

                        if (user.Major != null)
                        {
                            Major = user.Major;
                        }
                        else
                        {
                            Major = "ไม่พบสาขา";
                        }

                        if (user.StudentId != null)
                        {
                            StudentId = user.StudentId;
                        }
                        else
                        {
                            StudentId = "ไม่พบรหัสนิสิต";
                        }
                    }
                    else
                    {
                        Debug.WriteLine("HomeViewModel: User not found.");
                        FullName = "ไม่พบผู้ใช้";
                        Year = 0;
                        Major = "ไม่พบสาขา";
                        StudentId = "ไม่พบรหัสนิสิต";
                    }
                }
                else
                {
                    Debug.WriteLine("HomeViewModel: Users list is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeViewModel: Error loading JSON: {ex.Message}");
            }
        }
        else
        {
            Debug.WriteLine("HomeViewModel: userId is null or empty.");
        }
    }

    [RelayCommand]
    public async Task NavigateToProfilePage()
    {
        await Shell.Current.GoToAsync(nameof(Pages.ProfilePage), new Dictionary<string, object> { { "userId", StudentId } });
    }

    [RelayCommand]
    public async Task NavigateToCurrentTermRegistrationPage()
    {
        await Shell.Current.GoToAsync(nameof(Pages.CurrentTermRegistrationPage), new Dictionary<string, object> { { "userId", StudentId } });
    }

    [RelayCommand]
    public async Task NavigateToSearchCoursesPage()
    {
        await Shell.Current.GoToAsync(nameof(Pages.SearchCoursesPage), new Dictionary<string, object> { { "userId", StudentId } });
    }
}