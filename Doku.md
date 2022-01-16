# SWE Dokumentation

# Verwendung von ORM Mapper

## Bei Klassen
In diesem Kapitel geht es um Klassen, welche in die Datenbank gespeichert werden soll.

### TableName
Zwischen dem Namespace und der Klassendefinition kann folgendes geschrieben werden: <br>
<b>[Entity(TableName = "tablename_in_database")]</b> <br>
Dieses Beispiel stellt den tabellen namen manuell ein, welcher in der Datenbank verwendet wird. Stellt man das nicht ein, wird als Tabellenname in der Datenbank nur der Klassenname verwendet. 

### PrimaryKeys
Property als PrimaryKey definieren dann verwendet man das Attribut: <br>
<b>[PrimaryKey]</b>

### ForeignKey
Wenn eine Property/Field als ForeignKey definiert werden solk tagged man es mit dem Attribut: <br>
<b>[ForeignKey(ColumnName = "Kxyz")]</b> <br>
Zu beachten ist, dass ColumnName den Namen der Spalte definiert.

### Ignore 
Will man ein Attribiut ignoriert haben muss man folgendes Attrobit verwenden <br>
<b>[Ignore]</b>

### Field
Es ist möglich den Namen einer Property in der Datenbank zu ändern mit dem folgenden Attribut: <br>
<b>[Field(ColumnName = "xyz")]</b> 

## Im Programm 

### Load Object
Möchte man ein Objekt laden, dann muss man folgende Zeile schreiben: <br>
<b>Orm.GetObject<ClassOfObject>("primaryKey")</b> 

### Insert / Modify Object
Möchte man ein Objekt speichern oder modifiziert in der Datenbank ablegen, dann muss man folgende Zeile schreiben: <br>
<b>Orm.SaveObject(variable)</b> 

### Caching
Möchte man eine Cache verwenden, dann muss folgender Befehl aufgerufen werden: <br>
<b>Orm.Cache = new Cache();</b> <br>

### Query
Möchte man abfragen selber machen, dann muss folgender Befehl ausgeführt werden: <br>
<b>Orm.From<ClassOfObject>().GreatherThan("FieldName", value);</b><br>
Es gibt mehr Abfragen elemente wie:
* Or
* Like
* And
* BeginGroup
* EndGroup
* Equals
* In
* LessThan


### Locking
Möchte man ein Objekt in der Datenbank Locken, dann müssen folgende Zeile aufgerufen werden: <br>
<b>Orm.Locking = new DbLocking();<br>
Orm.Lock(variable)</b><br>
Achtung möchte man das Objekt, wieder unlocken muss folgender Befehl ausgeführt werden: <br>
<b>Orm.Unlock(variable)</b>

# Inbetriebnahme der Test Anwendung
Um die Anwendung zu starten müssen Sie auf einem lokalen Server das Setup Script ausführen und den Connectionstring ausbessern und das SetUp Script laufen lassen.