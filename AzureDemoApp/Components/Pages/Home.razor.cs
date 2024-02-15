using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using AzureDemoApp.Models;
using AzureDemoApp.Services;

namespace AzureDemoApp.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    public ILogger<Home> Logger { get; set; } = default!;

    [Inject]
    public BlobStorageService BlobStorageService { get; set; } = default!;
    
    [SupplyParameterFromForm(FormName = "BlobStorageForm")]
    public BlobStorageForm? Model { get; set; }

    private bool _isSubmitting;
    
    protected override void OnInitialized() => Model ??= new BlobStorageForm();

    private void AddFile(InputFileChangeEventArgs e)
    {
        Model!.File = e.File;
    }
    
    private async Task Submit()
    {
        _isSubmitting = true;
        await InvokeAsync(StateHasChanged);
        
        await BlobStorageService.UploadAsync("documents", Model!);
        _isSubmitting = false;
    }
}