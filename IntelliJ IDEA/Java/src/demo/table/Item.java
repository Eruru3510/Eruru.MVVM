package demo.table;

public class Item {

	int id;
	
	private String name;

	public Item (int id, String name) {
		this.id = id;
		this.name = name;
	}

	public int getId () {
		System.out.printf ("get id %s\n", id);
		return id;
	}

	public void setId (int id) {
		this.id = id;
	}

	public String getName () {
		return name;
	}

	public void setName (String name) {
		System.out.printf ("item.name has changed. %s to %s \n", this.name, name);
		this.name = name;
	}

}