#Laboration 2
Jag valde att göra denna laboration med mongodb, .net mvc och .net web api för att experimentera lite med detta inför projektet. Av denna anledning saknar jag mycket kunskap för att lösa de olika punkterna och hinner inte inhämta allt detta innan deadline. För dessa fall används exempelkod och länkar till andra projekt.

---

##Säkerhetsproblem
###Säkerhetshål
Hashning av lösenord är otillräcklig, med dagens prestanda är ett varv med sha512 för lite. Det går för fort att skapa hashen och går därför för fort att gissa sig till det (bruteforce).
###Hur det kan utnyttjas
Om en attackerare får tag i databasen gör detta att det går väldigt fort att gissa sig till alla lösenord i databasen.
###Vilken skada kan det orsaka?
Läckta användaruppgifter ger ett dåligt intryck och väldigt dålig pr.
###Åtgärd
Jag har utnyttjat användarhanteringen från visual studio, som är vältestat och stabilt. Detta är vad som oftast rekommenderas, att utnyttja ett färdiga och vältestade bibliotek istället för att skriva egna lösningar. Är inte detta ok hänvisar jag till har jag egen implementering sedan tidigare jag kan hänvisa till(kör bara 5000 varv med bcrypt, men annars tror jag lösningen är hygglig).

Om man ska bygga en egen lösning bör man hasha med bcrypt tusentals gånger. Rekommenderas också att låta hashningen rotera mellan olika algoritmer som kräver allokering av variabler för att göra processen "dyrare". Valutan för knäckning är tid och beräkningsresurser. Man bör också använda sig av salt(unik nyckel för vardera användare) och peppar(en unik sträng som finns i applikationskoden). Denna kombination för att det sällan är både databas och kodbas som läcker ut. Antalet varv med hashning och vilka algoritmer som används måste ses över kontinuerligt i takt med att beräkningskapacitet ökar.

---

###Säkerhetshål
Det är inte ens lösenordet som hashas, lösenord används i klartext i databasen och kontrollen mot hashen i login_string åstadkommer ingenting alls
###Hur det kan utnyttjas
Om databasen läcker ut behöver inget göras för att alla användaruppgifter ska läcka ut.
###Vilken skada det kan orsaka
Som ovan
###Åtgärd
Som ovan

---

###Säkerhetshål
Sårbart för sql injections - sql anrop parameteriseras inte.
###Hur det kan utnyttjas
Kan utnyttjas för att antingen helt och hållet ta över servern om man lyckas få in shell i databasen och/eller komma över samtliga uppgifter i databasen.
###Vilken skada det kan orsaka
Detta är den vanligaste orsaken till läckta databaser. Gissningsvis också en av de vanligaste orsakerna till komplett övertagande av servern. 
###Åtgärd
Jag har bytt ut databasen mot mongodb och problemet är då inte längre sql injection. Problemen är nu istället om man släpper in javascript i databasen, eller på något vis lyckas få kod att köras i .net vid modelbindandet. Jag har ärligt talat dålig koll på detta, men tänker mig att det som gäller är whitelisting i .net [bind(include=args)] och att problemet med javascript försvinner i modelbindandet eftersom strängar och dylikt då blir just strängar och därför inte körs i mongodb.

Problemet med sql injections löses med parameterisering, vilket i php ser ut enligt följande:
```php
$q = "SELECT id FROM users WHERE username = :username AND password = :password";
$params = array(':username' => $u, ':password' => $p);
```

test: 0;var date=new Date(); do{curDate = new Date();}while(curDate-date<10000)
test ok

sanera: ' " \ ; { }

---

###Säkerhetshål
Data till server saneras inte - inte helt säker, men tror att det skulle gå att få php kod från användare att köras på servern, tror även att det skulle gå att få in shell via databasen.
###Hur det kan utnyttjas
Exekvering av kod på server alternativt på samtliga klienters datorer vid output
###Vilken skada det kan orsaka
Övertagande av server eller spridande av skadlig kod till klienter, dålig pr
###Åtgärd
Sanera input

---

###Säkerhetshål
Data från server saneras inte - utskrift av meddelande bör sättas med textContent
###Hur det kan utnyttjas
Exekvering av kod på klienter
###Vilken skada det kan orsaka
Spridande av skadlig kod till klienter, dålig pr
###Åtgärd
Sanera utdata, sätt utskriften med textContent istället så tillåts inget annat än strängar

---

###Säkerhetshål
ssl (tls) borde användas
###Hur det kan utnyttjas
Kommunikationen kan lätt avlyssnas
###Vilken skada det kan orsaka
Läckta användaruppgifter(lösenord, kreditkortsuppgifter..)
###Åtgärd
Inte åtgärdat, lösning: införskaffa certifikat för ssl och implementera. tls måste utnyttjas, ssl i sig är inte helt säkert.

---

###Säkerhetshål
cookies är inte httponly
###Hur det kan utnyttjas
Med cookies som inte är httponly är det avsevärt enklare att stjäla cookies. I samband med missad sanering av output kan ett javascript komma åt cookien och då logga denna någonstans.
###Vilken skada det kan orsaka
Stulna sessioner
###Åtgärd
Sätt httponly till true
```php
session_set_cookie_params(3600, $cookieParams["path"], $cookieParams["domain"], false, false);
->
session_set_cookie_params(3600, $cookieParams["path"], $cookieParams["domain"], false, true);
```

---

###Säkerhetshål
Inget som helst skydd mot session hijacking
###Hur det kan utnyttjas
En session kan väldigt lätt stjälas
###Vilken skada det kan orsaka
Stulna sessioner
###Åtgärd
Kontrollera webbläsare(ev. version) och ip

---

###Säkerhetshål
Det finns inget skydd mot csrf
###Hur det kan utnyttjas
Man kan från samma webbläsare utnyttja sessionen via en annan flik genom att skicka anrop i egenskap av användaren. 
* Köpa saker i en användares namn utan att denna är medveten om det.
* Lura tjänster som använder sig av maskinlärande för att presentera användarspecifik data (påverka vilka produkter som visas för användaren)
* Dra in pengar genom att använda tjänster som betalar för trafik (typ tradedoubler)
###Vilken skada det kan orsaka
dålig pr
###Åtgärd
Sätt token på samtliga sidor som har en post, och kontrollera denna när post görs.

---

##Prestandaproblem

###Problem
.htaccess - kanske är rätt inställningar satta i httpconf istället, men denna fil saknas annars. Detta innebär att inställningar såsom cache, etag, gzip, routing.. missats.
###Lösning
Det bästa är att sätta detta i serverinställningar, men för säkerhets skull kan en .htaccess sättas. Enklast är att leta upp en färdig mall att utgå ifrån. [Exempel](https://github.com/kristofferlind/projise/blob/master/client/.htaccess)

Vet inte hur detta görs

---

###Problem
Det finns inget buildstep, js, css, html och bilder bör konkateneras, komprimeras och revisionshanteras
###Lösning
För koden som finns skulle jag säga att lösningen är grunt/gulp som taskrunner samt concat, htmlmin, imagemin, cssmin och uglify som komponenter. Hur man löser det med koden som är invävd ihop med php har jag ingen aning om. För min omskrivna kod finns buildstep eftersom koden kompileras, men för att faktiskt göra dessa åtgärder på klientsidan krävs plugins. Jag tror webgrease löser det mesta av punkterna. [Exempel för grunt](https://github.com/kristofferlind/projise/blob/master/Gruntfile.js#L607-L624) Till största del från yeoman, men jag har korrigerat den en del.

---

###Problem
Sprites kunde använts för bilder
###Lösning
Sätt ihop bilder till en, nyttja sedan denna i css genom att lägga in som bakgrundsbild och positionera för att enbart visa en del av bilden.

Detta är inte riktigt ett problem än, det finns bara 2 bilder änsålänge och jag har inte fixat det.

---

###Problem
js bör länkas in i slutet av body
###Lösning
Flytta js till slutet av body

---

###Problem
cdn bör användas för både bootstrap och jquery (att det finns med kan ifrågasättas också, jquery används för 2 metoder, de hjälpmetoder som finns i bootstraps js utnyttjas inte heller, faktum är att bootstraps css inte används speciellt mkt heller)
###Lösning
Byta ut resurser mot cdn. Jag har valt att behålla jquery och bootstrap, det är inte supereffektivt. Men känns inte heller som något problem, detta är väldigt välanvända bibliotek och de flesta har förmodligen redan det i sin cache.

---

###Problem
longpoll.js finns inte, men länkas in
###Lösning
Ta bort referensen

---

###Problem
script.js och bootstrap.js innehåller samma kod(tror jag iaf..) väldigt mkt duplicering som leder till både onödig exekvering och pageload, dyn.css fanns även inline
###Lösning
Ta bort script.js, (även bootstrap eftersom den är utbytt mot cdn), plockade även bort den css som var inline. Tvärtom är absolut snabbast i just detta fallet, men det känns som en väldigt dum optimering.

---

###Problem
längden på flödet med chattmeddelanden kommer ganska fort att bli ett problem
###Lösning
Minska antalet meddelanden som visas.

---
