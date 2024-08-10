using System.ComponentModel;

namespace BlazorAppStoloneApp.Models;

public class JobModel : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string ServiceType { get; set; }
    public string Title { get; set; }
    public string CronExpression { get; set; }
    public TimeSpan? ExpryTime { get; set; }
    JobStatus _status;

    public event PropertyChangedEventHandler? PropertyChanged;

    public JobStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
