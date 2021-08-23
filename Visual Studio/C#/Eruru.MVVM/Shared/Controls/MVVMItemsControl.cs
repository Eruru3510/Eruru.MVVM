using System;
using System.Collections;
using System.Collections.Generic;

namespace Eruru.MVVM {

	public partial class MVVMItemsControl : MVVMControl {

		public MVVMBinding ItemsSource {

			get {
				return GetBinding (ref _ItemsSource, value => ItemsSource = value);
			}

			set {
				SetBinding (ref _ItemsSource, value, setTargetValue: targetValue => {
					CancelCollectionChanged ();
					Reset ();
					if (targetValue is IEnumerable) {
						AddRange ((IEnumerable)targetValue);
					}
					NotifyCollectionChanged = targetValue as IMVVMNotifyCollectionChanged;
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged += MVVMItemsControlBase_CollectionChanged;
					}
				}, unbind: () => {
					Reset ();
					CancelCollectionChanged ();
				});
			}

		}
		public MVVMBinding ItemTemplate {

			get {
				return GetBinding (ref _ItemTemplate, value => ItemTemplate = value);
			}

			set {
				SetBinding (ref _ItemTemplate, value);
			}

		}

		protected MVVMFunc<MVVMControl> OnUseDefaultDataTemplate;
		protected MVVMAction<int, MVVMControl> OnInsert;
		protected Action<int> OnRemoveAt;
		protected MVVMAction<int, int> OnMove;
		protected MVVMAction OnReset;

		readonly List<MVVMControl> Items = new List<MVVMControl> ();

		MVVMBinding _ItemsSource;
		MVVMBinding _ItemTemplate;
		IMVVMNotifyCollectionChanged NotifyCollectionChanged;

		protected virtual object Convert (object value) {
			MVVMDataTemplate dataTemplate = _ItemTemplate == null ? null : (_ItemTemplate.GetTargetValue () as MVVMDataTemplate);
			MVVMControl control;
			if (dataTemplate == null) {
				control = OnUseDefaultDataTemplate ();
			} else {
				control = dataTemplate.Build ();
			}
			return control;
		}

		protected virtual void Insert (int index, object value) {
			MVVMControl control = (MVVMControl)Convert (value);
			control.Parent = this;
			control.DataContext = new MVVMBinding (value);
			control.Build (Root);
			Items.Insert (index, control);
			OnInsert (index, control);
		}

		protected virtual void InsertRange (int index, IEnumerable collection) {
			int i = index;
			foreach (object value in collection) {
				Insert (i, value);
				i++;
			}
		}

		protected virtual void Add (object value) {
			Insert (Items.Count, value);
		}

		protected virtual void AddRange (IEnumerable collection) {
			foreach (object value in collection) {
				Add (value);
			}
		}

		protected new virtual void RemoveAt (int index) {
			Items[index].Unbind ();
			Items.RemoveAt (index);
			OnRemoveAt (index);
		}

		protected virtual void Replace (int index, object value) {
			RemoveAt (index);
			Insert (index, value);
		}

		protected new virtual void Move (int oldIndex, int newIndex) {
			MVVMControl control = Items[oldIndex];
			Items.RemoveAt (oldIndex);
			Items.Insert (newIndex, control);
			OnMove (oldIndex, newIndex);
		}

		protected virtual void Reset () {
			foreach (MVVMControl control in Items) {
				control.Unbind ();
			}
			Items.Clear ();
			OnReset ();
		}

		void MVVMItemsControlBase_CollectionChanged (object sender, MVVMNotifyCollectionChangedEventArgs e) {
			//Console.WriteLine ("CollectionChanged {0} from {1} to {2}", e.Action, sender, ItemsSource);
			switch (e.Action) {
				case MVVMNotifyCollectionChangedAction.Add:
					Insert (e.NewStartingIndex, e.NewItems[0]);
					break;
				case MVVMNotifyCollectionChangedAction.Remove:
					RemoveAt (e.OldStartingIndex);
					break;
				case MVVMNotifyCollectionChangedAction.Replace:
					Replace (e.OldStartingIndex, e.NewItems[0]);
					break;
				case MVVMNotifyCollectionChangedAction.Move:
					Move (e.OldStartingIndex, e.NewStartingIndex);
					break;
				case MVVMNotifyCollectionChangedAction.Reset:
					Reset ();
					break;
				default:
					throw new NotImplementedException (e.Action.ToString ());
			}
		}

		void CancelCollectionChanged () {
			if (NotifyCollectionChanged != null) {
				NotifyCollectionChanged.CollectionChanged -= MVVMItemsControlBase_CollectionChanged;
				NotifyCollectionChanged = null;
			}
		}

	}

}