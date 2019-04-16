using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RecolorImageExample
{
    class MainPageViewModel : INotifyPropertyChanged
    {
	    private byte _red;
	    private byte _green;
	    private byte _blue;
	    public event PropertyChangedEventHandler PropertyChanged;

	    public MainPageViewModel()
	    {
			UpdateImageCommand = new Command(() =>
			{
				OnPropertyChanged(nameof(ImageColor));
			});
	    }

	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }

	    public byte Red
	    {
		    get => _red;
		    set
		    {
			    if (_red != value)
			    {
				    _red = value;
					OnPropertyChanged();
				}
		    }
	    }

	    public byte Green
	    {
		    get => _green;
		    set
		    {
			    if (_green != value)
			    {
				    _green = value;
				    OnPropertyChanged();
				}
		    }
	    }

	    public byte Blue
	    {
		    get => _blue;
		    set
		    {
			    if (_blue != value)
			    {
				    _blue = value;
				    OnPropertyChanged();
				}
		    }
	    }

		public ICommand UpdateImageCommand { get; }

	    public Color ImageColor
	    {
		    get { return new Color(ToDouble(Red), ToDouble(Green), ToDouble(Blue)); }
	    }

	    private double ToDouble(byte b)
	    {
		    return ((double) b) / byte.MaxValue;
	    }
    }
}
