# SWE Dokumentation

# Verwendung von ORM Mapper

## Bei Klassen
In diesem Kapitel geht es um Klassen, welche in die Datenbank gespeichert werden soll.

### TableName
Zwischen dem Namespace und der Klassendefinition kann folgendes geschrieben werden: <br>
<b>[Entity(TableName = "tablename_in_database")]</b> <br>
Dieses Beispiel stellt den tabellen namen manuell ein, welcher in der Datenbank verwendet wird. Stellt man das nicht ein, wird als Tabellenname in der Datenbank nur der Klassenname verwendet. 

### PrimaryKeys
Will man ein Klassenattribut als PrimaryKey definieren so schreibt man folgende Zeile vor das Attribut: <br>
<b>[PrimaryKey]</b>

### ForeignKey
Will man ein ein Klassenattribut als ForeignKey definieren so schreibt man folgende Zeile vor das Attribut: <br>
<b>[ForeignKey(ColumnName = "Kxyz")]</b> <br>
Zu beachten ist, dass ColumnName den Namen der Spalte definiert.

### Ignore 
Will man ein Attribut nicht in der Datenbank haben, dann schreibt man vor das Klassenattribut folgende Zeile: <br>
<b>[Ignore]</b>

### Field
Will man den Namen von einem Klassenattribut in der Spalte einer Datenbanktabelle anderst benennen, dann schreibt man vor das Klassenattribut folgende Zeile: <br>
<b>[Field(ColumnName = "xyz")]</b> 

## Im Programm 

### Load Object
M�chte man ein Objekt laden, dann muss man folgende Zeile schreiben: <br>
<b>Orm.GetObject<ClassOfObject>("primaryKey")</b> 

### Insert / Modify Object
M�chte man ein Objekt speichern oder modifiziert in der Datenbank ablegen, dann muss man folgende Zeile schreiben: <br>
<b>Orm.SaveObject(variable)</b> 

### Caching
M�chte man eine Cache verwenden, dann muss folgender Befehl aufgerufen werden: <br>
<b>Orm.Cache = new Cache();</b> <br>

### Query
M�chte man abfragen selber machen, dann muss folgender Befehl ausgef�hrt werden: <br>
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
M�chte man ein Objekt in der Datenbank Locken, dann m�ssen folgende Zeile aufgerufen werden: <br>
<b>Orm.Locking = new DbLocking();<br>
Orm.Lock(variable)</b><br>
Achtung m�chte man das Objekt, wieder unlocken muss folgender Befehl ausgef�hrt werden: <br>
<b>Orm.Unlock(variable)</b>

# Inbetriebnahme der Test Anwendung
Um die Anwendung zu starten m�ssen Sie auf einem lokalen Server das Setup Script ausf�hren und den Connectionstring ausbessern und das SetUp Script laufen lassen.