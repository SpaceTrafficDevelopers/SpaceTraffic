Pro spr�vn� fungov�n� projektu je t�eba n�sleduj�c�:

1. Nainstalovat ASP.NET MVC 3, http://www.asp.net/mvc/mvc3

2. Nainstalovat knihovnu NLog, http://nlog-project.org/

3. Nastavit v Solution properties spou�t�n�:
	
	RMOUSE na Solution v Solution Exploreru ->Properties
	
	Common Properties -> Startup Project
	
	Nastavit na "Multiple startup projects" a u GameServer a GameUi vybrat Action:Start
	

Po tomto nastaven� by m�l j�t projekt spustit.

Pro b�nou pr�ci na UI je mo�n� spustit GameServer samostatn� pomoc� p�elo�en�ho exe a GameUi spou�t�t 
v debug m�du z Visual Studia.

4. V projektu GameUI je t�eba v souboru Web.config zm�nit v appSettings hodnotu assetPath na �plnou cestu k adres��i Assets.
