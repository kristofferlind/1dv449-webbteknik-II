#Reflektion

###Vad tror Du vi har för skäl till att spara det skrapade datat i JSON-format?
JSON är väldigt trevligt att jobba med på klientsidan.

###Olika jämförelsesiter är flitiga användare av webbskrapor. Kan du komma på fler typer av tillämplingar där webbskrapor förekommer?
Sökmotorer, portaler, nyhetssidor, mashup (om inte api finns tillgängligt)

###Hur har du i din skrapning underlättat för serverägaren?
Satt user-agent till scraper, samt användning av cache för att minska trafik.

###Vilka etiska aspekter bör man fundera kring vid webbskrapning?
Man bör läsa och följa robots.txt. Läsa tos för att utgöra om det ens är lagligt. Fundera över vilken mängd resurser man använder för webbplatsen. Fundera över innehållet man hämtar, vilket affärsvärde har datan som hämtas?

###Vad finns det för risker med applikationer som innefattar automatisk skrapning av webbsidor? Nämn minst ett par stycken!
Webbplatsen uppdateras och sökvägar ändras, vilket resulterar i att skrapningen behöver förnyas.
Avtal förändras, det blir kanske plötsligt olagligt att skrapa sidan.
Webbplatsen läggs ned, alternativ resurs måste hittas och det måste skapas en skrapare för denna.
Webbplatsens innehåll ändras drastiskt, materialet som skrapas kan bli tvivelaktigt eller direkt olagligt. Detta resulterar i sin tur i att den egna webbplatsen blir olaglig.

###Tänk dig att du skulle skrapa en sida gjord i ASP.NET WebForms. Vad för extra problem skulle man kunna få då?
Automatiskt satta selektorer för element.

###Välj ut två punkter kring din kod du tycker är värd att diskutera vid redovisningen. Det kan röra val du gjort, tekniska lösningar eller lösningar du inte är riktigt nöjd med.
Användande av timeout vid skrapning i samband med request, lösning med promises(q) hade varit klart bättre. Lösningen med skrapning som "bakgrundsjobb" är ännu bättre (beroende på trafik).
Koden kunde delats upp i moduler, annars inget som känns anmärkningsvärt bra/dåligt.

###Hitta ett rättsfall som handlar om webbskrapning. Redogör kort för detta.
LinkedIn 2014, en grupp hackare använde sig av amazon web services för att skapa massvis med falska medlemmar och sedan använda dessa för att skrapa LinkedIn. Detta är förbjudet enligt deras användaravtal. Algoritm för att ta sig förbi captcha användes. LinkedIn har senare kommit fram till att skrapningen utförts av HiringSolved, som i sin tur sålde sin insamlade data.

###Känner du att du lärt dig något av denna uppgift?
Jag har aldrig utfört någon skrapning tidigare, så det var nytt. Även bekantat mig lite med jquery tack vare cheerio. Får nog ut mer av det om jag snyggar till koden och gör extrauppgifterna, men först ska laborationerna i andra kursen betas av.