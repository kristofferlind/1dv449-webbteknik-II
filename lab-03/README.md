#Lab3
Jag har byggt in laborationsuppgiften i min projektgrund för att experimentera vidare med denna.
Projise.DomainModel/TrafficInfoModel och Projise/Areas/Traffic som är intressant för denna uppgift.

##Körinstruktioner
installera mongodb
kör

#Reflektion
####Vad finns det för krav du måste anpassa dig efter i de olika API:erna?
SR är helt öppet, google vill ha betalt vid över 25000förfrågningar/dag

####Hur och hur länga cachar du ditt data för att slippa anropa API:erna i onödan?
5min cache på klienten och på server hämtas ny data om det är mer än 5min sedan någon data skrevs, för detta används timestamp komponenten i objectId.

####Vad finns det för risker med din applikation?
API ändras eller tas bort, rättigheter för användande ändras.

####Hur har du tänkt kring säkerheten i din applikation?
Riskerna som finns är att javascript från sr skulle köras antingen i mongodb eller på klienten. I C# konverteras all data till objekt. Javascript skulle därför aldrig släppas vidare till mongodb. Riskerna jag skulle kunna tänka mig är om man på något vis lyckas bryta sig ur strängen antingen i mongodb eller på klienten och då lyckas köra javascript där. Alternativt att komponenten för att konvertera från json till objekt på något vis attackeras.

En man in the middle attack skulle kunna göras, denna låtsas vara srs api och kan då kontrollera vilken data det är som hämtas in. Detta kan givetvis utnyttjas för att ge felaktiga trafikrapporter och då i sin tur kanske göra att användare slutar nyttja tjänsten. Jag känner dock inte till någon attack som skulle leda till att elak kod körs på server, databas eller klient i detta scenario.

Google maps vill ha en innerHTML liknande sträng som input för InfoWindow, jag vet inte hur väl deras api hanterar detta och här kanske det finns någon möjlig attack. Man skulle kunna bygga upp denna sträng genom att först bygga dom av det och därmed kunna utnyttja textContent istället för den data som ska visas. För att sedan göra en sträng av det man skickar in. Alternativt utnyttja något saneringsbibliotek(detta skulle kunna göras även för input om man gör konverteringen manuellt och då kanske förhindra ev. risker även gällande databas). Jag har inte undersökt hur newtonsoft(eller mongodb driver) hanterar detta.

Det skulle också kunna vara så att sr's api direkt attackerats för att i sin tur attackera alla som nyttjar detta api.

####Hur har du tänkt kring optimeringen i din applikation?
Enda direkt märkbara problem är att maps tar väldigt lång tid att ladda(~10s utan cache). Detta beror till största del på att det är en väldigt stor karta som laddas(det mesta av denna tid är nedladdning av "kartbitar"). Jag har inte undersökt om man kan optimera detta med större chunks eller att börja med en mindre detaljrikedom, men mindre skärmyta borde definitivt hjälpa. Det skulle också vara intressant att välja bort visning av detaljer för kringliggande länder.

* Användandet av bootstraps css är otroligt onödigt, men cdn används och de flesta har förmodligen redan detta i sin cache(undvikt bootstraps js eftersom denna är tung och dessutom är beroende av jquery).
* Inga 
* CSS i head, javascript i slutet av body
* Cache används på både klient och server
* Det finns enbart en fil med js (borde delats upp i 4-5 filer) och det är endast 200rader js, vilket gör komprimering närmast meningslös. Koden borde dock delats upp och isf vid build konkateneras och komprimeras. Man borde också plocka ut de få delar av bootstrap som används, placera detta i egna cssen och då komprimerat även denna. Börjat lösa buildstep för Areas/Dashboard. I efterföljande steg vill jag förflytta de arbetsuppgifter grunt har till vs.
* Ytterligare optimering skulle vara att istället skicka ut nya enstaka händelser till klienter när de upptäcks, vilket skulle minska både mängden data som skickas och framförallt bearbetningen av dom. Att minska bearbetningen av dom vid filtrering skulle också kunna vara en ganska effektiv optimering.

Framförallt är det alltså optimering av maps som spelar någon roll (~1-10 sekunder), följt av domhantering(~10-100-tal ms). Komprimering av html, css och js skulle kanske göra en skillnad på enstaka millisekunder(och förmodligen inte ens det med en hygglig uppkoppling).