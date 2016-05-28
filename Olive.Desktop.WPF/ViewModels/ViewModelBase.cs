namespace Olive.Desktop.WPF.ViewModels
{
    using System.ComponentModel;
    using System.Windows;
    using Olive.Data.Uow;
    using System.Collections.Generic;
    using System.Windows.Data;
    using System.Reflection;
    using System.Diagnostics;
    using System.IO;
    using System;

    public abstract class ViewModelBase : DependencyObject,INotifyPropertyChanged
    {

        private IUowData db;
        private string assemblyPath = null;

        public string AssemblyPath
        {
            get 
            {
                string process = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string environment = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                this.assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
                this.assemblyPath = Assembly.GetExecutingAssembly().Location;
                this.assemblyPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                return assemblyPath; 
            }
        }

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

        protected ICollectionView GetDefaultView<T>(IEnumerable<T> collection)
        {
            return CollectionViewSource.GetDefaultView(collection);
        }
    }
}
