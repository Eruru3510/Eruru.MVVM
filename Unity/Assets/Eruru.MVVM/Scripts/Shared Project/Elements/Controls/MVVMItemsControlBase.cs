using System;
using System.Collections;

namespace Eruru.MVVM {

	public abstract partial class MVVMItemsControlBase : MVVMControl {

		public MVVMBinding ItemsSource {

			get {
				return GetBinding (ref _ItemsSource, value => ItemsSource = value);
			}

			set {
				SetBinding (ref _ItemsSource, value, null, targetValue => {
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged -= NotifyCollectionChanged_CollectionChanged;
						NotifyCollectionChanged = null;
					}
					Clear ();
					if (targetValue is IList) {
						IList list = (IList)targetValue;
						for (int i = 0; i < list.Count; i++) {
							Add (Convert (list[i]));
						}
					}
					NotifyCollectionChanged = targetValue as MVVMINotifyCollectionChanged;
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;
					}
				});
			}

		}
		public MVVMFunc<object, object> DataTemplate;

		MVVMINotifyCollectionChanged NotifyCollectionChanged;
		MVVMBinding _ItemsSource;

		private void NotifyCollectionChanged_CollectionChanged (object sender, MVVMNotifyCollectionChangedEventArgs e) {
			switch (e.Action) {
				case MVVMNotifyCollectionChangedAction.Add:
					Add (Convert (e.NewItems[0]));
					break;
				case MVVMNotifyCollectionChangedAction.Remove:
					RemoveAt (e.OldStartingIndex);
					break;
				case MVVMNotifyCollectionChangedAction.Replace:
					Replace (e.OldStartingIndex, Convert (e.NewItems[0]));
					break;
				case MVVMNotifyCollectionChangedAction.Move:
					Move (e.OldStartingIndex, e.NewStartingIndex);
					break;
				case MVVMNotifyCollectionChangedAction.Reset:
					Clear ();
					break;
				default:
					throw new NotImplementedException ();
			}
		}

		protected virtual object Convert (object value) {
			if (DataTemplate == null) {
				return value;
			}
			MVVMControl control = DataTemplate (value) as MVVMControl;
			if (control == null) {
				throw new Exception ("执行DataTemplate委托需要得到MVVMControl，请提前实现委托，或自行重写Convert虚方法");
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