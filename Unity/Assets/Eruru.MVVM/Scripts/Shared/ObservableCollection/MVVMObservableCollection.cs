//---------------------------------------------------------------------------
//
// <copyright file="ObservableCollection.cs" company="Microsoft">
//    Copyright (C) 2003 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
//
// Description: Implementation of an Collection<T> implementing INotifyCollectionChanged
//              to notify listeners of dynamic changes of the list.
//
// See spec at http://avalon/connecteddata/Specs/Collection%20Interfaces.mht
//
// History:
//  11/22/2004 : Microsoft - created
//
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Eruru.MVVM {
	/// <summary>
	/// Implementation of a dynamic data collection based on generic Collection&lt;T&gt;,
	/// implementing INotifyCollectionChanged to notify listeners
	/// when items get added, removed or the whole list is refreshed.
	/// </summary>
#if !FEATURE_NETCORE
	[Serializable ()]
	//[TypeForwardedFrom ("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
#endif
	public class MVVMObservableCollection<T> : Collection<T>, IMVVMNotifyCollectionChanged, INotifyPropertyChanged {
		//------------------------------------------------------
		//
		//  Constructors
		//
		//------------------------------------------------------

		#region Constructors
		/// <summary>
		/// Initializes a new instance of ObservableCollection that is empty and has default initial capacity.
		/// </summary>
		public MVVMObservableCollection () : base () { }

		/// <summary>
		/// Initializes a new instance of the ObservableCollection class
		/// that contains elements copied from the specified list
		/// </summary>
		/// <param name="list">The list whose elements are copied to the new list.</param>
		/// <remarks>
		/// The elements are copied onto the ObservableCollection in the
		/// same order they are read by the enumerator of the list.
		/// </remarks>
		/// <exception cref="ArgumentNullException"> list is a null reference </exception>
		public MVVMObservableCollection (List<T> list)
			: base ((list != null) ? new List<T> (list.Count) : list) {
			// Workaround for VSWhidbey bug 562681 (tracked by Windows bug 1369339).
			// We should be able to simply call the base(list) ctor.  But Collection<T>
			// doesn't copy the list (contrary to the documentation) - it uses the
			// list directly as its storage.  So we do the copying here.
			// 
			CopyFrom (list);
		}

		/// <summary>
		/// Initializes a new instance of the ObservableCollection class that contains
		/// elements copied from the specified collection and has sufficient capacity
		/// to accommodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		/// <remarks>
		/// The elements are copied onto the ObservableCollection in the
		/// same order they are read by the enumerator of the collection.
		/// </remarks>
		/// <exception cref="ArgumentNullException"> collection is a null reference </exception>
		public MVVMObservableCollection (IEnumerable<T> collection) {
			if (collection == null)
				throw new ArgumentNullException ("collection");

			CopyFrom (collection);
		}

		private void CopyFrom (IEnumerable<T> collection) {
			IList<T> items = Items;
			if (collection != null && items != null) {
				using (IEnumerator<T> enumerator = collection.GetEnumerator ()) {
					while (enumerator.MoveNext ()) {
						items.Add (enumerator.Current);
					}
				}
			}
		}

		#endregion Constructors


		//------------------------------------------------------
		//
		//  Public Methods
		//
		//------------------------------------------------------

		#region Public Methods

		/// <summary>
		/// Move item at oldIndex to newIndex.
		/// </summary>
		public void Move (int oldIndex, int newIndex) {
			MoveItem (oldIndex, newIndex);
		}

		#endregion Public Methods


		//------------------------------------------------------
		//
		//  Public Events
		//
		//------------------------------------------------------

		#region Public Events

		//------------------------------------------------------
		#region INotifyPropertyChanged implementation
		/// <summary>
		/// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
		/// </summary>
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
			add {
				PropertyChanged += value;
			}
			remove {
				PropertyChanged -= value;
			}
		}
		#endregion INotifyPropertyChanged implementation


		//------------------------------------------------------
		/// <summary>
		/// Occurs when the collection changes, either by adding or removing an item.
		/// </summary>
		/// <remarks>
		/// see <seealso cref="IMVVMNotifyCollectionChanged"/>
		/// </remarks>
#if !FEATURE_NETCORE
		[field: NonSerializedAttribute ()]
#endif
		public virtual event MVVMNotifyCollectionChangedEventHandler CollectionChanged;

		#endregion Public Events


		//------------------------------------------------------
		//
		//  Protected Methods
		//
		//------------------------------------------------------

		#region Protected Methods

		/// <summary>
		/// Called by base class Collection&lt;T&gt; when the list is being cleared;
		/// raises a CollectionChanged event to any listeners.
		/// </summary>
		protected override void ClearItems () {
			CheckReentrancy ();
			base.ClearItems ();
			OnPropertyChanged (CountString);
			OnPropertyChanged (IndexerName);
			OnCollectionReset ();
		}

		/// <summary>
		/// Called by base class Collection&lt;T&gt; when an item is removed from list;
		/// raises a CollectionChanged event to any listeners.
		/// </summary>
		protected override void RemoveItem (int index) {
			CheckReentrancy ();
			T removedItem = this[index];

			base.RemoveItem (index);

			OnPropertyChanged (CountString);
			OnPropertyChanged (IndexerName);
			OnCollectionChanged (MVVMNotifyCollectionChangedAction.Remove, removedItem, index);
		}

		/// <summary>
		/// Called by base class Collection&lt;T&gt; when an item is added to list;
		/// raises a CollectionChanged event to any listeners.
		/// </summary>
		protected override void InsertItem (int index, T item) {
			CheckReentrancy ();
			base.InsertItem (index, item);

			OnPropertyChanged (CountString);
			OnPropertyChanged (IndexerName);
			OnCollectionChanged (MVVMNotifyCollectionChangedAction.Add, item, index);
		}

		/// <summary>
		/// Called by base class Collection&lt;T&gt; when an item is set in list;
		/// raises a CollectionChanged event to any listeners.
		/// </summary>
		protected override void SetItem (int index, T item) {
			CheckReentrancy ();
			T originalItem = this[index];
			base.SetItem (index, item);

			OnPropertyChanged (IndexerName);
			OnCollectionChanged (MVVMNotifyCollectionChangedAction.Replace, originalItem, item, index);
		}

		/// <summary>
		/// Called by base class ObservableCollection&lt;T&gt; when an item is to be moved within the list;
		/// raises a CollectionChanged event to any listeners.
		/// </summary>
		protected virtual void MoveItem (int oldIndex, int newIndex) {
			CheckReentrancy ();

			T removedItem = this[oldIndex];

			base.RemoveItem (oldIndex);
			base.InsertItem (newIndex, removedItem);

			OnPropertyChanged (IndexerName);
			OnCollectionChanged (MVVMNotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
		}


		/// <summary>
		/// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
		/// </summary>
		protected virtual void OnPropertyChanged (PropertyChangedEventArgs e) {
			if (PropertyChanged != null) {
				PropertyChanged (this, e);
			}
		}

		/// <summary>
		/// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
		/// </summary>
#if !FEATURE_NETCORE
		[field: NonSerializedAttribute ()]
#endif
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raise CollectionChanged event to any listeners.
		/// Properties/methods modifying this ObservableCollection will raise
		/// a collection changed event through this virtual method.
		/// </summary>
		/// <remarks>
		/// When overriding this method, either call its base implementation
		/// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
		/// </remarks>
		protected virtual void OnCollectionChanged (MVVMNotifyCollectionChangedEventArgs e) {
			if (CollectionChanged != null) {
				using (BlockReentrancy ()) {
					CollectionChanged (this, e);
				}
			}
		}

		/// <summary>
		/// Disallow reentrant attempts to change this collection. E.g. a event handler
		/// of the CollectionChanged event is not allowed to make changes to this collection.
		/// </summary>
		/// <remarks>
		/// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
		/// <code>
		///         using (BlockReentrancy())
		///         {
		///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
		///         }
		/// </code>
		/// </remarks>
		protected IDisposable BlockReentrancy () {
			_monitor.Enter ();
			return _monitor;
		}

		/// <summary> Check and assert for reentrant attempts to change this collection. </summary>
		/// <exception cref="InvalidOperationException"> raised when changing the collection
		/// while another collection change is still being notified to other listeners </exception>
		protected void CheckReentrancy () {
			if (_monitor.Busy) {
				// we can allow changes if there's only one listener - the problem
				// only arises if reentrant changes make the original event args
				// invalid for later listeners.  This keeps existing code working
				// (e.g. Selector.SelectedItems).
				if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList ().Length > 1))
					throw new Exception ("ObservableCollectionReentrancyNotAllowed");
			}
		}

		#endregion Protected Methods


		//------------------------------------------------------
		//
		//  Private Methods
		//
		//------------------------------------------------------

		#region Private Methods
		/// <summary>
		/// Helper to raise a PropertyChanged event  />).
		/// </summary>
		private void OnPropertyChanged (string propertyName) {
			OnPropertyChanged (new PropertyChangedEventArgs (propertyName));
		}

		/// <summary>
		/// Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged (MVVMNotifyCollectionChangedAction action, object item, int index) {
			OnCollectionChanged (new MVVMNotifyCollectionChangedEventArgs (action, item, index));
		}

		/// <summary>
		/// Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged (MVVMNotifyCollectionChangedAction action, object item, int index, int oldIndex) {
			OnCollectionChanged (new MVVMNotifyCollectionChangedEventArgs (action, item, index, oldIndex));
		}

		/// <summary>
		/// Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged (MVVMNotifyCollectionChangedAction action, object oldItem, object newItem, int index) {
			OnCollectionChanged (new MVVMNotifyCollectionChangedEventArgs (action, newItem, oldItem, index));
		}

		/// <summary>
		/// Helper to raise CollectionChanged event with action == Reset to any listeners
		/// </summary>
		private void OnCollectionReset () {
			OnCollectionChanged (new MVVMNotifyCollectionChangedEventArgs (MVVMNotifyCollectionChangedAction.Reset));
		}
		#endregion Private Methods

		//------------------------------------------------------
		//
		//  Private Types
		//
		//------------------------------------------------------

		#region Private Types

		// this class helps prevent reentrant calls
#if !FEATURE_NETCORE
		[Serializable ()]
		//[TypeForwardedFrom ("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
#endif
		private class SimpleMonitor : IDisposable {
			public void Enter () {
				++_busyCount;
			}

			public void Dispose () {
				--_busyCount;
			}

			public bool Busy { get { return _busyCount > 0; } }

			int _busyCount;
		}

		#endregion Private Types

		//------------------------------------------------------
		//
		//  Private Fields
		//
		//------------------------------------------------------

		#region Private Fields

		private const string CountString = "Count";

		// This must agree with Binding.IndexerName.  It is declared separately
		// here so as to avoid a dependency on PresentationFramework.dll.
		private const string IndexerName = "Item[]";

		private readonly SimpleMonitor _monitor = new SimpleMonitor ();

		#endregion Private Fields
	}
}