namespace Olive.Desktop.WPF.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Olive.Data.Uow;

    public class DataPersister
    {
        private IUowData db;

        public DataPersister()
        {
            //this.Db = new UowData();
        }

        public static IUowData Db
        {
            get
            {
                return new UowData();
            }
        }

        //public static IEnumerable<Category> Categories
        //{
        //    get
        //    {
        //        return new UowData().Categories.All().ToList();
        //    }
        //}
    }
}
