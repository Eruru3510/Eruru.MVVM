package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.MVVMTableColumn;

import javax.swing.*;
import javax.swing.event.TableModelEvent;
import javax.swing.table.DefaultTableModel;
import java.util.ArrayList;

public class MVVMTable extends MVVMItemsControl {

	private final JTable control;
	private final DefaultTableModel tableModel = new DefaultTableModel ();
	private final ArrayList<MVVMTableColumn> columns = new ArrayList<> ();
	private final ArrayList<ArrayList<MVVMTableCell>> cells = new ArrayList<> ();

	int blockTableChanged;

	public MVVMTable (JTable control) {
		super (control);
		this.control = control;
		control.setModel (tableModel);
		control.getModel ().addTableModelListener (e -> {
			if (blockTableChanged > 0) {
				return;
			}
			if (e.getType () == TableModelEvent.UPDATE) {
				cells.get (e.getFirstRow ()).get (e.getColumn ()).onChanged ();
			}
		});
	}

	public JTable getControl () {
		return control;
	}

	public MVVMControl addColumns (MVVMTableColumn column) {
		blockTableChanged++;
		columns.add (column);
		tableModel.addColumn (column.getName ());
		blockTableChanged--;
		return this;
	}

	public MVVMControl addColumns (MVVMTableColumn... columns) {
		for (MVVMTableColumn column : columns) {
			addColumns (column);
		}
		return this;
	}

	@Override
	protected Object convert (Object value) {
		return value;
	}

	@Override
	protected void add (Object value, int index) {
		blockTableChanged++;
		int row = tableModel.getRowCount ();
		Object[] values = new Object[columns.size ()];
		tableModel.insertRow (index, values);
		ArrayList<MVVMTableCell> subCells = new ArrayList<> ();
		cells.add (index, subCells);
		for (int i = 0; i < columns.size (); i++) {
			MVVMTableCell cell = new MVVMTableCell (this, tableModel, row, i);
			cell.setValue (columns.get (i).getBinding ().clone ());
			super.add (cell, columns.size () * index + i);
			cell.setDataContext (new MVVMBinding (value));
			cell.build (getRoot ());
			subCells.add (cell);
		}
		blockTableChanged--;
	}

	@Override
	protected void add (Object value) {
		add (value, tableModel.getRowCount ());
	}

	@Override
	protected void removeAt (int index) {
		int start = columns.size () * index;
		super.removeRange (start, columns.size ());
		for (int i = index + 1; i < cells.size (); i++) {
			for (int n = 0; n < columns.size (); n++) {
				cells.get (i).get (n).setRow (i - 1);
			}
		}
		cells.remove (index);
		tableModel.removeRow (index);
	}

	@Override
	protected void replace (int index, Object value) {
		removeAt (index);
		add (value, index);
	}

	@Override
	public void move (int oldIndex, int newIndex) {

	}

	@Override
	protected void reset () {
		super.clear ();
		cells.clear ();
		while (tableModel.getRowCount () > 0) {
			tableModel.removeRow (tableModel.getRowCount () - 1);
		}
	}

}