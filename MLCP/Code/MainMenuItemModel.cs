using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MLCP.Code
{
    public class MainMenuItemModel : INotifyPropertyChanged
    {
        string _imageSource;
        string _titleString;
        string _subtitleString;

        public string ImageSource { get { return _imageSource; } set { _imageSource = value; OnPropChanged("ImageSource"); } }
        public string TitleString { get { return _titleString; } set { _titleString = value; OnPropChanged("TitleString"); } }
        public string SubtitleString { get { return _subtitleString; } set { _subtitleString = value; OnPropChanged("SubtitleString"); } }
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
