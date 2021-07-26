using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace Eruru.MVVM {

	public class MVVMBinding {

		public MVVMControl Element {

			get {
				return _Element;
			}

			set {
				_Element = value;
			}

		}
		public string Path {

			get {
				return _Path;
			}

			set {
				_Path = value;
			}

		}
		public string TargetPropertyName { get; internal set; }
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
		public MVVMBindingRelativeSource RelativeSource {

			get {
				return _RelativeSource;
			}

			set {
				_RelativeSource = value;
			}

		}

		internal MVVMControl Control;
		internal MVVMFunc<object> OnGetTargetValue;
		internal Action<object> OnSetTargetValue;
		internal MVVMAction OnRebinding;
		internal MVVMBindingMode DefaultMode;
		internal MVVMBindingUpdateSourceTrigger DefaultUpdateSourceTrigger;
		internal PropertyInfo PropertyInfo;
		internal bool BlockOnChanged;
		internal object Value;

		List<INotifyPropertyChanged> NotifyPropertyChangeds = new List<INotifyPropertyChanged> ();
		MVVMControl _Element;
		string _Path;
		MVVMBindingRelativeSource _RelativeSource;
		MVVMBindingMode _Mode = MVVMBindingMode.Default;
		MVVMBindingUpdateSourceTrigger _UpdateSourceTrigger = MVVMBindingUpdateSourceTrigger.Default;
		string PropertyName;
		object Instance;
		bool BlockSetValue;

		public MVVMBinding () {

		}
		public MVVMBinding (object value) {
			Value = value;
			_Path = string.Empty;
		}
		public MVVMBinding (string path) {
			_Path = path;
		}
		public MVVMBinding (string path, MVVMBindingMode mode) {
			_Path = path;
			_Mode = mode;
		}
		public MVVMBinding (string path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_Path = path;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (string path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_Path = path;
			_Mode = mode;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource) {
			_RelativeSource = relativeSource;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingMode mode) {
			_RelativeSource = relativeSource;
			_Mode = mode;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_RelativeSource = relativeSource;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_RelativeSource = relativeSource;
			_Mode = mode;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path) {
			_RelativeSource = relativeSource;
			_Path = path;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path, MVVMBindingMode mode) {
			_RelativeSource = relativeSource;
			_Path = path;
			_Mode = mode;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_RelativeSource = relativeSource;
			_Path = path;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMBindingRelativeSource relativeSource, string path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_RelativeSource = relativeSource;
			_Path = path;
			_Mode = mode;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControl control, string path) {
			_Element = control;
			_Path = path;
		}
		public MVVMBinding (MVVMControl control, string path, MVVMBindingMode mode) {
			_Element = control;
			_Path = path;
			_Mode = mode;
		}
		public MVVMBinding (MVVMControl control, string path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_Element = control;
			_Path = path;
			_UpdateSourceTrigger = updateSourceTrigger;
		}
		public MVVMBinding (MVVMControl control, string path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
			_Element = control;
			_Path = path;
			_Mode = mode;
			_UpdateSourceTrigger = updateSourceTrigger;
		}

		public object GetTargetValue () {
			if (OnGetTargetValue == null) {
				return Value;
			}
			return OnGetTargetValue ();
		}
		public T GetTargetValue<T> (T defaultValue = default (T)) {
			object value = GetTargetValue ();
			try {
				return (T)value;
			} catch {
				return defaultValue;
			}
		}

		public void SetTargetValue (object value) {
			Value = value;
			if (OnSetTargetValue != null) {
				BlockOnChanged = true;
				OnSetTargetValue (value);
				BlockOnChanged = false;
			}
			if (Control != null) {
				Control.OnChanged (this, value);
			}
		}

		public object GetValue () {
			if (PropertyInfo == null) {
				return Path == null ? Instance : Value;
			}
			object value = PropertyInfo.GetValue (Instance, null);
			if (value is MVVMBinding) {
				return ((MVVMBinding)value).GetTargetValue ();
			}
			return value;
		}
		public T GetValue<T> (T defaultValue = default (T)) {
			object value = GetValue ();
			try {
				return (T)value;
			} catch {
				return defaultValue;
			}
		}

		public void SetValue (object value) {
			if (BlockSetValue) {
				return;
			}
			if (PropertyInfo == null) {
				if (Path == null) {
					Instance = value;
					return;
				}
				Value = value;
				return;
			}
			MVVMAPI.SetPropertyValue (PropertyInfo, Instance, value);
		}

		public void UpdateSource () {
			switch (GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWayToSource:
					break;
				default:
					return;
			}
			SetValue (GetTargetValue ());
		}

		public void UpdateTarget () {
			SetTargetValue (GetValue ());
		}

		internal MVVMBindingMode GetMode () {
			return Mode == MVVMBindingMode.Default ? DefaultMode : Mode;
		}

		internal MVVMBindingUpdateSourceTrigger GetUpdateSourceTrigger () {
			return UpdateSourceTrigger == MVVMBindingUpdateSourceTrigger.Default ? DefaultUpdateSourceTrigger : UpdateSourceTrigger;
		}

		internal void Rebinding (object dataContext) {
			Debinding ();
			Instance = dataContext;
			object instance;
			if (RelativeSource == null) {
				instance = Element == null ? Instance : Element;
			} else {
				instance = Control;
				switch (RelativeSource.Mode) {
					case MVVMBindingRelativeSourceMode.Self:
						break;
					case MVVMBindingRelativeSourceMode.FindAncestor:
						int ancestorCount = 0;
						while (instance != null) {
							if (RelativeSource.AncestorType == null || RelativeSource.AncestorType.IsInstanceOfType (instance)) {
								ancestorCount++;
								if (ancestorCount == RelativeSource.AncestorLevel) {
									Instance = instance;
									break;
								}
							}
							if (instance is MVVMControl) {
								MVVMControl control = (MVVMControl)instance;
								if (control.Parent == null) {
									return;
								}
								instance = control.Parent;
								continue;
							}
							throw new Exception ("遇到了非MVVMControl元素");
						}
						break;
					default:
						throw new NotImplementedException ();
				}
			}
			if (instance != null && !string.IsNullOrEmpty (Path)) {
				Type type = instance.GetType ();
				string[] nodes = Path.Split (new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < nodes.Length; i++) {
					if (instance is MVVMBinding) {
						MVVMBinding binding = (MVVMBinding)instance;
						instance = binding.GetTargetValue ();
						if (instance == null) {
							break;
						}
						type = instance.GetType ();
					}
					PropertyInfo propertyInfo = type.GetProperty (nodes[i], BindingFlags.Instance | BindingFlags.Public);
					if (propertyInfo == null) {
						throw new Exception (string.Format ("{0}的属性{1}绑定路径{2}失败，{3}下没有公开的可读写属性{4}", Control.Name, TargetPropertyName, Path, instance, nodes[i]));
					}
					if (instance is INotifyPropertyChanged) {
						INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)instance;
						notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
						NotifyPropertyChangeds.Add (notifyPropertyChanged);
					}
					if (i == nodes.Length - 1) {
						PropertyName = nodes[i];
						PropertyInfo = propertyInfo;
						Instance = instance;
						break;
					}
					instance = propertyInfo.GetValue (instance, null);
					if (instance == null) {
						break;
					}
					type = instance.GetType ();
				}
			}
			BlockSetValue = true;
			SetTargetValue (GetValue ());
			BlockSetValue = false;
		}

		internal void Debinding () {
			PropertyInfo = null;
			for (int i = 0; i < NotifyPropertyChangeds.Count; i++) {
				NotifyPropertyChangeds[i].PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
			}
			NotifyPropertyChangeds.Clear ();
		}

		private void NotifyPropertyChanged_PropertyChanged (object sender, PropertyChangedEventArgs e) {
			switch (GetMode ()) {
				case MVVMBindingMode.TwoWay:
				case MVVMBindingMode.OneWay:
					Type type = sender.GetType ();
					for (int i = 0; i < NotifyPropertyChangeds.Count; i++) {
						if (NotifyPropertyChangeds[i].GetType ().IsAssignableFrom (type)) {
							if (i == NotifyPropertyChangeds.Count - 1) {
								if (e.PropertyName == PropertyName) {
									BlockSetValue = true;
									SetTargetValue (GetValue ());
									BlockSetValue = false;
								}
								break;
							}
							OnRebinding ();
							break;
						}
					}
					break;
			}
		}

	}

}