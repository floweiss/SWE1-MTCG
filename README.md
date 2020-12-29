# SWE1-MTCG

## Allgemein

Monster Card Trading Game von Florian Weiss im Zuge von SWE1
Link zum GitHub Repository: https://github.com/floweiss/SWE1-MTCG



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
    * Users (Username, Hashed PW, Fullname, Bio, Image)
    * Userdata (Token, Coins, ELO. Deck-CardIDs, Stack-CardIDs)
* Für die Speicherung von Daten aus dem ClientSingleton in die DB gibt es einen eigenen Service (UserDataService)

### Battle
* Der erste User, der ein Battle startet, wartet in einer Schleife, bis ein zweiter User ein Battle startet
* Das Battle wird im Thread des zweiten Users durchgeführt und das resultierende Battle Log wird in einem Singleton (ArenaSingleton) gespeichert
* Der erste User wartet in seinem Thread darauf, dass ein Eintrag in dem BattleLogs ConcurrentStack im Singleton gespeichert wird
* Wenn etwas auf dem ConcurrentStack liegt wird dies als Battle Log für den ersten User verwendet und vom Stack entfernt
