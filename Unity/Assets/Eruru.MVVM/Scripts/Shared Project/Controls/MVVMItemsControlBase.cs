using System;
using System.Collections;

namespace Eruru.MVVM {

	public abstract partial class MVVMItemsControlBase : MVVMControl {

		public MVVMBinding ItemsSource {

			get {
				return GetBinding (ref _ItemsSource, value => ItemsSource = value);
			}

			set {
				InitializeBinding (ref _ItemsSource, value, targetValue => {
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged -= NotifyCollectionChanged_CollectionChanged;
						NotifyCollectionChanged = null;
					}
					Clear ();
					if (targetValue is IList) {
						foreach (object item in (IList)targetValue) {
							Add (ToItem (item));
						}
					}
					NotifyCollectionChanged = targetValue as INotifyCollectionChanged;
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;
					}
				});
			}

		}
		public MVVMFunc<object, object> DataTemplate;

		INotifyCollectionChanged NotifyCollectionChanged;
		MVVMBinding _ItemsSource;

		private void NotifyCollectionChanged_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
			switch (e.Action) {
				case NotifyCollectionChangedAction.Add:
					Add (ToItem (e.NewItems[0]));
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveAt (e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					Replace (e.OldStartingIndex, ToItem (e.NewItems[0]));
					break;
				case NotifyCollectionChangedAction.Move:
					Move (e.OldStartingIndex, e.NewStartingIndex);
					break;
				case NotifyCollectionChangedAction.Reset:
					Clear ();
					break;
				default:
					throw new NotImplementedException ();
			}
		}

		protected virtual object ToItem (object value) {
			if (DataTemplate == null) {
				return value;
			}
			MVVMControl control = DataTemplate.Invoke (value) as MVVMControl;
			if (control == null) {
				throw new Exception ("执行DataTemplate委托需要得到MVVMControl，请提前实现委托");
			}
			control.DataContext.Rebinding (value);
			return control;
		}

		protected abstract void Add (object value);

		protected abstract void RemoveAt (int index);

		protected abstract void Replace (int index, object value);

		protected abstract void Move (int oldIndex, int newIndex);

		protected abstract void Clear ();

	}

}