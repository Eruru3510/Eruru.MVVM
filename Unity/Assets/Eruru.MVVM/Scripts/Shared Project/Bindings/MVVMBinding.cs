using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Eruru.MVVM {

	public class MVVMBinding {

		public MVVMBindingType Type {

			get {
				return _Type;
			}

			private set {
				_Type = value;
			}

		}
		public MVVMBindingMode Mode {

			get {
				return _Mode;
			}

			set {
				_Mode = value;
			}

		}
		public MVVMBindingUpdateSourceTrigger UpdateSourceTrigger {

			get {
				return _UpdateSourceTrigger;
			}

			set {
				_UpdateSourceTrigger = value;
			}

		}
		public string TargetPropertyName { get; internal set; }
		public string PropertyName { get; internal set; }

		internal MVVMControl Control;
		internal MVVMFunc<object> GetTargetValueFunc;
		internal Action<object> SetTargetValueAction;
		internal MVVMBindingMode DefaultMode;
		internal MVVMBindingUpdateSourceTrigger DefaultUpdateSourceTrigger;

		List<INotifyPropertyChanged> NotifyPropertyChangeds {

			get {
				return _NotifyPropertyChangeds ?? (_NotifyPropertyChangeds = new List<INotifyPropertyChanged> ());
			}

		}
		object Instance;
		object Value;

		List<INotifyPropertyChanged> _NotifyPropertyChangeds;
		MVVMBindingMode _Mode;
		MVVMBindingType _Type;
		MVVMBindingUpdateSourceTrigger _UpdateSourceTrigger;
		string Path;
		PropertyInfo PropertyInfo;

		public MVVMBinding () {
			Type = MVVMBindingType.Source;
		}
		public MVVMBinding (object value) {
			Value = value;
			Type = MVVMBindingType.Source;
			Path = string.Empty;
		}
		public MVVMBinding (string path) {
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Path = path;
			Type = MVVMBindingType.Source;
		}
		public MVVMBinding (string path, MVVMBindingMode mode) {
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Path = path;
			Mode = mode;
			Type = MVVMBindingType.Source;
		}
		public MVVMBinding (string path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Path = path;
			UpdateSourceTrigger = updateSourceTrigger;
			Type = MVVMBindingType.Source;
		}
		public MVVMBinding (string path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Path = path;
			Mode = mode;
			UpdateSourceTrigger = updateSourceTrigger;
			Type = MVVMBindingType.Source;
		}
		public MVVMBinding (MVVMControl control, string path) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Instance = control;
			Path = path;
			Type = MVVMBindingType.Element;
		}
		public MVVMBinding (MVVMControl control, string path, MVVMBindingMode mode) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Instance = control;
			Path = path;
			Mode = mode;
			Type = MVVMBindingType.Element;
		}
		public MVVMBinding (MVVMControl control, string path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Instance = control;
			Path = path;
			UpdateSourceTrigger = updateSourceTrigger;
			Type = MVVMBindingType.Element;
		}
		public MVVMBinding (MVVMControl control, string path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Instance = control;
			Path = path;
			Mode = mode;
			UpdateSourceTrigger = updateSourceTrigger;
			Type = MVVMBindingType.Element;
		}

		public object GetValue () {
			switch (Type) {
				case MVVMBindingType.Source:
					if (PropertyInfo == null) {
						return Path == null ? Instance : Value;
					}
					return PropertyInfo.GetValue (Instance, null);
				case MVVMBindingType.Element: {
					object instance = PropertyInfo.GetValue (Instance, null);
					if (instance is MVVMBinding) {
						return ((MVVMBinding)instance).GetTargetValue ();
					}
					return instance;
				}
				default:
					throw new NotImplementedException ();
			}
		}

		public void SetValue (object value) {
			switch (Type) {
				case MVVMBindingType.Source:
					if (PropertyInfo == null) {
						if (Path == null) {
							Instance = value;
							break;
						}
						Value = value;
						break;
					}
					PropertyInfo.SetValue (Instance, Convert.ChangeType (value, PropertyInfo.PropertyType), null);
					break;
				case MVVMBindingType.Element: {
					object instance = PropertyInfo.GetValue (Instance, null);
					if (instance is MVVMBinding) {
						((MVVMBinding)instance).SetTargetValue (value);
						break;
					}
					PropertyInfo.SetValue (instance, Convert.ChangeType (value, PropertyInfo.PropertyType), null);
					break;
				}
				default:
					throw new NotImplementedException ();
			}
		}

		public object GetTargetValue () {
			return GetTargetValueFunc == null ? null : GetTargetValueFunc ();
		}

		public void SetTargetValue (object value) {
			if (SetTargetValueAction != null) {
				SetTargetValueAction (value);
				Control.Changed (this, value);
			}
		}

		public void UpdateSource () {
			Control.Changed (this, GetTargetValue (), MVVMBindingChangedType.UpdateSource);
		}

		internal void Rebinding (object dataContext) {
			UnregisterPropertyChanged ();
			switch (Type) {
				case MVVMBindingType.Source: {
					PropertyInfo = null;
					Instance = dataContext;
					if (dataContext == null || string.IsNullOrEmpty (Path)) {
						break;
					}
					string[] nodes = Path.Split ('.');
					object instance = dataContext;
					Type type = dataContext.GetType ();
					for (int i = 0; i < nodes.Length; i++) {
						PropertyInfo propertyInfo = type.GetProperty (nodes[i]);
						if (propertyInfo == null) {
							throw new Exception (string.Format ("{0}没有{1}属性访问器", Instance, nodes[i]));
						}
						if (instance is INotifyPropertyChanged) {
							INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)instance;
							notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
							NotifyPropertyChangeds.Add (notifyPropertyChanged);
						}
						if (i < nodes.Length - 1) {
							instance = propertyInfo.GetValue (instance, null);
							if (instance == null) {
								break;
							}
							type = instance.GetType ();
							continue;
						}
						Instance = instance;
						PropertyInfo = propertyInfo;
						PropertyName = nodes[i];
					}
					break;
				}
				case MVVMBindingType.Element: {
					PropertyInfo = Instance.GetType ().GetProperty (Path, MVVMApi.BindingFlags);
					if (PropertyInfo == null) {
						throw new Exception (string.Format ("{0}没有{1}属性访问器", Instance, Path));
					}
					if (Instance is INotifyPropertyChanged) {
						INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)Instance;
						notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
						NotifyPropertyChangeds.Add (notifyPropertyChanged);
					}
					break;
				}
				default:
					throw new NotImplementedException ();
			}
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

		internal MVVMBindingUpdateSourceTrigger GetUpdateSourceTrigger () {
			return UpdateSourceTrigger == MVVMBindingUpdateSourceTrigger.Default ? DefaultUpdateSourceTrigger : UpdateSourceTrigger;
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
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (ushort value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (uint value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (ulong value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (sbyte value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (short value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (int value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (long value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (float value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (double value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (decimal value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (bool value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (char value) {
			return new MVVMBinding (value);
		}
		public static implicit operator MVVMBinding (string value) {
			return new MVVMBinding ((object)value);
		}
		public static implicit operator MVVMBinding (DateTime value) {
			return new MVVMBinding (value);
		}

	}

}