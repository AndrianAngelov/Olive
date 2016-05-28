namespace Olive.Web.MVC.ViewModels
{
    using System.ComponentModel;
    using Olive.Data.Uow;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        private IUowData db;

        public IUowData Db
        {
            get
            {
                if (this.db==null)
                {
                    this.db=new UowData();
                }
                return this.db;
            }
            set
            {
                this.db=value;
            }
        }

        protected IUowData GetUowDataInstance()
        {
            UowData db = new UowData();
            return db;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
