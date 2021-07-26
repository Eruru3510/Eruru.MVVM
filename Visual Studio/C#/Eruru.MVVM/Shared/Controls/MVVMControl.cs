using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public partial class MVVMControl : INotifyPropertyChanged, IEnumerable<MVVMControl>, IEnumerable {

		public MVVMBinding Name {

			get {
				return _Name;
			}

			set {
				SetBinding (ref _Name, value);
			}

		}
		public MVVMBinding DataContext {

			get {
				return GetBinding (ref _DataContext, binding => DataContext = binding);
			}

			set {
				SetBinding (ref _DataContext, value, isDataContext: true, setTargetValue: targetValue => {
					OnDataContextChanged?.Invoke ();
					foreach (MVVMControl control in Controls) {
						control.DataContext.Bind ();
					}
				});
			}

		}
		public MVVMControl Root {

			get {
				return _Root;
			}

		}
		public MVVMControl Parent {

			get {
				return _Parent;
			}

		}
		public List<MVVMControl> Controls {

			get {
				return _Controls;
			}

		}
		public MVVMControl this[int index] {

			get {
				return Controls[index];
			}

			set {
				Controls[index].Debind ();
				Controls[index] = value;
			}

		}
		public event PropertyChangedEventHandler PropertyChanged;

		internal Dictionary<string, MVVMControl> ControlNames = new Dictionary<string, MVVMControl> ();

		readonly List<MVVMBinding> Bindings = new List<MVVMBinding> ();
		readonly List<MVVMControl> _Controls = new List<MVVMControl> ();

		event MVVMAction OnDataContextChanged;
		MVVMBinding _Name;
		MVVMBinding _DataContext;
		MVVMControl _Root;
		MVVMControl _Parent;

		public MVVMControl Add (MVVMControl control) {
			control._Parent = this;
			Controls.Add (control);
			return this;
		}
		public MVVMControl Add (params MVVMControl[] controls) {
			foreach (MVVMControl control in controls) {
				Add (control);
			}
			return this;
		}
		public MVVMControl Add (MVVMControl control, int index) {
			control._Parent = this;
			Controls.Insert (index, control);
			return this;
		}

		public void RemoveAt (int index) {
			Controls[index].Debind ();
			Controls.RemoveAt (index);
		}

		public void Swap (int oldIndex, int newIndex) {
			MVVMControl control = Controls[oldIndex];
			Controls[oldIndex] = Controls[newIndex];
			Controls[newIndex] = control;
		}

		public void Move (int oldIndex, int newIndex) {
			MVVMControl control = Controls[oldIndex];
			Controls.RemoveAt (oldIndex);
			Controls.Insert (newIndex, control);
		}

		public void Clear () {
			foreach (MVVMControl control in Controls) {
				control.Debind ();
			}
			Controls.Clear ();
		}

		public MVVMControl Build (MVVMControl root = null) {
			if (root == null) {
				root = this;
			}
			ControlNames.Clear ();
			ForEach (control => control.Register (root));
			DataContext.Bind ();
			return this;
		}

		public void Debind () {
			ForEach (control => {
				control.DataContext.Debind ();
				foreach (MVVMBinding binding in control.Bindings) {
					binding.Debind ();
				}
			});
		}

		void ForEach (Action<MVVMControl> action) {
			action (this);
			foreach (MVVMControl control in Controls) {
				control.ForEach (action);
			}
		}

		void Register (MVVMControl root) {
			_Root = root;
			if (_Name != null) {
				Root.ControlNames[_Name.GetTargetValue<string> ()] = this;
			}
		}

		protected MVVMBinding GetBinding (ref MVVMBinding binding, Action<MVVMBinding> setBinding) {
			if (binding == null) {
				setBinding (new MVVMBinding ());
			}
			return binding;
		}

		protected void SetBinding (
			ref MVVMBinding binding, MVVMBinding value,
			MVVMFunc<object> getTargetValue = null, Action<object> setTargetValue = null,
			bool hasOnChanged = false, bool hasLostFocus = false,
			bool isDataContext = false, [CallerMemberName] string propertyName = null
		) {
			if (!isDataContext && binding != null) {
				OnDataContextChanged -= binding.Bind;
				Bindings.Remove (binding);
			}
			binding = value;
			if (binding == null) {
				return;
			}
			binding._Control = this;
			if (propertyName == null) {
				propertyName = MVVMAPI.GetCallerMemberName ();
			}
			binding._TargetPropertyName = propertyName;
			binding._IsDataContext = isDataContext;
			binding.OnGetTargetValue = getTargetValue;
			binding.OnSetTargetValue = setTargetValue;
			binding.DefaultMode = hasOnChanged ? MVVMBindingMode.TwoWay : MVVMBindingMode.OneWay;
			binding.DefaultUpdateSourceTrigger = hasLostFocus ? MVVMBindingUpdateSourceTrigger.LostFocus : MVVMBindingUpdateSourceTrigger.PropertyChanged;
			if (!isDataContext) {
				Bindings.Add (binding);
				OnDataContextChanged += binding.Bind;
			}
		}

		internal protected void OnChanged (MVVMBinding binding, MVVMBindingOnChangedType onChangedType = MVVMBindingOnChangedType.PropertyChanged) {
			if (binding == null || binding.BlockOnChanged) {
				return;
			}
			switch (binding.GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWayToSource:
					switch (binding.GetUpdateSourceTrigger ()) {
						case MVVMBindingUpdateSourceTrigger.PropertyChanged:
							if (onChangedType == MVVMBindingOnChangedType.PropertyChanged) {
								binding.OneWaySetSourceValue ();
							}
							break;
						case MVVMBindingUpdateSourceTrigger.LostFocus:
							if (onChangedType == MVVMBindingOnChangedType.LostFocus) {
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
			MVVMRelayCommand relayCommand = command.GetTargetValue<MVVMRelayCommand> ();
			if (relayCommand == null) {
				return;
			}
			object value = parameter?.GetTargetValue ();
			if (relayCommand.CanExecute (value)) {
				relayCommand.Execute (value);
			}
		}

		public IEnumerator<MVVMControl> GetEnumerator () {
			return Controls.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return Controls.GetEnumerator ();
		}

	}

}