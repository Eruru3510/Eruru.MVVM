using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Generic;

namespace Eruru.MVVM {

	public class MVVMElement : INotifyPropertyChanged, IEnumerable<MVVMElement> {

		public event PropertyChangedEventHandler PropertyChanged;
		public MVVMBinding DataContext {

			get {
				return GetBinding (ref _DataContext, value => DataContext = value, true);
			}

			set {
				SetBinding (
					ref _DataContext, value,
					null, targetValue => {
						if (OnDataContextChanged != null) {
							OnDataContextChanged (targetValue);
						}
						for (int i = 0; i < Elements.Count; i++) {
							Elements[i].DataContext.SetTargetValue (targetValue);
						}
					},
					null, null,
					null, null,
					true
				);
			}

		}
		public MVVMElement Parent {

			get {
				return _Parent;
			}

			internal set {
				_Parent = value;
				if (OnParentChanged != null) {
					OnParentChanged ();
				}
			}

		}

		internal protected event Action<object> OnDataContextChanged;
		internal protected event MVVMAction OnParentChanged;
		internal protected List<MVVMElement> Elements = new List<MVVMElement> ();

		MVVMBinding _DataContext;
		MVVMElement _Parent;

		protected MVVMBinding GetBinding (ref MVVMBinding binding, Action<MVVMBinding> setBinding, bool isDataContext = false) {
			if (binding == null) {
				binding = new MVVMBinding (isDataContext ? null : string.Empty);
				setBinding (binding);
			}
			return binding;
		}

		protected void SetBinding (
			ref MVVMBinding binding, MVVMBinding value,
			MVVMFunc<object> getTargetValue = null, Action<object> setTargetValue = null,
			MVVMAction registerChanged = null, MVVMAction unregisterChanged = null,
			MVVMAction registerLostFocus = null, MVVMAction unregisterLostFocus = null,
			bool isDataContext = false, [CallerMemberName] string propertyName = null
		) {
			if (binding != null) {
				OnDataContextChanged -= binding.Rebinding;
				binding.Debinding ();
			}
			if (unregisterChanged != null) {
				unregisterChanged ();
			}
			if (unregisterLostFocus != null) {
				unregisterLostFocus ();
			}
			binding = value;
			if (binding == null) {
				return;
			}
			if (propertyName == null) {
				propertyName = MVVMAPI.GetCallerMemberName ();
			}
			binding.Control = this as MVVMControl;
			binding.TargetPropertyName = propertyName;
			binding.OnGetTargetValue = getTargetValue;
			binding.OnSetTargetValue = setTargetValue;
			binding.OnRebinding = () => value.Rebinding (DataContext.GetTargetValue ());
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
			binding.OnRebinding ();
			OnDataContextChanged += binding.Rebinding;
		}

		internal protected void OnChanged (MVVMBinding binding, object value, MVVMBindingOnChangedType changeType = MVVMBindingOnChangedType.PropertyChanged) {
			if (binding.BlockOnChanged || binding.Value == value) {
				return;
			}
			switch (binding.GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWayToSource:
					switch (binding.GetUpdateSourceTrigger ()) {
						case MVVMBindingUpdateSourceTrigger.PropertyChanged:
							if (changeType == MVVMBindingOnChangedType.PropertyChanged) {
								binding.SetValue (value);
							}
							break;
						case MVVMBindingUpdateSourceTrigger.LostFocus:
							if (changeType == MVVMBindingOnChangedType.LostFocus) {
								binding.SetValue (value);
							}
							break;
					}
					break;
			}
			this.RaisePropertyChanged (binding.TargetPropertyName);
		}

		protected void OnCommand (MVVMBinding command, MVVMBinding parameter) {
			MVVMRelayCommand relayCommand = command.GetTargetValue () as MVVMRelayCommand;
			if (relayCommand == null) {
				return;
			}
			object value = parameter == null ? null : parameter.GetTargetValue ();
			if (relayCommand.CanExecute (value)) {
				relayCommand.Execute (value);
			}
		}

		public IEnumerator<MVVMElement> GetEnumerator () {
			return Elements.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return Elements.GetEnumerator ();
		}

	}

}