Pro správné fungování projektu je tøeba následující:

1. Nainstalovat ASP.NET MVC 3, http://www.asp.net/mvc/mvc3

2. Nainstalovat knihovnu NLog, http://nlog-project.org/

3. Nastavit v Solution properties spouštìní:
	
	RMOUSE na Solution v Solution Exploreru ->Properties
	
	Common Properties -> Startup Project
	
	Nastavit na "Multiple startup projects" a u GameServer a GameUi vybrat Action:Start
	

Po tomto nastavení by mìl jít projekt spustit.

Pro bìžnou práci na UI je možné spustit GameServer samostatnì pomocí pøeloženého exe a GameUi spouštìt 
v debug módu z Visual Studia.

4. V projektu GameUI je tøeba v souboru Web.config zmìnit v appSettings hodnotu assetPath na úplnou cestu k adresáøi Assets.
