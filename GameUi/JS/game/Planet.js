

function Planet(name, altName, trajectory, description, details, appearance) {
	this.name = name;
	this.altName = altName;
	this.trajectory = trajectory;
	this.description = description;
	this.details = details;
	this.size = appearance.find('size').text();
}