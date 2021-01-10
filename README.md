# SWE1-MTCG

## Allgemein

Monster Trading Cards Game (MTCG) von Florian Weiss im Zuge von SWE1  
Link zum GitHub Repository: https://github.com/floweiss/SWE1-MTCG  
Um die Applikation verwenden zu können, muss der Connection String für die DB je nach Setup angepasst werden
```
private string _cs = "Host=localhost;Username=postgres;Password=*******;Database=postgres";
```
  
Dies muss in folgenden Files durchgeführt werden:
* UserService.cs
* UserDataService.cs
* TradeService.cs
* CardService.cs
* PackageService.cs
* DeckService.cs
* ScoreService.cs


## Technische Schritte

### Aufbau der Applikation
* Es gibt einen Server, der für jede Request einen Thread abspaltet
* Mithilfe des RequestContext werden alle Informationen aus der Request herausgefiltert
* Je nachdem, welche Resource angefordert wird, wird die entsprechende API aufgerufen
* Die aufgerufenen API überprüft die Informationen aus der Request und führt erste Schritte durch
* Für jede API gibt es einen entsprechenden Controller der an den entsprechenden Service weiterdelegiert
* Der aufgerufene Service führt die Aktionen durch die an diesem Endpoint passieren sollen (z.B.: Karte erstellen, Deck konfigurieren, etc.)
* Je nach Ergebnis der Aktion wird ein ResponseContext erstellt und dieser wird an den Client zurück geschickt

### Usermanagement
* Die eingeloggten User werden in einem Singleton (ClientSingleton) abgespeichert
* Alle Aktionen, die den User betreffen, werden für die User in der ClientMap im Singleton durchgeführt

### Persistierung
* Daten, die persistiert werden, werden in einer PostgreSQL DB gespeichert
* Es gibt folgenden Tables:
    * Cards (ID, Name, CardType, Element, Damage)
    * Packages (ID, CardIDs)
    * Trades (ID, CardToTrade, Type, MinimumDamage, Usertoken)
    * Userdata (Token, Coins, ELO, Deck-CardIDs, Stack-CardIDs)
    * Users (Username, Password (hashed), Fullname, Bio, Image)
* Für die Speicherung von Daten aus dem ClientSingleton in die DB gibt es einen eigenen Service (UserDataService)

### Battle
* Der erste User, der ein Battle startet, wartet in einer Schleife, bis ein zweiter User ein Battle startet
* Das Battle wird im Thread des zweiten Users durchgeführt und das resultierende Battle Log wird in einem Singleton (ArenaSingleton) gespeichert
* Der erste User wartet in seinem Thread darauf, dass ein Eintrag in dem BattleLogs ConcurrentStack im Singleton gespeichert wird
* Wenn etwas auf dem ConcurrentStack liegt wird dies als Battle Log für den ersten User verwendet und vom Stack entfernt



## Tests

### Unit Tests
* Mit den Unit Tests wurde die grundlegende Funktion der wichtigsten Komponenten des Servers (Request- und Response-Context, ApiService) und des MTCG (Battle, Deck, Stack, ElementTypes, Cards, Trades) getestet
* Weitere Unit Tests beinhalten das Testen der MessageApi, bei der die grundlegenden Funktionen einer API (HTTP-Methoden) getestet wurden
* Die eigentlichen durchführenden Units (Services) wurden bei den MessageApi Tests gemockt, da diese wirklich auf das File-System bzw. beim MTCG auf die PostgreSQL Datenbank zugreifen und dies nicht wirklich testbar ist, deshalb wurde auch auf weitere API-Tests verzichtet

### Integration Tests
* Ein kompletter Test des MTCG wurde mittels eines curl Skriptes durchgeführt, dieses ist leicht abgewandelt im Vergleich zum vorgegebenen Skript
* Einzelne APIs wurden durch einzelne Requests mit dem Tool Postman getestet



## Lessons Learned

Verbesserungswürdig ist auf jeden Fall der Umgang mit Requests, die nicht korrekt sind.
Momentan wird oft jeder einzelne Wert einer Request (z.B.: CardID, CardType, Damage, usw.) überprüft und anschließend ein Return String mit einem entsprechendem Error zurückgegeben.
Dies könnte besser mit Exceptions gelöst werden.  
Weiters wäre es gut gewesen ein Config-File zu haben, in dem übergreifende Informationen, wie der Connection String der Datenbank, hinterlegt werden.  
Auch die Abhandlung des Battles wäre sicherlich eleganter möglich gewesen, da bei mehreren Battles gleichzeitig die verwendete Methode vermutlich zu einem Durcheinander der BattleLogs führen würde.
Diesen Fall (mehrere Battles) habe ich aber nicht getestet, weil er mir in unserer Situation als nicht so relevant vorgekommen ist.
