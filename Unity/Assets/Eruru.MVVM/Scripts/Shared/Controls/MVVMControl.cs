using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public partial class MVVMControl : INotifyPropertyChanged, IEnumerable<MVVMControl> {

		public event PropertyChangedEventHandler PropertyChanged;
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
				SetBinding (ref _DataContext, value, isDataContext: true, setTargetValue: targetValue => {
					foreach (MVVMBinding binding in Bindings) {
						binding.Bind ();
					}
					foreach (MVVMTrigger trigger in Triggers) {
						trigger.Bind ();
					}
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
				Controls[index].Unbind ();
				Controls[index] = value;
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

		internal Dictionary<string, MVVMControl> ControlNames = new Dictionary<string, MVVMControl> ();
		internal MVVMControl _Parent;

		readonly List<MVVMBinding> Bindings = new List<MVVMBinding> ();
		readonly List<MVVMControl> _Controls = new List<MVVMControl> ();
		readonly List<MVVMTrigger> Triggers = new List<MVVMTrigger> ();

		MVVMBinding _Name;
		MVVMBinding _DataContext;
		MVVMControl _Root;
		MVVMValidation _Validation = new MVVMValidation ();

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

		public MVVMControl AddTrigger (MVVMTrigger trigger) {
			trigger.Control = this;
			Triggers.Add (trigger);
			return this;
		}
		public MVVMControl AddTrigger (params MVVMTrigger[] triggers) {
			foreach (MVVMTrigger trigger in triggers) {
				AddTrigger (trigger);
			}
			return this;
		}

		public void RemoveAt (int index) {
			Controls[index].Unbind ();
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
				control.Unbind ();
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

		public void Unbind () {
			ForEach (control => {
				control.DataContext.Unbind ();
				foreach (MVVMTrigger trigger in control.Triggers) {
					trigger.Unbind ();
				}
				foreach (MVVMBinding binding in control.Bindings) {
					binding.Unbind ();
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
				setBinding (new MVVMBinding (string.Empty));
			}
			return binding;
		}

		protected void SetBinding (
			ref MVVMBinding binding, MVVMBinding value,
			MVVMFunc<object> getTargetValue = null, Action<object> setTargetValue = null,
			MVVMAction unbind = null,
			MVVMBindingMode defaultMode = MVVMBindingMode.OneWay,
			MVVMUpdateSourceTrigger defaultUpdateSourceTrigger = MVVMUpdateSourceTrigger.PropertyChanged,
			bool isDataContext = false, [CallerMemberName] string propertyName = null
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
			binding.DefaultMode = defaultMode;
			binding.DefaultUpdateSourceTrigger = defaultUpdateSourceTrigger;
			if (!isDataContext) {
				Bindings.Add (binding);
			}
		}

		internal protected void OnChanged (MVVMBinding binding, MVVMOnChangedType onChangedType = MVVMOnChangedType.PropertyChanged) {
			if (binding == null || binding.BlockOnChanged || binding.IsTrigger) {
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
			MVVMRelayCommand relayCommand = command.GetTargetValue<MVVMRelayCommand> ();
			if (relayCommand == null) {
				return;
			}
			object value = parameter == null ? null : parameter.GetTargetValue ();
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