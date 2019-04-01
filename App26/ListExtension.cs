public static class ListExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list) where T : class
        {
            try
            {
                ObservableCollection<T> oCollection = new ObservableCollection<T>();

                foreach (T item in list)
                    oCollection.Add(item);

                return oCollection;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<T>();
            }
        }
    }
