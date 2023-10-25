using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDataGrid.Base
{
    /// <summary>
    /// from https://www.codeproject.com/Articles/819087/Introducing-the-AwesomeObservableCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AwesomeObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public AwesomeObservableCollection()
        {
            base.CollectionChanged += CollectionChanged_Handler;
        }

        public AwesomeObservableCollection(IEnumerable<T> range) : base(range) 
        {
            base.CollectionChanged += CollectionChanged_Handler;
        }

        #region RangeObservableCollection

        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
                item.PropertyChanged += ItemChanged;
            }
            _suppressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void ClearRange()
        {
            _suppressNotification = true;

            foreach (T item in Items)
                item.PropertyChanged -= ItemChanged;

            ClearItems();

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion RangeObservableCollection

        #region ItemObservableCollection

        void CollectionChanged_Handler(object sender, NotifyCollectionChangedEventArgs e)
        {
            //unsubscribe all old objects
            if (e.OldItems != null)
            {
                foreach (T x in e.OldItems)
                    x.PropertyChanged -= ItemChanged;
            }

            //subscribe all new objects
            if (e.NewItems != null)
            {
                foreach (T x in e.NewItems)
                    x.PropertyChanged += ItemChanged;
            }
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion ItemObservableCollection
    }
}
