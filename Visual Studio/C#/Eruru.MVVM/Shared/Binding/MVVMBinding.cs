using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Eruru.MVVM {

	public class MVVMBinding {

		public MVVMControl Control {

			get {
				return _Control;
			}

		}
		public string TargetPropertyName {

			get {
				return _TargetPropertyName;
			}

		}
		public string Path {

			get {
				return _Path;
			}

		}
		public string ElementName {

			get {
				return _ElementName;
			}

		}
		public string SourcePropertyName {

			get {
				return _SourcePropertyName;
			}

		}
		public bool IsDataContext {

			get {
				return _IsDataContext;
			}

		}
		public MVVMControl Element {

			get {
				return _Element;
			}

		}
		public MVVMBindingRelativeSource RelativeSource {

			get {
				return _RelativeSource;
			}

		}
		public MVVMBindingType Type { get; }
		public MVVMBindingMode Mode {

			get {
				return _Mode;
			}

		}
		public MVVMUpdateSourceTrigger UpdateSourceTrigger {

			get {
				return _UpdateSourceTrigger;
			}

		}

		internal MVVMControl _Control;
		internal string _TargetPropertyName;
		internal MVVMFunc<object> OnGetTargetValue;
		internal Action<object> OnSetTargetValue;
		internal MVVMAction OnUnbind;
		internal bool BlockOnChanged;
		internal bool _IsDataContext;
		internal MVVMBindingMode DefaultMode;
		internal MVVMUpdateSourceTrigger DefaultUpdateSourceTrigger;

		readonly string _Path;
		readonly string _ElementName;
		readonly MVVMControl _Element;
		readonly MVVMBindingRelativeSource _RelativeSource;
		readonly List<KeyValuePair<INotifyPropertyChanged, string>> NotifyPropertyChangeds = new List<KeyValuePair<INotifyPropertyChanged, string>> ();
		readonly MVVMBindingMode _Mode = MVVMBindingMode.Default;
		readonly MVVMUpdateSourceTrigger _UpdateSourceTrigger = MVVMUpdateSourceTrigger.Default;

		object TargetValue;
		object SourceValue;
		object Instance;
		string _SourcePropertyName;
		bool BlockSetSourceValue;
		bool BlockOnPropertyChanged;
		bool IsFirstBind = true;
		PropertyInfo PropertyInfo;

		public MVVMBinding () {
			Type = MVVMBindingType.Path;
		}
		public MVVMBinding (MVVMBindingMode mode) : this () {
			_Mode = mode;
		}
		public MVVMBinding (MVVMUpdateSourceTrigger updateSourceTrigger) : this () {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (string path) : this () {
			_Path = path;
		}
		public MVVMBinding (string path, MVVMBindingMode mode) : this (path) {
			_Mode = mode;
		}
		public MVVMBinding (string path, MVVMUpdateSourceTrigger updateSourceTrigger) : this (path) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (string path, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (path, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (object value) {
			TargetValue = value;
			Type = MVVMBindingType.Value;
		}
		public MVVMBinding (object value, MVVMBindingMode mode) : this (value) {
			_Mode = mode;
		}
		public MVVMBinding (object value, MVVMUpdateSourceTrigger updateSourceTrigger) : this (value) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (object value, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (value, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource) {
			_RelativeSource = relativeSource;
			Type = MVVMBindingType.Relative;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingMode mode) : this (relativeSource) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMUpdateSourceTrigger updateSourceTrigger) : this (relativeSource) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger)
		: this (relativeSource, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path) : this (relativeSource) {
			_Path = path;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path, MVVMBindingMode mode) : this (relativeSource, path) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path, MVVMUpdateSourceTrigger updateSourceTrigger) : this (relativeSource, path) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger)
		: this (relativeSource, path, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (string elementName, string path) {
			_ElementName = elementName;
			_Path = path;
			Type = MVVMBindingType.ElementName;
		}
		public MVVMBinding (string elementName, string path, MVVMBindingMode mode) : this (elementName, path) {
			_Mode = mode;
		}
		public MVVMBinding (string elementName, string path, MVVMUpdateSourceTrigger updateSourceTrigger) : this (elementName, path) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (string elementName, string path, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (elementName, path, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControl element) {
			_Element = element;
			Type = MVVMBindingType.Element;
		}
		public MVVMBinding (MVVMControl element, MVVMBindingMode mode) : this (element) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMControl element, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControl element, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControl element, string path) : this (element) {
			_Path = path;
		}
		public MVVMBinding (MVVMControl element, string path, MVVMBindingMode mode) : this (element, path) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMControl element, string path, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element, path) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControl element, string path, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element, path, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
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
		public static implicit operator MVVMBinding (MVVMDataTemplate value) {
			return new MVVMBinding (value);
		}

		public object GetTargetValue () {
			if (OnGetTargetValue == null) {
				return TargetValue;
			}
			return OnGetTargetValue ();
		}
		public T GetTargetValue<T> () {
			return MVVMAPI.To<T> (GetTargetValue ());
		}

		public void SetTargetValue (object value) {
			TargetValue = value;
			if (OnSetTargetValue != null) {
				BlockOnChanged = true;
				try {
					OnSetTargetValue (value);
				} catch {

				}
				BlockOnChanged = false;
			}
			Control.OnChanged (this);
		}

		public object GetSourceValue () {
			switch (Type) {
				case MVVMBindingType.Value:
					return TargetValue;
				case MVVMBindingType.Path:
				case MVVMBindingType.ElementName:
				case MVVMBindingType.Relative:
					if (SourcePropertyName == null) {
						return SourceValue;
					}
					object value = PropertyInfo.GetValue (Instance, null);
					if (value is MVVMBinding) {
						return ((MVVMBinding)value).GetTargetValue ();
					}
					return value;
				default:
					throw new NotImplementedException (Type.ToString ());
			}
		}
		public T GetSourceValue<T> () {
			return MVVMAPI.To<T> (GetSourceValue ());
		}

		public void SetSourceValue (object value) {
			if (BlockSetSourceValue) {
				return;
			}
			SourceValue = value;
			if (SourcePropertyName != null) {
				if (PropertyInfo.PropertyType == typeof (MVVMBinding)) {
					(PropertyInfo.GetValue (Instance, null) as MVVMBinding)?.SetTargetValue (value);
					return;
				}
				PropertyInfo.SetValue (Instance, MVVMAPI.ChangeType (value, PropertyInfo.PropertyType), null);
			}
		}

		public void UpdateSource () {
			switch (GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWayToSource:
					break;
				default:
					return;
			}
			SetSourceValue (GetTargetValue ());
		}

		public void UpdateTarget () {
			SetTargetValue (GetSourceValue ());
		}

		internal void Unbind () {
			_SourcePropertyName = null;
			foreach (var notifyPropertyChanged in NotifyPropertyChangeds) {
				notifyPropertyChanged.Key.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
			}
			NotifyPropertyChangeds.Clear ();
			OnUnbind?.Invoke ();
			IsFirstBind = true;
		}

		internal void Bind () {
			if (!IsFirstBind) {
				switch (Type) {
					case MVVMBindingType.Value:
					case MVVMBindingType.Element:
					case MVVMBindingType.ElementName:
					case MVVMBindingType.Relative:
						return;
				}
			}
			IsFirstBind = false;
			Unbind ();
			switch (Type) {
				case MVVMBindingType.Value:
					break;
				case MVVMBindingType.Path:
					BindSource (IsDataContext ? Control.Parent?.DataContext.GetTargetValue () : Control.DataContext.GetTargetValue ());
					break;
				case MVVMBindingType.Element:
					BindSource (Element);
					break;
				case MVVMBindingType.ElementName: {
					Control.Root.ControlNames.TryGetValue (ElementName, out MVVMControl control);
					BindSource (control);
					break;
				}
				case MVVMBindingType.Relative: {
					MVVMControl control = Control;
					switch (RelativeSource.Mode) {
						case MVVMRelativeSourceMode.Self:
							break;
						case MVVMRelativeSourceMode.FindAncestor:
							int ancestorCount = 0;
							while (control != null) {
								if (RelativeSource.AncestorType == null || RelativeSource.AncestorType.IsInstanceOfType (control)) {
									ancestorCount++;
									if (ancestorCount == RelativeSource.AncestorLevel) {
										break;
									}
								}
								control = control.Parent;
							}
							break;
						default:
							throw new NotImplementedException (RelativeSource.Mode.ToString ());
					}
					BindSource (control);
					break;
				}
				default:
					throw new NotImplementedException (Type.ToString ());
			}
			OneWaySetTargetValue ();
		}

		internal MVVMBindingMode GetMode () {
			return Mode == MVVMBindingMode.Default ? DefaultMode : Mode;
		}

		internal MVVMUpdateSourceTrigger GetUpdateSourceTrigger () {
			return UpdateSourceTrigger == MVVMUpdateSourceTrigger.Default ? DefaultUpdateSourceTrigger : UpdateSourceTrigger;
		}

		internal void OneWaySetSourceValue () {
			BlockOnPropertyChanged = true;
			SetSourceValue (GetTargetValue ());
			BlockOnPropertyChanged = false;
		}

		void OneWaySetTargetValue () {
			BlockSetSourceValue = true;
			SetTargetValue (GetSourceValue ());
			BlockSetSourceValue = false;
		}

		void BindSource (object dataContext) {
			SourceValue = dataContext;
			if (SourceValue != null && Path != null) {
				object instance = SourceValue;
				Type type = instance.GetType ();
				string[] pathNodes = Path.Split ('.');
				for (int i = 0; i < pathNodes.Length; i++) {
					PropertyInfo propertyInfo = type.GetProperty (pathNodes[i]);
					if (propertyInfo == null) {
						Console.Error.WriteLine (string.Format ("绑定{0}失败，{1}下没有公开的可读写属性{2}", string.Join (".", pathNodes, 0, i + 1), instance, pathNodes[i]));
					}
					if (instance is INotifyPropertyChanged) {
						INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)instance;
						notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
						NotifyPropertyChangeds.Add (new KeyValuePair<INotifyPropertyChanged, string> (notifyPropertyChanged, pathNodes[i]));
					}
					if (i == pathNodes.Length - 1) {
						Instance = instance;
						PropertyInfo = propertyInfo;
						if (propertyInfo != null) {
							_SourcePropertyName = pathNodes[i];
						}
						break;
					}
					instance = propertyInfo.GetValue (instance, null);
					if (instance is MVVMBinding) {
						instance = ((MVVMBinding)instance).GetTargetValue ();
					}
					if (instance == null) {
						break;
					}
					type = instance.GetType ();
				}
			}
		}

		void NotifyPropertyChanged_PropertyChanged (object sender, PropertyChangedEventArgs e) {
			if (BlockOnPropertyChanged) {
				return;
			}
			switch (GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWay:
					for (int i = 0; i < NotifyPropertyChangeds.Count; i++) {
						if (NotifyPropertyChangeds[i].Key == sender && e.PropertyName == NotifyPropertyChangeds[i].Value) {
							if (i == NotifyPropertyChangeds.Count - 1) {
								OneWaySetTargetValue ();
								break;
							}
							Bind ();
							break;
						}
					}
					break;
			}
		}

	}

}