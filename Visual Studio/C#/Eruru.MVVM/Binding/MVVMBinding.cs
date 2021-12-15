using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Eruru.MVVM {

	public class MVVMBinding {

		public MVVMControlBase Control { get; internal set; }
		public string TargetPropertyName { get; internal set; }
		public string Path { get; internal set; }
		public string ElementName { get; private set; }
		public string SourcePropertyName { get; private set; }
		public bool IsDataContext { get; internal set; }
		public MVVMControlBase Element { get; private set; }
		public MVVMRelativeSource RelativeSource { get; private set; }
		public MVVMBindingType Type { get; private set; }
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
		public MVVMBindingMode DefaultMode;
		public MVVMUpdateSourceTrigger DefaultUpdateSourceTrigger;

		internal MVVMFunc<object> OnGetTargetValue;
		internal Action<object> OnSetTargetValue;
		internal MVVMAction OnUnbind;
		internal int BlockOnChanged;
		internal bool IsTrigger;

		readonly List<KeyValuePair<INotifyPropertyChanged, string>> NotifyPropertyChangeds = new List<KeyValuePair<INotifyPropertyChanged, string>> ();
		readonly MVVMBindingMode _Mode = MVVMBindingMode.Default;
		readonly MVVMUpdateSourceTrigger _UpdateSourceTrigger = MVVMUpdateSourceTrigger.Default;
		readonly bool ValidatesOnExceptions;

		object TargetValue;
		object SourceValue;
		object DataContext;
		object Instance;
		bool BlockSetSourceValue;
		bool BlockOnPropertyChanged;
		bool IsFirstBind = true;
		int PathNodeCount;
		PropertyInfo PropertyInfo;

		public MVVMBinding (
			MVVMBindingMode mode = MVVMBindingMode.Default,
			MVVMUpdateSourceTrigger updateSourceTrigger = MVVMUpdateSourceTrigger.Default,
			string path = null,
			object value = null,
			MVVMRelativeSource relativeSource = null,
			string elementName = null,
			MVVMControlBase element = null,
			bool validatesOnExceptions = false
		) {
			if (element != null) {
				Type = MVVMBindingType.Element;
			} else if (elementName != null) {
				Type = MVVMBindingType.ElementName;
			} else if (relativeSource != null) {
				Type = MVVMBindingType.RelativeSource;
			} else if (value != null) {
				Type = MVVMBindingType.Value;
			} else {
				Type = MVVMBindingType.Path;
			}
			_Mode = mode;
			_UpdateSourceTrigger = updateSourceTrigger;
			Path = path;
			TargetValue = value;
			ElementName = elementName;
			Element = element;
			ValidatesOnExceptions = validatesOnExceptions;
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
			Path = path;
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
		public MVVMBinding (MVVMRelativeSource relativeSource) {
			RelativeSource = relativeSource;
			Type = MVVMBindingType.RelativeSource;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, MVVMBindingMode mode) : this (relativeSource) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, MVVMUpdateSourceTrigger updateSourceTrigger) : this (relativeSource) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger)
		: this (relativeSource, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, string path) : this (relativeSource) {
			Path = path;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, string path, MVVMBindingMode mode) : this (relativeSource, path) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, string path, MVVMUpdateSourceTrigger updateSourceTrigger) : this (relativeSource, path) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMRelativeSource relativeSource, string path, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger)
		: this (relativeSource, path, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (string elementName, string path) {
			ElementName = elementName;
			Path = path;
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
		public MVVMBinding (MVVMControlBase element) {
			Element = element;
			Type = MVVMBindingType.Element;
		}
		public MVVMBinding (MVVMControlBase element, MVVMBindingMode mode) : this (element) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMControlBase element, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControlBase element, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element, mode) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControlBase element, string path) : this (element) {
			Path = path;
		}
		public MVVMBinding (MVVMControlBase element, string path, MVVMBindingMode mode) : this (element, path) {
			_Mode = mode;
		}
		public MVVMBinding (MVVMControlBase element, string path, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element, path) {
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControlBase element, string path, MVVMBindingMode mode, MVVMUpdateSourceTrigger updateSourceTrigger) : this (element, path, mode) {
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
				BlockOnChanged++;
				OnSetTargetValue (value);
				BlockOnChanged--;
			}
			Control.OnChanged (this);
		}

		public object GetSourceValue () {
			switch (Type) {
				case MVVMBindingType.Value:
					return TargetValue;
				case MVVMBindingType.Path:
				case MVVMBindingType.ElementName:
				case MVVMBindingType.RelativeSource:
					if (SourcePropertyName == null || !PropertyInfo.CanRead) {
						return Path == null ? DataContext : SourceValue;
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
			if (SourcePropertyName == null || !PropertyInfo.CanWrite) {
				return;
			}
			if (ValidatesOnExceptions) {
				Control.Validation.Clear ();
			}
			try {
				if (PropertyInfo.PropertyType == typeof (MVVMBinding)) {
					MVVMBinding binding = PropertyInfo.GetValue (Instance, null) as MVVMBinding;
					if (binding != null) {
						binding.SetTargetValue (value);
					}
					return;
				}
				PropertyInfo.SetValue (Instance, MVVMAPI.ChangeType (value, PropertyInfo.PropertyType), null);
			} catch (Exception exception) {
				if (ValidatesOnExceptions) {
					Control.Validation.Add (new MVVMValidationError (exception.GetBaseException ().Message, exception));
				}
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

		public override string ToString () {
			return string.Format ("{{ Control: {0}, TargetProperty: {1}, Type: {2}, Path: {3} }}", Control, TargetPropertyName, Type, Path);
		}

		internal void Unbind () {
			MVVMAPI.Log ("Unbind {0}", this);
			SourcePropertyName = null;
			foreach (var notifyPropertyChanged in NotifyPropertyChangeds) {
				notifyPropertyChanged.Key.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
			}
			NotifyPropertyChangeds.Clear ();
			if (OnUnbind != null) {
				OnUnbind ();
			}
			IsFirstBind = true;
		}

		internal void Bind (bool isForce = false) {
			MVVMAPI.Log ("Bind {0}", this);
			if (!IsFirstBind && !isForce) {
				switch (Type) {
					case MVVMBindingType.Value:
					case MVVMBindingType.Element:
					case MVVMBindingType.ElementName:
					case MVVMBindingType.RelativeSource:
						MVVMAPI.Log ("Denied Bind");
						return;
				}
			}
			MVVMAPI.Log ("Allow Bind");
			Unbind ();
			IsFirstBind = false;
			switch (Type) {
				case MVVMBindingType.Value:
					break;
				case MVVMBindingType.Path: {
					object dataContext = null;
					if (IsDataContext) {
						if (Control.Parent != null) {
							dataContext = Control.Parent.DataContext.GetTargetValue ();
						}
					} else {
						dataContext = Control.DataContext.GetTargetValue ();
					}
					BindSource (dataContext);
					break;
				}
				case MVVMBindingType.Element:
					BindSource (Element);
					break;
				case MVVMBindingType.ElementName: {
					MVVMControlBase control;
					Control.Root.NamedControls.TryGetValue (ElementName, out control);
					BindSource (control);
					break;
				}
				case MVVMBindingType.RelativeSource: {
					MVVMControlBase control = Control;
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
			BlockOnChanged++;
			OneWaySetTargetValue ();
			BlockOnChanged--;
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
			DataContext = dataContext;
			if (DataContext != null && !string.IsNullOrEmpty (Path)) {
				object instance = DataContext;
				Type type = instance.GetType ();
				string[] pathNodes = Path.Split ('.');
				PathNodeCount = pathNodes.Length;
				for (int i = 0; i < pathNodes.Length; i++) {
					PropertyInfo propertyInfo = type.GetProperty (pathNodes[i]);
					if (propertyInfo == null) {
						MVVMAPI.Error ("Bind Failed, Path: {0}, Reason: {1} has no can read and write public property: {2}, From: {3}", string.Join (".", pathNodes, 0, i + 1), instance, pathNodes[i], this);
						break;
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
							SourcePropertyName = pathNodes[i];
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
			MVVMAPI.Log ("PropertyChanged Sender: {0}, Property: {1}, Receiver: {2}", sender, e.PropertyName, this);
			if (BlockOnPropertyChanged) {
				MVVMAPI.Log ("Block PropertyChanged");
				return;
			}
			switch (GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWay:
					MVVMAPI.Log ("Mode Allow");
					for (int i = 0; i < NotifyPropertyChangeds.Count; i++) {
						if (NotifyPropertyChangeds[i].Key == sender && e.PropertyName == NotifyPropertyChangeds[i].Value) {
							if (i == PathNodeCount - 1) {
								MVVMAPI.Log ("Only Change Value");
								OneWaySetTargetValue ();
								return;
							}
							MVVMAPI.Log ("Rebind");
							Bind (true);
							return;
						}
					}
					MVVMAPI.Log ("No Operator");
					return;
			}
			MVVMAPI.Log ("Mode Denied");
		}

	}

}