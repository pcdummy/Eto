using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Eto.Forms
{
	/// <summary>
	/// Manages the selection of a grid view, and maintains it
	/// under four types of operations:
	/// 
	/// 1) The user changes the selection in the control. In this case the selection indexes are recalculated.
	/// 2) The application programmatically changes the selection using SelectRow(), SelectAll(), etc.
	///    In this case the internal selection is updated and SelectionChanged is fired.
	/// 3) A sort or filter is applied to the GridView. 
	/// 4) The DataStore changes, i.e. items are added, removed or modified.
	/// <copyright>(c) 2013 by Vivek Jhaveri</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	/// </summary>
	public class GridViewSelection
	{
		enum GridViewSelectionState
		{
			Normal,
			/// <summary>
			/// The selection is being changed programmatically 
			/// by GridViewSelection, not by a user.
			/// </summary>
			SelectionChanging,
			/// <summary>
			/// The selection was changed programmatically 
			/// by GridViewSelection, not by a user.
			/// </summary>
			SelectionChanged
		}

		GridViewSelectionState state;
		bool areAllObjectsSelected;
		readonly GridView gridView;

		IDataStore DataStore { get { return gridView == null ? null : gridView.DataStore; } }

		IDataStoreView DataStoreView { get { return gridView == null ? null : gridView.DataStoreView; } }

		IGridView Handler { get { return gridView == null ? null : gridView.Handler; } }

		bool AllowMultipleSelection { get { return gridView != null && gridView.AllowMultipleSelection; } }

		/// <summary>
		/// Called when the underlying control's selection changes.
		/// Returns true if the change notification should be suppressed. This is
		/// the case when this GridViewSelection is making changes programmatically
		/// and needs to suppress the selection change notifications.
		/// </summary>
		public bool SuppressSelectionChanged
		{
			get
			{
				// In the Normal state, this is called when the user
				// changed the selection. We refer back to the control
				// to determine the new set of rows.
				if (state == GridViewSelectionState.Normal)
				{
					if (!areAllObjectsSelected) // optimization to avoid iterating if all objects are selected
					{
						selectedRows.Clear();
						var s = Handler.SelectedRows;
						if (s != null)
							foreach (var i in s)
								selectedRows.Add(DataStoreView.ViewToModel(i));
					}
				}

				// Don't suppress notifications when the state is Normal or SelectionChanged.
				if (state == GridViewSelectionState.SelectionChanged)
				{
					state = GridViewSelectionState.Normal;
					return false;
				}
				return state == GridViewSelectionState.SelectionChanging;
			}
		}

		readonly SortedSet<int> selectedRows;

		/// <summary>
		/// The indexes of the selected objects in the model
		/// </summary>
		public IEnumerable<int> SelectedRows
		{
			get
			{
				// If all objects are selected, return the range [0, count-1]
				if (areAllObjectsSelected && DataStore != null)
					return Enumerable.Range(0, DataStore.Count);
				return selectedRows ?? Enumerable.Empty<int>();
			}
		}

		void ChangeSelection(Action a)
		{
			state = GridViewSelectionState.SelectionChanging;
			a(); // Causes GridView.OnSelectionChanged to trigger which calls SuppressSelectionChanged which returns true.
			state = GridViewSelectionState.SelectionChanged;
			gridView.OnSelectionChanged(EventArgs.Empty); // Calls SuppressSelectionChanged which returns false.
			state = GridViewSelectionState.Normal; // This should already be done in SuppressSelectionChanged but repeated for robustness
		}

		/// <summary>
		/// Programmatically selects a row.
		/// </summary>
		/// <param name="row">Index of the row to select in the model</param>
		public void SelectRow(int row)
		{
			ChangeSelection(() => {
				if (!AllowMultipleSelection)
					selectedRows.Clear();

				selectedRows.Add(row);
				var viewIndex = DataStoreView.ModelToView(row);
				if (viewIndex != null)
				{
					if (!AllowMultipleSelection)
						Handler.UnselectAll();
					Handler.SelectRow(viewIndex.Value);
				}
			});
		}

		/// <summary>
		/// Unselects the specified row
		/// </summary>
		/// <param name="row">Index of the row to unselect in the model</param>
		public void UnselectRow(int row)
		{
			ChangeSelection(() => {
				areAllObjectsSelected = false;
				if (selectedRows.Contains(row))
					selectedRows.Remove(row);

				var viewIndex = DataStoreView.ModelToView(row);
				if (viewIndex != null) // can be null if the row is not in the view because it is filtered out
					Handler.UnselectRow(viewIndex.Value);
			});
		}

		internal void SelectAll()
		{
			ChangeSelection(() => {
				areAllObjectsSelected = true;
				selectedRows.Clear();
				Handler.SelectAll();
			});
		}

		internal void UnselectAll()
		{
			ChangeSelection(() => {
				areAllObjectsSelected = false;
				selectedRows.Clear();
				Handler.UnselectAll();
			});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.GridViewSelection"/> class.
		/// </summary>
		/// <param name="gridView">Grid view associated with the selection object</param>
		/// <param name="dataStore">Data store to iterate</param>
		public GridViewSelection(GridView gridView, IDataStore dataStore)
		{
			this.gridView = gridView;
			this.selectedRows = new SortedSet<int>();
			this.state = GridViewSelectionState.Normal;
			var collection = dataStore as INotifyCollectionChanged;
			if (collection != null)
				collection.CollectionChanged += OnCollectionChanged;
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
			}
			else if (e.Action == NotifyCollectionChangedAction.Move)
			{
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
			}
		}
	}
}
