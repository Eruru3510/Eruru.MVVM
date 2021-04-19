using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eruru.MVVM {

	public abstract class MVVMBinding {

		public MVVMBindingMode Mode {

			get {
				return _Mode;
			}

			set {
				_Mode = value;
			}

		}
		public string TargetPropertyName { get; internal set; }
		public string PropertyName { get; internal set; }

		internal MVVMControl Control;
		internal MVVMFunc<object> GetTargetValueFunc;
		internal Action<object> SetTargetValueAction;
		internal MVVMBindingMode DefaultMode;

		protected List<INotifyPropertyChanged> NotifyPropertyChangeds {

			get {
				return _NotifyPropertyChangeds ?? (_NotifyPropertyChangeds = new List<INotifyPropertyChanged> ());
			}

		}
		protected object Value;

		List<INotifyPropertyChanged> _NotifyPropertyChangeds;
		MVVMBindingMode _Mode = MVVMBindingMode.Default;

		public MVVMBinding () {

		}
		public MVVMBinding (MVVMBindingMode mode) {
			Mode = mode;
		}

		public abstract object GetValue ();

		public abstract void SetValue (object value);

		public object GetTargetValue () {
			return GetTargetValueFunc == null ? null : GetTargetValueFunc ();
		}

		public void SetTargetValue (object value) {
			if (SetTargetValueAction != null) {
				SetTargetValueAction (value);
			}
		}

		internal void Rebinding (object dataContext) {
			_Rebinding (dataContext);
			SetTargetValue (GetValue ());
		}

		internal void UnregisterPropertyChanged () {
			foreach (INotifyPropertyChanged notifyPropertyChanged in NotifyPropertyChangeds) {
				notifyPropertyChanged.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
			}
			NotifyPropertyChangeds.Clear ();
		}

		internal MVVMBindingMode GetMode () {
			return Mode == MVVMBindingMode.Default ? DefaultMode : Mode;
		}

		protected virtual void _Rebinding (object dataContext) {

		}

		protected void NotifyPropertyChanged_PropertyChanged (object sender, PropertyChangedEventArgs e) {
			switch (GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWay:
					if (e.PropertyName != PropertyName) {
						Rebinding (Control.ParentDataContext);
					}
					SetTargetValue (GetValue ());
					break;
			}
		}

		public static implicit operator MVVMBinding (byte value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (ushort value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (uint value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (ulong value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (sbyte value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (short value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (int value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (long value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (float value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (double value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (decimal value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (bool value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (char value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (string value) {
			return new MVVMBindingValue (value);
		}
		public static implicit operator MVVMBinding (DateTime value) {
			return new MVVMBindingValue (value);
		}

	}

}