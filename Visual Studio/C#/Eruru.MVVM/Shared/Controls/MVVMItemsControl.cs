using System;
using System.Collections;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMItemsControl : MVVMControl {

		public MVVMBinding ItemsSource {

			get {
				return GetBinding (ref _ItemsSource, value => ItemsSource = value);
			}

			set {
				SetBinding (ref _ItemsSource, value, setTargetValue: targetValue => {
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged -= MVVMItemsControlBase_CollectionChanged;
					}
					Reset ();
					if (targetValue is IEnumerable) {
						foreach (object item in (IEnumerable)targetValue) {
							Add (item);
						}
					}
					NotifyCollectionChanged = targetValue as IMVVMNotifyCollectionChanged;
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged += MVVMItemsControlBase_CollectionChanged;
					}
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

		MVVMBinding _ItemsSource;
		MVVMBinding _ItemTemplate;
		IMVVMNotifyCollectionChanged NotifyCollectionChanged;

		protected virtual object Convert (object value) {
			MVVMDataTemplate dataTemplate = _ItemTemplate?.GetTargetValue () as MVVMDataTemplate;
			MVVMControl control;
			if (dataTemplate == null) {
				control = new MVVMLabel (new Label () { AutoSize = true }) {
					Text = new MVVMBinding ()
				};
			} else {
				control = dataTemplate.Build ();
			}
			return control;
		}

		void MVVMItemsControlBase_CollectionChanged (object sender, MVVMNotifyCollectionChangedEventArgs e) {
			switch (e.Action) {
				case MVVMNotifyCollectionChangedAction.Add:
					Add (e.NewItems[0]);
					break;
				case MVVMNotifyCollectionChangedAction.Remove:
					Remove (e.OldStartingIndex);
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

	}

}