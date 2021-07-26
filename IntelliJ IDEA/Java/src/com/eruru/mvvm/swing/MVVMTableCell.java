package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMBinding;

import javax.swing.table.TableModel;

public class MVVMTableCell extends MVVMControl {

	private final MVVMTable table;
	private final TableModel tableModel;

	private MVVMBinding value;
	private int row;
	private int column;

	public MVVMTableCell (MVVMTable table, TableModel tableModel, int row, int column) {
		super (null);
		this.table = table;
		this.tableModel = tableModel;
		this.row = row;
		this.column = column;
	}

	public void onChanged () {
		onChanged (value);
	}

	public MVVMBinding getValue () {
		return getBinding (value, this::setValue);
	}

	public void setValue (MVVMBinding binding) {
		value = setBinding (binding, () -> tableModel.getValueAt (row, column), targetValue -> {
			table.blockTableChanged++;
			tableModel.setValueAt (targetValue, row, column);
			table.blockTableChanged--;
		}, true);
	}

	public int getRow () {
		return row;
	}

	public void setRow (int row) {
		this.row = row;
	}

	public int getColumn () {
		return column;
	}

	public void setColumn (int column) {
		this.column = column;
	}

}