using System.Collections.ObjectModel;
using App10.Model;

namespace App10.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<DemoItem> FirstCollection { get; set; }

        public ObservableCollection<DemoItem> SecondCollection { get; set; }

        public MainViewModel()
        {
            FirstCollection = new ObservableCollection<DemoItem>
            {
                new DemoItem{ Title = "Item 1", Subtitle = "This is the same item as before..."},
                new DemoItem{ Title = "Item 2", Subtitle = "It's just the not the same datatemplate"},
                new DemoItem{ Title = "Item 3", Subtitle = "You can place this item wherever you want"},
                new DemoItem{ Title = "Item 4", Subtitle = "You can reorder items in the same list"},
            };

            SecondCollection = new ObservableCollection<DemoItem>();
        }

        /// <summary>
        /// Switches the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void SwitchItem(DemoItem item)
        {
            if (FirstCollection.Contains(item))
            {
                FirstCollection.Remove(item);
                SecondCollection.Add(item);
            }
            else
            {
                SecondCollection.Remove(item);
                FirstCollection.Add(item);
            }
        }
    }
}
