/**
 * @author Azaroth
 */

function Star(name, trajectory, graphics, description, appearance) {
	this.name = name;
	this.trajectory = trajectory;
	this.graphics = graphics;
	this.description = description;
	this.size = appearance.find('size').text();
	this.randomSeed = appearance.find('randomSeed').text();
	this.colorPrimary = appearance.find('colorPrimary').text();
	this.colorSecondary = appearance.find('colorSecondary').text();
	var optionalElement = appearance.find('colorOptional');
	this.colorOptional = optionalElement.length > 0 ? optionalElement.text() : 'transparent';
}