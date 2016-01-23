/**
 * @author Azaroth
 */

function Star(name, trajectory, graphics, description, appearance) {
	this.name = name;
	this.trajectory = trajectory;
	this.graphics = graphics;
	this.description = description;
	this.size = appearance.find('size').text();
}