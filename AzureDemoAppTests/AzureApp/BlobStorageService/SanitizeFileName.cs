using FluentAssertions;

namespace AzureDemoAppTests.AzureApp.BlobStorageService;

public class SanitizeFileName
{
    [Theory]
    [InlineData("<>:\"/\\|?*.docx")]
    public void RemovesUnsafeCharacters(string unsafeFileName)
    {
        // Arrange
        
        // Act
        var result = AzureDemoApp.Services.BlobStorageService.SanitizeFileName(unsafeFileName);
        
        // Assert
        var invalidStrings = Path.GetInvalidFileNameChars().Select(c => c.ToString());
        result.Should().NotContainAny(invalidStrings);
    }

    [Theory]
    [InlineData("filename.docx.")]
    public void RemovesLeadingPeriod(string unsafeFileName)
    {
        // Arrange
        
        // Act
        var result = AzureDemoApp.Services.BlobStorageService.SanitizeFileName(unsafeFileName);
        
        // Assert
        result.Should().NotEndWith(".");
    }
}