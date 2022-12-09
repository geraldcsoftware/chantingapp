namespace ChantingApp.Api.ViewModels;

public class ChantNameEnquiryResultViewModel
{
    public bool IsNameTaken { get; set; }
    public ChantViewModel? ExistingChant { get; set; }
}