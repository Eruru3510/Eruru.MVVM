using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public abstract class MVVMControlBase : INotifyPropertyChanged, IEnumerable<MVVMControlBase> {

		public event PropertyChangedEventHandler PropertyChanged;
		public MVVMControlBase Root { get; private set; }
		public MVVMControlBase Parent { get; internal set; }
		public List<MVVMControlBase> Controls {

			get {
				return _Controls;
			}

		}
		public MVVMControlBase this[int index] {

			get {
				return Controls[index];
			}

			set {
				RemoveAt (index);
				Insert (index, value);
			}

		}
		public MVVMValidation Validation {

			get {
				return _Validation;
			}

			set {
				_Validation = value;
			}

		}
		public MVVMBinding Name {

			get {
				return GetBinding (ref _Name, binding => Name = binding);
			}

			set {
				SetBinding (ref _Name, value);
			}

		}
		public MVVMBinding DataContext {

			get {
				return GetBinding (ref _DataContext, binding => {
					binding.Path = null;
					DataContext = binding;
				});
			}

			set {
				SetBinding (ref _DataContext, value, setTargetValue: targetValue => {
					foreach (MVVMBinding binding in Bindings) {
						binding.Bind ();
					}
					foreach (MVVMTrigger trigger in Triggers) {
						trigger.Bind ();
					}
					foreach (MVVMControlBase control in Controls) {
						control.DataContext.Bind ();
					}
				}, isDataContext: true);
			}

		}

		internal Dictionary<string, MVVMControlBase> NamedControls = new Dictionary<string, MVVMControlBase> ();

		readonly List<MVVMBinding> Bindings = new List<MVVMBinding> ();
		readonly List<MVVMControlBase> _Controls = new List<MVVMControlBase> ();
		readonly List<MVVMTrigger> Triggers = new List<MVVMTrigger> ();

		MVVMBinding _Name;
		MVVMBinding _DataContext;
		MVVMValidation _Validation = new MVVMValidation ();

		public MVVMControlBase Add (MVVMControlBase control) {
			OnAdd (control);
			Controls.Add (control);
			return this;
		}
		public MVVMControlBase Add (IEnumerable<MVVMControlBase> controls) {
			foreach (MVVMControlBase control in controls) {
				OnAdd (control);
			}
			Controls.AddRange (controls);
			return this;
		}
		public MVVMControlBase Add (params MVVMControlBase[] controls) {
			foreach (MVVMControlBase control in controls) {
				OnAdd (control);
			}
			Controls.AddRange (controls);
			return this;
		}

		public MVVMControlBase Insert (int index, MVVMControlBase control) {
			OnAdd (control);
			Controls.Insert (index, control);
			return this;
		}

		public MVVMControlBase Insert (int index, IEnumerable<MVVMControlBase> controls) {
			foreach (MVVMControlBase control in controls) {
				OnAdd (control);
			}
			Controls.InsertRange (index, controls);
			return this;
		}
		public MVVMControlBase Insert (int index, params MVVMControlBase[] controls) {
			foreach (MVVMControlBase control in controls) {
				OnAdd (control);
			}
			Controls.InsertRange (index, controls);
			return this;
		}

		public MVVMControlBase AddTrigger (MVVMTrigger trigger) {
			trigger.Control = this;
			Triggers.Add (trigger);
			return this;
		}
		public MVVMControlBase AddTrigger (params MVVMTrigger[] triggers) {
			foreach (MVVMTrigger trigger in triggers) {
				trigger.Control = this;
			}
			Triggers.AddRange (triggers);
			return this;
		}

		public void RemoveAt (int index) {
			OnRemove (Controls[index]);
			Controls.RemoveAt (index);
		}

		public void Swap (int oldIndex, int newIndex) {
			MVVMControlBase control = Controls[oldIndex];
			Controls[oldIndex] = Controls[newIndex];
			Controls[newIndex] = control;
		}

		public void Move (int oldIndex, int newIndex) {
			MVVMControlBase control = Controls[oldIndex];
			Controls.RemoveAt (oldIndex);
			Controls.Insert (newIndex, control);
		}

		public void Clear () {
			foreach (MVVMControlBase control in Controls) {
				OnRemove (control);
			}
			Controls.Clear ();
		}

		public MVVMControlBase Build (MVVMControlBase root = null) {
			if (root == null) {
				root = Parent == null ? this : Parent.Root;
			}
			NamedControls.Clear ();
			ForEach (control => control.Register (root));
			DataContext.Bind ();
			return this;
		}

		public void Unbind () {
			ForEach (control => {
				control.Unregister ();
				control.DataContext.Unbind ();
				foreach (MVVMTrigger trigger in control.Triggers) {
					trigger.Unbind ();
				}
				foreach (MVVMBinding binding in control.Bindings) {
					binding.Unbind ();
				}
			});
		}

		public IEnumerator<MVVMControlBase> GetEnumerator () {
			return Controls.GetEnumerator ();
		}

		protected MVVMBinding GetBinding (ref MVVMBinding binding, Action<MVVMBinding> setBinding) {
			if (binding == null) {
				setBinding (new MVVMBinding (string.Empty));
			}
			return binding;
		}

		protected void SetBinding (
			ref MVVMBinding binding, MVVMBinding value,
			MVVMFunc<object> getTargetValue = null, Action<object> setTargetValue = null,
			bool isInteractive = false, bool isText = false,
			bool isDataContext = false, MVVMAction unbind = null, [CallerMemberName] string propertyName = null
		) {
			if (!isDataContext && binding != null) {
				Bindings.Remove (binding);
			}
			binding = value;
			if (binding == null) {
				return;
			}
			binding.Control = this;
			if (propertyName == null) {
				propertyName = MVVMAPI.GetCallerMemberName ();
			}
			binding.TargetPropertyName = propertyName;
			binding.IsDataContext = isDataContext;
			binding.OnGetTargetValue = getTargetValue;
			binding.OnSetTargetValue = setTargetValue;
			binding.OnUnbind = unbind;
			binding.DefaultMode = isInteractive ? MVVMBindingMode.TwoWay : MVVMBindingMode.OneWay;
			binding.DefaultUpdateSourceTrigger = isText ? MVVMUpdateSourceTrigger.LostFocus : MVVMUpdateSourceTrigger.PropertyChanged;
			if (!isDataContext) {
				Bindings.Add (binding);
			}
		}

		internal protected void OnChanged (MVVMBinding binding, MVVMOnChangedType onChangedType = MVVMOnChangedType.PropertyChanged) {
			if (binding == null || binding.BlockOnChanged > 0 || binding.IsTrigger) {
				return;
			}
			switch (binding.GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWayToSource:
					switch (binding.GetUpdateSourceTrigger ()) {
						case MVVMUpdateSourceTrigger.PropertyChanged:
							if (onChangedType == MVVMOnChangedType.PropertyChanged) {
								binding.OneWaySetSourceValue ();
							}
							break;
						case MVVMUpdateSourceTrigger.LostFocus:
							if (onChangedType == MVVMOnChangedType.LostFocus) {
								binding.OneWaySetSourceValue ();
							}
							break;
					}
					break;
			}
			this.RaisePropertyChanged (binding.TargetPropertyName);
		}

		protected void OnCommand (MVVMBinding command, MVVMBinding parameter) {
			if (command == null) {
				return;
			}
			IMVVMCommand relayCommand = command.GetTargetValue<IMVVMCommand> ();
			if (relayCommand == null) {
				return;
			}
			object value = parameter == null ? null : parameter.GetTargetValue ();
			if (relayCommand.CanExecute (value)) {
				relayCommand.Execute (value);
			}
		}

		void ForEach (Action<MVVMControlBase> action) {
			action (this);
			foreach (MVVMControlBase control in Controls) {
				control.ForEach (action);
			}
		}

		void OnAdd (MVVMControlBase control) {
			control.Parent = this;
		}

		void OnRemove (MVVMControlBase control) {
			control.Unbind ();
		}

		void Register (MVVMControlBase root) {
			Root = root;
			if (_Name != null) {
				Root.NamedControls[_Name.GetTargetValue<string> ()] = this;
			}
		}

		void Unregister () {
			if (_Name != null) {
				Root.NamedControls.Remove (_Name.GetTargetValue<string> ());
			}
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return Controls.GetEnumerator ();
		}

		#region ItemsControl

		public MVVMBinding ItemsSource {

			get {
				return GetBinding (ref _ItemsSource, value => ItemsSource = value);
			}

			set {
				SetBinding (ref _ItemsSource, value, setTargetValue: targetValue => {
					ItemsControlItemsSourceUnbind ();
					if (targetValue is IEnumerable) {
						ItemsControlAddRange ((IEnumerable)targetValue);
					}
					NotifyCollectionChanged = targetValue as IMVVMNotifyCollectionChanged;
					if (NotifyCollectionChanged != null) {
						NotifyCollectionChanged.CollectionChanged += ItemsControl_CollectionChanged;
					}
				}, unbind: ItemsControlItemsSourceUnbind);
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

		protected MVVMFunc<MVVMControlBase> OnDefaultDataTemplate;
		protected MVVMAction<int, MVVMControlBase> OnItemsControlInsert;
		protected Action<int> OnItemsControlRemoveAt;
		protected MVVMAction<int, int> OnItemsControlMove;
		protected MVVMAction OnItemsControlReset;

		readonly List<MVVMControlBase> Items = new List<MVVMControlBase> ();

		MVVMBinding _ItemsSource;
		MVVMBinding _ItemTemplate;
		IMVVMNotifyCollectionChanged NotifyCollectionChanged;

		protected virtual object ItemsControlConvert (object value) {
			MVVMDataTemplate dataTemplate = _ItemTemplate == null ? null : (_ItemTemplate.GetTargetValue () as MVVMDataTemplate);
			MVVMControlBase control;
			if (dataTemplate == null) {
				control = OnDefaultDataTemplate ();
			} else {
				control = dataTemplate.Build ();
			}
			return control;
		}

		protected virtual void ItemsControlInsert (int index, object value) {
			MVVMControlBase control = (MVVMControlBase)ItemsControlConvert (value);
			control.Parent = this;
			control.DataContext = new MVVMBinding (value);
			control.Build (Root);
			Items.Insert (index, control);
			OnItemsControlInsert (index, control);
		}

		protected virtual void ItemsControlInsertRange (int index, IEnumerable collection) {
			int i = index;
			foreach (object value in collection) {
				ItemsControlInsert (i, value);
				i++;
			}
		}

		protected virtual void ItemsControlAdd (object value) {
			ItemsControlInsert (Items.Count, value);
		}

		protected virtual void ItemsControlAddRange (IEnumerable collection) {
			foreach (object value in collection) {
				ItemsControlAdd (value);
			}
		}

		protected virtual void ItemsControlRemoveAt (int index) {
			Items[index].Unbind ();
			Items.RemoveAt (index);
			OnItemsControlRemoveAt (index);
		}

		protected virtual void ItemsControlReplace (int index, object value) {
			ItemsControlRemoveAt (index);
			ItemsControlInsert (index, value);
		}

		protected virtual void ItemsControlMove (int oldIndex, int newIndex) {
			MVVMControlBase control = Items[oldIndex];
			Items.RemoveAt (oldIndex);
			Items.Insert (newIndex, control);
			OnItemsControlMove (oldIndex, newIndex);
		}

		protected virtual void ItemsControlReset () {
			foreach (MVVMControlBase control in Items) {
				control.Unbind ();
			}
			Items.Clear ();
			OnItemsControlReset ();
		}

		void ItemsControlItemsSourceUnbind () {
			if (NotifyCollectionChanged != null) {
				NotifyCollectionChanged.CollectionChanged -= ItemsControl_CollectionChanged;
				NotifyCollectionChanged = null;
			}
			ItemsControlReset ();
		}

		void ItemsControl_CollectionChanged (object sender, MVVMNotifyCollectionChangedEventArgs e) {
			switch (e.Action) {
				case MVVMNotifyCollectionChangedAction.Add:
					ItemsControlInsert (e.NewStartingIndex, e.NewItems[0]);
					break;
				case MVVMNotifyCollectionChangedAction.Remove:
					ItemsControlRemoveAt (e.OldStartingIndex);
					break;
				case MVVMNotifyCollectionChangedAction.Replace:
					ItemsControlReplace (e.OldStartingIndex, e.NewItems[0]);
					break;
				case MVVMNotifyCollectionChangedAction.Move:
					ItemsControlMove (e.OldStartingIndex, e.NewStartingIndex);
					break;
				case MVVMNotifyCollectionChangedAction.Reset:
					ItemsControlReset ();
					break;
				default:
					throw new NotImplementedException (e.Action.ToString ());
			}
		}

		#endregion

	}

}