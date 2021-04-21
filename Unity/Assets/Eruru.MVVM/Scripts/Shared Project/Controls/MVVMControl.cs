using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public partial class MVVMControl : INotifyPropertyChanged, IEnumerable<MVVMControl> {

		public event PropertyChangedEventHandler PropertyChanged;
		public MVVMView View {

			get {
				return _View;
			}

			protected set {
				_View = value;
			}

		}
		public MVVMControl Parent { get; protected set; }
		public MVVMBinding DataContext {

			get {
				return _DataContext == null ? (DataContext = new MVVMBinding ()) : _DataContext;
			}

			set {
				if (value != null) {
					value.Rebinding (ParentDataContext);
				}
				InitializeBinding (ref _DataContext, value,
					targetValue => {
						if (OnDataContextChanged != null) {
							OnDataContextChanged (targetValue);
						}
						foreach (MVVMControl control in Controls) {
							control.DataContext.Rebinding (targetValue);
						}
					}, () => DataContext.GetValue (),
					null, null,
					null, null,
					true
				);
			}

		}

		internal object ParentDataContext {

			get {
				if (Parent != null) {
					return Parent.DataContext.GetValue ();
				}
				if (View != null) {
					return View.DataContext;
				}
				return null;
			}

		}

		readonly List<MVVMControl> Controls = new List<MVVMControl> ();

		event Action<object> OnDataContextChanged;
		MVVMBinding _DataContext;
		MVVMView _View;
		bool IsChanging;

		public MVVMControl Add (MVVMControl control) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			control.Parent = this;
			control.DataContext.Rebinding (DataContext.GetValue ());
			Controls.Add (control);
			return this;
		}
		public MVVMControl Add (params MVVMControl[] controls) {
			if (controls == null) {
				throw new ArgumentNullException ("controls");
			}
			foreach (MVVMControl control in controls) {
				Add (control);
			}
			return this;
		}

		protected void InitializeBinding (
			ref MVVMBinding binding, MVVMBinding value,
			Action<object> setTargetValue = null, MVVMFunc<object> getTargetValue = null,
			MVVMAction registerChanged = null, MVVMAction unregisterChanged = null,
			MVVMAction registerLostFocus = null, MVVMAction unregisterLostFocus = null,
			bool isDataContext = false, [CallerMemberName] string propertyName = null
		) {
			if (binding != null) {
				OnDataContextChanged -= binding.Rebinding;
				binding.UnregisterPropertyChanged ();
			}
			if (unregisterChanged != null) {
				unregisterChanged ();
			}
			if (unregisterLostFocus != null) {
				unregisterLostFocus ();
			}
			binding = value;
			if (value == null) {
				return;
			}
			value.Control = this;
			value.TargetPropertyName = propertyName == null ? MVVMApi.GetCallerMemberName () : propertyName;
			if (setTargetValue != null) {
				value.SetTargetValueAction = setTargetValue;
			}
			if (getTargetValue != null) {
				value.GetTargetValueFunc = getTargetValue;
			}
			if (registerChanged == null) {
				value.DefaultMode = MVVMBindingMode.OneWay;
			} else {
				value.DefaultMode = MVVMBindingMode.TwoWay;
				registerChanged ();
			}
			if (registerLostFocus == null) {
				value.DefaultUpdateSourceTrigger = MVVMBindingUpdateSourceTrigger.PropertyChanged;
			} else {
				value.DefaultUpdateSourceTrigger = MVVMBindingUpdateSourceTrigger.LostFocus;
				registerLostFocus ();
			}
			if (isDataContext) {
				return;
			}
			value.Rebinding (DataContext.GetValue ());
			OnDataContextChanged += binding.Rebinding;
		}

		protected MVVMBinding GetBinding (ref MVVMBinding binding, Action<MVVMBinding> set) {
			if (binding == null) {
				set (new MVVMBinding (string.Empty));
			}
			return binding;
		}

		internal void Changed (MVVMBinding binding, object value, MVVMBindingChangedType type = MVVMBindingChangedType.PropertyChanged) {
			if (binding == null) {
				return;
			}
			if (IsChanging) {
				return;
			}
			IsChanging = true;
			switch (binding.GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWayToSource:
					switch (binding.GetUpdateSourceTrigger ()) {
						case MVVMBindingUpdateSourceTrigger.PropertyChanged:
							binding.SetValue (value);
							break;
						case MVVMBindingUpdateSourceTrigger.LostFocus:
							if (type == MVVMBindingChangedType.LostFocus) {
								binding.SetValue (value);
							}
							break;
						case MVVMBindingUpdateSourceTrigger.Explicit:
							if (type == MVVMBindingChangedType.UpdateSource) {
								binding.SetValue (value);
							}
							break;

					}
					break;
			}
			IsChanging = false;
			Notify (binding);
		}

		protected void Command (MVVMBinding command, MVVMBinding parameter) {
			if (command == null) {
				return;
			}
			MVVMRelayCommand relayCommand = command.GetValue () as MVVMRelayCommand;
			if (relayCommand != null) {
				relayCommand.Execute (parameter == null ? null : parameter.GetValue ());
			}
		}

		void Notify (MVVMBinding binding) {
			if (binding == null) {
				throw new ArgumentNullException ("binding");
			}
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (binding.TargetPropertyName));
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