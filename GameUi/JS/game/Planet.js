

function Planet(name, altName, trajectory, description, details, appearance) {
	this.name = name;
	this.altName = altName;
	this.trajectory = trajectory;
	this.description = description;
	this.details = details;
	this.type = appearance.find('type').text();

	var rotPerEl = appearance.find('rotationPeriod');
	this.rotationPeriod = rotPerEl.length > 0 ? rotPerEl.text() : 10;
	this.size = appearance.find('size').text();
	this.randomSeed = appearance.find('randomSeed').text();
	this.colorPrimary = appearance.find('colorPrimary').text();
	this.colorSecondary = appearance.find('colorSecondary').text();
	var optionalElement = appearance.find('colorOptional');
	this.colorOptional = optionalElement.length > 0 ? optionalElement.text() : 'transparent';
	
	var optionalRing = appearance.find('ringColorPrimary');
	this.ringColorPrimary = optionalRing.length > 0 ? optionalRing.text() : 'transparent';
	var optionalRing2 = appearance.find('ringColorSecondary');
	this.ringColorSecondary = optionalRing2.length > 0 ? optionalRing2.text() : 'transparent';
	this.hasRing = optionalRing != 'transparent' || optionalRing2 != 'transparent';
}